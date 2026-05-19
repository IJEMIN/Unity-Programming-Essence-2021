using UnityEngine;

public class BackgroundLoop : MonoBehaviour {
    private float height; 

    private void Awake() {
        // 배경의 세로 길이를 측정
        BoxCollider2D backgroundCollider = GetComponent<BoxCollider2D>();
        height = backgroundCollider.size.y;
    }

    // Update에서 Translate(이동) 코드를 아예 삭제했습니다.
    private void Update() {
        // 카메라가 위로 올라가서 배경이 화면 아래로 사라질 때쯤
        // 배경을 위로 텔레포트 시켜서 무한히 이어지게 만듭니다.
        if (Camera.main.transform.position.y > transform.position.y + height) {
            Reposition();
        }
    }

    private void Reposition() {
        // 배경을 위쪽으로 두 칸 이동 (무한 루프)
        Vector2 offset = new Vector2(0, height * 2f);
        transform.position = (Vector2)transform.position + offset;
    }
}