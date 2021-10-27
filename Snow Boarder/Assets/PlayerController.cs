using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField] float torqueAmount = 1f;
    [SerializeField] float baseSpeed = 20f;
    [SerializeField] float boostSpeed = 35f;

    Rigidbody2D rb;
    SurfaceEffector2D surfaceEffector2D;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        surfaceEffector2D = FindObjectOfType<SurfaceEffector2D>();
    }

    // Update is called once per frame
    void Update()
    {
        RotatePlayer();
        HandleBoost();
    }

    private void HandleBoost()
    {

        if(Input.GetAxis("Vertical") < 0){
            surfaceEffector2D.speed = boostSpeed;
        }
        else{
            surfaceEffector2D.speed = baseSpeed;
        }

    }

    private void RotatePlayer()
    {
        if (Input.GetAxis("Horizontal") < 0)
        {
            rb.AddTorque(torqueAmount * Time.deltaTime);
        }

        if (Input.GetAxis("Horizontal") > 0)
        {
            rb.AddTorque(-torqueAmount * Time.deltaTime);
        }
    }
}
