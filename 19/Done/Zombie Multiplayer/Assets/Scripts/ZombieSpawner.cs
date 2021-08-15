using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using UnityEngine;


// 좀비 게임 오브젝트를 주기적으로 생성
public class ZombieSpawner : MonoBehaviourPun, IPunObservable {
    public Zombie zombiePrefab; // 생성할 좀비 원본 프리팹
    
    public ZombieData[] zombieDatas; // 사용할 좀비 셋업 데이터들
    public Transform[] spawnPoints; // 좀비 AI를 소환할 위치들
    
    private List<Zombie> zombies = new List<Zombie>(); // 생성된 좀비들을 담는 리스트

    private int zombieCount = 0; // 남은 좀비 수
    private int wave; // 현재 웨이브

    // 주기적으로 자동 실행되는, 동기화 메서드
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
        // 로컬 오브젝트라면 쓰기 부분이 실행됨
        if (stream.IsWriting)
        {
            // 남은 좀비 수를 네트워크를 통해 보내기
            stream.SendNext(zombies.Count);
            // 현재 웨이브를 네트워크를 통해 보내기
            stream.SendNext(wave);
        }
        else
        {
            // 리모트 오브젝트라면 읽기 부분이 실행됨
            // 남은 좀비 수를 네트워크를 통해 받기
            zombieCount = (int) stream.ReceiveNext();
            // 현재 웨이브를 네트워크를 통해 받기 
            wave = (int) stream.ReceiveNext();
        }
    }

    private void Awake() {
        PhotonPeer.RegisterType(typeof(Color), 128, ColorSerialization.SerializeColor,
            ColorSerialization.DeserializeColor);
    }

    private void Update() {
        // 호스트만 좀비를 직접 생성할 수 있음
        // 다른 클라이언트들은 호스트가 생성한 좀비를 동기화를 통해 받아옴
        if (PhotonNetwork.IsMasterClient)
        {
            // 게임 오버 상태일때는 생성하지 않음
            if (GameManager.instance != null && GameManager.instance.isGameover)
            {
                return;
            }

            // 좀비들을 모두 물리친 경우 다음 스폰 실행
            if (zombies.Count <= 0)
            {
                SpawnWave();
            }
        }

        // UI 갱신
        UpdateUI();
    }

    // 웨이브 정보를 UI로 표시
    private void UpdateUI() {
        if (PhotonNetwork.IsMasterClient)
        {
            // 호스트는 직접 갱신한 좀비 리스트를 통해 남은 좀비의 수를 표시함
            UIManager.instance.UpdateWaveText(wave, zombies.Count);
        }
        else
        {
            // 클라이언트는 좀비 리스트를 갱신할 수 없으므로, 호스트가 보내준 zombieCount를 통해 좀비의 수를 표시함
            UIManager.instance.UpdateWaveText(wave, zombieCount);
        }
    }

    // 현재 웨이브에 맞춰 좀비를 생성
    private void SpawnWave() {
        // 웨이브 1 증가
        wave++;

        // 현재 웨이브 * 1.5에 반올림 한 개수 만큼 좀비를 생성
        int spawnCount = Mathf.RoundToInt(wave * 1.5f);

        // spawnCount 만큼 좀비 생성
        for (int i = 0; i < spawnCount; i++)
        {
            // 좀비 생성 처리 실행
            CreateZombie();
        }
    }

    // 좀비 생성
    private void CreateZombie() {
        // 사용할 좀비 데이터 랜덤으로 결정
        ZombieData zombieData = zombieDatas[Random.Range(0, zombieDatas.Length)];

        // 생성할 위치를 랜덤으로 결정
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        // 좀비 프리팹으로부터 좀비 생성, 네트워크 상의 모든 클라이언트들에게 생성됨
        GameObject createdZombie = PhotonNetwork.Instantiate(zombiePrefab.gameObject.name,
            spawnPoint.position,
            spawnPoint.rotation);
        
        // 생성한 좀비를 셋업하기 위해 Zombie 컴포넌트를 가져옴
        Zombie zombie = createdZombie.GetComponent<Zombie>();

        // 생성한 좀비의 능력치 설정
        zombie.photonView.RPC("Setup", RpcTarget.All, zombieData.health, zombieData.damage, zombieData.speed, zombieData.skinColor);

        // 생성된 좀비를 리스트에 추가
        zombies.Add(zombie);

        // 좀비의 onDeath 이벤트에 익명 메서드 등록
        // 사망한 좀비를 리스트에서 제거
        zombie.onDeath += () => zombies.Remove(zombie);
        // 사망한 좀비를 10 초 뒤에 파괴
        zombie.onDeath += () => StartCoroutine(DestroyAfter(zombie.gameObject, 10f));
        // 좀비 사망시 점수 상승
        zombie.onDeath += () => GameManager.instance.AddScore(100);
    }

    // 포톤의 Network.Destroy()는 지연 파괴를 지원하지 않으므로 지연 파괴를 직접 구현함
    IEnumerator DestroyAfter(GameObject target, float delay) {
        // delay 만큼 쉬고
        yield return new WaitForSeconds(delay);
    
        // target이 아직 파괴되지 않았다면
        if (target != null)
        {
            // target을 모든 네트워크 상에서 파괴
            PhotonNetwork.Destroy(target);
        }
    }
}