using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBlob : AEnemy
{
    [SerializeField] float speed;
    [SerializeField] int health;

    [SerializeField] float distanceToWake;

    private GameObject player;

    private float initialHeight;

    void Start()
    {
        Speed = speed;
        Health = health;

        player = GameObject.FindGameObjectWithTag("Player");

        initialHeight = transform.position.y;
    }

    private void Update()
    {
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, Mathf.NegativeInfinity, initialHeight), transform.position.z);
    }

    private void FixedUpdate()
    {
        DistanceToWake(player, distanceToWake);
    }

    protected override void Die()
    {
        throw new System.NotImplementedException();
    }

    protected override void LostHealth()
    {
        throw new System.NotImplementedException();
    }

    // Update is called once per frame
}
