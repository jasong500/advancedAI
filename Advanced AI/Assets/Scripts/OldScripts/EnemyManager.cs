using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    [Range(1.5f, 5f)] public float enemyMinSpeed = 1.5f;
    [Range(1.5f, 5f)] public float enemyMaxSpeed = 5f;
    [Range(15f, 50f)] public float enemyMaxHealth = 50f;
    public Vector2 homeBase;
    public Vector2 boundsX;
    public Vector2 boundsY;

    [Header("Enemy Waves")]
    public int enemiesPerWave = 20;
    public float waveSpawnMultiplier = 1.25f;
    public float timeToStartWave = 2f;
    public TextMeshProUGUI currentWaveText;

    [Header("Misc.")]
    public int profitPerKill = 250;
    public int profitPerWave = 1500;

    //Helper data
    int currentWave;
    int numOfEnemiesAlive;
    int numOfEnemiesToSpawn;
    int numOfEnemiesPrevWave;
    TowerManager towerManager;
    GridManager gridManager;

    // Start is called before the first frame update
    void Start()
    {
        currentWave = 0;
        currentWaveText.text = "Wave: " + currentWave;

        numOfEnemiesPrevWave = (int)(enemiesPerWave / waveSpawnMultiplier);

        towerManager = FindObjectOfType<TowerManager>();

        gridManager = FindObjectOfType<GridManager>();

        StartCoroutine(StartNextWave());
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void subtractLife()
    {
        gridManager.subtractLife();
    }

    void SpawnEnemies()
    {
        for (int i = 0; i < numOfEnemiesToSpawn; i++)
        {
            //Data for the enemy
            float moveSpeed = Random.Range(enemyMinSpeed, enemyMaxSpeed);
            Vector2 spawnPos = gridManager.GetCellWorldPos(new Vector2(Random.Range(boundsX.x, boundsX.y), Random.Range(boundsY.x, boundsY.y)));

            //Spawn enemy
            GameObject enemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity, transform);

            //Set data
            enemy.GetComponent<EnemyBehavior>().moveSpeed = moveSpeed;
            enemy.GetComponent<EnemyBehavior>().seekDestination = homeBase;
            enemy.GetComponent<EnemyBehavior>().maxHealth = enemyMaxHealth;
            enemy.GetComponent<EnemyBehavior>().enemyManager = this;
            Debug.Log("My Destination is Grid: " + gridManager.GetCellAtPos(homeBase).myGridX + ", " + gridManager.GetCellAtPos(homeBase).myGridY);
            enemy.GetComponent<EnemyBehavior>().SetStartFinish(gridManager.GetCellAtPos(spawnPos), gridManager.GetCellAtPos(homeBase));

            numOfEnemiesAlive++;
        }

        numOfEnemiesPrevWave = numOfEnemiesAlive;
    }

    public void MarkEnemyDead()
    {
        numOfEnemiesAlive--;
        towerManager.currentCash += profitPerKill;

        //Check if there are no enemies left
        if (numOfEnemiesAlive <= 0)
        {
            //Start next wave
            StartCoroutine(StartNextWave());
        }
    }

    IEnumerator StartNextWave()
    {
        towerManager.currentCash += profitPerWave;

        //Wait
        yield return new WaitForSeconds(timeToStartWave);

        //Start next wave
        currentWave++;
        currentWaveText.text = "Wave: " + currentWave;
        numOfEnemiesToSpawn = (int)(numOfEnemiesPrevWave * waveSpawnMultiplier);
        SpawnEnemies();
    }

}
