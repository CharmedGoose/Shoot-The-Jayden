using UnityEngine;

public class EndingSelector : MonoBehaviour
{
    void Awake()
    {
        int i = 0;
        foreach (Transform ending in transform)
        {
            if (i == GameManager.instance.GetEnding())
            {
                ending.gameObject.SetActive(true);
            }
            i++;
        }
    }
}
