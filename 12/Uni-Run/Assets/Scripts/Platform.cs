using UnityEngine;

public class Platform : MonoBehaviour {
    public GameObject[] obstacles; // 장애물들
    public GameObject coinGroup;   // 코인 그룹
    private bool stepped = false;  // 밟았는지 체크

    // 발판이 활성화될 때마다 실행되는 함수
    private void OnEnable() {
        stepped = false;

        // 1. 장애물 랜덤 활성화
        for (int i = 0; i < obstacles.Length; i++) {
            // 33% 확률로 장애물 활성화
            obstacles[i].SetActive(Random.Range(0, 3) == 0);
        }

        // 2. 코인 개별 랜덤 활성화 (우리가 만든 새 로직)
        if (coinGroup != null) {
            coinGroup.SetActive(true); // 그룹은 켜두고

            // 자식 코인들을 하나씩 검사
            foreach (Transform child in coinGroup.transform) {
                // 50% 확률로 각 코인을 보여줄지 말지 결정
                bool shouldShow = (Random.Range(0, 2) == 0);
                child.gameObject.SetActive(shouldShow);
            }
        }
    }

    // 플레이어가 밟았을 때 점수 추가 (기존 유니런 로직)
    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.collider.tag == "Player" && !stepped) {
            stepped = true;
            GameManager.instance.AddScore(1);
        }
    }
}