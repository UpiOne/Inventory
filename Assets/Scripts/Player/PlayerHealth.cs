using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour,IAttackable
{
    [Header("Health Settings")]
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;

    public Image healthBar;

    private void Start()
    {
        currentHealth = maxHealth; 
        UpdateHealthBar();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        currentHealth = Mathf.Max(currentHealth, 0);
        UpdateHealthBar();
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    private void Die()
    {
        SceneManager.LoadScene(0);
        Debug.Log("Player died!");
    }
    private void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.fillAmount = (float)currentHealth / maxHealth;
        }
    }
}