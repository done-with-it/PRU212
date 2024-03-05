using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterDeathEX : MonoBehaviour
{
    public Transform respawnPoint; // Assign your desired respawn point in the Inspector

    private Rigidbody2D rb;
    private Animator anim;
    private bool die = true;
    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (die && collision.gameObject.CompareTag("Trap"))
        {
            Die();
        }
    }

    private void Die()
    {
        rb.constraints= RigidbodyConstraints2D.FreezeAll;
        anim.SetTrigger("Death");
        die = false;
    }

    

    public void RestartLevel()
    {
        if (!ItemColettor.CoinCollected)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            die = true;
        }
        else {
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;

            transform.position = respawnPoint.position;
            anim.SetInteger("state", 1);
            die = true;
        }
        //  SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        
    }
}
