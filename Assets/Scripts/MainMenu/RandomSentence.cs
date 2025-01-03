using UnityEngine;

public class RandomSentence : MonoBehaviour
{
    readonly string[] sentences = { "test", "it look's goofy ik", "2?", "fun!", "less polygons!", "50% bug free!", "fudge!", "i'm running out of ideas", "scary!", "i ran out of ideas" };

    void Awake()
    {
        GetComponent<TypingEffect>().fullText = sentences[Random.Range(0, sentences.Length)];
    }
}
