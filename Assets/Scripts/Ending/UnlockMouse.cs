using UnityEngine;

public class UnlockMouse : MonoBehaviour
{
    void Awake()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
