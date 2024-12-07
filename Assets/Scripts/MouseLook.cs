// Tutorial Used: https://www.youtube.com/watch?v=_QajrabyTJc

using UnityEngine;
using UnityEngine.InputSystem;

public class MouseLook : MonoBehaviour
{
    [Header("Settings")]
    public float mouseSensitivity = 100f;
    public float mouseADSSensitivity = 50f;
    [HideInInspector]
    public float currentSensitivity;

    [Header("References")]
    public Transform body;

    float xRotation = 0f;

    InputAction mouse;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        currentSensitivity = mouseSensitivity;

        mouse = InputSystem.actions.FindAction("Look");
    }

    void Update()
    {   
        if (mouse == null) return;

        Vector2 mousePosition = mouse.ReadValue<Vector2>();

        float mouseX = mousePosition.x * currentSensitivity * Time.deltaTime;
        float mouseY = mousePosition.y * currentSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        
        transform.parent.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        body.Rotate(Vector3.up * mouseX);
    }
}
