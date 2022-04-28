using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySlime : AEnemy
{
    [SerializeField] float speed;
    [SerializeField] int health;

    void Start()
    {
        Speed = speed;
        Health = health;
    }

    private void FixedUpdate()
    {
        FreeMovement();
    }

    protected override void Die()
    {
        throw new System.NotImplementedException();
    }

    protected override void LostHealth()
    {
        throw new System.NotImplementedException();
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            Flip();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            Flip();
        }
    }
}
