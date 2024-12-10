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

    float defaultFOV;

    bool resume;

    Camera mainCamera;
    MouseLook mouseLook;
    GameObject weaponCamera;
    Animator animator;
    InputAction scopeButton;

    void Awake()
    {
        mainCamera = Camera.main;
        defaultFOV = mainCamera.fieldOfView;
        animator = GetComponent<Animator>();
        mouseLook = mainCamera.GetComponent<MouseLook>();
        weaponCamera = transform.parent.Find("WeaponCamera").gameObject;
        scopeButton = InputSystem.actions.FindAction("Aim");
    }

    void OnEnable()
    {
        scopeButton.performed += ctx =>
        {
            if (animator.GetBool("eject") || animator.GetBool("isReloading") || resume) return;
            animator.SetBool("isScoped", true);
            StartCoroutine(OnScope());
        };

        scopeButton.canceled += ctx =>
        {
            animator.SetBool("isScoped", false);
            OnUnscope();
        };
    }

    void OnDisable()
    {
        scopeButton.performed -= ctx =>
        {
            if (animator.GetBool("eject") || animator.GetBool("isReloading") || resume) return;
            animator.SetBool("isScoped", true);
            StartCoroutine(OnScope());
        };

        scopeButton.canceled -= ctx =>
        {
            animator.SetBool("isScoped", false);
            OnUnscope();
        };
    }

    void Update()
    {
        if (!scopeButton.IsPressed() || animator.GetBool("eject") || animator.GetBool("isReloading"))
        {
            if((animator.GetBool("eject") || animator.GetBool("isReloading")) && animator.GetBool("isScoped"))
            {
                resume = true;
            }
            animator.SetBool("isScoped", false);
            OnUnscope();
        }
        if(resume && !animator.GetBool("eject") && !animator.GetBool("isReloading") && scopeButton.IsPressed())
        {
            animator.SetBool("isScoped", true);
            StartCoroutine(OnScope());
            resume = false;
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
