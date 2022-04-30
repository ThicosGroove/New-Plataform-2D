using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowBehaviour : MonoBehaviour
{
    [SerializeField] float KnockBackPower;
    public ParticleSystem particle;

    SpriteRenderer sprite;
    Rigidbody2D rb;
    BoxCollider2D boxCollider;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player")) { return; }
        if (collision.collider.CompareTag("Bow")) { return; }

        if (collision.collider.CompareTag("Ground") || collision.collider.CompareTag("Wall"))
        {
            Destroy(this.gameObject);
        }

        if (collision.collider.GetComponent<AEnemy>())
        {
            Debug.Log("Acertou!!");

            boxCollider.enabled = false;
            sprite.enabled = false;

            particle.Play();
            KnockBack(collision.gameObject);
        }
    }

    void KnockBack(GameObject enemy)
    {
        Rigidbody2D enemyRB = enemy.GetComponent<Rigidbody2D>();

        Vector2 dir = rb.velocity;
        enemyRB.AddForce(dir * KnockBackPower, 0f);
    }
}
