using UnityEngine;

public class ShowBorder : MonoBehaviour
{
    [Header("Borders")]
    public Transform northWall;
    public Transform eastWall;
    public Transform southWall;
    public Transform westWall;

    [Header("Settings")]
    public float fadeDistance = 91f;
    public float fadeStrength = 0.1f;


    Transform player;
    Renderer northWallRenderer;
    Renderer eastWallRenderer;
    Renderer southWallRenderer;
    Renderer westWallRenderer;

    float opacity;
    float distance;

    void Start()
    {
        player = Camera.main.transform;
        northWallRenderer = northWall.GetComponent<Renderer>();
        eastWallRenderer = eastWall.GetComponent<Renderer>();
        southWallRenderer = southWall.GetComponent<Renderer>();
        westWallRenderer = westWall.GetComponent<Renderer>();

        northWallRenderer.material.color = new Color(0, 0, 0, 0);
        eastWallRenderer.material.color = new Color(0, 0, 0, 0);
        southWallRenderer.material.color = new Color(0, 0, 0, 0);
        westWallRenderer.material.color = new Color(0, 0, 0, 0);
    }

    void Update()
    {
        UpdateBorderOpacity(northWallRenderer, GetNorthDistance);
        UpdateBorderOpacity(eastWallRenderer, GetEastDistance);
        UpdateBorderOpacity(southWallRenderer, GetSouthDistance);
        UpdateBorderOpacity(westWallRenderer, GetWestDistance);
    }

    void UpdateBorderOpacity(Renderer renderer, System.Func<float> getDistance)
    {
        distance = getDistance();
        if(distance < fadeDistance)
        {
            opacity = (fadeDistance - distance) * fadeStrength;
            renderer.material.color = new Color(0, 0, 0, opacity);
        }
    }

    float GetNorthDistance() => northWall.position.z - player.position.z;
    float GetEastDistance() => eastWall.position.x - player.position.x;
    float GetSouthDistance() => Mathf.Abs(southWall.position.z - player.position.z);
    float GetWestDistance() => Mathf.Abs(westWall.position.x - player.position.x);
}
