using System;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [Header("Settings")]
    public float timeAmount = 300;

    [Header("References")]
    public TMPro.TextMeshProUGUI timerText;

    void Update()
    {
        timeAmount -= Time.deltaTime;
        timerText.text = TimeSpan.FromSeconds(timeAmount).ToString(@"mm\:ss");
    }
}
