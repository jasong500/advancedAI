using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class Snake : Agent
{
    [SerializeField] private Transform targetTransform;
    [SerializeField] private Material winMat;
    [SerializeField] private Material loseMat;
    [SerializeField] private MeshRenderer floorMesh;
    [SerializeField] private TextMeshPro score;

    int scoreNum = 0;
    private float moveXAI = 0;
    private float moveZAI = 0;

    public List<Transform> bodyParts = new List<Transform>();

    public float minDistance = 0.25f;

    public int beginSize;

    public float moveSpeed = 5.0f;
    public float rotationSpeed = 50;

    public GameObject bodyprefabs;

    private float dis;
    bool isSet = false;
    int dir = 0;
    private Transform curBodyPart;
    private Transform PrevBodyPart;
    float timer;

    public override void OnEpisodeBegin()
    {
        moveSpeed = 5.0f;
        scoreNum = 0;
        transform.localPosition = new Vector3(Random.Range(-7.2f, 6.3f), -2.9f, Random.Range(-3.2f, 5.3f));
        targetTransform.localPosition = new Vector3(Random.Range(-7.2f, 6.3f), -2.705419f, Random.Range(-3.2f, 5.3f));

        //score = GameObject.Find("Score").GetComponent<TextMeshPro>();
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

        Move(moveX, moveZ);
        Update();
    }

    private void Start()
    {
        bodyParts.Add(this.transform);
        timer = Time.deltaTime;
    }

    private void Update()
    {
        Move(moveXAI, moveZAI);

        timer += Time.deltaTime;

        if(timer >= 5.0f)
        {
            if (moveXAI == 0 && moveZAI == 0)
            {
                SetReward(-100.0f);
                EndEpisode();
            }
            timer = 0.0f;
        }
    }

    public void Move(float moveX, float moveZ)
    {

        float curspeed = moveSpeed;
        Vector3 myForward = bodyParts[0].forward;

        if (moveX < 0)
        {
            myForward = new Vector3(-myForward.y, 0.0f, myForward.z);
            isSet = true;
            dir = 1;
            //bodyParts[0].Rotate(Vector3.up * rotationSpeed * Time.deltaTime * -Input.GetAxis("Horizontal"));
        }
        else if (moveX > 0)
        {
            myForward = new Vector3(myForward.y, 0.0f, myForward.z);
            isSet = true;
            dir = 2;
            //bodyParts[0].Rotate(Vector3.up * rotationSpeed * Time.deltaTime * Input.GetAxis("Horizontal"));
        }
        else if (moveZ < 0)
        {
            myForward = new Vector3(myForward.x, 0.0f, -myForward.y);
            isSet = true;
            dir = 3;
            //bodyParts[0].Rotate(Vector3.right * rotationSpeed * Time.deltaTime * -Input.GetAxis("Vertical"));
        }
        else if (moveZ > 0)
        {
            myForward = new Vector3(myForward.x, 0.0f, myForward.y);
            isSet = true;
            dir = 4;
            //bodyParts[0].Rotate(Vector3.right * rotationSpeed * Time.deltaTime * Input.GetAxis("Vertical"));
        }
        else
        {
            //Debug.Log(moveX);
            //Debug.Log(moveZ);
        }

        if(isSet)
        {
            if (dir == 1)
            {
                Vector3 temp = myForward * curspeed * Time.smoothDeltaTime;
                temp = new Vector3(-temp.y, 0.0f, temp.z);
                bodyParts[0].Translate(temp, Space.Self);
            }
            else if (dir == 2)
            {
                Vector3 temp = myForward * curspeed * Time.smoothDeltaTime;
                temp = new Vector3(temp.y, 0.0f, temp.z);
                bodyParts[0].Translate(temp, Space.Self);
            }
            else if (dir == 3)
            {
                Vector3 temp = myForward * curspeed * Time.smoothDeltaTime;
                temp = new Vector3(temp.x, 0.0f, -temp.y);
                bodyParts[0].Translate(temp, Space.Self);
            }
            else if (dir == 4)
            {
                Vector3 temp = myForward * curspeed * Time.smoothDeltaTime;
                temp = new Vector3(temp.x, 0.0f, temp.y);
                bodyParts[0].Translate(temp, Space.Self);
            }
            else
            {
                Debug.Log("Dir is: " + dir);
            }
        }

        //for (int i = 1; i < bodyParts.Count; i++)
        //{
        //    Debug.Log(bodyParts.Count);
        //    curBodyPart = bodyParts[i];
        //    PrevBodyPart = bodyParts[i - 1];

        //    dis = Vector3.Distance(PrevBodyPart.position, curBodyPart.position);

        //    Vector3 newpos = PrevBodyPart.position;

        //    newpos.y = bodyParts[0].position.y;

        //    float T = (Time.deltaTime/2.0f) * dis / minDistance * 1.0f;

        //    if (T > 0.5f)
        //    {
        //        T = 0.5f;
        //    }

        //    curBodyPart.position = Vector3.Lerp(curBodyPart.position, PrevBodyPart.position, T);
        //}
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
            SetReward(-100.0f);
            floorMesh.material = loseMat;
            destroyBody();
            EndEpisode();
        }
        else if (other.tag == "goal")
        {
            SetReward(100.0f);
            floorMesh.material = winMat;
            moveSpeed = moveSpeed * 1.15f;
            respawnTarget();
            scoreNum++;
            score.text = "Score: " + scoreNum;
            //AddBodyPart();
            //EndEpisode();
        }
        else if (other.tag == "tile")
        {
            destroyBody();
            EndEpisode();
        }
    }

    public void respawnTarget()
    {
        targetTransform.localPosition = new Vector3(Random.Range(-7.6f, 6.68f), -2.705419f, Random.Range(-3.548f, 5.7f));
    }

    public void destroyBody()
    {
        for (int i = bodyParts.Count-1; i > 0; i++)
        {
            Destroy(bodyParts[i].gameObject);
            bodyParts.RemoveAt(i);
        }
    }

    public void AddBodyPart()
    {
        if (bodyParts.Count == 0)
        {
            Debug.Log(bodyParts.Count);
        }

        Vector3 temp = bodyParts[bodyParts.Count - 1].position;

        Transform newPart = gameObject.AddComponent<Transform>();

        if (dir == 1)
        {
            temp = new Vector3(temp.x, temp.y, temp.z - 1.2f);
            newPart = (Instantiate(bodyprefabs, temp, bodyParts[bodyParts.Count - 1].rotation) as GameObject).transform;
        }
        else if (dir == 2)
        {
            temp = new Vector3(temp.x, temp.y, temp.z + 1.2f);
            newPart = (Instantiate(bodyprefabs, temp, bodyParts[bodyParts.Count - 1].rotation) as GameObject).transform;
        }
        else if (dir == 3)
        {
            temp = new Vector3(temp.x - 1.2f, temp.y, temp.z);
            newPart = (Instantiate(bodyprefabs, temp, bodyParts[bodyParts.Count - 1].rotation) as GameObject).transform;
        }
        else if (dir == 4)
        {
            temp = new Vector3(temp.x + 1.2f, temp.y, temp.z);
            newPart = (Instantiate(bodyprefabs, temp, bodyParts[bodyParts.Count - 1].rotation) as GameObject).transform;
        }

        newPart.SetParent(transform);

        bodyParts.Add(newPart);
    }
}