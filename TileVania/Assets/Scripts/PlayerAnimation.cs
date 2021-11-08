using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{

    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();

    }

    void Update()
    {

    }

    public void ClimbAnimation(bool state)
    {
        animator.SetBool("isClimbing", state);
    }

    public void RunAnimation(bool state)
    {
        animator.SetBool("isRunning", state);
    }

    public void HitAnimation()
    {
        animator.SetTrigger("TakeDamage");
    }

    public void FireAnimation()
    {
        animator.SetTrigger("Fire");
    }


    public void DieAnimation()
    {
        animator.SetTrigger("Dying");
    }




}
