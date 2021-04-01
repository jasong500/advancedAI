using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarSearch : MonoBehaviour
{
    public GridManager manager;
    Cell startPos;
    Cell endPos;

    private void Start()
    {

        GameObject findManager = GameObject.FindGameObjectWithTag("GameController");
        manager = findManager.GetComponent<GridManager>();

        if(manager == null)
        {
            Debug.Log("FUCK, GRID MANAGER IS NULL");
        }
    }

    public  List<Cell> findPath(Cell startPos, Cell endPos)
    {
        return FindPath(startPos, endPos);
    }

    List<Cell> FindPath(Cell startPosition, Cell endPosition)
    {
        Cell startNode = startPosition;
        Cell targetNode = endPosition;

        List<Cell> openSet = new List<Cell>();
        List<Cell> closedSet = new List<Cell>();
        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            Cell currentNode = openSet[0];

            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].cost < currentNode.cost || openSet[i].cost == currentNode.cost)
                {
                    currentNode = openSet[i];
                }
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            if (currentNode == targetNode)
            {
                RetracePath(startNode, targetNode);
                return closedSet;
            }

            foreach (Cell neighbour in currentNode.myNeighbours)
            {
                if (!neighbour.walkable || closedSet.Contains(neighbour)) continue;

                int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
                if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                {
                    neighbour.cost = newMovementCostToNeighbour;
                    neighbour.cost = GetDistance(neighbour, targetNode);
                    neighbour.parent = currentNode;

                    if (!openSet.Contains(neighbour))
                    {
                        openSet.Add(neighbour);
                    }
                }
            }
        }
        return closedSet;
    }

    List<Cell> RetracePath(Cell startNode, Cell targetNode)
    {
        List<Cell> path = new List<Cell>();
        Cell currentNode = targetNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }

        path.Reverse();
        return path;
    }

    int GetDistance(Cell nodeA, Cell nodeB)
    {
        int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if (dstX > dstY)
            return 14 * dstY + 10 * (dstX - dstY);
        return 14 * dstX + 10 * (dstY - dstX);
    }
}
