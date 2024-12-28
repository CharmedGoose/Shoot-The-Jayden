using System;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [Header("Settings")]
    public float timeAmount = 300;

    [Header("References")]
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI objectiveText;

    void Update()
    {
        timeAmount -= Time.deltaTime;
        timerText.text = TimeSpan.FromSeconds(timeAmount).ToString(@"mm\:ss");
        if (timeAmount <= 0)
        {
            objectiveText.text = "<b>Objective:</b>\nRUN";
        }
    }
}
