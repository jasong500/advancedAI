using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    //public vars
    public float moveSpeed = 5f;
    public float maxHealth = 30f;
    public EnemyManager enemyManager;
    //[HideInInspector]
    public Vector2 seekDestination;

    //private vars
    Cell startCell;
    Cell finishCell;
    float currentHealth;
    List<Cell> myPath;
    Cell currentSeekingCell;
    bool foundPath = false;
    bool reachedDest = false;
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        myPath = new List<Cell>();
        currentSeekingCell = null;
        myPath = new List<Cell>();

        FindPathAStar();
    }

    // Update is called once per frame
    void Update()
    {
        if (foundPath && !reachedDest)
            FollowPath();
    }

    public void DealDamage(float damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0f)
        {
            enemyManager.MarkEnemyDead();
            Destroy(gameObject);
        }
    }

    public void SetStartFinish(Cell start, Cell finish)
    {
        startCell = start;
        finishCell = finish;
    }

    void FindPathAStar()
    {
        //Open and closed lists
        List<Cell> openList = new List<Cell>();
        Dictionary<Cell, Cell> cameFrom = new Dictionary<Cell, Cell>();
        Dictionary<Cell, float> costSoFar = new Dictionary<Cell, float>();

        //Starting node on the open list
        openList.Add(startCell);
        cameFrom.Add(startCell, null);
        costSoFar.Add(startCell, 0);

        Cell currentNode = null;

        while (openList.Count > 0)
        {
            //Starting node (front of openList)
            currentNode = openList[0];
            openList.RemoveAt(0);

            //If we found the end node
            if (currentNode == finishCell)
            {
                //Temp node
                Cell tempNode = currentNode;

                //"Trace" the path
                while (tempNode != finishCell)
                {
                    //Add it to the path
                    myPath.Add(tempNode);

                    //Get where the temp node came from
                    tempNode = cameFrom[tempNode];
                }

                //Early exit
                foundPath = true;
                return;
            }

            //Get the neighbours and add them to the cameFrom dictionary
            List<Cell> neighborCells = currentNode.GetNeighbouringCells();
            foreach (Cell neighbour in neighborCells)
            {
                //Calculate cost - costSoFar + neighbourCost + heuristicCost
                float heuristicCost = (finishCell.transform.position - neighbour.transform.position).magnitude;
                float estimatedCost = costSoFar[currentNode] + neighbour.cost + heuristicCost;

                //If there is not cost for the neighbour
                if (!costSoFar.ContainsKey(neighbour))
                {
                    //Set the new cost
                    costSoFar.Add(neighbour, estimatedCost);

                    //Add the neighbour to the openList
                    openList.Add(neighbour);

                    //Set where it came from
                    cameFrom.Add(neighbour, currentNode);
                }
                else if (estimatedCost < costSoFar[neighbour])  //If we found a cheaper cost
                {
                    //Set the new cost
                    costSoFar[neighbour] = estimatedCost;

                    //Add the neighbour to the openList
                    openList.Add(neighbour);

                    //Set where it came from
                    cameFrom[neighbour] = currentNode;
                }
            }
        }
    }

    void FollowPath()
    {
        if(myPath.Count - 1 >= 0)
        {
            //Set the current seek node
            currentSeekingCell = myPath[myPath.Count - 1];

            //Move towards it
            rb.velocity = (currentSeekingCell.transform.position - transform.position).normalized * moveSpeed;

            //If close enough, continue
            if (Vector2.Distance(transform.position, currentSeekingCell.transform.position) <= 0.5f)
            {
                //Check to see if the enemy has reached the destination
                if (myPath.Count == 1)
                {
                    //Game Over
                    //enemyManager.GameOver();
                    enemyManager.MarkEnemyDead();
                    Destroy(gameObject);
                    reachedDest = true;
                    enemyManager.subtractLife();
                    return;
                }

                //Done with this node
                myPath.RemoveAt(myPath.Count - 1);
            }
        }
        else
        {
            Debug.Log("Path Length: " + myPath.Count);
        }
        
    }
}