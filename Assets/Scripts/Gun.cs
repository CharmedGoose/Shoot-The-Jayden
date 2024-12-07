// Tutorial Used: https://www.youtube.com/watch?v=THnivyG0Mvo

using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class Gun : MonoBehaviour
{
    [Header("Gun Stats")]
    public float damage = 10f;
    public bool headshotInstantKill = false;
    public float range = 100f;
    public float fireRate = 15f;

    public float maxAmmo = 10f;
    public float reloadTime = 1f;

    [Header("References")]
    public ParticleSystem muzzleFlash;
    public GameObject impactEffect;
    public GameObject bloodEffect;
    public AudioClip shootSound;

    float currentAmmo;

    bool isReloading = false;

    float nextTimeToFire = 0f;

    int layerMask;

    Transform cameraTransform;

    InputAction shootButton;

    void Start()
    {
        shootButton = InputSystem.actions.FindAction("Shoot");
        cameraTransform = Camera.main.transform;
        currentAmmo = maxAmmo;
        layerMask =~ LayerMask.GetMask("Player");
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
        AudioSource.PlayClipAtPoint(shootSound, muzzleFlash.transform.position);

        currentAmmo--;

        if (Physics.Raycast(cameraTransform.position, transform.forward, out RaycastHit hit, range, layerMask))
        {
            Debug.Log(hit.transform.name);
            if (hit.transform.CompareTag("Head") && headshotInstantKill)
            {
                Target target = hit.transform.parent.GetComponent<Target>();
                if (target != null)
                {
                    target.TakeDamage(9999);
                }
            } else {
                if (hit.transform.CompareTag("Head"))
                {
                    Target target = hit.transform.parent.GetComponent<Target>();
                    if (target != null)
                    {
                        target.TakeDamage(damage);
                    }
                }
                else {
                    Target target = hit.transform.GetComponent<Target>();
                    if (target != null)
                    {
                        target.TakeDamage(damage);
                    }
                }
            }

            GameObject impact = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));

            Destroy(impact, 0.75f);

            if (hit.transform.CompareTag("Jayden"))
            {
                Instantiate(bloodEffect, hit.point, Quaternion.LookRotation(hit.normal), hit.transform);
            }
            else if (hit.transform.CompareTag("Head"))
            {
                Instantiate(bloodEffect, hit.point, Quaternion.LookRotation(hit.normal), hit.transform.parent);
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
