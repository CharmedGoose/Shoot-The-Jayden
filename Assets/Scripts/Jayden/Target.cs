using UnityEngine;

public class Target : MonoBehaviour
{
    [Header("Settings")]
    public float health = 100f;
    public bool isInvincible = false;
    public bool isTraining = false;

    [Header("References")]
    public TMPro.TextMeshProUGUI text;

    public void TakeDamage(float amount)
    {
        if (isInvincible) return;
        
        health -= amount;

        if (health <= 0f)
        {
            Die();
        }
    }

    void Die()
    {
        if (isTraining) return;
        Destroy(gameObject);
        GameManager.instance.RemoveJayden(text);
    }
}
