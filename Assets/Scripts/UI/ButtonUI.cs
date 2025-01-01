using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonUI : MonoBehaviour
{
    [Header("References")]
    public GameObject menu;
    public GameObject settings;
    public Slider mouseSliderX;
    public Slider mouseSliderY;
    public TMP_InputField mouseInputX;
    public TMP_InputField mouseInputY;
    public MouseLook mouseLook;

    void OnEnable()
    {
        if (!mouseSliderX) return;

        mouseSliderX.value = PlayerPrefs.GetFloat("MouseSensitivityX");
        mouseSliderY.value = PlayerPrefs.GetFloat("MouseSensitivityY");
        mouseInputX.text = PlayerPrefs.GetFloat("MouseSensitivityX").ToString();
        mouseInputY.text = PlayerPrefs.GetFloat("MouseSensitivityY").ToString();
    }

    public void TryAgain()
    {
        SceneManager.LoadScene("Game");
    }

    public void Settings()
    {
        menu.SetActive(false);
        settings.SetActive(true);
    }

    public void Back()
    {
        menu.SetActive(true);
        settings.SetActive(false);
    }

    public void UpdateSensitivityX(float sensitivity)
    {
        PlayerPrefs.SetFloat("MouseSensitivityX", sensitivity);
        mouseLook.UpdateSensitivity();
        mouseInputX.text = sensitivity.ToString();
    }
    public void UpdateSensitivityX(string sensitivity)
    {
        PlayerPrefs.SetFloat("MouseSensitivityX", float.Parse(sensitivity, System.Globalization.NumberStyles.Integer));
        mouseLook.UpdateSensitivity();
        mouseSliderX.value = PlayerPrefs.GetFloat("MouseSensitivityX");
    }

    public void UpdateSensitivityY(float sensitivity)
    {
        PlayerPrefs.SetFloat("MouseSensitivityY", sensitivity);
        mouseLook.UpdateSensitivity();
        mouseInputY.text = sensitivity.ToString();
    }
    public void UpdateSensitivityY(string sensitivity)
    {
        PlayerPrefs.SetFloat("MouseSensitivityY", float.Parse(sensitivity, System.Globalization.NumberStyles.Integer));
        mouseLook.UpdateSensitivity();
        mouseSliderY.value = PlayerPrefs.GetFloat("MouseSensitivityY");
    }
}
