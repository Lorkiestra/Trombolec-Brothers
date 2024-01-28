using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class levelmusicsetter : MonoBehaviour
{
    public AudioClip levelmusicMain;
    
    // Start is called before the first frame update
    void Start()
    {
        SoundVisualizer.Instance.AddToQueue(levelmusicMain);
        SoundVisualizer.Instance.StartPlayingQueue();
    }
}
