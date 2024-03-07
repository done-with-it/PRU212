using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinMovementV2 : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public Transform player;
    public float moveSpeed = 2f;
    public float stayDuration = 3f;
    private Animator anim;
    private Vector3 targetPosition;
    private bool isWaiting = false;
    private bool isAttacking = false;
    private bool isDead = false; // Thêm biến trạng thái chết
    private float waitTimer = 0f;
    public float attackRange = 1.5f;
    private SpriteRenderer sprite;

    private enum MovementState
    {
        idle, running, attack, hit, die
    }

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        targetPosition = new Vector3(pointB.position.x, transform.position.y, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead)
        {
            // Nếu đã chết, dừng tất cả hành động
            return;
        }

        if (isAttacking)
        {
            // Nếu đang tấn công, dừng di chuyển
            return;
        }

        // Kiểm tra trạng thái chết khi di chuyển
        if (!isDead)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            if (!isWaiting && Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                isWaiting = true;
                waitTimer = 0f;
            }

            if (isWaiting)
            {
                waitTimer += Time.deltaTime;

                if (waitTimer >= stayDuration)
                {
                    targetPosition = (targetPosition.x == pointA.position.x) ? new Vector3(pointB.position.x, transform.position.y, transform.position.z) : new Vector3(pointA.position.x, transform.position.y, transform.position.z);
                    isWaiting = false;
                }
            }
        }

        UpdateAnimationState();
    }

    private void UpdateAnimationState()
    {
        MovementState state;

        if (Vector3.Distance(transform.position, player.position) <= attackRange)
        {
            state = MovementState.attack;
            isAttacking = true;
        }
        else
        {
            if (Vector3.Distance(transform.position, targetPosition) < 0.1f &&
                (targetPosition.x == pointA.position.x || targetPosition.x == pointB.position.x))
            {
                state = MovementState.idle;
            }
            else
            {
                state = MovementState.running;
                sprite.flipX = targetPosition.x == pointA.position.x;
            }
        }

        anim.SetInteger("state", (int)state);
    }

    // Phương thức được gọi khi animation tấn công kết thúc
    public void OnAttackAnimationFinished()
    {
        isAttacking = false;
    }

    // Thêm một phương thức để gọi khi goblin chết
    public void Die()
    {
        isDead = true;
        isAttacking = false;
        anim.SetTrigger("death");
        // Dừng bất kỳ logic di chuyển nào ở đây nếu cần
        this.enabled = false; // Tắt script di chuyển khi chết
    }
}
