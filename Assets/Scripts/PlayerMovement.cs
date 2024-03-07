using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    //private BoxCollider2D coll;
    private SpriteRenderer sprite;
    public Animator anim;
    public bool isAttack = false;
    [SerializeField] private LayerMask jumpableGoround;
    
    private float dirX = 0f;
    [SerializeField] private float movementSpeed = 6f;
    [SerializeField] private float jumpForce = 8f;
    public static PlayerMovement instance;
    private enum MovementState { idle, running, jumping, falling }
    

    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //coll = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    private void Update()
    {
        dirX = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(dirX * movementSpeed, rb.velocity.y);
        
        if (Input.GetButtonDown("Jump"))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        UpdateAnimationUpdate();
        Attack();
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
        }else if (rb.velocity.y < -.1f)
        {
            state = MovementState.falling;
        }

        anim.SetInteger("state", (int)state);
    }

    void Attack()
    {
        if (Input.GetKeyDown(KeyCode.G) && !isAttack)
        {
            isAttack = true;
        }
    }

}
