using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float fallMultiplier = 3.5f;
    [SerializeField] private float lowJumpMultiplier = 2f;
    private float jumpInput;

    private Input_PlayerController input;

    private Rigidbody2D rb;
    private Animator anim;

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
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        Move();
        Jump();
        //BetterJump();
    }

    void Move()
    {
        float move = input.Player.Move.ReadValue<float>();

        rb.velocity = Vector2.right * move * speed *  Time.deltaTime;

        if (move > 0f)
        {
            transform.eulerAngles = new Vector3(0f, 0f, 0f);
            anim.SetFloat("runningTrigger", 1f);            
        }
        else if (move < 0f)
        {
            transform.eulerAngles = new Vector3(0f, 180f, 0f);
            anim.SetFloat("runningTrigger", 1f);
        }
        else
        {
            rb.velocity = Vector2.zero;
            anim.SetFloat("runningTrigger", -1f);
        }
    }

    void Jump()
    {
        jumpInput = input.Player.Jump.ReadValue<float>();

        if (jumpInput > 0.01f) 
        { 
            rb.velocity = Vector2.up * jumpForce * Time.deltaTime;
        }  
        if (jumpInput > 0.01f && rb.velocity.x != 0) 
        { 
            rb.velocity = Vector2.up * jumpForce * rb.velocity.x * Time.deltaTime;
        }   
    }

    private void BetterJump()
    {
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rb.velocity.y > 0 && jumpInput == 0)
        {
            rb.velocity += Vector2.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }
}
