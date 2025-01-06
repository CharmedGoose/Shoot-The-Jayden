using UnityEngine;

public class EndingSelector : MonoBehaviour
{
    void Awake()
    {
        int i = 0;
        foreach (Transform ending in transform)
        {
            if (GameManager.instance.GetEnding() == i)
            {
                ending.gameObject.SetActive(true);
            }
            i++;
        }
    }
}
