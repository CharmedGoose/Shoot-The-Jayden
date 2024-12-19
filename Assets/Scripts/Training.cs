using UnityEngine;

public class Training : MonoBehaviour
{
    [Header("References")]
    public Transform playerCamera;
    public Transform weaponHolder;

    void Start()
    {
        GetComponent<PlayerMovement>().enabled = false;
        playerCamera.GetComponent<MouseLook>().enabled = false;
        foreach (Transform weapon in weaponHolder)
        {
            if (weapon.name != "Hands")
            {
                weapon.GetComponent<Gun>().isTraining = true;
            }
        }
    }
}
