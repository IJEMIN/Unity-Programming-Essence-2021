using System.Collections;
using UnityEngine;

// 총을 구현
public class Gun : MonoBehaviour {
    // 총의 상태를 표현하는 데 사용할 타입을 선언
    public enum State {
        Ready, // 발사 준비됨
        Empty, // 탄알집이 빔
        Reloading // 재장전 중
    }

    public State state { get; private set; } // 현재 총의 상태

    public Transform fireTransform; // 탄알이 발사될 위치

    public ParticleSystem muzzleFlashEffect; // 총구 화염 효과
    public ParticleSystem shellEjectEffect; // 탄피 배출 효과

    private LineRenderer bulletLineRenderer; // 탄알 궤적을 그리기 위한 렌더러
    private AudioSource gunAudioPlayer; // 총 소리 재생기

    public GunData gunData; // 총의 현재 데이터

    private float fireDistance = 50f; // 사정거리

    public int ammoRemain = 100; // 남은 전체 탄알
    public int magAmmo; // 현재 탄알집에 남아 있는 탄알

    private float lastFireTime; // 총을 마지막으로 발사한 시점

    // 재장전 애니메이션을 제어하기 위한 애니메이터 컴포넌트 참조
    private Animator playerAnimator; 

    private void Awake() {
        // 사용할 컴포넌트의 참조 가져오기
        gunAudioPlayer = GetComponent<AudioSource>();
        bulletLineRenderer = GetComponent<LineRenderer>();
        bulletLineRenderer.positionCount = 2;
        bulletLineRenderer.enabled = false;
        
        // 부모 오브젝트나 자신에게서 Animator 컴포넌트를 안전하게 찾아옴
        playerAnimator = GetComponentInParent<Animator>(); 
    }

    private void OnEnable() {
        // 총 상태 초기화
        ammoRemain = gunData.startAmmoRemain;
        magAmmo = gunData.magCapacity;
        state = State.Ready;
        lastFireTime = 0;
    }

    // 발사 시도
    public void Fire() {
        // 준비 상태이고, 쿨타임이 지났으며, 탄창에 총알이 있을 때만 발사 가능
        if (state == State.Ready && Time.time >= lastFireTime + gunData.timeBetFire && magAmmo > 0) {
            lastFireTime = Time.time;
            Shot();
        }
    }

    // 실제 발사 처리
    private void Shot() {
        RaycastHit hit;
        Vector3 hitPosition = Vector3.zero;

        // [오타 수정 완료] Physics 대소문자 및 position 철자 수정, 느낌표(!) 제거 완료
        if (Physics.Raycast(fireTransform.position, fireTransform.forward, out hit, fireDistance)) {
            hitPosition = hit.point;
        } else {
            hitPosition = fireTransform.position + fireTransform.forward * fireDistance;
        }

        // 총을 쐈으니 탄창의 총알을 1발 감소시킴 (무한 재장전 방지)
        magAmmo--;

        // 발사 이펙트 재생
        StartCoroutine(ShotEffect(hitPosition));

        // 탄창이 다 떨어졌다면 상태를 Empty로 변경
        if (magAmmo <= 0) {
            state = State.Empty;
        }
    }

    // 발사 이펙트와 소리를 재생하고 탄알 궤적을 그림
    private IEnumerator ShotEffect(Vector3 hitPosition) {
        // [오타 수정 완료] 대문자 Play()로 정상 작동
        muzzleFlashEffect.Play();
        shellEjectEffect.Play();
        gunAudioPlayer.PlayOneShot(gunData.shotClip);
        
        bulletLineRenderer.SetPosition(0, fireTransform.position);
        bulletLineRenderer.SetPosition(1, hitPosition);
        bulletLineRenderer.enabled = true;

        yield return new WaitForSeconds(0.03f);

        bulletLineRenderer.enabled = false;
    }

    // 재장전 시도
    public bool Reload() {
        if (state == State.Reloading || ammoRemain <= 0 || magAmmo >= gunData.magCapacity) {
            return false;
        }
        
        StartCoroutine(ReloadRoutine());
        return true; 
    }

    // 실제 재장전 처리를 진행
    private IEnumerator ReloadRoutine() {
        state = State.Reloading;
        gunAudioPlayer.PlayOneShot(gunData.reloadClip);
      
        // 재장전 애니메이션 연동
        if (playerAnimator != null) {
            playerAnimator.SetTrigger("Reload");
        }

        yield return new WaitForSeconds(gunData.reloadTime);

        // 탄창을 실제로 채워주는 핵심 계산 로직 (무한 루프 방지)
        int ammoToFill = gunData.magCapacity - magAmmo; 

        if (ammoRemain >= ammoToFill) {
            magAmmo = gunData.magCapacity;
            ammoRemain -= ammoToFill;
        } 
        else {
            magAmmo += ammoRemain;
            ammoRemain = 0;
        }

        // 총의 현재 상태를 다시 발사 준비 완료 상태로 변경
        state = State.Ready;
    }
}