using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Finish : MonoBehaviour
{
    [SerializeField] Brothers lewyGolec;
    [SerializeField] Brothers prawyGolec;

    [SerializeField] private Transform lewyGolecFinishRotation;
    [SerializeField] private Transform prawyGolecFinishRotation;

    [SerializeField] private List<Debris> debris;

    [SerializeField] private int playersFinished;
    [SerializeField] private CinemachineVirtualCamera winCamera;

    private void Start() {
        lewyGolec = FindObjectOfType<LawelTrombolec>();
        prawyGolec = FindObjectOfType<PukaszTrombolec>();
    }

    public void OnTriggerEnter(Collider other) {
        if (other.GetComponent<Brothers>()) {
            playersFinished++;
            if (playersFinished == 2)
                StartCoroutine(WinCutscene());
        }
    }
    
    public void OnTriggerExit(Collider other) {
        if (other.GetComponent<Brothers>())
            playersFinished--;
    }

    IEnumerator WinCutscene() {
        foreach (Debris d in debris) {
            d.DropAndDestroy();
        }
        lewyGolec.GetComponent<Movement>().canMove = false;
        prawyGolec.GetComponent<Movement>().canMove = false;
        lewyGolec.GetComponent<Movement>().model.localRotation = lewyGolecFinishRotation.rotation;
        prawyGolec.GetComponent<Movement>().model.localRotation = prawyGolecFinishRotation.rotation;
        lewyGolec.animator.SetTrigger("wild_dance");
        prawyGolec.animator.SetTrigger("wild_dance");
        MusicManager.Instance.PlayMusic(MusicManager.Instance.winMusic);
        winCamera.Priority = 100;
        yield return null;
        for (float timeToEnd = 0f; timeToEnd < 10f; timeToEnd += Time.deltaTime) {
            yield return null;
        }
        SceneManager.LoadScene("MainHub");
    }
}
