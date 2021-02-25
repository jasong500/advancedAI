using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(Collider))]
public class FlockAgent : MonoBehaviour
{

    Flock agentFlock;
    public Flock AgentFlock { get { return agentFlock; } }
    Collider agentCollider;
    public Collider AgentCollider { get { return agentCollider; } }
    public TextMeshProUGUI hitCounter;
    public TextMeshProUGUI hitCounterAgent;

    // Start is called before the first frame update
    void Start()
    {
        agentCollider = GetComponent<Collider>();
        GameObject temp = GameObject.FindGameObjectWithTag("Counter");
        hitCounter = temp.GetComponent<TextMeshProUGUI>();
        temp = GameObject.FindGameObjectWithTag("CounterAgent");
        hitCounterAgent = temp.GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        if(hitCounter == null)
        {
            GameObject temp = GameObject.FindGameObjectWithTag("Counter");
            hitCounter = temp.GetComponent<TextMeshProUGUI>();
        }
        if(hitCounterAgent == null)
        {
            GameObject temp = GameObject.FindGameObjectWithTag("CounterAgent");
            hitCounterAgent = temp.GetComponent<TextMeshProUGUI>();
        }
    }

    public void Initialize(Flock flock)
    {
        agentFlock = flock;
    }

    public void Move(Vector3 velocity)
    {
        transform.up = velocity;
        transform.position += (Vector3)velocity * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.name.Contains("Cube"))
        {
            //on collision with obstacles
            Debug.Log(collision.gameObject.name);
            int temp = int.Parse(hitCounter.text);
            temp++;
            hitCounter.text = temp.ToString();
        }
        else if (collision.gameObject.name.Contains("Agent"))
        {
            //on collision with other agent
            Debug.Log(this.name + " collided with " + collision.gameObject.name);
            int temp = int.Parse(hitCounter.text);
            temp++;
            hitCounter.text = temp.ToString();
        }
        else
        {
            //do nothing
        }
    }
}