using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinDeath : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Trap") || (collision.gameObject.CompareTag("Player")))
        {
            Die();
        }
    }
    private void Die()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        anim.SetTrigger("death");
    }
}
