using UnityEngine;
using UnityEngine.U2D;

public class MonsterMovement : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public Transform player;
    public float moveSpeed = 2f;
    public float stayDuration = 3f; // Duration to stay at each point
    private Animator anim;
    private Vector3 targetPosition;
    private bool isWaiting = false;
    private bool isAttacking = false;
    private float waitTimer = 0f;
    public float attackRange = 1.5f;

    private enum MovementState
    {
        idle, running, attack, hit, die
    }
    private SpriteRenderer sprite;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>(); // Initialize the Animator component
        sprite = GetComponent<SpriteRenderer>(); // Initialize the SpriteRenderer component

        // Start by moving towards point B
        targetPosition = new Vector3(pointB.position.x, transform.position.y, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        if (isAttacking)
        {
            isAttacking = false;
            // If attacking, stop movement
            return;
        }

        // Move towards the target position
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        // Check if the monster has reached the target position
        if (!isWaiting && Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            // Start waiting
            isWaiting = true;
            waitTimer = 0f;
        }

        if (isWaiting)
        {
            // Increment the timer  
            waitTimer += Time.deltaTime;

            // Check if the waiting duration has passed
            if (waitTimer >= stayDuration)
            {
                // Toggle between points A and B
                targetPosition = (targetPosition.x == pointA.position.x) ? new Vector3(pointB.position.x, transform.position.y, transform.position.z) : new Vector3(pointA.position.x, transform.position.y, transform.position.z);

                isWaiting = false; // Stop waiting
            }
        }

        // Update animation state
        UpdateAnimationState();
    }

    private void UpdateAnimationState()
    {
        MovementState state;

        // Determine movement direction
        Vector3 movementDirection = (targetPosition - transform.position).normalized;

        // Determine if the player is within attack range and in front of the monster
        Vector3 playerDirection = (player.position - transform.position).normalized;
        float dotProduct = Vector3.Dot(playerDirection, movementDirection);
        bool playerInFront = dotProduct > 0f;

        if (Vector3.Distance(transform.position, player.position) <= attackRange && playerInFront)
        {
            // Initiate attack animation or logic here
            state = MovementState.attack;
            isAttacking = true;
        }
        else
        {
            // Determine movement direction and set animation state
            if (Vector3.Distance(transform.position, targetPosition) < 0.1f &&
                (targetPosition.x == pointA.position.x || targetPosition.x == pointB.position.x))
            {
                state = MovementState.idle; // Monster is idle
            }
            else
            {
                state = MovementState.running; // Monster is running

                // Flip sprite based on movement direction
                if (movementDirection.x < 0f)
                    transform.localScale = new Vector3(-1f, 1f, 1f); // Flip sprite horizontally
                else if (movementDirection.x > 0f)
                    transform.localScale = new Vector3(1f, 1f, 1f); // Reset sprite scale
            }
        }

        // Update the animation state parameter
        anim.SetInteger("state", (int)state);
    }




    // Call this method when the attack animation finishes
    public void OnAttackAnimationFinished()
    {
        isAttacking = false;
    }
}