using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;
using System.Threading.Tasks;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float MoveSpeed, JumpHeight, ClimbSpeed, FireDelay = 1f;
    [SerializeField] float ReloadDelay = 3f;
    [SerializeField] GameObject Arrow, HeartsParent, RespawnPoint;
    [SerializeField] Transform Bow;
    Vector2 moveInput;
    Rigidbody2D rb;
    CapsuleCollider2D bodyCollider, feetCollider;
    private PlayerAnimation playerAnimator;
    private bool isAlive = true;
    private bool isFiring, isMoving = false;
    private float startGravity;
    private Image[] hearts;
    private AudioController audioController;
    void Start()
    {

        rb = GetComponent<Rigidbody2D>();
        startGravity = rb.gravityScale;

        hearts = HeartsParent.GetComponentsInChildren<Image>();
        playerAnimator = GetComponent<PlayerAnimation>();


        // I hate this but I don't know an elegant way of differentiating multiple types of components
        bodyCollider = GetComponent<CapsuleCollider2D>();
        feetCollider = GetComponentsInChildren<CapsuleCollider2D>().Where(c => c.tag != "Player").First();

        audioController = FindObjectOfType<AudioController>();

        // fire and forget task to keep track of respawn point
#pragma warning disable CS4014
        UpdateRespawnPoint();

    }
    void Update()
    {
        if (!isAlive) return;

        Move();
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

    void OnJump(InputValue value)
    {
        if (!isAlive) return;
        if (IsAllowedToDoActions() && value.isPressed)
        {
            audioController?.PlayerJumpSound();
            rb.velocity += new Vector2(0f, JumpHeight);
        }
    }
    private void ClimbLadder()
    {
        if (!isAlive) return;

        if (!feetCollider.IsTouchingLayers(LayerMask.GetMask("Ladder")))
        {
            rb.gravityScale = startGravity;
            playerAnimator.ClimbAnimation(false);
            return;
        }

        rb.velocity = new Vector2(rb.velocity.x, moveInput.y * ClimbSpeed);
        rb.gravityScale = 0f;

        bool playHasVerticalSpeed = Mathf.Abs(rb.velocity.y) > Mathf.Epsilon;
        playerAnimator.ClimbAnimation(playHasVerticalSpeed);
    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    private void Move()
    {
        if (isFiring && feetCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            // TODO: create run and fire animation
        }

        bool playHasHorizontalSpeed = Mathf.Abs(moveInput.x) > 0;
        playerAnimator.RunAnimation(playHasHorizontalSpeed);

        var playerVelocity = new Vector2(moveInput.x * MoveSpeed, rb.velocity.y);
        rb.velocity = playerVelocity;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (isAlive && other.gameObject.tag == "Enemy" || rb.IsTouchingLayers(LayerMask.GetMask("Hazard")))
        {
            TakeDamage();
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {

    }

    private void TakeDamage()
    {
        for (int i = hearts.Length - 1; i >= 0; i--)
        {
            if (hearts[i].enabled)
            {
                hearts[i].enabled = false;
                if (i == 0)
                {
                    Die();
                    return;
                }
                transform.position = RespawnPoint.transform.position;
                playerAnimator.HitAnimation();
                return;
            }
        }
    }

    private void Die()
    {
        isAlive = false;
        playerAnimator.DieAnimation();
        rb.velocity = new Vector2(0, 30f);
        Invoke("ReloadScene", ReloadDelay);
    }

    void OnFire(InputValue value)
    {
        if (!isAlive || isFiring)
            return;

        isFiring = true;
        StartCoroutine("Fire");
    }

    private IEnumerator Fire()
    {
        isFiring = true;
        audioController?.PlayerFireSound();
        playerAnimator.FireAnimation();
        Instantiate(Arrow, Bow.position, transform.rotation);
        yield return new WaitForSeconds(FireDelay);
        isFiring = false;
    }

    private async Task UpdateRespawnPoint()
    {

        while (true)
        {
            if (feetCollider.IsTouchingLayers(LayerMask.GetMask("Ground")) && Mathf.Abs(rb.velocity.y) < 0.0001)
            {
                RespawnPoint.transform.position = new Vector3(transform.position.x, transform.position.y);
            }
            await Task.Delay(1000);

        }
    }

    // allow actions (jump, fire, roll, etc) when touching certain layers
    private bool IsAllowedToDoActions()
    {
        return feetCollider.IsTouchingLayers(LayerMask.GetMask("Ground")) || feetCollider.IsTouchingLayers(LayerMask.GetMask("Platform"));
    }


    private void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


}
