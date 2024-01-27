using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MusicManager : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    
    public AudioClip winMusic;

    public static MusicManager Instance;
    
    private void Awake() {
        Instance = this;
        audioSource = GetComponent<AudioSource>();
    }

    private void Start() {
        audioSource.Play();
    }

    public void StopMusic() {
        audioSource.Stop();
    }

    public void PlayMusic(AudioClip audioClip) {
        audioSource.Stop();
        if (!audioSource)
            return;
        audioSource.PlayOneShot(audioClip);
    }
}
