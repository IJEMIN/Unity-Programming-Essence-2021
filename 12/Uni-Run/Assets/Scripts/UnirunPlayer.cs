using UnityEngine;

public class UniRunPlayer : MonoBehaviour {
    public AudioClip deathClip; 
    public float jumpForce = 700f; 

    private int jumpCount = 0; 
    private bool isGrounded = false; 
    private bool isDead = false; 

    private Rigidbody2D playerRigidbody; 
    private Animator animator; 
    private AudioSource playerAudio; 

    private void Start() {
        playerRigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();
    }

    private void Update() {
        if (isDead) return;

        // [추가] 외부 마찰력이나 부모 관성 때문에 X축으로 밀리는 것을 코드로 강제 차단합니다.
        if (playerRigidbody.linearVelocity.x != 0) {
            playerRigidbody.linearVelocity = new Vector2(0, playerRigidbody.linearVelocity.y);
        }

        // 🌟 [핵심 수정] 마우스 왼쪽 버튼을 눌렀고 + 마우스가 UI(시작 버튼, 상점 버튼 등) 위에 올라와 있지 않을 때만 점프 처리!
        if (Input.GetMouseButtonDown(0) && jumpCount < 2 && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()) {
            jumpCount++;
            playerRigidbody.linearVelocity = Vector2.zero; 
            playerRigidbody.AddForce(new Vector2(0, jumpForce));
            playerAudio.Play();
        }
        else if (Input.GetMouseButtonUp(0) && playerRigidbody.linearVelocity.y > 0) {
            playerRigidbody.linearVelocity = playerRigidbody.linearVelocity * 0.5f;
        }

        animator.SetBool("Grounded", isGrounded);
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
            jumpCount = 0;
        }
    }

    private void OnCollisionExit2D(Collision2D collision) {
        isGrounded = false;
    }
}