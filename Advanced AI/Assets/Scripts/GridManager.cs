using UnityEngine;

public class GridManager : MonoBehaviour
{
    public GameObject cellPrefab;

    [Header("Grid Data")]
    public Vector2Int gridSize;
    [Range(0.1f, 0.5f)] public float cellSpacing = 0.1f;

    //Grid
    Cell[,] grid;

    // Start is called before the first frame update
    void Start()
    {
        CreateGrid();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void CreateGrid()
    {
        grid = new Cell[gridSize.x, gridSize.y];

        //For through the 2D array
        for (int i = 0; i < gridSize.x; i++)
        {
            for (int j = 0; j < gridSize.y; j++)
            {
                //Spawn cell at (i, j) with offset
                Vector2 spawnPos = new Vector2(i + cellSpacing, j + cellSpacing);
                GameObject newCell = Instantiate(cellPrefab, spawnPos, Quaternion.identity, transform);
                newCell.GetComponent<Cell>().cost = 0;
                newCell.GetComponent<Cell>().gridX = i;
                newCell.GetComponent<Cell>().gridY = j;
                grid[i, j] = newCell.GetComponent<Cell>();
            }
        }
    }

    public Vector2 GetCellWorldPos(Vector2 clickPosition)
    {
        float percentX = clickPosition.x / (gridSize.x);
        float percentY = clickPosition.y / (gridSize.y);

        //Clamp values from 0 to 1
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.Clamp(Mathf.FloorToInt((gridSize.x) * percentX), 0, gridSize.x - 1);
        int y = Mathf.Clamp(Mathf.FloorToInt((gridSize.y) * percentY), 0, gridSize.y - 1);

        return grid[x, y].transform.position;
    }

    public Cell GetCellWorldPosEnemy(Vector2 enemyPos)
    {
        float percentX = enemyPos.x / (gridSize.x);
        float percentY = enemyPos.y / (gridSize.y);

        //Clamp values from 0 to 1
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.Clamp(Mathf.FloorToInt((gridSize.x) * percentX), 0, gridSize.x - 1);
        int y = Mathf.Clamp(Mathf.FloorToInt((gridSize.y) * percentY), 0, gridSize.y - 1);

        return grid[x, y];
    }

    public Cell GetWorldTileByCellPosition(Vector3 worldPosition)
    {
        Vector2 cellPosition = GetCellWorldPos(worldPosition);
        Cell wt = null;
        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                if (grid[x, y] != null)
                {
                    Cell _wt = grid[x, y].GetComponent<Cell>();

                    // we are interested in walkable cells only
                    if (_wt.walkable && _wt.cellX == cellPosition.x && _wt.cellY == cellPosition.y)
                    {
                        wt = _wt;
                        break;
                    }
                    else
                    {
                        continue;
                    }
                }
            }
        }
        return wt;
    }

    public void ApplyInfluence(Vector2 origin, float range, float maxInfluence)
    {
        //Get all the cells in the radius
        Collider2D[] cells = Physics2D.OverlapCircleAll(origin, range);

        if(cells != null)
        {
            //Apply influence (decreases based on distance from origin)
            for (int i = 0; i < cells.Length; i++)
            {
                //Calculate distance away and corresponding influence to add (linearly decreases)
                Vector2 pos2D = new Vector2(cells[i].transform.position.x, cells[i].transform.position.y);
                float distanceFromOrigin = (pos2D - origin).magnitude;
                float increaseAmount = maxInfluence - (maxInfluence * (distanceFromOrigin / range));

                cells[i].GetComponent<Cell>().IncreaseCost(Mathf.Abs(increaseAmount));  //Absolute value in case it goes negative
            }
        }
        else
        {
            Debug.Log("No cells in range????");
        }

    }

    public void RemoveInfluence(Vector2 origin, float range, float maxInfluence)
    {
        //Get all the cells in the radius
        Collider2D[] cells = Physics2D.OverlapCircleAll(origin, range);

        if (cells != null)
        {
            //Apply influence (decreases based on distance from origin)
            for (int i = 0; i < cells.Length; i++)
            {
                //Calculate distance away and corresponding influence to add (linearly decreases)
                Vector2 pos2D = new Vector2(cells[i].transform.position.x, cells[i].transform.position.y);
                float distanceFromOrigin = (pos2D - origin).magnitude;
                float decreaseAmount = maxInfluence - (maxInfluence * (distanceFromOrigin / range));

                cells[i].GetComponent<Cell>().DecreaseCost(Mathf.Abs(decreaseAmount));  //Absolute value in case it goes negative
            }
        }
        else
        {
            Debug.Log("No cells in range");
        }

    }
}
