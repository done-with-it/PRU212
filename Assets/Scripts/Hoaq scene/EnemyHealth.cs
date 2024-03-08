using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class EnemyHealth : MonoBehaviour
{
    public Animator animator;
    public int maxHealth = 100;
    int currentHealth;
    public static bool die = false;

    public Image healthBar;
    public Image Blank;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthBar();
    }
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        UpdateHealthBar();
        animator.SetTrigger("Hurt");
        if(currentHealth <= 0)
        {
            Die();
            healthBar.enabled = false;
            Blank.enabled = false;
        }
    }
    void Die()
    {
        Debug.Log("Enemy died!");
        animator.SetBool("Dead", true);
        die = true;
        //this.enabled = false;
        GetComponent<Collider2D>().enabled = false;
    }
    void UpdateHealthBar()
    {
        float healthPercent = (float)currentHealth / maxHealth;
        healthBar.fillAmount = healthPercent;
       
    }
    // Update is called once per frame
    void Update()
    {
        
        
    }
}
