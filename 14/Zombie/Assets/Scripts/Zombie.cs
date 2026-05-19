using System.Collections;
using UnityEngine;
using UnityEngine.AI; // NavMeshAgent 사용을 위해 필수

// 좀비 AI 구현
public class Zombie : LivingEntity
{
    public LayerMask whatIsTarget; // 추적 대상 레이어

    private LivingEntity targetEntity; // 추적 대상
    private NavMeshAgent navMeshAgent; // 경로 계산 AI 에이전트

    public ParticleSystem hitEffect; // 피격 시 재생할 파티클 효과
    public AudioClip deathClip; // 사망 효과음
    public AudioClip hitClip; // 피격 효과음

    private AudioSource zombieAudioPlayer; // 오디오 소스 재생기
    private Animator zombieAnimator; // 애니메이터 컴포넌트

    public float damage = 20f; // 공격력
    public float timeBetAttack = 0.5f; // 공격 주기
    private float lastAttackTime; // 마지막 공격 시점

    // 추적할 대상이 존재하는지 확인하는 프로퍼티
    private bool hasTarget
    {
        get
        {
            // 추적할 대상이 존재하고, 대상이 사망하지 않았다면 true
            if (targetEntity != null && !targetEntity.dead)
            {
                return true;
            }
            // 그렇지 않다면 false
            return false;
        }
    }

    private void Awake()
    {
        // 게임 오브젝트로부터 사용할 컴포넌트 가져오기
        navMeshAgent = GetComponent<NavMeshAgent>();
        zombieAnimator = GetComponent<Animator>();
        zombieAudioPlayer = GetComponent<AudioSource>();
    }

    // 좀비 AI의 초기 스펙을 결정하는 셋업 메서드
    public void Setup(float newHealth, float newDamage, float newSpeed, Color skinColor)
    {
        // 체력 설정
        startingHealth = newHealth;
        health = newHealth;
        // 공격력 설정
        damage = newDamage;
        // 내비메시 에이전트의 이동 속도 설정
        navMeshAgent.speed = newSpeed;
        // 렌더러 컴포넌트의 컬러 변경 (자식 오브젝트의 렌더러를 찾아내어 적용)
        GetComponentInChildren<Renderer>().material.color = skinColor;
    }

    private void Start()
    {
        // 게임 오브젝트 활성화와 동시에 AI의 추적 루틴 시작
        StartCoroutine(UpdatePath());
    }

    private void Update()
    {
        // 추적 대상이 존재하며, 현재 시점이 마지막 공격 시점 + 공격 주기보다 크거나 같다면
        if (hasTarget && Time.time >= lastAttackTime + timeBetAttack)
        {
            // 추적 대상과 자신 간의 거리를 계산
            float distance = Vector3.Distance(transform.position, targetEntity.transform.position);

            // 거리가 에이전트의 공격 사정거리(stoppingDistance) 이내라면 공격 실행
            if (distance <= navMeshAgent.stoppingDistance)
            {
                lastAttackTime = Time.time;
                // 공격 대상의 크래시 포인트를 대략 계산하여 이펙트 위치로 지정
                Vector3 hitPoint = targetEntity.transform.position + Vector3.up * 0.5f;
                // 공격 실행
                OnAttack(targetEntity, hitPoint);
            }
        }

        // 추적 대상의 존재 여부에 따라 애니메이터의 HasTarget 파라미터를 갱신
        zombieAnimator.SetBool("HasTarget", hasTarget);
    }

    // 주기적으로 추적 대상의 위치를 찾아 경로를 갱신하는 루틴
    private IEnumerator UpdatePath()
    {
        // 살아 있는 동안 무한 루프
        while (!dead)
        {
            if (hasTarget)
            {
                // ⚠️ [안전장치 추가] 에이전트가 활성화되어 있고, 내비메시 바닥 위에 안착했을 때만 경로 지정
                if (navMeshAgent.enabled && navMeshAgent.isOnNavMesh)
                {
                    navMeshAgent.isStopped = false;
                    navMeshAgent.SetDestination(targetEntity.transform.position);
                }
            }
            else
            {
                // ⚠️ [안전장치 추가] 내비메시 위에 있을 때만 정지 명령 수행
                if (navMeshAgent.enabled && navMeshAgent.isOnNavMesh)
                {
                    navMeshAgent.isStopped = true;
                }

                // 20단위의 반경 내에서 whatIsTarget 레이어를 가진 콜라이더들을 감지
                Collider[] colliders = Physics.OverlapSphere(transform.position, 20f, whatIsTarget);

                // 감지된 모든 콜라이더를 순회
                for (int i = 0; i < colliders.Length; i++)
                {
                    // 상대방으로부터 LivingEntity 컴포넌트 가져오기 시도
                    LivingEntity livingEntity = colliders[i].GetComponent<LivingEntity>();

                    // 상대방 LivingEntity가 존재하며, 살아 있다면 추적 대상으로 지정
                    if (livingEntity != null && !livingEntity.dead)
                    {
                        targetEntity = livingEntity;
                        break; // 루프 탈출
                    }
                }
            }

            // 0.25초 주기로 처리를 대기하며 무한 반복
            yield return new WaitForSeconds(0.25f);
        }
    }

    // 대미지를 입었을 때 실행되는 메서드 (LivingEntity의 OnDamage 오버라이드)
    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        // 아직 사망하지 않았다면 피격 이펙트와 소리 재생
        if (!dead)
        {
            hitEffect.transform.position = hitPoint;
            hitEffect.transform.rotation = Quaternion.LookRotation(hitNormal);
            hitEffect.Play();

            zombieAudioPlayer.PlayOneShot(hitClip);
        }

        // LivingEntity의 오리지널 OnDamage 메서드를 실행하여 실제 체력 차감
        base.OnDamage(damage, hitPoint, hitNormal);
    }

    // 공격을 실행하는 메서드
    private void OnAttack(LivingEntity target, Vector3 hitPoint)
    {
        // 상대방에게 대미지 적용 (자신의 공격력, 맞은 위치, 공격 방향 전달)
        target.OnDamage(damage, hitPoint, transform.forward);
        // 공격 애니메이션 트리거 작동
        zombieAnimator.SetTrigger("Attack");
        // 공격 효과음 재생
        zombieAudioPlayer.PlayOneShot(hitClip);
    }

    // 사망 시 실행되는 메서드 (LivingEntity의 Die 오버라이드)
    public override void Die()
    {
        // LivingEntity의 오리지널 Die 메서드를 실행하여 사망 처리 적용
        base.Die();

        // 다른 캐릭터들과의 충돌을 방지하기 위해 자신에게 붙은 모든 콜라이더 비활성화
        Collider[] zombieColliders = GetComponents<Collider>();
        for (int i = 0; i < zombieColliders.Length; i++)
        {
            zombieColliders[i].enabled = false;
        }

        // ⚠️ 사망 처리 시에도 에이전트가 내비메시 위에 있을 때만 정지 처리를 하도록 안전하게 보호
        if (navMeshAgent.enabled && navMeshAgent.isOnNavMesh)
        {
            navMeshAgent.isStopped = true;
        }
        navMeshAgent.enabled = false;

        // 사망 애니메이션 트리거 작동
        zombieAnimator.SetTrigger("Die");
        // 사망 효과음 재생
        zombieAudioPlayer.PlayOneShot(deathClip);
    }
}