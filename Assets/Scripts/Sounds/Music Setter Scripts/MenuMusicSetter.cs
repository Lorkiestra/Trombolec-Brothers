using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuMusicSetter : MonoBehaviour {
    public AudioClip menuMusic;
    
    void Start()
    {
        SoundVisualizer.Instance.AddToQueue(menuMusic);
        SoundVisualizer.Instance.StartPlayingQueue();
    }
}