using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class Cell : MonoBehaviour
{
    public float cost = 0f;

    //Enemies in the cell
    int enemiesInCell = 0;
    public int EnemiesInCell {  get { return enemiesInCell; } }

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
