using UnityEngine;

public class Target : MonoBehaviour
{
    [Header("Settings")]
    public float health = 100f;

    [Header("References")]
    public TMPro.TextMeshProUGUI text;

    public void TakeDamage(float amount)
    {
        health -= amount;

        if (health <= 0f)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
        JaydenCounter.RemoveJayden(text);
    }
}
