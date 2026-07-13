using UnityEngine;

public class BackgroundLoop : MonoBehaviour {
    private float width; // 배경의 가로 길이

    private void Awake() {
        // BoxCollider2D의 가로 사이즈를 가져옵니다.
        BoxCollider2D backgroundCollider = GetComponent<BoxCollider2D>();
        if (backgroundCollider != null) {
            width = backgroundCollider.size.x;
        }
    }

    private void Update() {
        // 현재 오브젝트가 왼쪽으로 완전히 밀려나면 오른쪽으로 순간이동
        if (transform.position.x <= -width) {
            Reposition();
        }
    }

    private void Reposition() {
        // 현재 위치에서 오른쪽으로 가로 길이의 2배만큼 이동
        Vector2 offset = new Vector2(width * 2f, 0);
        transform.position = (Vector2)transform.position + offset;
    }
}