using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public static GameManager instance; 

    public bool isGameover = false; 
    public Text scoreText; 
    public Text coinText; // 코인 개수를 표시할 UI 텍스트
    public GameObject gameoverUI;
     
    private int scoreMultiplier = 1;
    public int score = 0; 
    public int coinCount = 0; // 먹은 코인 개수 저장할 변수

    public bool hasShield = false; // [추가] 상점 에러 해결용 실드 변수

    private void Awake() {
        if (instance == null) {
            instance = this;
        } else {
            Debug.LogWarning("씬에 두 개 이상의 게임 매니저가 존재합니다!");
            Destroy(gameObject);
        }
    }

    private void Start() {
        // 💾 게임 시작 시 하드디스크에 저장되어 있던 내 진짜 코인 개수를 불러옵니다. (없으면 0)
        coinCount = PlayerPrefs.GetInt("TotalCoins", 0);
        
        // 💾 저장되어 있던 점수 배율 레벨도 불러와서 동기화합니다. (없으면 기본 1배)
        scoreMultiplier = PlayerPrefs.GetInt("ScoreMultiplier_Level", 1);

        if (coinText != null) {
            coinText.text = "COIN : " + coinCount;
        }
    }

    private void Update() {
        // 마우스가 UI 위에 없을 때만 재시작되도록 버그 방지 조건 추가
        if (isGameover && Input.GetMouseButtonDown(0) && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void AddScore(int newScore) {
        if (!isGameover) {
            // 상점에서 올린 배율(scoreMultiplier)을 곱해서 점수를 추가합니다.
            score += newScore * scoreMultiplier;
            scoreText.text = "SCORE : " + score;
        }
    }

    public void AddCoin(int amount) {
        if (!isGameover) {
            coinCount += amount;

            // 💾 코인을 먹을 때마다 실시간으로 데이터를 영구 저장합니다.
            PlayerPrefs.SetInt("TotalCoins", coinCount);
            PlayerPrefs.Save();

            if (coinText != null) {
                coinText.text = "COIN : " + coinCount;
            }
        }
    }

    public void OnPlayerDead() {
        isGameover = true;
        gameoverUI.SetActive(true);
    }

    public void SetScoreMultiplier(int newLevel)
    {
        scoreMultiplier = newLevel;

        // 💾 상점에서 배율 레벨을 올리면 이 레벨도 하드디스크에 영구 저장합니다.
        PlayerPrefs.SetInt("ScoreMultiplier_Level", scoreMultiplier);
        PlayerPrefs.Save();

        Debug.Log("GameManager: 현재 점수 배율이 " + scoreMultiplier + "배로 변경되었습니다.");
    }
}