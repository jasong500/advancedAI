using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [HideInInspector] public GameObject projectilePrefab;
    [HideInInspector] public float projectileMoveSpeed = 5f;

    [HideInInspector] public float projectileLifeSpan = 1.5f;
    [HideInInspector] public float damagePerProjectile = 5f;
    [HideInInspector] public float fireRatePerSec = 2f;

    //Cells
    List<Cell> cellsInRange;
    Cell mostInfectedCell;

    //Helper data
    Vector2 towerPos;
    bool canFire = true;
    bool enemyInCell = false;

    // Start is called before the first frame update
    void Start()
    {
        //Convert the position to 2D
        towerPos = new Vector2(transform.position.x, transform.position.y);
    }

    // Update is called once per frame
    void Update()
    {
        //Check if there is a enemy in a cell
        if (mostInfectedCell.EnemiesInCell > 0)
            enemyInCell = true;
        else
            enemyInCell = false;

        //Fire on a cooldown
        if (enemyInCell && canFire)
            StartCoroutine(AttackCell(mostInfectedCell));

        //Find a new cell with the highest enemy count
        foreach (Cell cell in cellsInRange)
        {
            //If a cell has a enemy in it
            if (cell.EnemiesInCell > 0)
            {
                //See if it is greater than the highest enemy count cell
                if (cell.EnemiesInCell > mostInfectedCell.EnemiesInCell)
                {
                    //Update the most infected cell
                    mostInfectedCell = cell;
                }
            }
        }
    }

    public void SetCellsInRange(Collider2D[] colliders)
    {
        cellsInRange = new List<Cell>();    //Default so it is not null
        cellsInRange.Clear();

        foreach(Collider2D col in colliders)
        {
            Cell cell = col.GetComponent<Cell>();
            cellsInRange.Add(cell);
            mostInfectedCell = cell;
        }
    }

    IEnumerator AttackCell(Cell target)
    {
        Vector2 cellPos = new Vector2(target.transform.position.x, target.transform.position.y);
        Vector2 fireDirection = cellPos - towerPos;
        float rotationAngle = Mathf.Atan2(fireDirection.y, fireDirection.x) * Mathf.Rad2Deg - 90f;

        //Spawn a projectile
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.Euler(0f, 0f, rotationAngle));
        projectile.GetComponent<Projectile>().lifeSpanSeconds = projectileLifeSpan;
        projectile.GetComponent<Projectile>().damageValue = damagePerProjectile;
        projectile.GetComponent<Projectile>().moveSpeed = projectileMoveSpeed;

        //Cannot fire
        canFire = false;

        //Cooldown
        yield return new WaitForSeconds(1f/fireRatePerSec);

        //Can fire
        canFire = true;
    }
}
