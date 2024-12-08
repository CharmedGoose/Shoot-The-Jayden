using UnityEngine;

public class ShellCollide : MonoBehaviour
{
    public AudioClip shellHitSound;

    void OnCollisionEnter()
    {
        AudioSource.PlayClipAtPoint(shellHitSound, transform.position);
        Destroy(transform.parent.gameObject, shellHitSound.length + 1f);
    }
}
