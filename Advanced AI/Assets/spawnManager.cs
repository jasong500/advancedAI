using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnManager : MonoBehaviour
{
    public int maxObjects;
    public GameObject obstacle1;
    public GameObject obstacle2;
    public Transform spawn1;
    public Transform spawn2;
    public Transform spawn3;
    public Transform spawn4;
    public Transform spawn5;
    public Transform spawn6;
    public Transform spawn7;
    public Transform spawn8;

    GameObject player;
    GameObject obstacle;

    [HideInInspector]
    public int x = 0;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        while(x < maxObjects)
        {
            int temp = 0;
            Transform spawnPoint;

            int obj = Random.Range(1, 100);

            if (obj % 2 == 0)
            {
                obstacle = obstacle2;
            }
            else
            {
                obstacle = obstacle1;
            }

            if (player.transform.position.y < -0.4)
            {
                temp = Random.Range(5,9);
            }
            else if (player.transform.position.y > 0.4)
            {
                temp = Random.Range(1, 3);
            }
            else
            {
                temp = Random.Range(3, 5);
            }

            switch (temp)
            {
                case 1: 
                    spawnPoint = spawn1;
                    break;
                case 2:
                    spawnPoint = spawn2;
                    break;
                case 3:
                    spawnPoint = spawn3;
                    break;
                case 4:
                    spawnPoint = spawn4;
                    break;
                case 5:
                    spawnPoint = spawn5;
                    break;
                case 6:
                    spawnPoint = spawn6;
                    break;
                case 7:
                    spawnPoint = spawn7;
                    break;
                case 8:
                    spawnPoint = spawn8;
                    break;
                default:
                    spawnPoint = spawn1;
                    break;
            }
            Instantiate(obstacle, spawnPoint);
            x++;
        }
    }
}
