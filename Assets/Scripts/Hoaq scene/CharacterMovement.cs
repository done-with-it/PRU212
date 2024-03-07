using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    Rigidbody2D rb;
    private BoxCollider2D coll;
    private SpriteRenderer sprite;
    public Animator anim;
    private float dirX = 0f;
    public bool isAttack = false;
    public int attackDamage = 100;
    [SerializeField] private LayerMask jumpableGround;
    [SerializeField] public float jumpForce = 14f;
    [SerializeField] public float moveSpeed = 7f;
    public static CharacterMovement instance;
    public Transform attackPoint;
    private Transform attackRange2;
    public float attackRange = 0.5f;
    public LayerMask enemyLayer;
    private enum MovementState
    {
        idle, running, jumping, falling, croush
    }
    private MovementState state = MovementState.idle;
    private int remainingJumps = 0; // Adjust this to the desired number of jumps
    private void Awake()
    {
        instance = this;
    }
    void Attack()
    {
        // Kiểm tra xem nút tấn công đã được nhấn và đối tượng không trong trạng thái tấn công
        if (Input.GetKeyDown(KeyCode.G) && !isAttack)
        {
            isAttack = true;

            // Nếu tấn công, thì thực hiện các hành động tấn công
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);
            foreach (Collider2D enemy in hitEnemies)
            {
                enemy.GetComponent<EnemyHealth>().TakeDamage(attackDamage);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if(attackPoint == null)
        
            return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        
    }

    private void UpdateAttackRangeDirection()
    {
        // Lấy hướng của di chuyển của nhân vật
        float characterDirection = Mathf.Sign(dirX);

        // Xác định vị trí của attack2 dựa trên hướng di chuyển của nhân vật
        float attack2Offset = characterDirection * Mathf.Abs(attackRange2.localPosition.x);
        attackRange2.localPosition = new Vector3(attack2Offset, attackRange2.localPosition.y, attackRange2.localPosition.z);
    }






    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        rb.freezeRotation = true;
        attackRange2 = transform.Find("attack2");
        if (attackRange2 == null)
        {
            Debug.LogError("Không tìm thấy đối tượng con attackRange!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        dirX = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);

        // Check if the character is grounded
        if (IsGrounded())
        {
            remainingJumps = 1; // Reset the remaining jumps when grounded
        }

        // Xử lý nhảy chỉ khi đối tượng đang nằm trên mặt đất và có lượt nhảy còn lại
        if (Input.GetButtonDown("Jump") && remainingJumps > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            remainingJumps--;
        }


        UpdateAnimationUpdate();
        UpdateAttackRangeDirection();
        Attack();
    }

    private bool IsGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }

    private void UpdateAnimationUpdate()
    {
        MovementState state;
        if (dirX > 0f)
        {
            state = MovementState.running;
            sprite.flipX = false;
        }
        else if (dirX < 0f)
        {
            state = MovementState.running;
            sprite.flipX = true;
        }

        else
        {
            state = MovementState.idle;
        }
        if (rb.velocity.y > .1f)
        {
            state = MovementState.jumping;
        }
        else if (rb.velocity.y < -.1f)
        {
            state = MovementState.falling;
        }
        anim.SetInteger("state", (int)state);
    }
}