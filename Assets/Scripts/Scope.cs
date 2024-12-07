//Tutorial Used: https://www.youtube.com/watch?v=adcKX1c-kag

using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class Scope : MonoBehaviour
{
    [Header("Settings")]
    public float scopeFOV = 15f;

    [Header("References")]
    public GameObject scope;
    public Camera mainCamera;

    float defaultFOV;

    MouseLook mouseLook;
    GameObject weaponCamera;
    Animator animator;
    InputAction scopeButton;

    void Start()
    {
        defaultFOV = mainCamera.fieldOfView;
        animator = GetComponent<Animator>();
        mouseLook = mainCamera.GetComponent<MouseLook>();
        weaponCamera = transform.parent.Find("WeaponCamera").gameObject;
        scopeButton = InputSystem.actions.FindAction("Aim");

        scopeButton.performed += ctx =>
        {
            animator.SetBool("isScoped", true);
            StartCoroutine(OnScope());
        };

        scopeButton.canceled += ctx =>
        {
            animator.SetBool("isScoped", false);
            OnUnscope();
        };
    }

    void Update()
    {
        if (!scopeButton.IsPressed())
        {
            animator.SetBool("isScoped", false);
            OnUnscope();
        }
    }

    IEnumerator OnScope()
    {
        yield return new WaitForSeconds(0.15f);
        scope.SetActive(true);
        weaponCamera.SetActive(false);

        mainCamera.fieldOfView = scopeFOV;
        mouseLook.currentSensitivity = mouseLook.mouseADSSensitivity;
    }

    void OnUnscope()
    {
        scope.SetActive(false);
        weaponCamera.SetActive(true);

        mainCamera.fieldOfView = defaultFOV;
        mouseLook.currentSensitivity = mouseLook.mouseSensitivity;
    }
}
