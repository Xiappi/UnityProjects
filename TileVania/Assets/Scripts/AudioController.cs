using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{

    private AudioSource[] audioSources;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);
        audioSources = GetComponents<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    internal void PlayerJump()
    {
        audioSources[1].Play();
    }
}
