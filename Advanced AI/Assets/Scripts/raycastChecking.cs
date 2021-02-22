using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class raycastChecking : MonoBehaviour
{

    public Rigidbody rb;
    public float moveIncrement;
    public float timeToDodge;
    public bool anythingHere;
    public float rayLength;
    public Material mat;

    [HideInInspector]
    public int hitCount = 0;

    Vector3 destination;
    int mID = 0;
    GameObject randomObj;
    Ray raycast;
    Ray[] rays;
    float mRadiusSquaredDistance = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();

        rays = new Ray[9];
        rays[0] = new Ray(transform.position, new Vector3(rayLength, 0.5f, 0.0f));
        rays[1] = new Ray(transform.position, new Vector3(rayLength, 0.375f, 1.0f));
        rays[2] = new Ray(transform.position, new Vector3(rayLength, 0.25f, 0.0f));
        rays[3] = new Ray(transform.position, new Vector3(rayLength, 0.125f, 0.5f));
        rays[4] = new Ray(transform.position, new Vector3(rayLength, 0.0f, 0.0f));
        rays[5] = new Ray(transform.position, new Vector3(rayLength, -0.125f, -0.5f));
        rays[6] = new Ray(transform.position, new Vector3(rayLength, -0.25f, 0.0f));
        rays[7] = new Ray(transform.position, new Vector3(rayLength, -0.375f, -1.0f));
        rays[8] = new Ray(transform.position, new Vector3(rayLength, -0.5f, 0.0f));
    }

    // Update is called once per frame
    void Update()
    {

        updateRays();

        anythingHere = collideCheckRays();

        if (anythingHere)
        {
            dodgingSomething(randomObj);
        }

        this.GetComponent<Rigidbody>().velocity = FlockingBehaviour();
        //this.transform.rotation.SetEulerRotation(0.0f, 90.0f, 0.0f);
    }

    public Vector3 FlockingBehaviour()
    {
        List<GameObject> theFlock = flocking.instance.theFlock;

        Vector3 cohesionVector = new Vector3();
        Vector3 separateVector = new Vector3();
        Vector3 alignmentVector = new Vector3();

        int count = 0;

        for (int index = 0; index < theFlock.Count; index++)
        {
            if (mID != theFlock[index].GetComponent<raycastChecking>().getID)
            {
                float distance = (transform.position - theFlock[index].transform.position).sqrMagnitude;

                if (distance > 0 && distance < mRadiusSquaredDistance)
                {
                    cohesionVector += theFlock[index].transform.position;
                    separateVector += theFlock[index].transform.position - transform.position;
                    alignmentVector += theFlock[index].transform.forward;

                    count++;
                }
            }
        }

        if (count == 0)
        {
            return Vector3.zero;
        }

        // revert vector
        // separation step
        separateVector /= count;
        separateVector *= -1;

        // forward step
        alignmentVector /= count;

        // cohesione step
        cohesionVector /= count;
        cohesionVector = (cohesionVector - transform.position);

        // Add All vectors together to get flocking
        Vector3 flockingVector = ((separateVector.normalized * flocking.instance.separationWeight) + (cohesionVector.normalized * flocking.instance.cohesionWeight) + (alignmentVector.normalized * flocking.instance.alignmentWeight));

        return flockingVector;
    }

    void updateRays()
    {
        for (int i = 0; i < rays.Length; i++)
        {
            rays[i] = new Ray(transform.position, rays[i].direction);
            Debug.DrawRay(transform.position, rays[i].direction);
        }
    }

    void dodgingSomething(GameObject objHit)
    {
        if(gameObject.transform.position.y <= objHit.transform.position.y)
        {
            float lerpMe = transform.position.y - moveIncrement;

            float newPos = Mathf.Lerp(transform.position.y, lerpMe, timeToDodge);

            this.transform.position = new Vector3(transform.position.x, newPos, transform.position.z);
        }
        else if (gameObject.transform.position.y > objHit.transform.position.y)
        {
            float lerpMe = transform.position.y + moveIncrement;

            float newPos = Mathf.Lerp(transform.position.y, lerpMe, timeToDodge);

            this.transform.position = new Vector3(transform.position.x, newPos, transform.position.z);
        }
        else if(gameObject.transform.position.x <= objHit.transform.position.x)
        {
            float lerpMe = transform.position.x - moveIncrement;

            float newPos = Mathf.Lerp(transform.position.x, lerpMe, timeToDodge);

            this.transform.position = new Vector3(newPos, transform.position.y, transform.position.z);
        }
        else if (gameObject.transform.position.x > objHit.transform.position.x)
        {
            float lerpMe = transform.position.x + moveIncrement;

            float newPos = Mathf.Lerp(transform.position.x, lerpMe, timeToDodge);

            this.transform.position = new Vector3(newPos, transform.position.y, transform.position.z);
        }
        else if(gameObject.transform.position.z <= objHit.transform.position.z)
        {
            float lerpMe = transform.position.z - moveIncrement;

            float newPos = Mathf.Lerp(transform.position.z, lerpMe, timeToDodge);

            this.transform.position = new Vector3(transform.position.x, transform.position.y, newPos);
        }
        else if (gameObject.transform.position.z > objHit.transform.position.z)
        {
            float lerpMe = transform.position.z + moveIncrement;

            float newPos = Mathf.Lerp(transform.position.z, lerpMe, timeToDodge);

            this.transform.position = new Vector3(transform.position.x, transform.position.y, newPos);
        }
        else
        {
            Debug.Log("ERROR");
        }

    }

    void randomPos()
    {
        destination = new Vector3(Random.Range(-5.0f, 6.0f), Random.Range(-2.0f, 3.7f), Random.Range(-9.0f, 9.0f));
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag != "Player")
        {
            hitCount++;
            Debug.Log(collision.gameObject.name.ToString());
        }
    }

    bool collideCheckRays()
    {
        RaycastHit hit;
        bool temp = false;
        
        for(int i = 0; i < rays.Length; i++)
        {
            temp = Physics.Raycast(rays[i], out hit, rayLength);
            
            if(temp == true)
            {
                randomObj = hit.collider.gameObject;
                break;
            }
        }
        
        //Debug.Log(hit.collider.gameObject.ToString());
        return temp;
    }

    public void SetID(int ID)
    {
        mID = ID;
    }

    public int getID
    {
        get { return mID; }
    }
}