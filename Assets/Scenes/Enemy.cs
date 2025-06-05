using UnityEngine;
using UnityEngine.InputSystem; // Make sure to include this!

public class Enemy : MonoBehaviour
{
    public Animator animator;
    public int maxHealth = 100;
    int currentHealth;

    // Reference to the player input (optional, but useful)
    public PlayerInput playerInput;

    void Start()
    {
        currentHealth = maxHealth;

        // If not assigned manually, try getting it from the GameObject
        if (playerInput == null)
            playerInput = GetComponent<PlayerInput>();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        animator.SetTrigger("Hurt");

        if (currentHealth <= 0)
        {

            Die();
        }
    }

    public void Die()
    {
        animator.SetTrigger("IsDead");
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
    }
}
