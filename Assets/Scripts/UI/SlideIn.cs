using System.Collections;
using UnityEngine;

public class SlideIn : MonoBehaviour
{
    [Header("Settings")]
    public float speed;
    public float delay;

    bool start = false;

    float time = 0;

    Vector3 position;

    RectTransform rectTransform;

    void Awake()
    {
        start = false;
        time = 0;
        rectTransform = GetComponent<RectTransform>();

        position = rectTransform.anchoredPosition;
        rectTransform.anchoredPosition = rectTransform.anchoredPosition + new Vector2(-240, 0);
        StartCoroutine(WaitSeconds());
    }

    void Update()
    {
        if (!start) return;

        time += Time.deltaTime * speed;
        rectTransform.anchoredPosition = Vector3.Lerp(rectTransform.anchoredPosition, position, time);
    }

    IEnumerator WaitSeconds()
    {
        yield return new WaitForSeconds(delay);

        start = true;
    }
}
