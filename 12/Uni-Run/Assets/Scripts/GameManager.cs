using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public static GameManager instance; 

    public bool isGameover = false; 
    public Text scoreText; 
    public Text coinText; // [추가] 코인 개수를 표시할 UI 텍스트
    public GameObject gameoverUI; 

    private int score = 0; 
    private int coinCount = 0; // [추가] 먹은 코인 개수 저장할 변수

    private void Awake() {
        if (instance == null) {
            instance = this;
        } else {
            Debug.LogWarning("씬에 두 개 이상의 게임 매니저가 존재합니다!");
            Destroy(gameObject);
        }
    }

    private void Update() {
        if (isGameover && Input.GetMouseButtonDown(0)) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void AddScore(int newScore) {
        if (!isGameover) {
            score += newScore;
            scoreText.text = "SCORE : " + score;
        }
    }

    // [중요] Coin.cs가 애타게 찾던 그 함수를 만들어 줍니다!
    public void AddCoin(int amount) {
        if (!isGameover) {
            coinCount += amount;
            if (coinText != null) {
                coinText.text = "COIN : " + coinCount;
            }
        }
    }

    public void OnPlayerDead() {
        isGameover = true;
        gameoverUI.SetActive(true);
    }
}