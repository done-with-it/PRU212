using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonMovement : MonoBehaviour
{
    public GameObject pointA;          // Điểm A cho di chuyển của quái
    public GameObject pointB;          // Điểm B cho di chuyển của quái
    public Transform character;        // Đối tượng mục tiêu để tấn công
    private Rigidbody2D rb;            // Rigidbody của quái
    private Animator animator;         // Animator của quái
    private Transform currentPoint;    // Điểm hiện tại mà quái đang đến
    public float speed;                // Tốc độ di chuyển

    private bool isDead = false;       // Kiểm tra quái đã chết

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentPoint = pointB.transform;    // Bắt đầu từ điểm B
        animator.SetBool("isRunning", true); // Bắt đầu ở trạng thái chạy
    }

    private void Update()
    {
        // Nếu không chết
        if (!isDead)
        {
            // Di chuyển đến điểm tiếp theo
            Vector2 point = currentPoint.position - transform.position;
            if (currentPoint == pointB.transform)
            {
                rb.velocity = new Vector2(speed, 0);
            }
            else
            {
                rb.velocity = new Vector2(-speed, 0);
            }

            // Đổi hướng khi đến gần điểm và chuyển điểm
            if (Vector2.Distance(transform.position, currentPoint.position) < 0.5f && currentPoint == pointB.transform)
            {
                Flip();
                currentPoint = pointA.transform;
            }
            if (Vector2.Distance(transform.position, currentPoint.position) < 0.5f && currentPoint == pointA.transform)
            {
                Flip();
                currentPoint = pointB.transform;
            }
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }

    // Đổi hướng quái
    private void Flip()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }
}
