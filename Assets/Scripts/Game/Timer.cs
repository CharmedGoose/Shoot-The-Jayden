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
    public WeaponSwitcher weaponSwitcher;

    void Update()
    {
        timeAmount -= Time.deltaTime;

        if (timeAmount <= -60)
        {
            weaponSwitcher.selectedWeapon = 1;
            weaponSwitcher.SelectWeapon();
            timerText.text = TimeSpan.FromSeconds(timeAmount + 60).ToString(@"mm\:ss");
            objectiveText.text = $"<b>Objective:</b>\nGun Them Down {GameManager.instance.GetJaydenCount()} / 10";
        }  else 
        if (timeAmount <= 0)
        {
            timerText.text = "???";
            objectiveText.text = "<b>Objective:</b>\nRUN";
        }
        else
        {
            timerText.text = TimeSpan.FromSeconds(timeAmount).ToString(@"mm\:ss");
        }
    }
}
