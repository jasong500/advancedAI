using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class raycastChecking : MonoBehaviour
{

    public Rigidbody rb;
    public TextMeshProUGUI hitCountT;
    public TextMeshProUGUI dodgeCountT;
    public TextMeshProUGUI modeT;
    public float moveIncrement;
    public float timeToDodge;
    public bool anythingHere;
    public float sphereSize;
    public float rayLength;
    public Material mat;

    private Transform shape;
    public float range = 10; // range of the capsule cast
    private float freeDistance = 0;

    int hitCount = 0;
    int dodgeCount = 0;
    GameObject randomObj;
    bool visible = false;
    bool sphereCheck = false;
    Ray raycast;
    Ray[] rays;

    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();

        rays = new Ray[9];
        rays[0] = new Ray(transform.position, new Vector3(rayLength, 0.5f, 0.0f));
        rays[1] = new Ray(transform.position, new Vector3(rayLength, 0.375f, 0.0f));
        rays[2] = new Ray(transform.position, new Vector3(rayLength, 0.25f, 0.0f));
        rays[3] = new Ray(transform.position, new Vector3(rayLength, 0.125f, 0.0f));
        rays[4] = new Ray(transform.position, new Vector3(rayLength, 0.0f, 0.0f));
        rays[5] = new Ray(transform.position, new Vector3(rayLength, -0.125f, 0.0f));
        rays[6] = new Ray(transform.position, new Vector3(rayLength, -0.25f, 0.0f));
        rays[7] = new Ray(transform.position, new Vector3(rayLength, -0.375f, 0.0f));
        rays[8] = new Ray(transform.position, new Vector3(rayLength, -0.5f, 0.0f));
    }

    // Update is called once per frame
    void Update()
    {

        if (sphereCheck)
        {
            RaycastHit hit;
            float radius = sphereSize;
            float height = 1.0f / 2 - radius;
            Vector3 p1 = transform.position;
            Vector3 p2 = p1;
            p2.y += height;
            p1.y -= height;
            RenderVolume(p1, p2, radius, transform.forward, range);
            if (Physics.CapsuleCast(p1, p2, radius, transform.forward, out hit, range))
            {
                // if some obstacle inside range
                randomObj = hit.collider.gameObject;
                dodgingSomething(randomObj);
            }
        }
        else
        {
            updateRays();

            anythingHere = collideCheckRays();
        }

        if (anythingHere)
        {
            dodgingSomething(randomObj);
        }

        if (!visible)
        {
            HideVolume();
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            if (visible)
            {
                visible = false;
                sphereCheck = false;
                modeT.text = "Ray";
            }
            else
            {
                visible = true;
                sphereCheck = true;
                modeT.text = "Capsule";
            }
        }
        
        if (Input.GetKeyDown(KeyCode.X))
        {
            if (sphereCheck)
            {
                sphereCheck = false;
            }
            else
            {
                sphereCheck = true;
            }
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
        dodgeCount++;
        dodgeCountT.text = dodgeCount.ToString();

        if(gameObject.transform.position.y >= 1.008f || gameObject.transform.position.y <= -1.1f)
        {
            if(gameObject.transform.position.y >= 1.008f)
            {
                gameObject.transform.position = new Vector3(gameObject.transform.position.x, 0.0f, transform.position.z);
            }
            else
            {
                gameObject.transform.position = new Vector3(gameObject.transform.position.x, 0.0f, transform.position.z);
            }
        }
        else if(gameObject.transform.position.y <= objHit.transform.position.y)
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
        else
        {
            Debug.Log("ERROR");
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        hitCount++;
        hitCountT.text = hitCount.ToString();
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

    bool collideCheckSphere()
    {
        Collider[] cols;
        cols = Physics.OverlapSphere(transform.position, sphereSize);

        bool temp = false;

        if(cols.Length <= 0)
        {
            temp = true;
        }

        return temp;
    }

    GameObject collideCheckObject()
    {
        RaycastHit hit;
        bool temp = Physics.SphereCast(transform.position, range, transform.forward, out hit);

        return hit.collider.gameObject;
    }

    void RenderVolume(Vector3 p1, Vector3 p2, float radius, Vector3 dir, float distance)
    {
        if (!shape)
        {
            shape = GameObject.CreatePrimitive(PrimitiveType.Sphere).transform;
            shape.GetComponent<Renderer>().material = mat;
            Destroy(shape.GetComponent<Collider>());
        }

        Vector3 scale;
        float diam = 2 * radius;
        scale.x = diam;
        scale.y = Vector3.Distance(p2, p1) + diam;
        scale.z = distance + diam;
        shape.localScale = scale;
        shape.position = (p1 + p2 + dir.normalized * distance) / 2;
        shape.position = new Vector3(shape.position.x, shape.position.y, 0.0f);
        shape.rotation = Quaternion.LookRotation(dir, p2 - p1);
        shape.GetComponent<Renderer>().enabled = true; // show it
    }

    void HideVolume()
    { // hide the volume
        if (shape) shape.GetComponent<Renderer>().enabled = false;
    }
}