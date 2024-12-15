public static class JaydenCounter
{
    public static int JaydenCount;

    public static bool targetPlayer = false;

    public static void RemoveJayden(TMPro.TextMeshProUGUI text)
    {
        JaydenCount++;
        text.text = $"<b>Objective:</b>\nShoot Jaydens: {JaydenCount} / 10";
    }
}
