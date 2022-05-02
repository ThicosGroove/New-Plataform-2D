using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBat : AEnemy
{
    [SerializeField] float _patrolSpeed;
    [SerializeField] float _chaseSpeed;
    [SerializeField] float maxHealth;
    [SerializeField] private float currentHealth;

    [SerializeField] private float distanceToWake;

    private GameObject player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        patrolSpeed = _patrolSpeed;
        chaseSpeed = _chaseSpeed;
        Health = maxHealth;
        currentHealth = maxHealth;
    }

    private void Update()
    {
        HealthBarFiller(currentHealth, 6f);
    }

    private void FixedUpdate()
    {
        MovementTowardsPlayer(player);
        DistanceToWake(player, distanceToWake);
    }

    protected override void Die()
    {
        Destroy(gameObject, 0.5f);
    }

    protected override void LostHealth()
    {
        currentHealth--;

        if (currentHealth <= 0) { Die(); }
    }
}
