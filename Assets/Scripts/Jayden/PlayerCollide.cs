using UnityEngine;

public class PlayerCollide : MonoBehaviour
{
    [Header("References")]
    public GameObject player;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == player)
        {
            
        }
    }
}
