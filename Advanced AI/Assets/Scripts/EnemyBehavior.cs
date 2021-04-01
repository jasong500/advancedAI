using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Vector2 seekDestination;
    public float maxHealth = 20f;
    public EnemyManager enemyManager;

    Vector3 lastDirection = Vector3.zero;
    bool moveDone = false;
    List<Cell> reachedPathTiles = new List<Cell>();
    List<Cell> path = new List<Cell>();
    public Transform movePoint;
    public LayerMask stopMovementMask;
    public Vector2 movement;

    //Private data
    float currentHealth;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        movePoint.parent = null;
        setPath();
    }

    // Update is called once per frame
    void Update()
    {
        SetMovementVector();

        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.fixedDeltaTime);

        if (Vector3.Distance(transform.position, movePoint.position) <= .001f)
        {
            if (Mathf.Abs(movement.x) == 1f)
            {
                // we add 0.5f to 'y' component of the 'position'
                // to account the bottom pivot point of the sprite
                if (!Physics2D.OverlapCircle(movePoint.position + new Vector3(movement.x, 0.5f, 0f), .2f, stopMovementMask))
                {
                    movePoint.position += new Vector3(movement.x, 0f, 0f);
                }
            }
            else if (Mathf.Abs(movement.y) == 1f)
            {
                // we add 0.5f to 'y' component of the 'position'
                // to account the bottom pivot point of the sprite
                if (!Physics2D.OverlapCircle(movePoint.position + new Vector3(0, movement.y + 0.5f, 0f), .2f, stopMovementMask))
                {
                    movePoint.position += new Vector3(0f, movement.y, 0f);
                }
            }
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
        path = this.GetComponent<AStarSearch>().findPath();
    }

    void SetMovementVector()
    {
        if (path != null)
        {
            if (path.Count > 0)
            {
                if (!moveDone)
                {
                    Debug.Log("Path Length: " + path.Count);
                    for (int i = 0; i < path.Count; i++)
                    {
                        if (reachedPathTiles.Contains(path[i]))
                        {
                            continue;
                        }
                        else
                        {
                            reachedPathTiles.Add(path[i]); 
                            break;
                        }
                    }

                    if(reachedPathTiles[reachedPathTiles.Count - 1] == null)
                    {
                        Debug.Log("Reached Path Tiles is null");
                    }

                    Cell wt = reachedPathTiles[reachedPathTiles.Count - 1];

                    if(wt == null)
                    {
                        Debug.Log("WT is null");
                    }
                    Debug.Log(wt.ToString() +  " = X POS");
                    Debug.Log(wt.transform.position.y +  " = Y POS");
                    Debug.Log("Outputting Math: " + Mathf.Ceil(wt.transform.position.x - transform.position.x) + " = X POS\n" + Mathf.Ceil(wt.transform.position.y - transform.position.y) + " = Y POS");
                    lastDirection = new Vector3(Mathf.Ceil(wt.transform.position.x - transform.position.x), Mathf.Ceil(wt.transform.position.y - transform.position.y), 0);
                    if (lastDirection.Equals(Vector3.up)) movement.y = 1;
                    if (lastDirection.Equals(Vector3.down)) movement.y = -1;
                    if (lastDirection.Equals(Vector3.left)) movement.x = -1;
                    if (lastDirection.Equals(Vector3.right)) movement.x = 1;
                    moveDone = true;
                }
                else
                {
                    movement = Vector2.zero;
                    if (Vector3.Distance(transform.position, movePoint.position) <= .001f)
                        moveDone = false;
                }
            }
        }
    }
}
