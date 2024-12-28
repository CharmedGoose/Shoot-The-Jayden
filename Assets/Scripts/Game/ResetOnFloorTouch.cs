using UnityEngine;

public class ResetOnFloorTouch : MonoBehaviour
{

    GameObject jaydenMovement;
    void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Jayden"))
        {
            jaydenMovement = collider.transform.parent.gameObject;
            jaydenMovement.SetActive(false);
            jaydenMovement.transform.localPosition = new Vector3(Random.Range(-GameManager.instance.spawnX, GameManager.instance.spawnX), 50, Random.Range(-GameManager.instance.spawnZ, GameManager.instance.spawnZ));
            jaydenMovement.SetActive(true);
        }
    }
}
