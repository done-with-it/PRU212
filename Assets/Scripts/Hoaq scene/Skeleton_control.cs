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

    private bool isAttacking = false;
    private bool isHit = false;
    private bool isDead = false;
    private AttackState currentAttackState;

    private SpriteRenderer spriteRenderer; // Thêm biến để điều khiển SpriteRenderer

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
        currentPoint = pointB.transform;
        animator.SetBool("isRunning", true);

        attackCollider = GetComponent<BoxCollider2D>();
        currentHealth = maxHealth;

        spriteRenderer = GetComponent<SpriteRenderer>(); // Lấy component SpriteRenderer
    }

    private void Update()
    {
        if (hasBarrier && Time.time >= barrierEndTime)
        {
            DeactivateBarrier();
        }

        float distanceToPlayer = Vector2.Distance(transform.position, character.position);
        if (distanceToPlayer <= attackRange && Time.time - lastAttackTime > attackCooldown)
        {
            int randomAttack = Random.Range(1, 3);
            if (randomAttack == 1)
            {
                Attack1();
            }
            else
            {
                Attack2();
            }
        }

        if (!isAttacking && !isHit && !hasBarrier && !isDead)
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
        }
        else
        {
            rb.velocity = Vector2.zero;
        }

        if (currentHealth <= 0 && !isDead)
        {
            Die();
        }
    }

    void ActivateBarrier()
    {
        hasBarrier = true;
        barrierEndTime = Time.time + barrierDuration;
        animator.SetBool("Shield", true);
    }

    void DeactivateBarrier()
    {
        hasBarrier = false;
        animator.SetBool("Shield", false);
    }

    private void Attack1()
    {
        lastAttackTime = Time.time;
        isAttacking = true;
        attackCollider.enabled = true;
        animator.SetTrigger("attack1");
        currentAttackState = AttackState.Attack1;
        //StartCoroutine(AttackWithDelay(2f));
    }

    private void Attack2()
    {
        lastAttackTime = Time.time;
        isAttacking = true;
        attackCollider.enabled = true;
        animator.SetTrigger("attack2");
        currentAttackState = AttackState.Attack2;
        //StartCoroutine(AttackWithDelay(2f));
    }

    private IEnumerator AttackWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        TakeDamage(20);
    }

    private void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            isHit = true;
            animator.SetTrigger("isHit");
        }
    }

    private void Die()
    {
        isDead = true;
        animator.SetTrigger("isDeath");

        // Khi animation chết kết thúc, làm mờ vật thể và sau 5 giây destroy
        StartCoroutine(FadeOutAndDestroy(5f));
    }

    private IEnumerator FadeOutAndDestroy(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Làm mờ vật thể
        Color color = spriteRenderer.color;
        while (color.a > 0)
        {
            color.a -= Time.deltaTime / delay;
            spriteRenderer.color = color;
            yield return null;
        }

        // Destroy vật thể
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
                ActivateBarrier();
            }
        }
    }

    private void DeactivateAttackCollider()
    {
        attackCollider.enabled = false;
        isAttacking = false;
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
