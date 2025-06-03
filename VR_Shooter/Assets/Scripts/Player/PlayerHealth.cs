using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 5;
    public int currentHealth;

    public float healDelay = 3f;
    private float lastDamageTime;
    private bool isHealing = false;

    void Start()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
        if (currentHealth < maxHealth && !isHealing && Time.time - lastDamageTime >= healDelay)
        {
            StartCoroutine(HealPlayer());
        }
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        lastDamageTime = Time.time;
        isHealing = false;

        Debug.Log("Player took damage. Health: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    IEnumerator HealPlayer()
    {
        isHealing = true;
        yield return new WaitForSeconds(0.1f);
        currentHealth = maxHealth;
        Debug.Log("Player healed to full!");
        isHealing = false;
    }

    void Die()
    {
        Debug.Log("Player died!");
        StartCoroutine(LoadGameSceneAfterDelay());
    }

    IEnumerator LoadGameSceneAfterDelay()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("Game");
    }
}
