using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Skeleton_control : MonoBehaviour
{
    public GameObject pointA;
    public GameObject pointB;
    public Transform character;
    private Rigidbody2D rb;
    private Animator animator;
    private Transform currentPoint;
    public float speed;
    public float attackRange;
    public float attackCooldown;
    private float lastAttackTime;
    public GameObject attackHitbox;

    private enum MovementState
    {
        attack, hit, die
    }
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentPoint = pointB.transform;
        animator.SetBool("isRunning", true);
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 point = currentPoint.position - transform.position;
        if (currentPoint == pointB.transform)
        {
            rb.velocity = new Vector2(speed, 0);
        }
        else
        {
            rb.velocity = new Vector2(-speed, 0);
        }

        if (Vector2.Distance(transform.position, currentPoint.position) < 0.5f && currentPoint == pointB.transform)
        {
            flip();
            currentPoint = pointA.transform;
        }
        if (Vector2.Distance(transform.position, currentPoint.position) < 0.5f && currentPoint == pointA.transform)
        {
            flip();
            currentPoint = pointB.transform;
        }

        // Kiểm tra khoảng cách với player
        float distanceToPlayer = Vector2.Distance(transform.position, character.position);
        if (distanceToPlayer <= attackRange && Time.time - lastAttackTime > attackCooldown)
        {
            // Tấn công
            Attack();
        }
    }
    void Attack()
    {
        lastAttackTime = Time.time;
        // Kích hoạt hitbox tấn công
        attackHitbox.SetActive(true);
        animator.SetTrigger("attack"); // Kích hoạt animation attack

        // Tắt hitbox sau một khoảng thời gian attackCooldown
        Invoke("DeactivateAttackHitbox", attackCooldown);
    }

    void DeactivateAttackHitbox()
    {
        attackHitbox.SetActive(false);
    }


    private void flip()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }
    public void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(pointA.transform.position, 0.5f);
        Gizmos.DrawWireSphere(pointB.transform.position, 0.5f);
        Gizmos.DrawLine(pointA.transform.position, pointB.transform.position);
    }

}
