using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public enum PlayerState
    {
        Idle,
        Running,
        Jumping,
        Falling
    }

    private PlayerState currentState;
    private PlayerState dirX = PlayerState.Idle;
    private SpriteRenderer sprite;

    [Header("Horizontal Movement Settings")]
    [SerializeField] private float walkSpeed = 1;

    [Header("Vertical Movement Settings")]
    [SerializeField] private float jumpForce = 45;
    private int jumpBufferCounter;
    [SerializeField] private int jumpBufferFrames;
    private float conyoteTimeCounter = 0;
    [SerializeField] private float conyoteTime;
    private int airJumpCounter = 0;
    [SerializeField] private int maxAirJumps;

    [Header("Ground Check Settings")]
    [SerializeField] private Transform groundCheckPoint;
    [SerializeField] private float groundCheckY = 0.2f;
    [SerializeField] private float groundCheckX = 0.5f;
    [SerializeField] private LayerMask whatisGround;

    PlayerStateList pState;
    private Rigidbody2D rb;
    private float xAsis;
    Animator anim;

    public static PlayerController Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        pState = GetComponent<PlayerStateList>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        GetInputs();
        UpdateJumpVariables();
        Flip();
        Jump();
        Move();
        UpdateAnimationUpdate();
    }

    void GetInputs()
    {
        xAsis = Input.GetAxisRaw("Horizontal");

        if (xAsis > 0f)
        {
            dirX = PlayerState.Running;
        }
        else if (xAsis < 0f)
        {
            dirX = PlayerState.Running;
        }
        else
        {
            dirX = PlayerState.Idle;
        }
    }

    void Flip()
    {
        sprite.flipX = (dirX == PlayerState.Running && xAsis < 0f);
    }

    private void Move()
    {
        rb.velocity = new Vector2(walkSpeed * xAsis, rb.velocity.y);

        if (rb.velocity.x != 0 && Grounded())
        {
            currentState = PlayerState.Running;
        }
        else if (!Grounded())
        {
            currentState = PlayerState.Jumping;
        }
        else
        {
            currentState = PlayerState.Idle;
        }
    }

    private void UpdateAnimationUpdate()
    {
        if (dirX == PlayerState.Running)
        {
            currentState = PlayerState.Running;
        }
        else
        {
            currentState = PlayerState.Idle;
        }

        if (rb.velocity.y > .1f)
        {
            currentState = PlayerState.Jumping;
        }
        else if (rb.velocity.y < -.1f)
        {
            currentState = PlayerState.Falling;
        }
        anim.SetInteger("state", (int)currentState);
    }

    public bool Grounded()
    {
        if (Physics2D.Raycast(groundCheckPoint.position, Vector2.down, groundCheckY, whatisGround)
            || Physics2D.Raycast(groundCheckPoint.position + new Vector3(groundCheckX, 0, 0), Vector2.down, groundCheckY, whatisGround)
            || Physics2D.Raycast(groundCheckPoint.position + new Vector3(-groundCheckX, 0, 0), Vector2.down, groundCheckY, whatisGround))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void Jump()
    {
        int jumpAnimationState = 0; // Giá trị mặc định

        if (Input.GetButtonDown("Jump") && rb.velocity.y > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
            pState.jumping = false;
        }

        if (!pState.jumping)
        {
            if (jumpBufferCounter > 0 && conyoteTimeCounter > 0)
            {
                rb.velocity = new Vector3(rb.velocity.x, jumpForce);
                pState.jumping = true;
                jumpAnimationState = 1; // Set giá trị animation khi nhảy
            }
            else if (!Grounded() && airJumpCounter < maxAirJumps && Input.GetButtonDown("Jump"))
            {
                pState.jumping = true;
                airJumpCounter++;
                rb.velocity = new Vector3(rb.velocity.x, jumpForce);
                jumpAnimationState = 1; // Set giá trị animation khi nhảy
            }
        }

        if (jumpAnimationState == 0 && rb.velocity.y > 0.1f)
        {
            jumpAnimationState = 2; // Set giá trị animation khi đang lên cao
        }
        else if (jumpAnimationState == 0 && rb.velocity.y < -0.1f)
        {
            jumpAnimationState = 3; // Set giá trị animation khi đang rơi
        }

        anim.SetInteger("state", jumpAnimationState); // Sử dụng chung biến "state"
    }


    void UpdateJumpVariables()
    {
        int jumpState = 0; // Giá trị mặc định

        if (Grounded())
        {
            pState.jumping = false;
            conyoteTimeCounter = conyoteTime;
            airJumpCounter = 0;
        }
        else
        {
            conyoteTimeCounter -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump"))
        {
            jumpBufferCounter = jumpBufferFrames;
            jumpState = 1; // Set giá trị animation khi bắt đầu nhảy
        }
        else
        {
            jumpBufferCounter--;
        }

        anim.SetInteger("state", jumpState); // Sử dụng chung biến "state" cho trạng thái nhảy
    }
}
