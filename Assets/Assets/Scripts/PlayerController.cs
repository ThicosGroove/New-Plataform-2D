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
    [SerializeField] private LayerMask ladderLayerMask;

    [Header("Movement forces")]
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float _gravity;
    [SerializeField] private float rollSpeed;

    [Header("Climb Forces")]
    [SerializeField] private float speedToLadder;
    [SerializeField] private float climbSpeed;
    [SerializeField] private float ladderDist = 0.3f;
    private bool isClimb;
    

    private float jumpInput;
    private float moveInput;
    private float rollInput;
    private float climbInput;
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

    PlayerState state;

    enum PlayerState
    {
        Moving,
        Climbing,
        Wait
    }

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

        state = PlayerState.Moving;
    }

    //used in physics updates 
    private void FixedUpdate()
    {
        switch (state)
        {
            case PlayerState.Moving:
                Move();
                Jump();
                //BetterJump();
                Roll();
                Attack();
                OnLadderRrange();
                break;
            case PlayerState.Climbing:
                Climb();
                OnLadderRrange();
                break;
            case PlayerState.Wait:
                break;
            default:
                break;
        }
    }

    void Move()
    {
        moveInput = input.Player.Move.ReadValue<float>();

        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);
        FlipAnimation();
    }

    void FlipAnimation()
    {

        if (moveInput > 0)
        {
            anim.SetBool("isRunning", true);
            transform.eulerAngles = new Vector3(0, 0, 0);
            isFacingRight = true;
        }
        else if (moveInput < 0)
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
        }
        if (rb.velocity.y > 15f){ rb.velocity = Vector2.up * 15f; }
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

        if (rollInput > 0 && rollDelay <= 0f && IsGrounded() && moveInput != 0)
        {

            rb.velocity = new Vector2(moveInput * rollSpeed, rb.velocity.y);
            anim.SetBool("isRolling", true);

            StayOnGround();

            rollDelay = 0.5f;
        }
        else
        {
            anim.SetBool("isRolling", false);

        }
    }

    void StayOnGround()
    {
        float distToGround = raycastHit.distance;

        transform.position = new Vector2(transform.position.x, transform.position.y - distToGround);
        rb.velocity = new Vector2(rb.velocity.x, 0f);
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

    public void GoToLadder(GameObject ladder)
    {
        state = PlayerState.Wait;

        Vector2 ladderDir = ladder.transform.position;
  
        float dirTwoardsLadder = ladderDir.x > this.transform.position.x ? 1 : -1; 
        rb.velocity = new Vector2(dirTwoardsLadder * speedToLadder, rb.velocity.y);

        Debug.LogWarning("Indo em direção escada");

        //Player automatic Moves to ladder 

        //if on ladder range 
        //player climbs

        //on reach top
        //player automatic moves to plataform  
    }

    private void OnLadderRrange()
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, Vector2.up, ladderDist, ladderLayerMask);
        Color rayColor = Color.white;

        if (hitInfo.collider != null)
        {
            Debug.LogWarning("Escada");

            state = PlayerState.Climbing;

            rayColor = Color.green;
            Physics.gravity = Vector3.zero;
            rb.gravityScale = 0f;

            Climb();
        }
        else if (hitInfo.collider == null)
        {
            //animator.SetBool("IsClimbing", false);

            rayColor = Color.red;

            //Fazer o jogador se mover automaticamente para a proxima plataforma.
            rb.gravityScale = _gravity;

            isClimb = false;
            state = PlayerState.Moving;
        }
        Debug.DrawRay(capsuleCollider2D.bounds.center, Vector2.up * ladderDist, rayColor);
    }


    private void Climb()
    {
        climbInput = input.Player.Climb.ReadValue<float>();

        if (!isClimb)
        {
            rb.velocity = Vector2.up * climbSpeed * Time.deltaTime;
            isClimb = true;
        }
        // Nesse jogo não se desce a escada.
    }

    //public void ClimbOutOfLadder(Transform nextPlataform)
    //{
    //    state = PlayerState.Wait;

    //    Vector2 plataformDir = nextPlataform.position;

    //    float DisTwoardsPlataform = plataformDir.x > this.transform.position.x ? 1 : -1;
    //    rb.velocity = new Vector2(DisTwoardsPlataform * speedToLadder, rb.velocity.y);

    //    if (this.transform.position == nextPlataform.position)
    //    {
    //        rb.velocity = Vector2.zero;

    //        Debug.LogWarning("Pulou fora");

    //        state = PlayerState.Moving;
    //    }
    //}

}
