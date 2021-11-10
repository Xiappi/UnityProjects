using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformAnimation : MonoBehaviour
{
    private Rigidbody2D rb;
    private float rotationSpeed = 0.5f;
    void Start()
    {
        rb = GetComponentInParent<Rigidbody2D>();
    }
    void Update()
    {
        if (rb.velocity.x != 0)
        {
            var rotationDirection = Mathf.Sign(rb.velocity.x);
            transform.Rotate(new Vector3(0, 0, rotationSpeed * rotationDirection));
        }
        else if (rb.velocity.y != 0)
        {
            var rotationDirection = Mathf.Sign(rb.velocity.y);
            transform.Rotate(new Vector3(0, 0, rotationSpeed * rotationDirection));
        }


    }
}
