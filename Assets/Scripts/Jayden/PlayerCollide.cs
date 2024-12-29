using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCollide : MonoBehaviour
{
    [Header("References")]
    public GameObject player;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == player)
        {
            GameManager.instance.SetEnding(0);
            SceneManager.LoadScene("End");
        }
    }
}
