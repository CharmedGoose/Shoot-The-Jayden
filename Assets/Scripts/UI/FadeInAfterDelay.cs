using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EnableAfterDelay : MonoBehaviour
{
    [Header("Settings")]
    public float delay;

    Image image;

    void Awake()
    {
        image = GetComponent<Image>();
        image.color = new Color(1, 1, 1, 0);
        StartCoroutine(FadeIn());
    }

    // https://discussions.unity.com/t/simple-ui-animation-fade-in-fade-out-c/645154/2
    // i'm too lazy to do it myself
    IEnumerator FadeIn()
    {
        yield return new WaitForSeconds(delay);

        for (float i = 0; i <= 1; i += Time.deltaTime)
        {
            image.color = new Color(1, 1, 1, i);
            yield return null;
        }
    }
}
