using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Input_PlayerController input;
    private Rigidbody2D rb;
    private BoxCollider2D boxCollider2D;
    private Animator anim;

    [SerializeField] private LayerMask plataformLayerMask;

    [Header("Movement forces")]
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float fallMultiplier;
    [SerializeField] private float lowJumpMultiplier;
    [SerializeField] private float rollSpeed;

    private float jumpInput;
    private bool doubleJump;
    private float move;
    private float rollInput;
    private float rollDelay;
    private float attackInput;
    public bool isfacingRight = true;

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
        boxCollider2D = GetComponent<BoxCollider2D>();
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
            isfacingRight = true;
        }
        else if (move < 0)
        {
            anim.SetBool("isRunning", true);
            transform.eulerAngles = new Vector3(0, 180, 0);
            isfacingRight = false;
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

        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider2D.bounds.center, boxCollider2D.bounds.size, 0f, Vector2.down, extraHeight, plataformLayerMask);

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
        Debug.DrawRay(boxCollider2D.bounds.center, Vector2.down * (extraHeight), rayColor);

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

    void Attack()
    {
        attackInput = input.Player.Attack.ReadValue<float>();

        fireRate -= Time.deltaTime;

        if (attackInput > 0.01f)
        {
            if (fireRate < 0)
            {
                GameObject newArrow = Instantiate(arrowPrefab, bow.transform.position, bow.transform.rotation);

                bowForce = isfacingRight ? bowForce *= (1) : bowForce *= (-1);
       
                newArrow.GetComponent<Rigidbody2D>().AddForce(new Vector2(bowForce, 0f));
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
