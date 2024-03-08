using UnityEngine;
using System.Collections;

public class MushroomAttack : MonoBehaviour
{
    public float attackRange = 1.5f;
    public Transform player;
    public MushroomMovement mushroomMovement; // Reference to the MushroomMovement script
    private Animator anim;
    private bool isAttacking = false;
    public float attackDelay = 0.5f; // Delay before attack animation starts
    public float attackCooldown = 1.0f; // Cooldown between attacks
    private bool isCooldown = false; // Flag to track if the attack is on cooldown

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        // Check if the player is within attack range and in front of the monster
        if (Vector3.Distance(transform.position, player.position) <= attackRange && IsPlayerInFront())
        {
            // Initiate attack animation or logic here
            if (!isAttacking && !isCooldown)
            {
                isAttacking = true;
                StartCoroutine(StartAttackAnimation()); // Start coroutine for attack animation

                // Disable MushroomMovement script
                if (mushroomMovement != null)
                {
                    mushroomMovement.enabled = false;
                }
            }
        }
       // else mushroomMovement.enabled = true;
    }

    private IEnumerator StartAttackAnimation()
    {
        // Wait for the specified delay before triggering the attack animation
        yield return new WaitForSeconds(attackDelay);

        anim.SetInteger("state", 2); // Trigger attack animation

        // Start the attack cooldown
        StartCoroutine(AttackCooldown());
    }

    private IEnumerator AttackCooldown()
    {
        isCooldown = true; // Set the cooldown flag to true
        yield return new WaitForSeconds(attackCooldown);
        isCooldown = false; // Reset the cooldown flag
    }

    private bool IsPlayerInFront()
    {
        Vector3 movementDirection = (transform.position - player.position).normalized;
        Vector3 playerDirection = transform.right;
        float dotProduct = Vector3.Dot(playerDirection, movementDirection);
        return dotProduct > 0f;
    }

    // Call this method when the attack animation finishes
    public void OnAttackAnimationFinished()
    {
        isAttacking = false;
    }
}
