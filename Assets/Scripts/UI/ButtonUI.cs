using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonUI : MonoBehaviour
{
    [Header("References")]
    public Animator animator;
    public GameObject menu;
    public GameObject settings;
    public Slider mouseSliderX;
    public Slider mouseSliderY;
    public Slider ADSSliderX;
    public Slider ADSSliderY;
    public TMP_InputField mouseInputX;
    public TMP_InputField mouseInputY;
    public TMP_InputField ADSInputX;
    public TMP_InputField ADSInputY;
    public MouseLook mouseLook;

    public void UpdateValues()
    {
        mouseSliderX.value = PlayerPrefs.GetFloat("MouseSensitivityX");
        mouseSliderY.value = PlayerPrefs.GetFloat("MouseSensitivityY");
        mouseInputX.text = PlayerPrefs.GetFloat("MouseSensitivityX").ToString();
        mouseInputY.text = PlayerPrefs.GetFloat("MouseSensitivityY").ToString();
        ADSSliderX.value = PlayerPrefs.GetFloat("ADSSensitivityX");
        ADSSliderY.value = PlayerPrefs.GetFloat("ADSSensitivityY");
        ADSInputX.text = PlayerPrefs.GetFloat("ADSSensitivityX").ToString();
        ADSInputY.text = PlayerPrefs.GetFloat("ADSSensitivityY").ToString();
    }

    public void Play()
    {
        StartCoroutine(LevelLoader.instance.LoadLevel(1));
    }

    public void Settings()
    {
        animator.SetBool("settingsOpen", true);
    }

    public void MainMenu()
    {
        StartCoroutine(LevelLoader.instance.LoadLevel(0));
        Time.timeScale = 1;
        GameManager.instance.SetPaused(false);
    }

    public void Credits()
    {
        StartCoroutine(LevelLoader.instance.LoadLevel(3));
    }

    public void ExitSettings()
    {
        animator.SetBool("settingsOpen", false);
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

    public void UpdateADSSensitivityX(float sensitivity)
    {
        PlayerPrefs.SetFloat("ADSSensitivityX", sensitivity);
        mouseLook.UpdateADSSensitivity();
        ADSInputX.text = sensitivity.ToString();
    }
    public void UpdateADSSensitivityX(string sensitivity)
    {
        PlayerPrefs.SetFloat("ADSSensitivityX", float.Parse(sensitivity, System.Globalization.NumberStyles.Integer));
        mouseLook.UpdateADSSensitivity();
        ADSSliderX.value = PlayerPrefs.GetFloat("ADSSensitivityX");
    }

    public void UpdateADSSensitivityY(float sensitivity)
    {
        PlayerPrefs.SetFloat("ADSSensitivityY", sensitivity);
        mouseLook.UpdateADSSensitivity();
        ADSInputY.text = sensitivity.ToString();
    }
    public void UpdateADSSensitivityY(string sensitivity)
    {
        PlayerPrefs.SetFloat("ADSSensitivityY", float.Parse(sensitivity, System.Globalization.NumberStyles.Integer));
        mouseLook.UpdateADSSensitivity();
        ADSSliderY.value = PlayerPrefs.GetFloat("ADSSensitivityY");
    }
}
