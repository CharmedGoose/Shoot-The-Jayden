using UnityEngine;
using UnityEngine.Rendering;

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
            RenderSettings.ambientMode = AmbientMode.Flat;
        }
        else
        {
            RenderSettings.skybox = daySkybox;
            RenderSettings.ambientMode = AmbientMode.Skybox;
        }
    }
}
