using UnityEngine;
using UnityEngine.InputSystem;

public class Gun : MonoBehaviour
{
    public float damage = 10f;
    public float range = 100f;
    public float fireRate = 15f;

    float nextTimeToFire = 0f;

    public ParticleSystem muzzleFlash;

    Transform cameraTransform;

    InputAction shootButton;

    void Start()
    {
        shootButton = InputSystem.actions.FindAction("Shoot");
        cameraTransform = Camera.main.transform;
    }

    void Update()
    {
        if (shootButton.IsPressed() && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            Shoot();
        }
    }

    void Shoot()
    {
        muzzleFlash.Play();

        if (Physics.Raycast(cameraTransform.position, transform.forward, out RaycastHit hit, range))
        {
            Debug.Log(hit.transform.name);

            Target target = hit.transform.GetComponent<Target>();
            if (target != null)
            {
                target.TakeDamage(damage);
            }
        }
    }
}
