using UnityEngine;

public class PlayerController : MonoBehaviour {
    public AudioClip deathClip; 
    public float moveSpeed = 5f; // 좌우 이동 속도 추가
    public float maxChargeTime = 2f; // 최대 기 모으기 시간 (2초면 풀충전)
    public float maxJumpForce = 25f; // 풀충전 시 점프 힘 (AddForce가 아닌 velocity 방식을 쓸 거라 숫자가 낮아집니다)

    private bool isGrounded = false; 
    private bool isDead = false; 

    private Rigidbody2D playerRigidbody; 
    private Animator animator; 
    private AudioSource playerAudio; 

    private bool isCharging = false; // 현재 기를 모으고 있는지 체크
    private float chargeTime = 0f; // 모인 시간
    private Vector3 originalScale; // 기 모을 때 찌그러지는 연출용

    private void Start() {
        playerRigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();
        originalScale = transform.localScale; // 처음 캐릭터 크기 저장
    }

    private void Update() {
        if (isDead) return;

        // 1. 좌우 이동 로직 (발판 위든 공중이든 기본 키보드 좌우 방향키나 A, D로 이동)
        float xInput = Input.GetAxisRaw("Horizontal");
        playerRigidbody.linearVelocity = new Vector2(xInput * moveSpeed, playerRigidbody.linearVelocity.y);

        // 2. 기 모으기 시작 (마우스 왼쪽 버튼을 누르는 순간 + 바닥에 있을 때만)
        if (isGrounded && Input.GetMouseButtonDown(0)) {
            isCharging = true;
            chargeTime = 0f;
        }

        // 3. 기 모으는 중 처리
        if (isCharging) {
            chargeTime += Time.deltaTime;
            if (chargeTime > maxChargeTime) chargeTime = maxChargeTime; // 최대치 고정

            // 시각 효과: 기를 모을 때 캐릭터가 위아래로 살짝 압축(찌그러짐)됩니다.
            transform.localScale = new Vector3(originalScale.x, originalScale.y * 0.7f, originalScale.z);

            // 마우스 버튼에서 손을 떼는 순간 점프 발사!
            if (Input.GetMouseButtonUp(0)) {
                Jump();
            }
        }

        // 애니메이터 파라미터 갱신
        animator.SetBool("Grounded", isGrounded);
    }

    private void Jump() {
        isCharging = false;
        transform.localScale = originalScale; // 캐릭터 크기 원래대로 복구

        // 기 모은 비율 계산 (0.0 ~ 1.0)
        float chargePercent = chargeTime / maxChargeTime;
        float finalJumpForce = maxJumpForce * chargePercent;

        // 순간적으로 위쪽으로 힘을 빡 주기 (Y축 속도를 직접 변경)
        playerRigidbody.linearVelocity = new Vector2(playerRigidbody.linearVelocity.x, finalJumpForce);
        
        playerAudio.Play();
    }

    private void Die() {
        animator.SetTrigger("Die");
        playerAudio.clip = deathClip;
        playerAudio.Play();

        playerRigidbody.linearVelocity = Vector2.zero;
        isDead = true;

        GameManager.instance.OnPlayerDead();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Dead" && !isDead) {
            Die();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.contacts[0].normal.y > 0.7f) {
            isGrounded = true;
            // jumpCount 변수는 이제 필요 없어서 제거했습니다.
        }
    }

    private void OnCollisionExit2D(Collision2D collision) {
        isGrounded = false;
    }
}