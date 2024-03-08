using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushoomHealth : MonoBehaviour
{
    public Animator animator;
    public int maxHealth = 100;
    int currentHealth;
    public float downwardDistance = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        animator.SetInteger("state",3);
        if(currentHealth <= 0)
        {
            Die();
        }
    }
    void Die()
    {
      
        Debug.Log("Enemy died!");
        animator.SetInteger("state",4);
        MushroomMovement mushroomMovement = GetComponent<MushroomMovement>();
        if (mushroomMovement != null)
        {
            mushroomMovement.enabled = false;
            MushroomAttack attack = GetComponent<MushroomAttack>();
            attack.enabled = false;
        }
        // Disable the Collider2D
        GetComponent<Collider2D>().enabled = false;

        // Lock the animator
       

        // Freeze the Rigidbody
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.simulated = false;
        }
        transform.Translate(Vector3.down * downwardDistance, Space.World);
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }
}
