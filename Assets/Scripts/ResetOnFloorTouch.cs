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
            jaydenMovement.transform.position = new Vector3(Random.Range(-325, 325), 50, Random.Range(-325, 325));
            jaydenMovement.SetActive(true);
        }
    }
}
