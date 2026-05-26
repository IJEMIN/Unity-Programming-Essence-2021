using UnityEngine;

public class ScrollingObject : MonoBehaviour {
    public float speed = 5f; // 이동 속도

    private void Update() {
        // 게임오버가 아닐 때만 "이 스크립트가 붙은 본인"만 왼쪽으로 이동
        if (GameManager.instance != null && !GameManager.instance.isGameover) {
            transform.Translate(Vector2.left * speed * Time.deltaTime);
        }
    }
}