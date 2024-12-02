using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : Character // Inheritance
{
    
    private bool isHurt = false;
    private bool isAttacking;

    private void Start()
    {
        
        health = 100f;
        maxHealth = health;
        speed = 5f;
        attackRange = 2f;
        attackDamage = 20f;
        knockbackForce = 10f;
    }

    private void Update()
    {
        Move();
        if (Input.GetKeyDown(KeyCode.Q) && !isAttacking) 
        {
            Attack();
        }


    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage); 
        PlayHurtAnimation();    
    }

    private void PlayHurtAnimation()
    {
        if (isHurt) return; 

        if (animator != null)
        {
            isHurt = true;
            animator.SetTrigger("Hurt");
            StartCoroutine(ResetHurtStatus());
        }
    }

    
    public override void Move() 
    {
        float horizontal = Input.GetAxis("Horizontal"); 
        Vector2 movement = new Vector2(horizontal, 0).normalized; 
        transform.Translate(movement * speed * Time.deltaTime);
        animator.SetFloat("Speed", Mathf.Abs(horizontal));

        
        if (horizontal > 0) 
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z); 
        }
        else if (horizontal < 0) 
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z); 
        }
    }

    
    public override void Attack()
    {
        if (!isAttacking)
        {
            isAttacking = true;
            animator.SetTrigger("Attack");

            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, attackRange);
            foreach (Collider2D hit in hits)
            {
                if (hit.CompareTag("Enemy"))
                {

                    Vector2 direction = (hit.transform.position - transform.position).normalized;
                    hit.GetComponent<Character>()?.ApplyKnockback(direction); 
                    hit.GetComponent<KingEnemy>()?.TakeDamage(attackDamage, "Player");
                }
            }

            Invoke(nameof(ResetAttack), 1f);
        }
    }

    private void ResetAttack()
    {
        isAttacking = false;
    }

    private IEnumerator ResetHurtStatus()
    {
        yield return new WaitForSeconds(0.625f); 
        isHurt = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange); 
    }

    protected override void Die()
    {
        base.Die(); 
        ReloadGame(); 
    }

    private void ReloadGame()
    {
        Scene currentScene = SceneManager.GetActiveScene(); 
        SceneManager.LoadScene(currentScene.name); 
    }


}
