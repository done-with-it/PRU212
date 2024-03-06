using System.Collections;
using System.Collections.Generic;
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
    private BoxCollider2D attackCollider;
    private bool hasBarrier = false;
    public float barrierDuration = 2f;
    private float barrierEndTime;
    public int maxHealth = 100;
    private int currentHealth;
    public GameObject hitEffectPrefab;

    private enum AttackState
    {
        None,
        Attack1,
        Attack2
    }

    private AttackState currentAttackState = AttackState.None;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentPoint = pointB.transform;
        animator.SetBool("isRunning", true);

        // Lấy BoxCollider2D từ GameObject của Skeleton
        attackCollider = GetComponent<BoxCollider2D>();

        // Khởi tạo máu ban đầu
        currentHealth = maxHealth;
    }


    private void Update()
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

        // Kiểm tra và kích hoạt barrier
        if (hasBarrier && Time.time >= barrierEndTime)
        {
            DeactivateBarrier();
        }

        // Kiểm tra khoảng cách với player
        float distanceToPlayer = Vector2.Distance(transform.position, character.position);
        if (distanceToPlayer <= attackRange && Time.time - lastAttackTime > attackCooldown)
        {
            int randomAttack = Random.Range(1, 3); // Chọn ngẫu nhiên giữa attack1 và attack2
            if (randomAttack == 1)
            {
                Attack1();
            }
            else
            {
                Attack2();
            }
        }
    }
    void ActivateBarrier()
    {
        hasBarrier = true;
        barrierEndTime = Time.time + 2f; // Thời gian bất tử khi barrier được kích hoạt (ví dụ: 2 giây)
                                         // Thực hiện các hành động khi có barrier, ví dụ: thay đổi màu sắc, hiệu ứng, ...
    }

    void DeactivateBarrier()
    {
        hasBarrier = false;
        // Thực hiện các hành động khi không còn barrier, ví dụ: đổi lại màu sắc ban đầu, ...
    }



    private void Attack1()
    {
        lastAttackTime = Time.time;
        // Kích hoạt collider tấn công
        attackCollider.enabled = true;
        animator.SetTrigger("attack1"); // Kích hoạt animation attack1

        // Tắt collider sau một khoảng thời gian attackCooldown
        Invoke("DeactivateAttackCollider", attackCooldown);

        // Chuyển sang trạng thái tấn công 1
        currentAttackState = AttackState.Attack1;

        // Giảm máu của Skeleton
        TakeDamage(20); // Số máu mất sau mỗi lần tấn công
    }

    private void Attack2()
    {
        lastAttackTime = Time.time;
        // Kích hoạt collider tấn công
        attackCollider.enabled = true;
        animator.SetTrigger("attack2"); // Kích hoạt animation attack2

        // Tắt collider sau một khoảng thời gian attackCooldown
        Invoke("DeactivateAttackCollider", attackCooldown);

        // Chuyển sang trạng thái tấn công 2
        currentAttackState = AttackState.Attack2;

        // Giảm máu của Skeleton
        TakeDamage(30); // Số máu mất sau mỗi lần tấn công
    }

    private void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // Xử lý khi Skeleton chết, ví dụ: phát hiệu ứng, phá hủy đối tượng, ...

        Destroy(gameObject);
    }
    public void TakeDamagePlayer(int damage)
    {
        if (!hasBarrier)
        {
            currentHealth -= damage;

            if (currentHealth <= 0)
            {
                Die();
            }
            else
            {
                // Kích hoạt barrier
                ActivateBarrier();
            }
        }
    }



    private void DeactivateAttackCollider()
    {
        attackCollider.enabled = false;
        // Chuyển về trạng thái tấn công None sau khi tấn công kết thúc
        currentAttackState = AttackState.None;
    }


    private void flip()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(pointA.transform.position, 0.5f);
        Gizmos.DrawWireSphere(pointB.transform.position, 0.5f);
        Gizmos.DrawLine(pointA.transform.position, pointB.transform.position);
    }
}
