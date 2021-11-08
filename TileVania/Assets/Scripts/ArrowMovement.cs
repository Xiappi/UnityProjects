using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowMovement : MonoBehaviour
{
    [SerializeField] float ArrowSpeed = 1f;
    PlayerMovement player;
    private Rigidbody2D rb;
    void Start()
    {
        player = FindObjectOfType<PlayerMovement>();
        rb = GetComponent<Rigidbody2D>();

        rb.velocity = new Vector2(player.transform.localScale.x * ArrowSpeed, 0);
        rb.transform.localScale = player.transform.localScale;
    }

    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.tag == "Enemy"){
            Destroy(other.gameObject);
        }
        Destroy(gameObject);
    }
}
