using UnityEngine;

public class Spawner : MonoBehaviour
{
    public float amountToSpawn = 10;
    public GameObject Object;
    void Start()
    {
        for (int i = 0; i < amountToSpawn; i++)
        {
            Instantiate(Object, new Vector3(Random.Range(-325, 325), 50, Random.Range(-325, 325)), Quaternion.identity);
        }
    }
}
