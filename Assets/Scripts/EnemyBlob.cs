using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBlob : AEnemy
{
    [SerializeField] float _patrolSpeed;
    [SerializeField] float _chaseSpeed;
    [SerializeField] float maxHealth;
    [SerializeField] private float currentHealth;

    [SerializeField] float distanceToWake;

    private GameObject player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        state = State.Patrol;

        patrolSpeed = _patrolSpeed;
        chaseSpeed = _chaseSpeed;
        Health = maxHealth;
        currentHealth = maxHealth;
    }

    private void Update()
    {           
        HealthBarFiller(currentHealth, 4f);
    }

    private void FixedUpdate()
    {
        switch (state)
        {
            case State.Sleep:
                break;
            case State.Patrol:
                PatrolMovement();
                DistanceToWake(player, distanceToWake);
                break;
            case State.Chase:
                MovementTowardsPlayer(player);
                DistanceToWake(player, distanceToWake);
                break;
            default:
                break;
        }       
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
}
