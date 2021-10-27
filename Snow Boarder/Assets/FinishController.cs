using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishController : MonoBehaviour
{

    [SerializeField] ParticleSystem finishEffect;
    [SerializeField] float ReloadDelay = 2f;
    

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            finishEffect.Play();
            Invoke("ReloadScene", ReloadDelay);
        }
    }

    private void ReloadScene(){
        SceneManager.LoadScene(0);
    }
}
