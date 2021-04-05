using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class Cell : MonoBehaviour
{
    public float cost = 0f;
    public bool hasTower = false;
    public GameObject myTower;
    public GridManager gridManager;
    public int myGridX;
    public int myGridY;
    int enemiesInCell = 0;

    public int EnemiesInCell { get { return enemiesInCell; } }

    private void Start()
    {
        gridManager = FindObjectOfType<GridManager>();
    }

    public void IncreaseCost(float increaseAmount)
    {
        cost += increaseAmount;

        //Update colour
        GetComponent<SpriteRenderer>().color = new Color(0f, 1f, 0.5f, Mathf.Min(1f, cost));
    }

    public void DecreaseCost(float decreaseAmount)
    {
        cost -= decreaseAmount;

        //Update color
        GetComponent<SpriteRenderer>().color = new Color(0f, 1f, 0.5f, Mathf.Min(1f, cost));

        if (cost <= 0)
        {
            cost = 0;
            GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("enemy"))
        {
            enemiesInCell++;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("enemy"))
            enemiesInCell--;
    }

    void OnDrawGizmos()
    {
        GUIStyle style = new GUIStyle(GUI.skin.label);
        style.alignment = TextAnchor.MiddleCenter;
        Handles.Label(transform.position, cost.ToString(), style);
    }

    public List<Cell> GetNeighbouringCells()
    {
        List<Cell> neighbours = new List<Cell>();
        Vector2 cellPosition2D = new Vector2(transform.position.x, transform.position.y);
        Vector2 dir;

        //Go through all the directions
        dir = new Vector2(0f, 1f);
        Cell up = gridManager.GetCellAtPos(cellPosition2D + dir);

        dir = new Vector2(0f, -1f);
        Cell down = gridManager.GetCellAtPos(cellPosition2D + dir);

        dir = new Vector2(-1f, 0f);
        Cell left = gridManager.GetCellAtPos(cellPosition2D + dir);

        dir = new Vector2(1f, 0f);
        Cell right = gridManager.GetCellAtPos(cellPosition2D + dir);

        dir = new Vector2(-1f, 1f);
        Cell topLeft = gridManager.GetCellAtPos(cellPosition2D + dir);

        dir = new Vector2(1f, 1f);
        Cell topRight = gridManager.GetCellAtPos(cellPosition2D + dir);

        dir = new Vector2(-1f, -1f);
        Cell bottomLeft = gridManager.GetCellAtPos(cellPosition2D + dir);

        dir = new Vector2(1f, -1f);
        Cell bottomRight = gridManager.GetCellAtPos(cellPosition2D + dir);

        //Add the cells to the list
        neighbours.Add(up);
        neighbours.Add(down);
        neighbours.Add(left);
        neighbours.Add(right);
        neighbours.Add(topLeft);
        neighbours.Add(topRight);
        neighbours.Add(bottomLeft);
        neighbours.Add(bottomRight);

        //Return the list
        return neighbours;
    }
}
