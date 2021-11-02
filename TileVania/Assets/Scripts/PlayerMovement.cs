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
    [SerializeField] GameObject Arrow;
    [SerializeField] Transform Bow;
    Vector2 moveInput;
    Rigidbody2D rb;
    Animator animator;
    Collider2D bodyCollider;
    BoxCollider2D feetCollider;
    private bool isAlive = true;
    private float startGravity;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        bodyCollider = GetComponent<Collider2D>();
        feetCollider = GetComponent<BoxCollider2D>();
        startGravity = rb.gravityScale;
    }
    void Update()
    {
        if (!isAlive) return;

        Run();
        FlipSprite();
        ClimbLadder();


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
        if (!isAlive) return;

        if (feetCollider.IsTouchingLayers(LayerMask.GetMask("Ground")) && value.isPressed)
        {
            rb.velocity += new Vector2(0f, JumpHeight);
        }
    }
    private void ClimbLadder()
    {
        if (!isAlive) return;

        if (!feetCollider.IsTouchingLayers(LayerMask.GetMask("Ladder")))
        {
            rb.gravityScale = startGravity;
            animator.SetBool("isClimbing", false);
            return;
        }


        rb.velocity = new Vector2(rb.velocity.x, moveInput.y * climbSpeed);
        rb.gravityScale = 0f;


        bool playHasVerticalSpeed = Mathf.Abs(rb.velocity.y) > Mathf.Epsilon;
        animator.SetBool("isClimbing", playHasVerticalSpeed);
    }

    private void Run()
    {
        bool playHasHorizontalSpeed = Mathf.Abs(rb.velocity.x) > Mathf.Epsilon;
        animator.SetBool("isRunning", playHasHorizontalSpeed);

        var playerVelocity = new Vector2(moveInput.x * MoveSpeed, rb.velocity.y);
        rb.velocity = playerVelocity;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (isAlive && other.gameObject.tag == "Enemy")
        {
            Die();

        }
    }

    private void Die()
    {
        isAlive = false;
        animator.SetTrigger("Dying");
        rb.velocity = new Vector2(0, 30f);

    }

    void OnFire(InputValue value){
        if(!isAlive) return;
        Instantiate(Arrow, Bow.position, transform.rotation);
    }
}
