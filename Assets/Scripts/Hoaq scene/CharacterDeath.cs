using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterDeath : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    public Transform respawnPoint;
    private bool die = true;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Trap") || collision.gameObject.CompareTag("Goblin") 
            || collision.gameObject.CompareTag("Sea"))
        {
            Die();
        }
    }

    private void Die()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        anim.SetTrigger("Death");
        Invoke("RestartLevel", 0.5f); // Restart after 0.5 seconds, adjust as needed
        die = false;
    }

    public void RestartLevel()
    {
        if (!ItemColettor.CoinCollected)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            die = true;
        }
        else
        {
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;

            transform.position = respawnPoint.position;
            anim.SetInteger("state", 1);
            die = true;
        }
        //  SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }
}
