using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class CrashController : MonoBehaviour
{
    [SerializeField] float reloadDelay = 0.75f;
    [SerializeField] ParticleSystem crashEffect;
    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.tag == "Ground")
        {
            crashEffect.Play();
            Invoke("ReloadScene", reloadDelay);
        }
    }
    private void ReloadScene()
    {
        SceneManager.LoadScene(0);
    }

}
