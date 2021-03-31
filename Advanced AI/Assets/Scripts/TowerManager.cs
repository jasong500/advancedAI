using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TowerManager : MonoBehaviour
{
    enum TowerType { basicTower, notBasicTower }
    TowerType currentTower;

    [Header("Costs")]
    public int basicTowerCost = 500;
    public int notBasicTowerCost = 1500;
    public int startingCash = 5000;

    [Header("Default")]
    public GameObject basicTowerTower;
    public GameObject basicTowerProjectile;
    public float basicTowerProjectileMoveSpeed = 5f;
    [Range(0.5f, 5.0f)] public float basicTowerRange = 2.5f;
    public float basicTowerProjectileLifeSpan = 1.5f;
    public float basicTowerDamagePerProjectile = 5f;
    public float basicTowerFireRatePerSec = 2f;

    [Header("Not Default")]
    public GameObject notBasicTowerTower;
    public GameObject notBasicTowerProjectile;
    public float notBasicTowerProjectileMoveSpeed = 7.5f;
    [Range(0.5f, 5.0f)] public float notBasicTowerRange = 4f;
    public float notBasicTowerProjectileLifeSpan = 1.5f;
    public float notBasicTowerDamagePerProjectile = 2f;
    public float notBasicTowerFireRatePerSec = 5f;

    [Header("UI")]
    public TextMeshProUGUI currentTowerText;
    public TextMeshProUGUI cashAmountText;

    //Private data
    [HideInInspector] public int currentCash;
    GridManager gridManager;

    // Start is called before the first frame update
    void Start()
    {
        currentCash = startingCash;
        cashAmountText.text = "$" + currentCash;

        //Current tower
        currentTower = TowerType.basicTower;
        currentTowerText.text = "Build Tower Type: Basic";

        gridManager = FindObjectOfType<GridManager>();
    }

    // Update is called once per frame
    void Update()
    {
        //GET KEY DOWN - KEYCODE TAB - Change tower type
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (currentTower == TowerType.basicTower)
            {
                currentTower = TowerType.notBasicTower;
                currentTowerText.text = "Build Tower Type: Not Basic";
            }
            else if (currentTower == TowerType.notBasicTower)
            {
                currentTower = TowerType.basicTower;
                currentTowerText.text = "Build Tower Type: Basic";
            }
        }

        //GET MOUSE BUTTON DOWN - 0 - Place tower
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 spawnPos = gridManager.GetCellWorldPos(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            PlaceTower(spawnPos);
        }
    }

    void PlaceTower(Vector2 pos)
    {
        //Check current tower and cost
        int cost;

        switch (currentTower)
        {
            case TowerType.basicTower:
                cost = basicTowerCost;
                break;

            case TowerType.notBasicTower:
                cost = notBasicTowerCost;
                break;

            default:
                cost = 99999999;
                break;
        }

        //If they have enough to place a tower
        if (currentCash >= cost)
        {
            //Check which tower
            switch (currentTower)
            {
                case TowerType.basicTower:
                {
                    //Place it here
                    GameObject basicTower = Instantiate(basicTowerTower, pos, Quaternion.identity, transform);

                    //Update influence map
                    gridManager.ApplyInfluence(pos, basicTowerRange, 1f);

                    //Set data
                    basicTower.GetComponent<Tower>().projectilePrefab = basicTowerProjectile;
                    basicTower.GetComponent<Tower>().projectileMoveSpeed = basicTowerProjectileMoveSpeed;
                    basicTower.GetComponent<Tower>().projectileLifeSpan = basicTowerProjectileLifeSpan;
                    basicTower.GetComponent<Tower>().damagePerProjectile = basicTowerDamagePerProjectile;
                    basicTower.GetComponent<Tower>().fireRatePerSec = basicTowerFireRatePerSec;

                    //Set the cells the tower has influence over
                    basicTower.GetComponent<Tower>().SetCellsInRange(Physics2D.OverlapCircleAll(pos, basicTowerRange));

                    //Pay for it you greedy mf
                    currentCash -= cost;
                    cashAmountText.text = "$" + currentCash;
                }
                    break;
                case TowerType.notBasicTower:
                {
                    //Place it here
                    GameObject notBasicTower = Instantiate(notBasicTowerTower, pos, Quaternion.identity, transform);

                    //Update influence map
                    gridManager.ApplyInfluence(pos, notBasicTowerRange, 1f);

                    //Set data
                    notBasicTower.GetComponent<Tower>().projectilePrefab = notBasicTowerProjectile;
                    notBasicTower.GetComponent<Tower>().projectileMoveSpeed = notBasicTowerProjectileMoveSpeed;
                    notBasicTower.GetComponent<Tower>().projectileLifeSpan = notBasicTowerProjectileLifeSpan;
                    notBasicTower.GetComponent<Tower>().damagePerProjectile = notBasicTowerDamagePerProjectile;
                    notBasicTower.GetComponent<Tower>().fireRatePerSec = notBasicTowerFireRatePerSec;

                    //Set the cells the tower has influence over
                    notBasicTower.GetComponent<Tower>().SetCellsInRange(Physics2D.OverlapCircleAll(pos, notBasicTowerRange));

                    //Pay for it you greedy mf
                    currentCash -= cost;
                    cashAmountText.text = "$" + currentCash;
                }
                    break;
                
                default:
                    Debug.LogError("No such tower!");
                    break;
            }
        }
    }

    void SellTower(Vector2 pos)
    {

    }
}
