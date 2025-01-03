using UnityEngine;

public class RotateCameraOnCollide : MonoBehaviour
{
    [Header("References")]
    public Transform cameraPivot;

    Quaternion targetRotation;

    void Update()
    {
        cameraPivot.localRotation = Quaternion.Slerp(cameraPivot.localRotation, targetRotation, 1f - Mathf.Exp(-Time.deltaTime));
    }

    void OnTriggerEnter()
    {
        targetRotation = Quaternion.Euler(-75, 0, 0);
    }

    void OnTriggerExit()
    {
        targetRotation = Quaternion.Euler(0, 0, 0);
    }
}
