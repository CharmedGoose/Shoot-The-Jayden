using UnityEngine;

public class ShellCollide : MonoBehaviour
{
    public AudioClip shellHitSound;

    void OnCollisionEnter()
    {
        AudioSource.PlayClipAtPoint(shellHitSound, transform.position);
        Destroy(gameObject, shellHitSound.length + 1f);
    }
}
