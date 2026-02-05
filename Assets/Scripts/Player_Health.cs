using UnityEngine;

public class Player_Health : MonoBehaviour
{
    [SerializeField] int maxHealth = 5;
    int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        Debug.Log("Player HP: " + currentHealth);

        if (currentHealth <= 0)
            Die();
    }

    void Die()
    {
        Debug.Log("Player Defeated!");
        gameObject.SetActive(false);
    }
}
