using UnityEngine;

public class DayNightCycle : MonoBehaviour
{

    [Header("Settings")]
    public float speed = 1;
    public float dayThreshold = 180;

    [Header("References")]
    public Transform directionalLight;
    public GameObject playerLight;

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
            RenderSettings.ambientLight = new Color(0f, 0f, 0f);
            lightComponent.color = new Color(1f, 0f, 0f);
            playerLight.SetActive(true);
        }
        else
        {
            RenderSettings.skybox = daySkybox;
            RenderSettings.ambientLight = new Color(0.3113208f, 0.3039783f, 0.3039783f);
            lightComponent.color = new Color(1f, 1f, 1f);
            playerLight.SetActive(false);
        }
    }
}
