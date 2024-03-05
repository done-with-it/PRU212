using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
public class GoblinMovement : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float leftBound = 0f;   // Giới hạn bên trái
    public float rightBound = 5f;  // Giới hạn bên phải

    private bool isMovingRight = true;
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        MoveGoblin();
    }

    private void MoveGoblin()
    {
        // Lấy vị trí hiện tại của quái
        Vector3 currentPosition = transform.position;

        // Xác định hướng di chuyển
        float moveDirection = isMovingRight ? 1f : -1f;

        // Di chuyển quái
        currentPosition.x += moveDirection * moveSpeed * Time.deltaTime;

        // Kiểm tra giới hạn và đảo hướng nếu cần
        if (currentPosition.x > rightBound)
        {
            currentPosition.x = rightBound;
            isMovingRight = false;
            FlipGoblin();
        }
        else if (currentPosition.x < leftBound)
        {
            currentPosition.x = leftBound;
            isMovingRight = true;
            FlipGoblin();
        }

        // Gán vị trí mới cho quái
        transform.position = currentPosition;

        // Cập nhật animation
        UpdateAnimation();
    }

    private void UpdateAnimation()
    {
        // Cập nhật logic animation dựa trên hướng di chuyển của quái
        anim.SetBool("isRunning", moveSpeed > 0.1f);
    }

    private void FlipGoblin()
    {
        // Đảo ngược hướng quái
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}

