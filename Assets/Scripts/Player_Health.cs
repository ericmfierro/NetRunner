using UnityEngine;
using System.Collections;

public class Player_Health : MonoBehaviour
{
    [SerializeField] int maxHealth = 3;
    [SerializeField] float invincibilityDuration = 2f;

    int currentHealth;
    bool isInvincible = false;

    SpriteRenderer spriteRenderer;
    Collider2D playerCollider;

    void Start()
    {
        currentHealth = maxHealth;
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerCollider = GetComponent<Collider2D>();
    }

    public void TakeDamage(int amount)
    {
        if (isInvincible)
            return;

        currentHealth -= amount;
        Debug.Log("Player HP: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(InvincibilityCoroutine());
        }
    }

    IEnumerator InvincibilityCoroutine()
    {
        isInvincible = true;

        // Optional: disable collider so enemy can't hit repeatedly
        playerCollider.enabled = false;

        float timer = 0f;

        while (timer < invincibilityDuration)
        {
            if (spriteRenderer != null)
                spriteRenderer.enabled = !spriteRenderer.enabled;

            yield return new WaitForSeconds(0.15f);
            timer += 0.15f;
        }

        if (spriteRenderer != null)
            spriteRenderer.enabled = true;

        playerCollider.enabled = true;
        isInvincible = false;
    }

    void Die()
    {
        Debug.Log("Player Defeated!");

        // Disable player movement if needed
        gameObject.SetActive(false);
    }

    public int CurrentHealth() => currentHealth;
}
