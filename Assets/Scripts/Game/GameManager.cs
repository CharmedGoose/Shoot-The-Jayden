using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public float spawnX = 325;
    public float spawnZ = 325;

    bool isPaused = false;

    int ending = 0;

    int JaydenCount;
    bool targetPlayer = false;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    public int GetEnding()
    {
        return ending;
    }

    public void SetEnding(int value)
    {
        ending = value;
    }

    public int GetJaydenCount()
    {
        return JaydenCount;
    }
    public void SetJaydenCount(int count, TextMeshProUGUI text)
    {
        JaydenCount = count;
        text.text = $"<b>Objective:</b>\nShoot Jaydens: {JaydenCount} / 10";
    }

    public void AddJayden(TextMeshProUGUI text)
    {
        JaydenCount--;
        text.text = $"<b>Objective:</b>\nShoot Jaydens: {JaydenCount} / 10";
    }
    public void RemoveJayden(TextMeshProUGUI text)
    {
        JaydenCount++;
        text.text = $"<b>Objective:</b>\nShoot Jaydens: {JaydenCount} / 10";
    }

    public bool GetTargetPlayer()
    {
        return targetPlayer;
    }
    public void SetTargetPlayer(bool value)
    {
        targetPlayer = value;
    }

    public bool IsPaused()
    {
        return isPaused;
    }

    public void SetPaused(bool value)
    {
        isPaused = value;

        Time.timeScale = isPaused ? 0 : 1;
    }
}