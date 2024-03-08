using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;
    public GameObject deathEffect;
    private Animator animator;
    private bool isHurt = false;

    public Image healthBar; // Reference to the health bar image

    void Start()
    {
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
        UpdateHealthBar();
    }

    public void TakeDamage(int damage)
    {
        if (!isHurt)
        {
            currentHealth -= damage;
            UpdateHealthBar();
            if (currentHealth <= 0)
            {
                Die();
            }
            isHurt = true;
            StartCoroutine(ResetHurtFlag());
        }
    }

    IEnumerator ResetHurtFlag()
    {
        yield return new WaitForSeconds(0.5f);
        isHurt = false;
    }

    void Die()
    {
        if (animator != null)
        {
            animator.SetTrigger("Death");
        }
        else
        {
            Debug.LogError("Animator component is missing!");
        }
        StartCoroutine(ReloadScene());
    }

    IEnumerator ReloadScene()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    IEnumerator DamageAnimation()
    {
        SpriteRenderer[] srs = GetComponentsInChildren<SpriteRenderer>();
        for (int i = 0; i < 3; i++)
        {
            foreach (SpriteRenderer sr in srs)
            {
                Color c = sr.color;
                c.a = 0;
                sr.color = c;
            }

            yield return new WaitForSeconds(.1f);

            foreach (SpriteRenderer sr in srs)
            {
                Color c = sr.color;
                c.a = 1;
                sr.color = c;
            }

            yield return new WaitForSeconds(.1f);
        }
    }

    void UpdateHealthBar()
    {
        float healthPercent = (float)currentHealth / maxHealth;
        healthBar.fillAmount = healthPercent;
    }
}
