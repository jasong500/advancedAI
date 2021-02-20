﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveRight : MonoBehaviour
{
    public float moveModMax;
    [HideInInspector]
    public float moveMod;
    public float moveModMin;

    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        moveMod = Random.Range(moveModMin, moveModMax);
        moveMod /= 100;
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position = new Vector3(gameObject.transform.position.x - moveMod, gameObject.transform.position.y, gameObject.transform.position.z);

        if(gameObject.transform.position.x <= -2.4f || gameObject.transform.position.x >= 3.0f)
        {
            gameObject.GetComponentInParent<spawnManager>().x--;
            Destroy(gameObject);            
        }    
    }

    void onCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Something")
        {
            Physics.IgnoreCollision(collision.collider, gameObject.GetComponent<BoxCollider>());
        }
    }
}