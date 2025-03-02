using System.Collections;
using UnityEngine;

public class Credits : MonoBehaviour
{
    void Awake()
    {
        StartCoroutine(ReturnToMenu());
    }

    IEnumerator ReturnToMenu()
    {
        yield return new WaitForSeconds(45f);
        StartCoroutine(LevelLoader.instance.LoadLevel(0));
    }
}
