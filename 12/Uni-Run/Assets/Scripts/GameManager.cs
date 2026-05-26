using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    public static GameManager instance;

    public bool isGameover = false;
    public Text scoreText;
    public GameObject gameoverUI;

    public Text coinText;         
    public int totalCoins = 0;    
    public float scoreMultiplier = 1f; 

    private int score = 0;

    void Awake() {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    void Update() {
        if(isGameover && Input.GetMouseButtonDown(0)){
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void AddScore(int newScore) {
        if(!isGameover){
            score += (int)(newScore * scoreMultiplier);
            scoreText.text = "Score : " + score;
        }
    }

    public void AddCoin(int amount) {
        if (!isGameover) {
            totalCoins += amount;
            UpdateCoinUI();
        }
    }

    public void UpdateCoinUI() {
        if (coinText != null) {
            coinText.text = "Coins : " + totalCoins;
        }
    }

    public void OnPlayerDead() {
        isGameover = true;
        gameoverUI.SetActive(true);
    }
}