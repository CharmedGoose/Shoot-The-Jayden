using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class Gun : MonoBehaviour
{
    [Header("Gun Stats")]
    public float damage = 10f;
    public float range = 1000f;
    public float fireRate = 15f;

    public float maxAmmo = 10f;
    public float reloadTime = 1f;

    [Header("References")]
    public ParticleSystem muzzleFlash;
    public GameObject impactEffect;

    float currentAmmo;

    bool isReloading = false;

    float nextTimeToFire = 0f;

    Transform cameraTransform;

    InputAction shootButton;

    void Start()
    {
        shootButton = InputSystem.actions.FindAction("Shoot");
        cameraTransform = Camera.main.transform;
        currentAmmo = maxAmmo;
    }

    void Update()
    {
        if (isReloading)
        {
            return;
        }

        if (currentAmmo <= 0)
        {
            StartCoroutine(Reload());
            return;
        }

        if (shootButton.IsPressed() && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            Shoot();
        }
    }

    void Shoot()
    {
        muzzleFlash.Play();

        currentAmmo--;

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

    IEnumerator Reload()
    {
        isReloading = true;
        yield return new WaitForSeconds(reloadTime);
        currentAmmo = maxAmmo;
        isReloading = false;
    }
}
