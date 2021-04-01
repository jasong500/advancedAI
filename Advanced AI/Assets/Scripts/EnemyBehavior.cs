using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Cell seekDestination;
    public float maxHealth = 20f;
    public EnemyManager enemyManager;
    public GridManager gridManager;

    Vector3 lastDirection = Vector3.zero;
    bool moveDone = false;
    List<Cell> reachedPathTiles = new List<Cell>();
    List<Cell> path = new List<Cell>();
    public Vector2 movePoint;
    public LayerMask stopMovementMask;
    public Vector2 movement;

    //Private data
    float currentHealth;
    int index;

    // Start is called before the first frame update
    void Start()
    {
        gridManager = GameObject.FindObjectOfType<GridManager>();
        currentHealth = maxHealth;
        seekDestination = gridManager.GetCellWorldPosEnemy(this.transform.position);
        index = 0;
        setPath();
    }

    // Update is called once per frame
    void Update()
    {
        if(path != null)
        {
            if(path.Count <= 0)
            {
                Debug.Log("Path[0] Is Null");
            }
            else
            {
                movePoint = path[index].transform.position;
            }
        }

        if (Vector2.Distance(this.transform.position, movePoint) >= 10.0f)
        {
            Debug.Log("Path Count: " + path.Count);
            transform.position = Vector3.MoveTowards(transform.position, movePoint, moveSpeed * Time.fixedDeltaTime);
        }
        else if (path[index + 1] != seekDestination)
        {
            index++;
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, seekDestination.transform.position, moveSpeed * Time.fixedDeltaTime);
        }
    }

    public void DealDamage(float damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0f)
        {
            enemyManager.MarkZombieDead();
            Destroy(gameObject);
        }
    }

    void setPath()
    {
        Vector2 tempStart = new Vector2(this.transform.position.x, this.transform.position.y);
        Cell tempStartCell = gridManager.GetCellWorldPosEnemy(tempStart);
        Cell tempEndCell = seekDestination;
        path = this.GetComponent<AStarSearch>().findPath(tempStartCell, tempEndCell);
    }

    void SetMovementVector()
    {
        //if (path != null)
        //{
        //    if (path.Count > 0)
        //    {
        //        if (!moveDone)
        //        {
        //            Debug.Log("Path Length: " + path.Count);
        //            for (int i = 0; i < path.Count; i++)
        //            {
        //                if (reachedPathTiles.Contains(path[i]))
        //                {
        //                    continue;
        //                }
        //                else
        //                {
        //                    reachedPathTiles.Add(path[i]); 
        //                    break;
        //                }
        //            }

        //            if(reachedPathTiles[reachedPathTiles.Count - 1] == null)
        //            {
        //                Debug.Log("Reached Path Tiles is null");
        //            }

        //            Cell wt = reachedPathTiles[reachedPathTiles.Count - 1];

        //            if(wt == null)
        //            {
        //                Debug.Log("WT is null");
        //            }
        //            Debug.Log(wt.ToString() +  " = X POS");
        //            Debug.Log(wt.transform.position.y +  " = Y POS");
        //            Debug.Log("Outputting Math: " + Mathf.Ceil(wt.transform.position.x - transform.position.x) + " = X POS\n" + Mathf.Ceil(wt.transform.position.y - transform.position.y) + " = Y POS");
        //            lastDirection = new Vector3(Mathf.Ceil(wt.transform.position.x - transform.position.x), Mathf.Ceil(wt.transform.position.y - transform.position.y), 0);
        //            if (lastDirection.Equals(Vector3.up)) movement.y = 1;
        //            if (lastDirection.Equals(Vector3.down)) movement.y = -1;
        //            if (lastDirection.Equals(Vector3.left)) movement.x = -1;
        //            if (lastDirection.Equals(Vector3.right)) movement.x = 1;
        //            moveDone = true;
        //        }
        //        else
        //        {
        //            movement = Vector2.zero;
        //            if (Vector3.Distance(transform.position, movePoint.position) <= .001f)
        //                moveDone = false;
        //        }
    //        }
    //    }
    }
}
