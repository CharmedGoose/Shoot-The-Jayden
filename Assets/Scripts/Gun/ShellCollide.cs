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

    void OnCollisionEnter(Collision collision)
    {
        if ((collision.gameObject.layer != LayerMask.NameToLayer("Ground")) || hasCollided) return;
        hasCollided = true;
        AudioSource.PlayClipAtPoint(shellHitSound, transform.position);
    }
}
