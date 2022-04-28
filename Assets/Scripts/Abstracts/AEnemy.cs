using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AEnemy : MonoBehaviour
{
    protected int Health { get; set; }
    protected float Speed { get; set; }

    Rigidbody2D rb;
    GameObject foot;

    [SerializeField] Vector3[] walkPositions;
    private int index;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        foot = GameObject.FindGameObjectWithTag("Foot");
    }

    protected abstract void LostHealth();

    protected abstract void Die();

    protected virtual void FreeMovement(float minX, float maxX)
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

        if (dir.x > 0f)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else
        {
            transform.eulerAngles = new Vector3(0, 180f, 0f);
        }
    }

    void FlipOnPlataform()
    {
        CircleCollider2D footColl = foot.GetComponent<CircleCollider2D>();

        
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        
    }
}
