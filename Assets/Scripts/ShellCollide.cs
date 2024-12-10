using UnityEngine;

public class ShellCollide : MonoBehaviour
{
    [Header("References")]
    public AudioClip shellHitSound;

    void OnCollisionEnter()
    {
        AudioSource.PlayClipAtPoint(shellHitSound, transform.position);
    }
}
