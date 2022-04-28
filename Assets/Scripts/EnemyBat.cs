using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBat : AEnemy
{
    [SerializeField] private int health;
    [SerializeField] private float speed;

    [SerializeField] private float distanceToWake;

    private GameObject player;

    protected override void Die()
    {
        throw new System.NotImplementedException();
    }

    protected override void LostHealth()
    {
        throw new System.NotImplementedException();
    }

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
}
