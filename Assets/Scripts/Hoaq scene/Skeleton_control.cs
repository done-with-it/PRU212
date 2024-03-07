using System.Collections;
using UnityEngine;

public class Skeleton_control : MonoBehaviour
{
    public GameObject pointA;          // Điểm A cho di chuyển của quái
    public GameObject pointB;          // Điểm B cho di chuyển của quái
    public Transform character;        // Đối tượng mục tiêu để tấn công
    private Rigidbody2D rb;            // Rigidbody của quái
    private Animator animator;         // Animator của quái
    private Transform currentPoint;    // Điểm hiện tại mà quái đang đến
    public float speed;                // Tốc độ di chuyển
    public float attackRange;          // Phạm vi tấn công
    public float attackCooldown;       // Thời gian chờ giữa các lần tấn công
    private float lastAttackTime;      // Thời điểm lần tấn công cuối cùng
    private BoxCollider2D attackCollider;  // Collider của vùng tấn công
    private bool hasBarrier = false;   // Kiểm tra quái có sử dụng Barrier không
    public float barrierDuration = 2f; // Thời gian Barrier tồn tại
    private float barrierEndTime;      // Thời điểm kết thúc Barrier
    public int maxHealth = 100;        // Máu tối đa của quái
    private int currentHealth;         // Máu hiện tại của quái
    public GameObject hitEffectPrefab; // Prefab hiệu ứng khi bị đánh

    private bool isAttacking = false;  // Kiểm tra quái đang tấn công
    private bool isHit = false;        // Kiểm tra quái bị tấn công
    private bool isDead = false;       // Kiểm tra quái đã chết
    private AttackState currentAttackState;  // Trạng thái tấn công hiện tại

    // Trạng thái của tấn công
    private enum AttackState
    {
        None,
        Attack1,
        Attack2
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentPoint = pointB.transform;    // Bắt đầu từ điểm B
        animator.SetBool("isRunning", true); // Bắt đầu ở trạng thái chạy

        attackCollider = GetComponent<BoxCollider2D>();
        currentHealth = maxHealth;   // Khởi tạo máu

        // Khởi tạo các giá trị ban đầu
    }

    private void Update()
    {
        // Kiểm tra nếu Barrier đang được sử dụng và đã hết thời gian
        if (hasBarrier && Time.time >= barrierEndTime)
        {
            DeactivateBarrier();
        }

        // Tính khoảng cách đến đối tượng mục tiêu
        float distanceToPlayer = Vector2.Distance(transform.position, character.position);
        // Kiểm tra nếu đối tượng mục tiêu nằm trong phạm vi tấn công và đã hết cooldown
        if (distanceToPlayer <= attackRange && Time.time - lastAttackTime > attackCooldown)
        {
            StartCoroutine(PerformRandomAttack());
        }
        else
        {
            isAttacking = false;
        }

        // Nếu không đang tấn công, bị tấn công, không có Barrier, và chưa chết
        if (!isAttacking && !isHit && !hasBarrier && !isDead)
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
                flip();
                currentPoint = pointA.transform;
            }
            if (Vector2.Distance(transform.position, currentPoint.position) < 0.5f && currentPoint == pointA.transform)
            {
                flip();
                currentPoint = pointB.transform;
            }
        }

        // Kiểm tra nếu máu dưới 0 và chưa chết
        if (currentHealth <= 0 && !isDead)
        {
            Die();
        }
        isHit = false;
    }

    // Coroutine thực hiện tấn công ngẫu nhiên
    private IEnumerator PerformRandomAttack()
    {
        if (!isAttacking)
        {
            isAttacking = true;

            int randomAttack = Random.Range(1, 3);
            if (randomAttack == 1)
            {
                var tempspeed = speed;
                speed = 0;
                Attack1();
                yield return new WaitForSeconds(1f);
                speed = tempspeed;
            }
            else
            {
                var tempspeed = speed;
                speed = 0;
                Attack2();
                yield return new WaitForSeconds(1f);
                speed = tempspeed;
            }

            isAttacking = false;
            lastAttackTime = Time.time;
        }
    }

    // Kích hoạt Barrier
    void ActivateBarrier()
    {
        hasBarrier = true;
        barrierEndTime = Time.time + barrierDuration;
        animator.SetBool("barrier", true);
    }

    // Tắt Barrier
    void DeactivateBarrier()
    {
        hasBarrier = false;
        animator.SetBool("barrier", false);
    }

    // Tấn công 1
    private void Attack1()
    {
        lastAttackTime = Time.time;
        isAttacking = true;
        attackCollider.enabled = true;
        animator.SetTrigger("attack1");
        currentAttackState = AttackState.Attack1; // Chuyển sang trạng thái tấn công 1
    }

    // Tấn công 2
    private void Attack2()
    {
        lastAttackTime = Time.time;
        isAttacking = true;
        attackCollider.enabled = true;
        animator.SetTrigger("attack2");
        currentAttackState = AttackState.Attack2; // Chuyển sang trạng thái tấn công 2
    }

    // Hàm chết
    private void Die()
    {
        isDead = true;
        animator.SetBool("isDeath", true);
        this.enabled = false;
        
        Invoke("FadeOutAndDestroy", 5f); // Sau 5 giây gọi hàm FadeOutAndDestroy
        Debug.Log("Enemy die!");
    }

    // Hàm xóa đối tượng sau khi chết
    private void FadeOutAndDestroy()
    {
        Destroy(gameObject);
    }

    // Nhận sát thương từ player
    public void TakeDamage(int damage)
    {
        if (!hasBarrier)
        {
            currentHealth -= damage;
            animator.SetTrigger("isHit");
            if (currentHealth <= 0)
            {
                Die();
            }
            else
            {
                ActivateBarrier();
            }
        }
    }

    // Tắt Collider vùng tấn công
    private void DeactivateAttackCollider()
    {
        attackCollider.enabled = false;
        isAttacking = false;
        currentAttackState = AttackState.None;
    }

    // Đổi hướng quái
    private void flip()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    // Vẽ Gizmos để hiển thị điểm A, B và đường kẻ giữa chúng
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(pointA.transform.position, 0.5f); // Vẽ điểm A
        Gizmos.DrawWireSphere(pointB.transform.position, 0.5f); // Vẽ điểm B
        Gizmos.DrawLine(pointA.transform.position, pointB.transform.position); // Vẽ đường kẻ giữa A và B
    }

    // Gọi khi animation tấn công kết thúc
    public void EndAttackAnimation()
    {
        // Tiếp tục di chuyển sau khi kết thúc tấn công
        if (!isDead)
        {
            rb.velocity = new Vector2(speed, 0); // Hoặc -speed nếu muốn ngược lại
        }
    }
}
