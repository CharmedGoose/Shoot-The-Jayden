using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class MenuSwitcher : MonoBehaviour
{
    Animator animator;

    GameObject gameUI;
    GameObject menuUI;

    bool paused = false;

    InputAction menu;

    void Awake()
    {
        animator = GetComponent<Animator>();

        animator.SetBool("paused", false);
        animator.SetBool("settingsOpen", false);

        gameUI = transform.GetChild(0).gameObject;
        menuUI = transform.GetChild(1).gameObject;

        menu = InputSystem.actions.FindAction("Menu");

        menu.performed += ctx => ToggleUI();
    }

    void OnEnable()
    {
        menu.Enable();
    }
    
    void OnDisable()
    {
        menu.Disable();
    }

    void ToggleUI()
    {
        if (animator.GetBool("settingsOpen"))
        {
            animator.SetBool("settingsOpen", false);
            return;
        }

        paused = !paused;

        animator.SetBool("paused", paused);

        if (paused)
        {
            gameUI.SetActive(false);
            menuUI.SetActive(true);
        }
        else
        {
            StartCoroutine(ShowGameUI());
        }

        GameManager.instance.SetPaused(paused);

        Cursor.lockState = paused ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = paused;
    }

    IEnumerator ShowGameUI()
    {
        yield return new WaitForSeconds(0.25f);
        gameUI.SetActive(true);
        menuUI.SetActive(false);
    }
}
