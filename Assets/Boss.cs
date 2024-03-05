using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public Transform player;
    public Rigidbody2D rb;
    public bool isFlipped = false;
    public void LookAtPlayer()
    {
        Vector3 targetDirection = player.position - transform.position;
        targetDirection.y = 0f; // Chỉ xoay theo trục ngang

        // Kiểm tra hướng di chuyển của người chơi
        if (targetDirection.x > 0 && isFlipped)
        {
            // Người chơi đang di chuyển sang phía bên phải của con boss và con boss đang flipped
            // Không cần thực hiện việc quay của con boss
            return;
        }
        else if (targetDirection.x < 0 && !isFlipped)
        {
            // Người chơi đang di chuyển sang phía bên trái của con boss và con boss chưa flipped
            // Không cần thực hiện việc quay của con boss
            return;
        }
        Vector3 flipped = transform.localScale;
        flipped.z *= -1f;

        if (transform.position.x > player.position.x && isFlipped)
        {
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            isFlipped = false;
        }
        else if (transform.position.x < player.position.x && !isFlipped)
        {
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            isFlipped = true;
        }
    }
    void Start()
    {
        // Kiểm tra nếu Rigidbody2D đã được gán
        if (rb != null)
        {
            rb.freezeRotation = true; // Khoá quay của Rigidbody2D theo trục
        }
        else
        {
            Debug.LogError("Rigidbody2D has not been assigned!"); // Hiển thị thông báo lỗi nếu Rigidbody2D chưa được gán
        }
    }
    // Start is called before the first frame update

}
