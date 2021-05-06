using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class hide : Agent
{
    [SerializeField] private Transform targetTransform;
    [SerializeField] private TextMeshPro score;

    public GameObject seeker;

    int scoreNum = 0;
    private float moveXAI = 0;
    private float moveZAI = 0;

    public float moveSpeed = 5.0f;

    float timer;

    public override void OnEpisodeBegin()
    {
        moveSpeed = 5.0f;
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
            SetReward(100.0f);
            seeker.GetComponent<seek>().SetReward(-100.0f);
            timer = 0.0f;
            scoreNum++;
            score.text = "Hider Score: " + scoreNum;
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
            SetReward(-100.0f);
            scoreNum = 0;
            EndEpisode();
        }
        else if (other.tag == "blueAgent")
        {
            SetReward(-100.0f);
            scoreNum = -1;
            EndEpisode();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "blueAgent")
        {
            SetReward(-100.0f);
            scoreNum = -1;
            EndEpisode();
        }
        else if (collision.gameObject.tag == "iWall")
        {
            SetReward(-100.0f);
            scoreNum = -1;
            EndEpisode();
        }
    }

    public void addReward()
    {
        SetReward(100.0f);
    }
}