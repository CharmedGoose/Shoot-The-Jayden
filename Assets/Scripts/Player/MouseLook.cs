// Tutorial Used: https://www.youtube.com/watch?v=_QajrabyTJc

using UnityEngine;
using UnityEngine.InputSystem;

public class MouseLook : MonoBehaviour
{
    [Header("Settings")]
    public float mouseSensitivity = 100f;
    public float mouseADSSensitivity = 50f;
    public float headRotationY;
    [HideInInspector] public float currentSensitivity;
    [HideInInspector] public bool shot = false;

    [Header("References")]
    public Transform body;
    public Gun gun;

    float xRotation = 0f;

    float recoil;

    Vector2 mousePosition;

    float mouseX;
    float mouseY;

    Quaternion result;

    float vertical;
    float horizontal;

    InputAction mouse;

    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        currentSensitivity = mouseSensitivity;

        mouse = InputSystem.actions.FindAction("Look");
    }

    void Update()
    {   
        if (mouse == null) return;

        mousePosition = mouse.ReadValue<Vector2>();

        mouseX = mousePosition.x * currentSensitivity;
        mouseY = mousePosition.y * currentSensitivity;

        //https://discussions.unity.com/t/weapon-recoil/831478/2
        if (shot)
        {
            recoil = Time.deltaTime;
        }

        if (recoil > 0)
        {
            float time = recoil / gun.recoilTimeInterval;

            recoil += Time.deltaTime;
            if (recoil > gun.recoilTimeInterval)
            {
                recoil = 0;
                time = 0;
            }

            result = Recoil(time);

            mouseX += result.x;
            mouseY += result.y;
        }

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        
        transform.parent.localRotation = Quaternion.Euler(xRotation, headRotationY, 0f);
        body.Rotate(Vector3.up * mouseX);
    }

    //https://discussions.unity.com/t/weapon-recoil/831478/2
    Quaternion Recoil(float time) {
        vertical = -gun.recoilVertical.Evaluate(time);
        horizontal = gun.recoilHorizontal.Evaluate(time);

        if (time == 0)
        {
            vertical = 0;
            horizontal = 0;
        }

        return Quaternion.Euler(vertical, horizontal, 0);
    }
}
