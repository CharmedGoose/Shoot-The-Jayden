using UnityEngine;

public class ShellCollide : MonoBehaviour
{
    [Header("References")]
    public AudioClip shellHitSound;

    bool hasCollided = false;

    void OnEnable()
    {
        hasCollided = false;
    }

    void OnCollisionEnter()
    {
        if (hasCollided) return;
        hasCollided = true;
        AudioSource.PlayClipAtPoint(shellHitSound, transform.position);
    }
}
