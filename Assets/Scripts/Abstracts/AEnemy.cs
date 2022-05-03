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

    Vector2 lookDirection;

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

    private void Start()
    {
        lookDirection = transform.eulerAngles;
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
        Vector2 playerDir = (playerPos - transform.position).normalized;

        transform.eulerAngles = playerDir.x > 0f ? new Vector3(0f, 180f, 0f): Vector3.zero;

        rb.velocity = new Vector2(playerDir.x * chaseSpeed, rb.velocity.y);
    }

    protected virtual void PatrolMovement()
    {
        int playerDir = 0;
        transform.eulerAngles = playerDir > 0f ? new Vector3(0f, 180f, 0f) : Vector3.zero;

        playerDir = transform.eulerAngles.y == 0 ? -1 : 1;

        rb.velocity = new Vector2(playerDir * patrolSpeed, rb.velocity.y);
    }

    protected void DistanceToWake(GameObject target, float distToWake)
    {
        Vector2 playerPos = target.transform.position;

        float distanceToPlayer = Vector2.Distance(transform.position, playerPos);

        state = distanceToPlayer < distToWake ? State.Chase : State.Patrol;
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

}
