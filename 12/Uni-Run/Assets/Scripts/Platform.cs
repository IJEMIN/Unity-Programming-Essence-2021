using UnityEngine;

public class Platform : MonoBehaviour {
    public GameObject[] obstacles; // 기존 장애물 배열
    public GameObject[] coins;     // [수정] 단일 오브젝트에서 '코인 배열'로 변경!
    
    private bool stepped = false;

    // 발판이 화면 오른쪽에 새로 배치되어 활성화될 때마다 실행
    private void OnEnable() {
        stepped = false;

        // 1. 기존 장애물 랜덤 활성화 코드 (3분의 1 확률)
        for (int i = 0; i < obstacles.Length; i++) {
            if (Random.Range(0, 3) == 0) {
                obstacles[i].SetActive(true);
            } else {
                obstacles[i].SetActive(false);
            }
        }

        // 2. [수정] 코인들도 각각 독립적으로 랜덤 활성화 (예: 50% 확률)
        if (coins != null) {
            for (int i = 0; i < coins.Length; i++) {
                if (coins[i] != null) {
                    // Random.Range(0, 2) == 0 이면 50% 확률입니다.
                    // 확률을 낮추고 싶다면 (0, 3) == 0 (33% 확률) 등으로 조절해 보세요!
                    coins[i].SetActive(Random.Range(0, 2) == 0); 
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.collider.CompareTag("Player") && !stepped) {
            stepped = true;
            GameManager.instance.AddScore(1); // 발판 밟으면 1점 증가
        }
    }
}