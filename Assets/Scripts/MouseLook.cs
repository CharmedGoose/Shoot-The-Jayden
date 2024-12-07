// Tutorial Used: https://www.youtube.com/watch?v=_QajrabyTJc

using UnityEngine;
using UnityEngine.InputSystem;

public class MouseLook : MonoBehaviour
{
    [Header("Settings")]
    public float mouseSensitivity = 100f;
    public float mouseADSSensitivity = 50f;
    public float headRotationY;
    [HideInInspector]
    public float currentSensitivity;
    [HideInInspector]
    public bool shot = false;

    [Header("References")]
    public Transform body;
    public Gun gun;

    float xRotation = 0f;

    float recoil;

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

        gun = transform.parent.GetComponentInChildren<Gun>();

        Vector2 mousePosition = mouse.ReadValue<Vector2>();

        float mouseX = mousePosition.x * currentSensitivity * Time.deltaTime;
        float mouseY = mousePosition.y * currentSensitivity * Time.deltaTime;

        //Taken From https://discussions.unity.com/t/weapon-recoil/831478/2
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

            Quaternion result = Recoil(time);

            mouseX += result.x;
            mouseY += result.y;
        }

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        
        transform.parent.localRotation = Quaternion.Euler(xRotation, headRotationY, 0f);
        body.Rotate(Vector3.up * mouseX);
    }

    //Taken From https://discussions.unity.com/t/weapon-recoil/831478/2
    Quaternion Recoil(float time) {
        float vertical = -gun.recoilVertical.Evaluate(time);
        float horizontal = gun.recoilHorizontal.Evaluate(time);

        if (time == 0)
        {
            vertical = 0;
            horizontal = 0;
        }

        return Quaternion.Euler(vertical, horizontal, 0);
    }
}
