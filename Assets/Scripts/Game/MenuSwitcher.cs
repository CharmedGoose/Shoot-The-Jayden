using UnityEngine;
using UnityEngine.InputSystem;

public class SettingsMenuSwitcher : MonoBehaviour
{
    GameObject gameUI;
    GameObject menuUI;
    GameObject pauseUI;
    GameObject settingsUI;

    InputAction menu;

    void Awake()
    {
        gameUI = transform.GetChild(0).gameObject;
        menuUI = transform.GetChild(1).gameObject;

        pauseUI = menuUI.transform.GetChild(1).gameObject;
        settingsUI = menuUI.transform.GetChild(2).gameObject;
        
        gameUI.SetActive(true);
        menuUI.SetActive(false);

        pauseUI.SetActive(true);
        settingsUI.SetActive(false);

        menu = InputSystem.actions.FindAction("Menu");
    }

    void OnEnable()
    {
        menu.performed += ctx => ToggleUI();
    }
    
    void OnDisable()
    {
        menu.performed -= ctx => ToggleUI();
    }

    void ToggleUI()
    {
        if (settingsUI.activeSelf)
        {
            settingsUI.SetActive(false);
            pauseUI.SetActive(true);
            return;
        }

        menuUI.SetActive(!menuUI.activeSelf);
        gameUI.SetActive(!gameUI.activeSelf);

        GameManager.instance.SetPaused(menuUI.activeSelf);

        Cursor.lockState = menuUI.activeSelf ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = menuUI.activeSelf;
    }
}
