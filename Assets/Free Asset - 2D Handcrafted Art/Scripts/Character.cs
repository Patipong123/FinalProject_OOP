using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{

    [SerializeField] protected float health;
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
    
    public abstract void Move(); 
    
    public abstract void Attack();

    public void ApplyKnockback(Vector2 direction)
    {
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.AddForce(direction.normalized * knockbackForce, ForceMode2D.Impulse);
        }
    }



    public virtual void TakeDamage(float damage) 
    {
        health -= damage;
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

    protected virtual void Die() 
    {
        Debug.Log($"{gameObject.name} has died.");
        Destroy(gameObject);
    }

    
}
