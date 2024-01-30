using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackstageMusicSetter : MonoBehaviour {
    public AudioClip backstage;
    
    // Start is called before the first frame update
    void Start()
    {
        SoundVisualizer.Instance.AddToQueue(backstage);
        SoundVisualizer.Instance.StartPlayingQueue();
    }
}
