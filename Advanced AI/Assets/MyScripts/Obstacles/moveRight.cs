using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveRight : MonoBehaviour
{
    public float moveModMax;
    [HideInInspector]
    public float moveMod;
    public float moveModMin;

    bool reachedPos = false;
    float startTime;
    float journeyLength;
    Vector3 newPos;

    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        moveMod = Random.Range(moveModMin, moveModMax);
        moveMod /= 100;
        this.transform.rotation = Random.rotation;

        randomPos();
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, newPos) <= 1.0f)
        {
            reachedPos = true;
        }
        
        if (reachedPos)
        {
            reachedPos = false;
            randomPos();
        }
        else
        {
            float distCovered = (Time.time - startTime) * moveMod;
            float fractionOfJourney = distCovered / journeyLength;

            transform.position = Vector3.Lerp(this.transform.position, newPos, fractionOfJourney);
        }
    }

    void randomPos()
    {
        newPos = new Vector3(Random.Range(-5.0f, 6.0f), Random.Range(-2.0f, 3.7f), Random.Range(-9.0f, 9.0f));
        startTime = Time.time;
        journeyLength = Vector3.Distance(transform.position, newPos);
    }

    void onCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Something")
        {
            Physics.IgnoreCollision(collision.collider, gameObject.GetComponent<BoxCollider>());
        }
    }
}