using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class KingEnemy : Character // Inheritance
{
    public float AttackCooldown = 1.5f; 
    private float lastAttackTime = 0f; 
    private bool isAttacking = false; 
    public Transform Player;
    [SerializeField] private GameObject winUI;

    private void Start()
    {
        health = 100f;
        maxHealth = health;
        speed = 2f;
        attackRange = 1.5f;
        attackDamage = 10f;
        knockbackForce = 500f;
        Player = GameObject.FindWithTag("Player").transform;

        if (winUI != null)
        {
            winUI.SetActive(false); 
        }
    }

    private void Update()
    {
        if (Player != null)
        {
            Move(); 
        }
    }

    
    public override void Move()
    {
        if (isAttacking) return; 

        
        Vector2 direction = (Player.position - transform.position).normalized;

        
        transform.Translate(direction * speed * Time.deltaTime);

        
        if (direction.x != 0) 
        {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x) * Mathf.Sign(direction.x); 
            transform.localScale = scale;
        }

        
        if (Vector2.Distance(transform.position, Player.position) <= attackRange)
        {
            TryAttack(); 
        }
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        
    }

    protected override void Die()
    {
        base.Die(); 
        ShowWinUI(); 
    }

    private void ShowWinUI()
    {
        if (winUI != null)
        {
            winUI.SetActive(true); 
            Time.timeScale = 0; 
        }
    }

    private void TryAttack()
    {
        
        if (Time.time >= lastAttackTime + AttackCooldown && !isAttacking)
        {
            isAttacking = true; 
            Attack(); 
            lastAttackTime = Time.time; 
        }
    }

    
    public override void Attack()
    {
        
        animator.SetTrigger("hasTarget");

        
        Player playerComponent = Player.GetComponent<Player>();
        if (playerComponent != null)
        {
            Vector2 direction = (playerComponent.transform.position - transform.position).normalized;

            
            playerComponent.ApplyKnockback(direction);

            
            playerComponent.TakeDamage(attackDamage);
        }

        
        Invoke("EndAttack", AttackCooldown); 
    }

    private void EndAttack()
    {
        isAttacking = false; 
        animator.ResetTrigger("hasTarget");
    }



}
