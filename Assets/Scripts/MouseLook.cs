using UnityEngine;
using UnityEngine.InputSystem;

public class MouseLook : MonoBehaviour
{

    public Transform body;

    public float mouseSensitivity = 100f;

    float xRotation = 0f;

    InputAction mouse;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        mouse = InputSystem.actions.FindAction("Look");
    }

    void Update()
    {
        Vector2 mousePosition = mouse.ReadValue<Vector2>();

        float mouseX = mousePosition.x * mouseSensitivity * Time.deltaTime;
        float mouseY = mousePosition.y * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        
        transform.parent.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        body.Rotate(Vector3.up * mouseX);
    }
}
