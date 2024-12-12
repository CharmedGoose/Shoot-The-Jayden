using UnityEngine;

public class DayNightCycle : MonoBehaviour
{

    [Header("Settings")]
    public float speed = 1;
    public float dayThreshold = 180;

    [Header("References")]
    public Transform directionalLight;
    public Material daySkybox;
    public Material nightSkybox;

    void Update()
    {
        directionalLight.transform.Rotate(speed * Time.deltaTime * Vector3.right);
        if (directionalLight.transform.eulerAngles.x > dayThreshold)
        {
            RenderSettings.skybox = nightSkybox;
        }
        else
        {
            RenderSettings.skybox = daySkybox;
        }
    }
}
