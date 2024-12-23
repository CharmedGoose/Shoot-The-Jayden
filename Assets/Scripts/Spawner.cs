using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Settings")]
    public float amountToSpawn = 10;

    [Header("References")]
    public GameObject Object;
    public TargetJayden targetJayden;

    void Start()
    {
        JaydenCounter.JaydenCount = 0;
        for (int i = 0; i < amountToSpawn; i++)
        {
            GameObject obj = Instantiate(Object, new Vector3(Random.Range(-325, 325), 50, Random.Range(-325, 325)), Quaternion.identity, GameObject.Find("Jaydens").transform);
            obj.name = "Jayden " + (i + 1);

            if (targetJayden != null)
            {
                targetJayden.jaydens.Add(obj.transform);
            }
        }
    }
}
