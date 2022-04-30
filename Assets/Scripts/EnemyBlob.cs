using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBlob : AEnemy
{
    [SerializeField] float speed;
    [SerializeField] int maxHealth;
    [SerializeField] private int currentHealth;

    [SerializeField] float distanceToWake;

    private GameObject player;

    private float initialHeight;

    void Start()
    {
        Speed = speed;
        Health = maxHealth;
        currentHealth = maxHealth;

        player = GameObject.FindGameObjectWithTag("Player");

        initialHeight = transform.position.y;
    }

    private void Update()
    {

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, Mathf.NegativeInfinity, initialHeight), transform.position.z);

        HealthBarFiller(currentHealth);
    }

    private void FixedUpdate()
    {
        DistanceToWake(player, distanceToWake);
    }

    protected override void LostHealth()
    {
        currentHealth--;

        if (currentHealth <= 0) { Die(); }
    }

    protected override void Die()
    {
        Destroy(gameObject);
    }
    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.gameObject.CompareTag("Player")) { return; }

    //    if (collision.gameObject.GetComponent<ArrowBehaviour>())
    //    {
    //        LostHealth();
    //    }      
    //}
}
