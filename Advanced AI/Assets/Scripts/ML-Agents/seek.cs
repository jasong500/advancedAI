using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class seek : Agent
{
    [SerializeField] private Transform targetTransform;
    [SerializeField] private TextMeshPro score;

    int scoreNum = 0;
    private float moveXAI = 0;
    private float moveZAI = 0;

    public float moveSpeed = 5.0f;

    public int wallsToSpawn = 1;
    public GameObject wallPrefab;
    public List<GameObject> gameWalls = new List<GameObject>();

    public GameObject hider;
    public GameObject parent;

    private float dis;
    bool isSet = false;
    int dir = 0;
    float timer;

    public override void OnEpisodeBegin()
    {
        moveSpeed = 5.0f;
        transform.localPosition = new Vector3(Random.Range(-7.2f, 6.3f), -2.9f, Random.Range(-3.2f, 5.3f));
        targetTransform.localPosition = new Vector3(Random.Range(-7.2f, 6.3f), -3.15f, Random.Range(-3.2f, 5.3f));

        spawnWalls();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(targetTransform.localPosition);
    }

    //actions sent in from AI
    public override void OnActionReceived(ActionBuffers actions)
    {
        //continuous is floats and can be negative
        //discrete is ints and can't be negative
        float moveX = actions.ContinuousActions[0];
        float moveZ = actions.ContinuousActions[1];

        moveXAI = moveX;
        moveZAI = moveZ;

        transform.localPosition += new Vector3(moveX, 0.0f, moveZ) * Time.deltaTime * moveSpeed;
    }

    private void Start()
    {
        timer = Time.deltaTime;
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if(timer >= 3.0f)
        {
            if (moveXAI == 0 && moveZAI == 0)
            {
                SetReward(-175.0f);
                EndEpisode();
            }
            SetReward(-175.0f);
            timer = 0.0f;
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        moveXAI = Input.GetAxisRaw("Horizontal");
        moveZAI = Input.GetAxisRaw("Vertical");

        continuousActions[0] = moveXAI;
        continuousActions[1] = moveZAI;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "iWall")
        {
            SetReward(-175.0f);
            hider.GetComponent<hide>().addReward();
            scoreNum = -1;
            EndEpisode();
        }
        else if (other.tag == "goal")
        {
            SetReward(150.0f);
            hider.GetComponent<hide>().SetReward(-100.0f);
            timer = 0.0f;
            respawnTarget();
            scoreNum++;
            score.text = "Seeker Score: " + scoreNum;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "goal")
        {
            SetReward(150.0f);
            respawnTarget();
            scoreNum++;
            score.text = "Seeker Score: " + scoreNum;
        }
        else if (collision.gameObject.tag == "iWall")
        {
            SetReward(-175.0f);
            hider.GetComponent<hide>().addReward();
            scoreNum = -1;
            EndEpisode();
        }
    }

    public void spawnWalls()
    {
        for (int i = 0; i < gameWalls.Count; i++)
        {
            GameObject temp = gameWalls[i];
            gameWalls.RemoveAt(i);
            Destroy(temp);
        }

        while (gameWalls.Count < wallsToSpawn)
        {
            GameObject newWall = Instantiate(wallPrefab, parent.transform);
            Vector3 newScale = new Vector3(Random.Range(0.1f, 2.0f), 1.0f, Random.Range(0.1f, 5.0f));
            Vector3 newSpawn = new Vector3(Random.Range(-6.8f, 5.94f), -3.21f, Random.Range(-1.3f, 3.34f));
            Vector3 newRot = new Vector3(Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f));
            newWall.transform.localPosition = newSpawn;
            newWall.transform.localScale = newScale;
            newWall.transform.localEulerAngles = newRot;
            //newWall.GetComponent<BoxCollider>().size = newScale;
            gameWalls.Add(newWall);
        }
    }

    public void respawnTarget()
    {
        targetTransform.localPosition = new Vector3(Random.Range(-7.6f, 6.68f), -3.15f, Random.Range(-3.548f, 5.7f));
        spawnWalls();
    }
}