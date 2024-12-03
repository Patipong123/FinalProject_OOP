using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Character : MonoBehaviour // Abstract Class
{
    public Image Healthbar;

    // Encapsulation
    [SerializeField] protected float health;
    [SerializeField] protected float maxHealth;
    [SerializeField] protected float speed;
    [SerializeField] protected float attackRange;
    [SerializeField] protected float attackDamage;
    [SerializeField] protected float knockbackForce;


    protected Rigidbody2D rb;
    protected Animator animator;

    protected virtual void Awake() 
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        animator.SetFloat("Speed", Mathf.Abs(speed));
        
    }

    public void init(float newHealth, float newMaxHealth, float newSpeed, float newAttackRange, float newAttackDamage, float newKnockbackForce) 
    {
        health = newHealth;
        maxHealth = newHealth;
        speed = newSpeed;
        attackRange = newAttackRange;
        attackDamage = newAttackDamage;
        knockbackForce = newKnockbackForce;
    }

    #region Abstact Method

    //Abstact Method
    public abstract void Move(); 
    
    public abstract void Attack();

    #endregion

    #region Polymorphism

    public virtual void TakeDamage(float damage) 
    {
        health -= damage;
        Healthbar.fillAmount = health / maxHealth;
        Debug.Log($"{gameObject.name} took {damage} damage. Remaining health: {health}");
        if (health <= 0)
        {
            Die();
        }
    }

    public virtual void TakeDamage(float damage, string source)
    {
        Debug.Log($"{gameObject.name} was damaged by {source}");
        TakeDamage(damage);
    }

    #endregion

    protected virtual void Die() 
    {
        Debug.Log($"{gameObject.name} has died.");
        Destroy(gameObject);
    }

    public void ApplyKnockback(Vector2 direction)
    {
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.AddForce(direction.normalized * knockbackForce, ForceMode2D.Impulse);
        }
    }

}
