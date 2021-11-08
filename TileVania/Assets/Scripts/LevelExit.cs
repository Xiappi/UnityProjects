using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelExit : MonoBehaviour
{
    [SerializeField] int LevelTarget;

    void OnTriggerEnter2D(Collider2D other) {
        StartCoroutine("LoadLevel");
    }

    private IEnumerator LoadLevel(){
        yield return new WaitForSecondsRealtime(3f);
        SceneManager.LoadScene(LevelTarget);
    }

}
