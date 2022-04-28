using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AEnemy : MonoBehaviour
{
    protected int Health { get; set; }
    protected float Speed { get; set; }

    Rigidbody2D rb;

    [SerializeField] Vector3[] walkPositions;
    private int index;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    protected abstract void LostHealth();

    protected abstract void Die();

    protected virtual void FixedMovement(float minX, float maxX)
    {
        rb.velocity = Vector2.MoveTowards(transform.position, walkPositions[index], Speed * Time.deltaTime);
    }

    protected virtual void MovementTowardsPlayer(GameObject target)
    {
        Vector3 playerPos = target.transform.position;
        Vector2 dir = (playerPos - transform.position).normalized;

        // Tryed few differents things, these two ways seems to work best for this project 
        rb.MovePosition((Vector2)transform.position + (dir * Speed * Time.deltaTime));
        //rb.velocity = new Vector2(dir.x * Speed, transform.position.y);
       
        transform.eulerAngles = dir.x > 0f ? new Vector3(0f, 180f, 0f): Vector3.zero;
    }

    protected void DistanceToWake(GameObject target, float distToWake)
    {
        Vector2 playerPos = target.transform.position;

        float distanceToPlayer = Vector2.Distance(transform.position, playerPos);

        if (distanceToPlayer < distToWake) { MovementTowardsPlayer(target); }
    }

    protected virtual void FreeMovement()
    {
        int dir;
        Vector2 lookDirection = transform.eulerAngles;
        dir = lookDirection.y == 180f ? 1 : -1;

        rb.velocity = new Vector2(dir * Speed * Time.deltaTime, rb.velocity.y);
    }

    protected virtual void Flip()
    {
        transform.localScale = new Vector2(-(Mathf.Sign(Speed)), transform.localScale.y);
        Speed *= -1;
    }
}
