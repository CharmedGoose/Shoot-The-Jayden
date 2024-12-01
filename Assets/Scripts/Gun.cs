using UnityEngine;
using UnityEngine.InputSystem;

public class Gun : MonoBehaviour
{
    public float damage = 10f;
    public float range = 100f;

    Transform cameraTransform;

    InputAction shootButton;

    void Start()
    {
        shootButton = InputSystem.actions.FindAction("Shoot");
        cameraTransform = Camera.main.transform;
    }

    void Update()
    {
        if (shootButton.IsPressed())
        {
            Shoot();
        }
    }

    void Shoot()
    {
        if (Physics.Raycast(cameraTransform.position, transform.forward, out RaycastHit hit, range))
        {
            Debug.Log(hit.transform.name);
        }
    }
}
