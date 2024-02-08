using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Finish : MonoBehaviour
{
    [SerializeField] Brother[] brothers;

    // FIXME rotate brothers towards crowd
    [SerializeField] private Transform lewyGolecFinishRotation;
    [SerializeField] private Transform prawyGolecFinishRotation;

    [SerializeField] private List<Debris> debris;

    [SerializeField] private int playersFinished;
    [SerializeField] private CinemachineVirtualCamera winCamera;

    private void Start()
    {
        brothers = FindObjectsOfType<Brother>();
    }

    public void OnTriggerEnter(Collider other) {
        if (other.GetComponent<Brother>()) {
            playersFinished++;
            if (playersFinished == 2)
                StartCoroutine(WinCutscene());
        }
    }
    
    public void OnTriggerExit(Collider other) {
        if (other.GetComponent<Brother>())
            playersFinished--;
    }

    IEnumerator WinCutscene() {
        foreach (Debris d in debris) {
            d.DropAndDestroy();
        }

        foreach (Brother brother in brothers)
        {
            brother.GetComponent<Movement>().canMove = false;
            // FIXME brother.RotateToward(lookAt);
            brother.FinishDance();
        }
        SoundVisualizer.Instance.PlayWinJingle();
        winCamera.Priority = 100;
        for (float timeToEnd = 0f; timeToEnd < 6f; timeToEnd += Time.deltaTime) {
            yield return null;
        }
        SceneManager.LoadScene("MainHub");
    }
}
