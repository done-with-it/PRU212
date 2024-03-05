using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinMovementV2 : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public Transform player;
    public float moveSpeed = 2f;
    public float stayDuration = 3f; // Duration to stay at each point
    private Animator anim;
    private Vector3 targetPosition;
    private bool isWaiting = false;
    private float waitTimer = 0f;

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
        targetPosition = pointB.position;
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
                targetPosition = (targetPosition == pointA.position) ? pointB.position : pointA.position;

                // Reset attack flag when changing direction

                isWaiting = false; // Stop waiting
            }
        }

        // Update animation state
        UpdateAnimationState();
    }


    private void UpdateAnimationState()
    {
        MovementState state;
        // Determine movement direction and set animation state
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f &&
            (targetPosition == pointA.position || targetPosition == pointB.position))
        {
            state = MovementState.idle; // Monster is idle
        }
        else
        {
            state = MovementState.running; // Monster is running
                                           // Set the sprite's flipX based on movement direction
            sprite.flipX = targetPosition == pointA.position;
        }


        // Update the animation state parameter
        anim.SetInteger("state", (int)state);
    }
}
