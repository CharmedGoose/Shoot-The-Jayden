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
    public float currentAmmo;
    public float reloadTime = 1f;

    public bool hasRecoil = true;
    public AnimationCurve recoilHorizontal;
    public AnimationCurve recoilVertical;
    public float recoilTimeInterval = 0.25f;

    public float bulletEjectDelay = 0.1f;

    [HideInInspector] public bool isTraining = false;
    [HideInInspector] public bool hasMissed = false;
    [HideInInspector] public bool isShooting = false;

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
    public GameObject hitMarker;
    public AudioClip hitSound;
    [HideInInspector] public List<GameObject> bulletCasings = new();
    [HideInInspector] public List<GameObject> impacts = new();

    [Header("IK")]
    public Transform leftHand;
    public Transform leftHandDefault;
    public Transform leftHandReload;
    public Transform rightHand;
    public Transform rightHandDefault;
    public Transform rightHandEject;

    Target target;

    RaycastHit hit;

    GameObject impact;

    GameObject bulletCasing;

    Rigidbody bulletRigidbody;

    UnityEngine.UI.Image[] hitMarkers;

    bool isReloading = false;

    [HideInInspector] public float nextTimeToFire = 0f;

    bool hitHead;

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
        hitMarkers = hitMarker.GetComponentsInChildren<UnityEngine.UI.Image>();
    }

    void Update()
    {
        mouseLook.shot = false;
        if (animator.GetBool("eject")) Ejecting();

        if (isReloading)
        {
            leftHand.position = leftHandReload.position;
            return;
        };

        if (currentAmmo <= 0 || (reloadButton.IsPressed() && currentAmmo < maxAmmo && !isTraining))
        {
            StartCoroutine(Reload());
            return;
        }

        if ((shootButton.IsPressed() || isShooting)  && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            Shoot();
            if (hasRecoil) mouseLook.shot = true;
        }

    }

    public void Shoot()
    {
        muzzleFlash.Play();
        AudioSource.PlayClipAtPoint(shootSound, muzzleFlash.transform.position);

        currentAmmo--;
        ammoText.text = $"{currentAmmo} / {maxAmmo}";

        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, range, layerMask))
        {
            hitHead = hit.transform.CompareTag("Head");

            target = hitHead ? hit.transform.GetComponentInParent<Target>() : hit.transform.GetComponent<Target>();

            if (target != null)
            {
                if (hitHead && headshotInstantKill) 
                { 
                    target.TakeDamage(9999);
                    for (int i = 0; i < 4; i++)
                    {
                        hitMarkers[i].color = Color.red;
                    }
                }
                else
                {
                    for (int i = 0; i < 4; i++)
                    {
                        if (target.health - damage <= 0) hitMarkers[i].color = Color.red;
                        else hitMarkers[i].color = Color.white;
                    }
                    target.TakeDamage(damage);
                }

                hitMarker.SetActive(true);
                AudioSource.PlayClipAtPoint(hitSound, muzzleFlash.transform.position);
                StartCoroutine(DisableObject(hitMarker, 0.5f));
            }
            else hasMissed = true;

            if (hit.transform.CompareTag("Jayden") || hitHead)
            {
                Instantiate(bloodEffect, hit.point, Quaternion.LookRotation(hit.normal), hitHead ? hit.transform.parent : hit.transform);
            }
            else
            {
                impact = GetObjectFromPool(impacts);
                if (impact != null)
                {
                    impact.transform.SetPositionAndRotation(hit.point, Quaternion.LookRotation(hit.normal));
                    impact.SetActive(true);
                    StartCoroutine(DisableObject(impact, 0.75f));
                }
            }
        }

        StartCoroutine(Eject());
    }

    public IEnumerator Reload()
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
        animator.SetBool("eject", true);
        rightHand.position = rightHandEject.position;

        yield return new WaitForSeconds(bulletEjectDelay);

        bulletCasing = GetObjectFromPool(bulletCasings);
        if (bulletCasing == null) yield break;

        bulletRigidbody = bulletCasing.GetComponent<Rigidbody>();
        bulletRigidbody.linearVelocity = Vector3.zero;
        bulletRigidbody.angularVelocity = Vector3.zero;

        bulletCasing.transform.SetPositionAndRotation(bulletSpawn.position, bulletSpawn.rotation);

        bulletCasing.SetActive(true);

        bulletRigidbody.AddForce(bulletSpawn.right * 5f, ForceMode.Impulse);

        yield return new WaitForSeconds(5f);

        bulletCasing.SetActive(false);
    }

    void Ejecting()
    {
        if (Time.time >= nextTimeToFire)
        {
            animator.SetBool("eject", false);
            rightHand.position = rightHandDefault.position;
        }
        else
        {
            rightHand.position = rightHandEject.position;
        }
    }

    IEnumerator DisableObject(GameObject gameObject, float time)
    {
        yield return new WaitForSeconds(time);
        gameObject.SetActive(false);
    }

    GameObject GetObjectFromPool(List<GameObject> objectPool)
    {
        for (int i = 0; i < objectPool.Count; i++)
        {
            if (!objectPool[i].activeInHierarchy)
            {
                return objectPool[i];
            }
        }
        return null;
    }
}
