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
    WeaponSwitcher weaponSwitcher;
    GameObject weaponCamera;
    Animator animator;
    InputAction scopeButton;

    void Awake()
    {
        mainCamera = Camera.main;
        defaultFOV = mainCamera.fieldOfView;
        animator = GetComponent<Animator>();
        mouseLook = mainCamera.GetComponent<MouseLook>();
        weaponSwitcher = GetComponent<WeaponSwitcher>();
        weaponCamera = transform.parent.Find("WeaponCamera").gameObject;
        scopeButton = InputSystem.actions.FindAction("Aim");

        scopeButton.performed += ctx =>
        {
            if (GameManager.instance.IsPaused()) return;

            if (animator.GetBool("isReloading") || resume) return;
            if (weaponSwitcher.selectedWeapon == 1)
            {
                animator.SetBool("isVectorScoped", true);
                return;
            }
            if (animator.GetBool("eject")) return;
            animator.SetBool("isScoped", true);
            StartCoroutine(OnScope());
        };

        scopeButton.canceled += ctx => OnUnscope();
    }

    void OnEnable()
    {
        scopeButton.Enable();
    }

    void OnDisable()
    {
        scopeButton.Disable();
    }

    void Update()
    {
        if (GameManager.instance.IsPaused()) return;

        if (!scopeButton.IsPressed() || animator.GetBool("isReloading") || animator.GetBool("eject") && weaponSwitcher.selectedWeapon != 1)
        {
            if ((animator.GetBool("eject") || animator.GetBool("isReloading")) && animator.GetBool("isScoped"))
            {
                resume = true;
            }

            animator.SetBool("isScoped", false);
            OnUnscope();
        }
        if ((!scopeButton.IsPressed() || animator.GetBool("isReloading")) && weaponSwitcher.selectedWeapon == 1)
        {
            if (animator.GetBool("isReloading") && animator.GetBool("isVectorScoped"))
            {
                resume = true;
            }
            animator.SetBool("isVectorScoped", false);
            return;
        }
        if (resume && !animator.GetBool("isReloading") && scopeButton.IsPressed())
        {
            if (weaponSwitcher.selectedWeapon == 1)
            {
                animator.SetBool("isVectorScoped", true);
                resume = false;
                return;
            }
            if (animator.GetBool("eject")) return;
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
        mouseLook.currentSensitivityX = mouseLook.mouseADSSensitivityX;
        mouseLook.currentSensitivityY = mouseLook.mouseADSSensitivityY;
    }

    void OnUnscope()
    {
        if (GameManager.instance.IsPaused()) return;

        if (weaponSwitcher.selectedWeapon == 1)
        {
            animator.SetBool("isVectorScoped", false);
            return;
        }
        animator.SetBool("isScoped", false);

        scope.SetActive(false);
        weaponCamera.SetActive(true);

        mainCamera.fieldOfView = defaultFOV;
        mouseLook.currentSensitivityX = mouseLook.mouseSensitivityX;
        mouseLook.currentSensitivityY = mouseLook.mouseSensitivityY;
    }
}
