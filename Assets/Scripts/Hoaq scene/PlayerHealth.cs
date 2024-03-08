using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int health = 100;

    public GameObject deathEffect;
    private Animator animator;
    private bool isHurt = false;
    void Start()
    {
        animator = GetComponent<Animator>();
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Goblin") || collision.gameObject.CompareTag("Skeleton"))
        {
            TakeDamage(10);
        }
    }

    public void TakeDamage(int damage)
    {
        if (!isHurt) // Chỉ xử lý sát thương nếu người chơi chưa bị tổn thương trước đó
        {
            health -= damage;
            if (animator != null)
            {
                animator.SetTrigger("Hurt");
            }
            StartCoroutine(DamageAnimation());

            if (health <= 0)
            {
                Die();
            }

            isHurt = true; // Đánh dấu rằng người chơi đã bị tổn thương
            StartCoroutine(ResetHurtFlag()); // Gọi hàm để reset cờ isHurt sau một khoảng thời gian
        }
    }
    IEnumerator ResetHurtFlag()
    {
        yield return new WaitForSeconds(0.5f); // Đợi một khoảng thời gian trước khi reset cờ isHurt
        isHurt = false; // Reset cờ isHurt để cho phép người chơi nhận sát thương tiếp theo
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
