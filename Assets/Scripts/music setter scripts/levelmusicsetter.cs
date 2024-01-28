using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class levelmusicsetter : MonoBehaviour
{
    public AudioClip levelmusicWarmup;
    public AudioClip levelmusicMain;
    
    // Start is called before the first frame update
    void Start()
    {
        SoundVisualizer.Instance.AddToQueue(levelmusicWarmup);
        SoundVisualizer.Instance.AddToQueue(levelmusicMain);
        SoundVisualizer.Instance.StartPlayingQueue();
    }
}
