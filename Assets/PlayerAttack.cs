using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public Animator anim;
    public bool isAttacking = false;
    public static PlayerAttack instance;
    public Transform HitBox;
    public float attackRange = 1f;
    public LayerMask enemyLayers;
    // Start is called before the first frame update

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        anim.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Attack();
    }
    void Attack()
    {
        if (Input.GetKeyUp(KeyCode.Q) && !isAttacking)
        {
            isAttacking = true;
        }
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(HitBox.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            Debug.Log("We hit "+ enemy.name);
        }
    }
    private void OnDrawGizmos()
    {
        if (HitBox == null)
            return;
        Gizmos.DrawWireSphere(HitBox.position, attackRange);
    }
}
