using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingZone : MonoBehaviour {
    [SerializeField] private string sceneName;
    
    private void OnTriggerEnter(Collider other) {
        if (other.GetComponent<Brothers>())
            SceneManager.LoadScene(sceneName);
    }
}
