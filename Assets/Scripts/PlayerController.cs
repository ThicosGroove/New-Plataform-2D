using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Input_PlayerController input;
    private Rigidbody2D rb;
    private CapsuleCollider2D capsuleCollider2D;
    private Animator anim;

    [SerializeField] private LayerMask plataformLayerMask;

    [Header("Movement forces")]
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float fallMultiplier;
    [SerializeField] private float lowJumpMultiplier;
    [SerializeField] private float rollSpeed;

    private float jumpInput;
    private float move;
    private float rollInput;
    private float rollDelay;
    private float attackInput;

    private bool isFacingRight = true;

    private RaycastHit2D raycastHit;

    [Header("Attack forces")]
    public GameObject arrowPrefab;
    public GameObject bow;
    [SerializeField] private float bowForce;
    [SerializeField] private float _fireRate;

    private float fireRate;

    private void Awake()
    {
        input = new Input_PlayerController();
    }

    private void OnEnable()
    {
        input.Enable();
    }

    private void OnDisable()
    {
        input.Disable();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        capsuleCollider2D = GetComponentInChildren<CapsuleCollider2D>();
        anim = GetComponent<Animator>();
        fireRate = _fireRate;

    }

    //used in physics updates 
    private void FixedUpdate()
    {
        Move();
        Jump();
        BetterJump();
        Roll();
        Attack();
    }

    void Move()
    {
        move = input.Player.Move.ReadValue<float>();

        rb.velocity = new Vector2(move * speed, rb.velocity.y);
        FlipAnimation();
    }

    void FlipAnimation()
    {

        if (move > 0)
        {
            anim.SetBool("isRunning", true);
            transform.eulerAngles = new Vector3(0, 0, 0);
            isFacingRight = true;
        }
        else if (move < 0)
        {
            anim.SetBool("isRunning", true);
            transform.eulerAngles = new Vector3(0, 180, 0);
            isFacingRight = false;
        }
        else
        {
            anim.SetBool("isRunning", false);
        }
    }

    void Jump()
    {
        jumpInput = input.Player.Jump.ReadValue<float>();

        if (IsGrounded() && jumpInput > 0.01f)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

            // Maximum jump velocity
            // Debug.LogWarning(rb.velocity.y);
            if (rb.velocity.y > 8f) { rb.velocity = Vector2.up * 8f; }
        }
    }

    void BetterJump()
    {
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;

            // Maximum fall velocity for NOT enter the ground
            // Debug.LogWarning(rb.velocity.y);            
            if (rb.velocity.y < -15f) { rb.velocity = Vector2.up * (-15f); }
        }
        else if (rb.velocity.y > 0 && jumpInput == 0)
        {
            rb.velocity += Vector2.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }

    bool IsGrounded()
    {
        float extraHeight = 0.5f;

        raycastHit = Physics2D.BoxCast(capsuleCollider2D.bounds.center, capsuleCollider2D.bounds.size, 0f, Vector2.down, extraHeight, plataformLayerMask);

        Color rayColor;
        if (raycastHit.collider != null)
        {
            rayColor = Color.green;
            anim.SetBool("isGrounded", true);
        }
        else
        {
            rayColor = Color.red;
            anim.SetBool("isGrounded", false);
        }
        Debug.DrawRay(capsuleCollider2D.bounds.center, Vector2.down * (extraHeight), rayColor);

        return raycastHit.collider != null;
    }

    void Roll()
    {
        rollInput = input.Player.Roll.ReadValue<float>();

        rollDelay -= Time.deltaTime;

        if (rollInput > 0 && rollDelay <= 0f && IsGrounded() && move != 0)
        {

            rb.velocity = new Vector2(move * rollSpeed, rb.velocity.y);
            anim.SetBool("isRolling", true);

            StayOnGround();

            rollDelay = 0.5f;
        }
        else
        {
            anim.SetBool("isRolling", false);

        }

        // ao apertar o botao
        //pequeno dash durante 0.5 segundos
        // animação
        //desliga o collisor de dano
    }

    void StayOnGround()
    {
        float distToGround = raycastHit.distance;

        transform.position = new Vector2(transform.position.x, transform.position.y - distToGround);
        rb.velocity = new Vector2(rb.velocity.x, 0f);

        Debug.LogWarning(distToGround);

    }

    void Attack()
    {
        attackInput = input.Player.Attack.ReadValue<float>();

        fireRate -= Time.deltaTime;

        if (attackInput > 0.01f)
        {
            if (fireRate < 0)
            {
                GameObject newArrow = Instantiate(arrowPrefab, bow.transform.position, bow.transform.rotation);

                if (isFacingRight) { newArrow.GetComponent<Rigidbody2D>().AddForce(Vector2.right * bowForce, ForceMode2D.Impulse); }

                if (!isFacingRight) { newArrow.GetComponent<Rigidbody2D>().AddForce(Vector2.left * bowForce, ForceMode2D.Impulse); }

                anim.SetBool("isAttacking", true);

                fireRate = _fireRate;
            }
        }
        else
        {
            anim.SetBool("isAttacking", false);
        }
    }
}
