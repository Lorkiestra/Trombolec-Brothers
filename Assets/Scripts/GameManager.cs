using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    
    public static GameManager Instance;
    
    [SerializeField] private TextMeshProUGUI gameovertext;
    [SerializeField] private Brother leftBrother;
    [SerializeField] private Brother rightBrother;

    private void Awake() {
        Instance = this;
    }

    private int deadPlayers;
    public int DeadPlayers {
        get {
            return deadPlayers;
        }
        set {
            deadPlayers = value;
            // FIXME one player can die twice
            if (deadPlayers >= 2)
                GameOver();
        }
    }

    private void GameOver() {
        gameovertext.gameObject.SetActive(true);
        leftBrother.GetComponent<Movement>().canMove = false;
        rightBrother.GetComponent<Movement>().canMove = false;
        StartCoroutine(EGameOver());
    }

    private IEnumerator EGameOver() {
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
