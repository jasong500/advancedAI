using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class MoveToGoalAgent : Agent
{
    [SerializeField] private Transform targetTransform;
    [SerializeField] private Material winMat;
    [SerializeField] private Material loseMat;
    [SerializeField] private MeshRenderer floorMesh;

    public override void OnEpisodeBegin()
    {
        transform.localPosition = new Vector3(Random.Range(-7.2f, 6.3f), 0.0f, Random.Range(-3.2f, 5.3f));
        targetTransform.localPosition = new Vector3(Random.Range(-7.2f, 6.3f), -2.705419f, Random.Range(-3.2f, 5.3f));
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

        float moveSpeed = 5.0f;
        transform.localPosition += new Vector3(moveX, 0, moveZ) * Time.deltaTime * moveSpeed;
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        continuousActions[0] = Input.GetAxisRaw("Horizontal");
        continuousActions[1] = Input.GetAxisRaw("Vertical");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "iWall")
        {
            SetReward(-1.0f);
            floorMesh.material = loseMat;
            EndEpisode();
        }
        else if (other.tag == "goal")
        {
            SetReward(1.0f);
            floorMesh.material = winMat;
            EndEpisode();
        }
    }
}
