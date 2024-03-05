using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int health = 100;

    public GameObject deathEffect;
    private Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }
    public void TakeDamage(int damage)
    {
        health -= damage;

        StartCoroutine(DamageAnimation());

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Gọi animation "Death" bằng cách sử dụng Animator
        if (animator != null)
        {
            animator.SetTrigger("Death");
        }
        else
        {
            Debug.LogError("Animator component is missing!");
        }

        // Tạm dừng game trong 1 giây trước khi load lại scene
        StartCoroutine(ReloadScene());
    }
    IEnumerator ReloadScene()
    {
        yield return new WaitForSeconds(1f); // Đợi 1 giây trước khi load lại scene
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
}
