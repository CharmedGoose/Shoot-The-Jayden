using System.Collections;
using TMPro;
using UnityEngine;

public class TypingEffect : MonoBehaviour
{
    [Header("Settings")]
    public string fullText;
    public float delay = 0.1f;

    TextMeshProUGUI text;

    void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
        text.text = string.Empty;
        StartCoroutine(TypeText());
    }

    IEnumerator TypeText()
    {
        foreach (char letter in fullText)
        {
            text.text += letter;
            yield return new WaitForSeconds(delay);
        }
    }
}
