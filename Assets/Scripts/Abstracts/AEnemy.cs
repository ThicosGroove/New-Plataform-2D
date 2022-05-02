using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class AEnemy : MonoBehaviour
{
    [SerializeField] private LayerMask plataformLayerMask;

    protected float Health { get; set; }
    protected float patrolSpeed { get; set; }
    protected float chaseSpeed { get; set; }

    [SerializeField] private GameObject healtBar;
    [SerializeField] private Image filledHealtBar;

    Rigidbody2D rb;

    protected State state;
    protected enum State
    {
        Sleep,
        Patrol,
        Chase
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    protected abstract void LostHealth();

    protected abstract void Die();

    protected virtual void HealthBarFiller(float currentHealth, float filledSpeed)
    {
        float fillAmountPercentage = currentHealth / Health;
        float lerpSpeed = filledSpeed * Time.deltaTime;

        filledHealtBar.fillAmount = Mathf.Lerp(filledHealtBar.fillAmount, fillAmountPercentage, lerpSpeed);
    }

    protected virtual void MovementTowardsPlayer(GameObject target)
    {
        Vector3 playerPos = target.transform.position;
        Vector2 dir = (playerPos - transform.position).normalized;

        rb.MovePosition((Vector2)transform.position + (dir * chaseSpeed * Time.deltaTime));
        //StayOnGround();

        transform.eulerAngles = dir.x > 0f ? new Vector3(0f, 180f, 0f): Vector3.zero;
    }

    protected void DistanceToWake(GameObject target, float distToWake)
    {
        Vector2 playerPos = target.transform.position;

        float distanceToPlayer = Vector2.Distance(transform.position, playerPos);

        if (distanceToPlayer < distToWake) 
        {
            state = State.Chase;
        }
        else
        {
            state = State.Patrol;
        }
    }

    protected virtual void FreeMovement()
    {
        int dir;
        Vector2 lookDirection = transform.eulerAngles;
        dir = lookDirection.y == 180f ? 1 : -1;

        rb.velocity = new Vector2(dir * patrolSpeed, rb.velocity.y);
    }

    protected virtual void Flip()
    {
        transform.localScale = new Vector2(-(Mathf.Sign(patrolSpeed)), transform.localScale.y);
        patrolSpeed *= -1;
    }

    protected void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) { return; }

        if (collision.gameObject.GetComponent<ArrowBehaviour>())
        {
            LostHealth();
        }
    }

    protected void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            Flip();
        }
    }

    protected void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Wall") || other.gameObject.GetComponent<AEnemy>())
        {
            Flip();
        }
    }

    protected virtual void StayOnGround()
    {
        RaycastHit2D raycastHit2D = Physics2D.Raycast(transform.position, Vector2.down, 3f, plataformLayerMask);

        bool isGrounded = raycastHit2D.collider;

        float maxHeight = 1f;
        float distToGround = raycastHit2D.distance;
        Debug.LogWarning(distToGround);

        if (raycastHit2D.distance > distToGround)
        {
            raycastHit2D.distance = maxHeight;
        }

        //transform.position = new Vector2(rb.velocity.x, 3.66f);
    }
}
