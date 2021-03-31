using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Vector2 seekDestination;
    public float maxHealth = 20f;
    public EnemyManager enemyManager;

    //Private data
    float currentHealth;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
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
}
