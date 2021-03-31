using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [HideInInspector] public float lifeSpanSeconds = 1.5f;
    [HideInInspector] public float damageValue = 5f;
    [HideInInspector] public float moveSpeed = 5f;

    //Private data
    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        StartCoroutine(CountdownLife());
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = transform.up * moveSpeed;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("enemy"))
        {
            col.GetComponent<EnemyBehavior>().DealDamage(damageValue);
            Destroy(gameObject);
        }
    }

    IEnumerator CountdownLife()
    {
        yield return new WaitForSeconds(lifeSpanSeconds);

        Destroy(gameObject);
    }
}
