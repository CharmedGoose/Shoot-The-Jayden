using UnityEngine;
using System.Collections;

public class ShellCollide : MonoBehaviour
{
    public AudioClip shellHitSound;

    void OnCollisionEnter()
    {
        AudioSource.PlayClipAtPoint(shellHitSound, transform.position);
    }

    IEnumerator Destroy()
    {
        yield return new WaitForSeconds(shellHitSound.length + 1f);
        Destroy(gameObject);
    }
}
