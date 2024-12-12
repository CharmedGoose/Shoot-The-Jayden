public static class JaydenCounter
{
    public static int JaydenCount;

    public static void RemoveJayden(TMPro.TextMeshProUGUI text)
    {
        JaydenCount++;
        text.text = $"Objective:\nShoot Jaydens: {JaydenCount} / 10";
    }
}
