using UnityEngine;

public class PlatformSpawner : MonoBehaviour {
    public GameObject platformPrefab; // 생성할 발판의 원본 프리팹
    public int count = 3; // 생성할 발판의 개수

    public float timeBetSpawnMin = 1.25f; // 다음 발판 생성까지의 최소 시간
    public float timeBetSpawnMax = 2.25f; // Next 발판 생성까지의 최대 시간
    private float timeBetSpawn; // 실제 랜덤하게 결정된 생성 간격

    public float yMin = -2.0f; // 발판이 배치될 최소 Y 높이
    public float yMax = 2.0f;  // 발판이 배치될 최대 Y 높이
    private float xSpawnPosition = 20f; // 발판이 화면 우측 밖 어디서 생성될지 지정

    private GameObject[] platforms; // 미리 만들어둘 발판 배열 (오브젝트 풀)
    private int currentIndex = 0; // 사용할 발판의 순서(인덱스)

    // [수정 완료] 뒤에 닫는 괄호 ')'가 빠져있던 버그를 고쳤습니다!
    private Vector2 poolPosition = new Vector2(0, -25f); 
    private float lastSpawnTime; // 마지막으로 발판을 생성(재배치)한 시점


    private void Start() {
        // count 개수만큼 미리 발판을 생성하여 보이지 않는 지하에 숨겨둠 (오브젝트 풀링)
        platforms = new GameObject[count];

        for (int i = 0; i < count; i++) {
            platforms[i] = Instantiate(platformPrefab, poolPosition, Quaternion.identity);
        }

        lastSpawnTime = 0f;
        timeBetSpawn = 0f;
    }

    private void Update() {
        if (GameManager.instance.isGameover) return;

        // 순서가 되면 발판을 화면 우측 밖(xSpawnPosition)에서 Y값을 랜덤으로 조절해 재배치
        if (Time.time >= lastSpawnTime + timeBetSpawn) {
            lastSpawnTime = Time.time;
            timeBetSpawn = Random.Range(timeBetSpawnMin, timeBetSpawnMax);

            float ySpawnPosition = Random.Range(yMin, yMax);

            platforms[currentIndex].SetActive(false);
            platforms[currentIndex].SetActive(true);

            platforms[currentIndex].transform.position = new Vector2(xSpawnPosition, ySpawnPosition);

            currentIndex++;

            if (currentIndex >= count) {
                currentIndex = 0;
            }
        }
    }
}