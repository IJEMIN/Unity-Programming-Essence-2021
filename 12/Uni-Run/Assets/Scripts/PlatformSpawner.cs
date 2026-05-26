using UnityEngine;

public class PlatformSpawner : MonoBehaviour {
    public GameObject platformPrefab; 
    public int count = 30; // 위로 쌓을 발판 개수
    
    public float verticalInterval = 2.5f; // [중요] 발판 사이 높이를 3.5에서 2.5로 낮춰서 점프가 닿게 만듭니다.
    public float minX = -4.5f; 
    public float maxX = 4.5f;  
    public float minWidth = 1.5f; 
    public float maxWidth = 3f;   

    private float nextSpawnY = 2f; 
    private bool spawnOnRight = true; // 좌우 번갈아가며 생성하기 위한 변수

    void Start() {
        for (int i = 0; i < count; i++) {
            float randomX;

            // 지그재그 알고리즘: 이전 발판 위치에 따라 다음 발판 위치 영역을 강제 지정합니다.
            if (spawnOnRight) {
                randomX = Random.Range(1f, maxX); // 오른쪽 구역에 생성
            } else {
                randomX = Random.Range(minX, -1f); // 왼쪽 구역에 생성
            }
            
            // 다음 발판은 반대편에 생성되도록 뒤집기
            spawnOnRight = !spawnOnRight;

            Vector2 spawnPos = new Vector2(randomX, nextSpawnY);
            GameObject newPlatform = Instantiate(platformPrefab, spawnPos, Quaternion.identity);
            
            float randomWidth = Random.Range(minWidth, maxWidth);
            newPlatform.transform.localScale = new Vector3(randomWidth, 1f, 1f);

            nextSpawnY += verticalInterval;
        }
    }
}