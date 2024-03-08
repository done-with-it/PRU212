using UnityEngine;

public class MushroomMovement : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public float moveSpeed = 2f;
    public float stayDuration = 3f; // Duration to stay at each point
    private Animator anim;
    private Vector3 targetPosition;
    private bool isWaiting = false;
    private float waitTimer = 0f;
    public bool isFlipped = false;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>(); // Initialize the Animator component

        // Start by moving towards point B
        targetPosition = new Vector3(pointB.position.x, transform.position.y, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
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

        // Determine movement state
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f &&
            (targetPosition.x == pointA.position.x || targetPosition.x == pointB.position.x))
        {
            state = MovementState.idle; // Monster is idle
        }
        else
        {
            
            state = MovementState.running; // Monster is running

            // Flip sprite based on movement direction
            if (movementDirection.x < 0f && isFlipped)
            { 
                transform.localScale = new Vector3(-1f, 1f, 1f); // Flip sprite horizontally              
                transform.Rotate(0f, 180f, 0f);
                isFlipped = false;
            }
               
            else if (movementDirection.x > 0f && !isFlipped)
            {
                 transform.localScale = new Vector3(-1f, 1f, 1f); // Reset sprite scale           
                transform.Rotate(0f, 180f, 0f);
                isFlipped = true;
            }

        }

        // Update the animation state parameter
        anim.SetInteger("state", (int)state);
    }

    private enum MovementState
    {
        idle, running
    }
}
