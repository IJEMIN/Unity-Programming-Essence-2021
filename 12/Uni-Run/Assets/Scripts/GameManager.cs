using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public static GameManager instance; 

    public bool isGameover = false; 
    public Text scoreText; 
    public Text coinText; 
    public GameObject gameoverUI;
     
    private int scoreMultiplier = 1;
    public int score = 0; 
    public int coinCount = 0; 

    public bool hasShield = false; 

    private void Awake() {
        if (instance == null) {
            instance = this;
        } else {
            Debug.LogWarning("씬에 두 개 이상의 게임 매니저가 존재합니다!");
            Destroy(gameObject);
        }

        Time.timeScale = 0f; 
    }

    private void Start() {
        // 저장된 코인과 배율 불러오기
        coinCount = PlayerPrefs.GetInt("TotalCoins", 0);
        scoreMultiplier = PlayerPrefs.GetInt("ScoreMultiplier_Level", 1);

        if (coinText != null) {
            coinText.text = "COIN : " + coinCount;
        }

        // 🌟 [핵심 변경] 죽어서 '재시작' 했을 때만 시작 패널을 패스합니다.
        // 게임을 아예 껐다 켰을 때는 TempIsPlaying이 0이므로 스타트 패널이 정상 작동합니다.
        if (PlayerPrefs.GetInt("TempIsPlaying", 0) == 1) 
        {
            Time.timeScale = 1f;
            GameObject startPanel = GameObject.Find("StartPanel");
            if (startPanel != null) {
                startPanel.SetActive(false);
            }
        }
    }

    private void Update() {
        if (isGameover && Input.GetMouseButtonDown(0) && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void AddScore(int newScore) {
        if (!isGameover) {
            score += newScore * scoreMultiplier;
            scoreText.text = "SCORE : " + score;
        }
    }

    public void AddCoin(int amount) {
        if (!isGameover) {
            coinCount += amount;
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

    public void SetScoreMultiplier(int newLevel) {
        scoreMultiplier = newLevel;
        PlayerPrefs.SetInt("ScoreMultiplier_Level", scoreMultiplier);
        PlayerPrefs.Save();
    }

    // 🌟 [기존 시작 버튼] = 이어서 하기 (Continue)
    public void GameStart()
    {
        Time.timeScale = 1f;
        
        // 현재 게임 플레이 중임을 임시 기록 (죽고 재시작할 때 시작패널 안 뜨게 가로막는 역할)
        PlayerPrefs.SetInt("TempIsPlaying", 1);
        PlayerPrefs.Save();
    }

    // 🌟 [새로 추가] 완전히 처음부터 시작하기 (New Game / 초기화)
    public void ResetAndStart()
    {
        // 내 하드디스크에 저장된 코인, 배율, 아이템 레벨 등 모든 데이터를 싹 밀어버립니다!
        PlayerPrefs.DeleteAll(); 

        // 게임 플레이 중 상태는 다시 1로 켜줍니다. (재시작 방지용)
        PlayerPrefs.SetInt("TempIsPlaying", 1);
        PlayerPrefs.Save();

        // 데이터를 싹 밀었으니 깨끗한 상태로 씬을 한 번 새로고침해서 시작합니다!
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // 게임을 정상적으로 끄거나 이탈할 때 임시 플래그를 꺼주는 안전장치
    private void OnApplicationQuit() {
        PlayerPrefs.SetInt("TempIsPlaying", 0);
        PlayerPrefs.Save();
    }
}