using UnityEngine;
using UnityEngine.InputSystem;

public class SettingsMenuSwitcher : MonoBehaviour
{
    GameObject gameUI;
    GameObject settingsUI;

    InputAction menu;

    void Awake()
    {
        gameUI = transform.GetChild(0).gameObject;
        settingsUI = transform.GetChild(1).gameObject;
        
        gameUI.SetActive(true);
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
        gameUI.SetActive(!gameUI.activeSelf);
        settingsUI.SetActive(!settingsUI.activeSelf);
    }
}
