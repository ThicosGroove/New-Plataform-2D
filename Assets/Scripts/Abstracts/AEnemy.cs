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
    private Animator anim;

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
        anim = GetComponent<Animator>();
        healtBar.gameObject.SetActive(false);
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

        transform.eulerAngles = playerDir.x > 0f ? new Vector3(0f, 180f, 0f) : Vector3.zero;

        if (this.GetComponent<EnemyBat>())
        {
            rb.velocity = new Vector2(playerDir.x * chaseSpeed, playerDir.y * chaseSpeed);
        }
        else { rb.velocity = new Vector2(playerDir.x * chaseSpeed, rb.velocity.y); }
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

    protected virtual void Flip()
    {
        transform.localScale = new Vector2(-(Mathf.Sign(patrolSpeed)), transform.localScale.y);
        patrolSpeed *= -1;
    }

    IEnumerator PlayHitAnimation()
    {
        anim.SetBool("isHit", true);

        yield return new WaitForSeconds(0.15f);
        
        anim.SetBool("isHit", false);
    }

    // For main Body, Hit by Player and Arrow
    protected void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) { return; }

        if (collision.gameObject.GetComponent<ArrowBehaviour>())
        {
            healtBar.gameObject.SetActive(true);
            StartCoroutine(PlayHitAnimation());

            LostHealth();
        }
    }

    // Both Triggers are For foot, used to flip
    protected void OnTriggerExit2D(Collider2D collision)
    {
        if (!this.GetComponent<EnemyBat>() && collision.gameObject.CompareTag("Ground"))
        {
            Flip();
        }
    }

    protected void OnTriggerEnter2D(Collider2D other)
    {
        // any enemy flips if hit each other or a wall
        if (other.gameObject.CompareTag("Wall") || other.gameObject.GetComponent<AEnemy>())
        {
            Flip();
        }

        // because bat it's not on the ground, only it flips when touch a ground tile. Others are arleady in touch.
        if (this.GetComponent<EnemyBat>() && other.gameObject.CompareTag("Ground"))
        {
            Flip();
        }
    }

}
