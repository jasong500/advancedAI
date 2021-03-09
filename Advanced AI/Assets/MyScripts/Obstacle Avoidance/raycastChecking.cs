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

    Vector3 destination;
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
        if (gameObject.transform.position.y <= objHit.transform.position.y)
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
        else if (gameObject.transform.position.x <= objHit.transform.position.x)
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
        else if (gameObject.transform.position.z <= objHit.transform.position.z)
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

    bool collideCheckRays()
    {
        RaycastHit hit;
        bool temp = false;

        for (int i = 0; i < rays.Length; i++)
        {
            temp = Physics.Raycast(rays[i], out hit, rayLength);

            if (temp == true)
            {
                randomObj = hit.collider.gameObject;
                break;
            }
        }

        //Debug.Log(hit.collider.gameObject.ToString());
        return temp;
    }
}