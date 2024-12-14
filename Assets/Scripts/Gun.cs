// Tutorial Used: https://www.youtube.com/watch?v=THnivyG0Mvo

using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine.Animations.Rigging;
using TMPro;
using System.Collections.Generic;

public class Gun : MonoBehaviour
{
    [Header("Gun Stats")]
    public float damage = 10f;
    public bool headshotInstantKill = false;
    public float range = 100f;
    public float fireRate = 15f;

    public float maxAmmo = 10f;
    public float reloadTime = 1f;

    public bool hasRecoil = true;
    public AnimationCurve recoilHorizontal;
    public AnimationCurve recoilVertical;
    public float recoilTimeInterval = 0.25f;

    public float bulletEjectDelay = 0.1f;

    [Header("References")]
    public Animator animator;
    public ParticleSystem muzzleFlash;
    public GameObject impactEffect;
    public GameObject bloodEffect;
    public GameObject magazineBullet;
    public GameObject bullet;
    public Transform bulletSpawn;
    public AudioClip shootSound;
    public MouseLook mouseLook;
    public TextMeshProUGUI ammoText;
    [HideInInspector] public List<GameObject> bulletCasings = new();

    [Header("IK")]
    public TwoBoneIKConstraint leftHandIK;
    public Transform leftHand;
    public Transform leftHandDefault;
    public Transform leftHandReload;
    public TwoBoneIKConstraint rightHandIK;
    public Transform rightHand;
    public Transform rightHandDefault;
    public Transform rightHandEject;

    float currentAmmo;

    Target target;

    RaycastHit hit;

    GameObject impact;

    GameObject bulletCasing;

    bool isReloading = false;

    float nextTimeToFire = 0f;

    int layerMask;

    Transform cameraTransform;

    InputAction shootButton;
    InputAction reloadButton;

    void Awake()
    {
        shootButton = InputSystem.actions.FindAction("Shoot");
        reloadButton = InputSystem.actions.FindAction("Reload");
        cameraTransform = Camera.main.transform;
        currentAmmo = maxAmmo;
        ammoText.text = $"{currentAmmo} / {maxAmmo}";
        leftHand.position = leftHandDefault.position;
        rightHand.position = rightHandDefault.position;
        layerMask = ~LayerMask.GetMask("Player");
    }

    void Update()
    {
        mouseLook.shot = false;

        if (Time.time >= nextTimeToFire && animator.GetBool("eject"))
        {
            animator.SetBool("eject", false);
            rightHand.position = rightHandDefault.position;
        }
        else if (animator.GetBool("eject"))
        {
            rightHand.position = rightHandEject.position;
        }
        
        if (isReloading) 
        {
            leftHand.position = leftHandReload.position;
            return;
        };

        if (currentAmmo <= 0 || (reloadButton.IsPressed() && currentAmmo < maxAmmo))
        {
            StartCoroutine(Reload());
            return;
        }

        if (shootButton.IsPressed() && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            Shoot();
            if (hasRecoil) mouseLook.shot = true;
        }

    }

    void Shoot()
    {
        muzzleFlash.Play();
        AudioSource.PlayClipAtPoint(shootSound, muzzleFlash.transform.position);

        currentAmmo--;
        ammoText.text = $"{currentAmmo} / {maxAmmo}";

        if (Physics.Raycast(cameraTransform.position, transform.forward, out hit, range, layerMask))
        {
            if (hit.transform.CompareTag("Head") && headshotInstantKill)
            {
                target = hit.transform.parent.GetComponent<Target>();
                if (target != null)
                {
                    target.TakeDamage(9999);
                }
            }
            else
            {
                if (hit.transform.CompareTag("Head"))
                {
                    target = hit.transform.parent.GetComponent<Target>();
                    if (target != null)
                    {
                        target.TakeDamage(damage);
                    }
                }
                else
                {
                    target = hit.transform.GetComponent<Target>();
                    if (target != null)
                    {
                        target.TakeDamage(damage);
                    }
                }
            }

            impact = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));

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

        animator.SetBool("eject", true);
        rightHand.position = rightHandEject.position;
        StartCoroutine(Eject());
    }

    GameObject GetBulletCasing()
    {
        for (int i = 0; i < bulletCasings.Count; i++)
        {
            if(!bulletCasings[i].activeInHierarchy)
            {
                return bulletCasings[i];
            }
        }
        return null;
    }

    IEnumerator Reload()
    {
        isReloading = true;
        if (currentAmmo == 0) magazineBullet.SetActive(false);
        animator.SetBool("isReloading", true);
        leftHand.position = leftHandReload.position;
        yield return new WaitForSeconds(reloadTime / 2);
        magazineBullet.SetActive(true);
        yield return new WaitForSeconds(reloadTime / 2);
        currentAmmo = maxAmmo;
        ammoText.text = $"{currentAmmo} / {maxAmmo}";
        animator.SetBool("isReloading", false);
        leftHand.position = leftHandDefault.position;
        isReloading = false;
    }

    IEnumerator Eject()
    {
        yield return new WaitForSeconds(bulletEjectDelay);
        bulletCasing = GetBulletCasing();
        bulletCasing.transform.SetPositionAndRotation(bulletSpawn.position, bulletSpawn.rotation);
        bulletCasing.SetActive(true);
        bulletCasing.GetComponent<Rigidbody>().AddForce(bulletSpawn.right * 5f, ForceMode.Impulse);
        StartCoroutine(DisableBulletCasing(bulletCasing));
    }

    IEnumerator DisableBulletCasing(GameObject bulletCasing)
    {
        yield return new WaitForSeconds(5f);
        bulletCasing.SetActive(false);
    }
}
