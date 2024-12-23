using UnityEngine;

public class MoveSideToSide : MonoBehaviour
{
    void Update()
    {
        transform.position = new Vector3(-transform.position.x, 0, 0);
    }
}
