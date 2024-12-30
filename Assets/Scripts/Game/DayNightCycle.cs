// its messy but it works

using UnityEngine;

public class DayNightCycle : MonoBehaviour
{

    [Header("Settings")]
    public float speed = 1;
    public float night = 90;

    [Header("References")]
    public Transform directionalLight;
    public GameObject playerLight;
    public GameObject playerAreaLight;

    public Material daySkybox;
    public Material nightSkybox;
    public Material midnightSkybox;

    Light lightComponent;

    void Start()
    {
        lightComponent = directionalLight.GetComponent<Light>();
    }

    void Update()
    {
        directionalLight.transform.Rotate(speed * Time.deltaTime * Vector3.right);

        if (directionalLight.transform.eulerAngles.x < 90)
        {
            RenderSettings.skybox = daySkybox;
            RenderSettings.ambientLight = new Color(0.3113208f, 0.3039783f, 0.3039783f);
            lightComponent.color = new Color(1f, 1f, 1f);
            playerLight.SetActive(false);
            playerAreaLight.SetActive(false);
            GameManager.instance.SetTargetPlayer(false);
        }
        else if (directionalLight.transform.eulerAngles.x < night)
        {
            RenderSettings.skybox = midnightSkybox;
            RenderSettings.ambientLight = new Color(0f, 0f, 0f);
            lightComponent.color = new Color(0f, 0f, 0f);
            playerLight.SetActive(true);
            playerAreaLight.SetActive(true);
            GameManager.instance.SetTargetPlayer(true);
        }
        else
        {
            RenderSettings.skybox = nightSkybox;
            RenderSettings.ambientLight = new Color(0.066f, 0.066f, 0.066f);
            lightComponent.color = new Color(0.066f, 0.066f, 0.066f);
            playerLight.SetActive(true);
            playerAreaLight.SetActive(false);
        }
    }
}
