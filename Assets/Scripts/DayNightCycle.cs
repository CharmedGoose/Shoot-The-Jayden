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

    Light lightComponent;

    void Start()
    {
        lightComponent = directionalLight.GetComponent<Light>();
    }

    void Update()
    {
        directionalLight.transform.Rotate(speed * Time.deltaTime * Vector3.right);
        if (directionalLight.transform.eulerAngles.x > dayThreshold)
        {
            RenderSettings.skybox = nightSkybox;
            lightComponent.color = new Color(0f, 0f, 0f);
            RenderSettings.ambientLight = new Color(0f, 0f, 0f);
        }
        else
        {
            RenderSettings.skybox = daySkybox;
            lightComponent.color = new Color(1, 1, 1);
            RenderSettings.ambientLight = new Color(0.3113208f, 0.3039783f, 0.3039783f);
        }
    }
}
