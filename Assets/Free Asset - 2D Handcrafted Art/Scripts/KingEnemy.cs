using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class KingEnemy : Character
{
    public float attackCooldown = 1.5f; 
    private float lastAttackTime = 0f; 
    private bool isAttacking = false; 
    public Transform player;

    private void Start()
    {
        health = 500f;
        speed = 2f;
        attackRange = 1.5f;
        attackDamage = 10f;
        knockbackForce = 500f;
        player = GameObject.FindWithTag("Player").transform; 
    }

    private void Update()
    {
        if (player != null)
        {
            Move(); 
        }
    }

    public override void Move()
    {
        if (isAttacking) return; 

        
        Vector2 direction = (player.position - transform.position).normalized;

        
        transform.Translate(direction * speed * Time.deltaTime);

        
        if (direction.x != 0) 
        {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x) * Mathf.Sign(direction.x); 
            transform.localScale = scale;
        }

        
        if (Vector2.Distance(transform.position, player.position) <= attackRange)
        {
            TryAttack(); 
        }
    }

    private void TryAttack()
    {
        
        if (Time.time >= lastAttackTime + attackCooldown && !isAttacking)
        {
            isAttacking = true; 
            Attack(); 
            lastAttackTime = Time.time; 
        }
    }

    public override void Attack()
    {
        
        animator.SetTrigger("hasTarget");

        
        Player playerComponent = player.GetComponent<Player>();
        if (playerComponent != null)
        {
            Vector2 direction = (playerComponent.transform.position - transform.position).normalized;

            
            playerComponent.ApplyKnockback(direction);

            
            playerComponent.TakeDamage(attackDamage);
        }

        
        Invoke("EndAttack", attackCooldown); 
    }

    private void EndAttack()
    {
        isAttacking = false; 
        animator.ResetTrigger("hasTarget");
    }

}
