using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelExit : MonoBehaviour
{
    [SerializeField] int LevelTarget;
    private ParticleSystem ps;

    void Start() {
        ps = GetComponent<ParticleSystem>();    
    }

    void OnTriggerEnter2D(Collider2D other) {
        ps.Play();
        StartCoroutine("LoadLevel");
    }

    private IEnumerator LoadLevel(){
        yield return new WaitForSecondsRealtime(3f);
        SceneManager.LoadScene(LevelTarget);
    }

}
