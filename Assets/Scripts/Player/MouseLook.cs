// Tutorial Used: https://www.youtube.com/watch?v=_QajrabyTJc

using UnityEngine;
using UnityEngine.InputSystem;

public class MouseLook : MonoBehaviour
{
    [Header("Settings")]
    public float mouseSensitivityX = 50f;
    public float mouseSensitivityY = 50f;
    public float mouseADSSensitivityX = 5f;
    public float mouseADSSensitivityY = 5f;

    public float headRotationY;
    [HideInInspector] public float currentSensitivityX;
    [HideInInspector] public float currentSensitivityY;
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
        currentSensitivityX = mouseSensitivityX;

        mouse = InputSystem.actions.FindAction("Look");

        UpdateEmptySettings();
    }

    void Update()
    {   
        if (mouse == null) return;

        mousePosition = mouse.ReadValue<Vector2>();

        mouseX = mousePosition.x * (currentSensitivityX * 0.004f);
        mouseY = mousePosition.y * (currentSensitivityY * 0.004f);

        // https://discussions.unity.com/t/weapon-recoil/831478/2
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

    // https://discussions.unity.com/t/weapon-recoil/831478/2
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

    public void UpdateSensitivity()
    {
        mouseSensitivityX = PlayerPrefs.GetFloat("MouseSensitivityX");
        mouseSensitivityY = PlayerPrefs.GetFloat("MouseSensitivityY");
        currentSensitivityX = mouseSensitivityX;
        currentSensitivityY = mouseSensitivityY;
    }

    public void UpdateADSSensitivity()
    {
        mouseADSSensitivityX = PlayerPrefs.GetFloat("ADSSensitivityX");
        mouseADSSensitivityY = PlayerPrefs.GetFloat("ADSSensitivityY");
    }

    void UpdateEmptySettings()
    {
        if (!PlayerPrefs.HasKey("MouseSensitivityX"))
        {
            PlayerPrefs.SetFloat("MouseSensitivityX", mouseSensitivityX);
        }
        if (!PlayerPrefs.HasKey("MouseSensitivityY"))
        {
            PlayerPrefs.SetFloat("MouseSensitivityY", mouseSensitivityY);
        }
        if (!PlayerPrefs.HasKey("ADSSensitivityX"))
        {
            PlayerPrefs.SetFloat("ADSSensitivityX", mouseADSSensitivityX);
        }
        if (!PlayerPrefs.HasKey("ADSSensitivityY"))
        {
            PlayerPrefs.SetFloat("ADSSensitivityY", mouseADSSensitivityY);
        }

        UpdateSensitivity();

        UpdateADSSensitivity();

        PlayerPrefs.Save();
    }
}