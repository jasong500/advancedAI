using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class Cell : MonoBehaviour
{
    public float cost = 0f;

    //Enemies in the cell
    int enemiesInCell = 0;

    public int gCost;
    public int hCost;
    public int gridX, gridY, cellX, cellY;
    public bool walkable = true;
    public List<Cell> myNeighbours;
    public Cell parent;

    public int EnemiesInCell { get { return enemiesInCell; } }

    public Cell(bool _walkable, int _gridX, int _gridY)
    {
        walkable = _walkable;
        gridX = _gridX;
        gridY = _gridY;
    }

    public int fCost
    {
        get
        {
            return gCost + hCost;
        }
    }

    public void IncreaseCost(float increaseAmount)
    {
        cost += increaseAmount;

        //Update colour
        GetComponent<SpriteRenderer>().color = new Color(0f, 1f, 0f, Mathf.Min(1f, cost));
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
}
