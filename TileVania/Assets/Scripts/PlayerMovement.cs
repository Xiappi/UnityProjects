using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float MoveSpeed = 1f;
    [SerializeField] float JumpHeight;
    [SerializeField] float climbSpeed = 1f;
    Vector2 moveInput;
    Rigidbody2D rb;
    Animator animator;
    Collider2D cd;
    private bool canClimb = false; 
    private float startGravity;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        cd = GetComponent<Collider2D>();

        startGravity = rb.gravityScale;
    }
    void Update()
    {
        Run();
        FlipSprite();
        ClimbLadder();
    }

    private void ClimbLadder()
    {
        if (!cd.IsTouchingLayers(LayerMask.GetMask("Ladder")))
        {
            rb.gravityScale = startGravity;
            return;
        }

    
        rb.velocity = new Vector2(rb.velocity.x, moveInput.y * climbSpeed);
        rb.gravityScale = 0f;
        
    }

    private void FlipSprite()
    {

        bool playHasHorizontalSpeed = Mathf.Abs(rb.velocity.x) > Mathf.Epsilon;

        if (playHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(rb.velocity.x), 1f);
        }
    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();

    }

    void OnJump(InputValue value)
    {
        if (cd.IsTouchingLayers(LayerMask.GetMask("Ground")) && value.isPressed)
        {
            rb.velocity += new Vector2(0f, JumpHeight);
        }
    }

    private void Run()
    {
        bool playHasHorizontalSpeed = Mathf.Abs(rb.velocity.x) > Mathf.Epsilon;
        animator.SetBool("isRunning", playHasHorizontalSpeed);

        var playerVelocity = new Vector2(moveInput.x * MoveSpeed, rb.velocity.y);
        rb.velocity = playerVelocity;
    }
}
