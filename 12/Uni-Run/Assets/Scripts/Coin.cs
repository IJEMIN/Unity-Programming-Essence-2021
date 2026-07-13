using UnityEngine;

public class Coin : MonoBehaviour {
    public int scoreValue = 1; // 기본 코인 가치

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            // GameManager에 저장된 코인 개수 증가 (아래 2단계에서 구현)
            GameManager.instance.AddCoin(scoreValue);
            Destroy(gameObject); // 먹으면 삭제
        }
    }
}
