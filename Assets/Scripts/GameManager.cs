using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    
    public static GameManager Instance;
    
    [SerializeField] private TextMeshProUGUI gameovertext;
    [SerializeField] private Brothers lewyGolec;
    [SerializeField] private Brothers prawyGolec;

    private void Awake() {
        Instance = this;
    }

    int deadedPlayers = 0;
    public int DeadedPlayers {
        get {
            return deadedPlayers;
        }
        set {
            deadedPlayers = value;
            if (deadedPlayers >= 2)
                GameOver();
        }
    }

    void GameOver() {
        gameovertext.gameObject.SetActive(true);
        lewyGolec.GetComponent<Movement>().canMove = false;
        prawyGolec.GetComponent<Movement>().canMove = false;
        StartCoroutine(EGameOver());
    }

    IEnumerator EGameOver() {
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
