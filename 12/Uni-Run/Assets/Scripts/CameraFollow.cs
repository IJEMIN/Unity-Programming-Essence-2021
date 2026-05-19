using UnityEngine;

public class CameraFollow : MonoBehaviour {
    public Transform player; // 플레이어를 드래그해서 넣으세요
    public float smoothSpeed = 5f;
    private float maxVisibleY = 0f; // 플레이어가 도달한 최고 높이

    void LateUpdate() {
        if (player == null) return;

        // 플레이어가 현재 카메라 위치보다 더 위로 올라가려고 할 때만 카메라를 올립니다.
        if (player.position.y > transform.position.y) {
            Vector3 targetPos = new Vector3(transform.position.x, player.position.y, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, targetPos, smoothSpeed * Time.deltaTime);
        }
    }
}