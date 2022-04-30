using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBat : AEnemy
{
    [SerializeField] private int health;
    [SerializeField] private float speed;

    [SerializeField] private float distanceToWake;

    private GameObject player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        Health = health;
        Speed = speed;
    }

    private void FixedUpdate()
    {        
        DistanceToWake(player, distanceToWake);
    }

    protected override void Die()
    {
        Destroy(gameObject);
    }

    protected override void LostHealth()
    {
        health--;

        if (health <= 0) { Die(); }
    }
}
