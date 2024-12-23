using System.Collections.Generic;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class TargetJayden : MonoBehaviour
{
    [Header("Settings")]
    public float speed = 12f;
    public float sensitivity = 100f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;

    public float headRotationY;

    [Header("Ground Check")]
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    public List<Transform> jaydens;

    [Header("References")]
    public Transform head;
    public MapGenerator mapGenerator;
    public Timer timer;
    public RayPerceptionSensorComponent3D rayPerception;
    public Gun gun;

    Vector3 velocity;
    Transform groundCheck;
    bool isGrounded;

    Vector3 direction;

    bool isJaydenVisible;

    Vector3 lastPosition;

    Transform closestJayden;

    CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        groundCheck = transform.Find("GroundCheck");
    }

    void Update()
    {
        if (timer.timeAmount <= 0) End();

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        closestJayden = GetClosestJayden();
        direction = (closestJayden.position - transform.position).normalized;

        head.rotation = Quaternion.Slerp(head.rotation, Quaternion.LookRotation(direction), sensitivity);
        head.localRotation = Quaternion.Euler(head.localRotation.eulerAngles.x, headRotationY, 0);

        direction.y = 0;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), sensitivity);

        isJaydenVisible = IsJaydenVisible();

        if (isJaydenVisible)
        {
            if (Random.Range(0, 26) == 0 && gun.nextTimeToFire <= Time.time)
            {
                gun.nextTimeToFire = Time.time + 1f / gun.fireRate;
                gun.Shoot();
                if (gun.hasRecoil) gun.mouseLook.shot = true;
            }
        }
        else 
        {
            controller.Move(speed * Time.deltaTime * transform.forward);
        }


        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        if ((((lastPosition == transform.position) && !isJaydenVisible) || (Random.Range(0, 501) == 0)) && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        lastPosition = transform.position;

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }

    public void End()
    {
        for (int i = 0; i < jaydens.Count; i++)
        {
            jaydens[i].GetComponent<JaydenAgent>().End();
        }
        transform.localPosition = new Vector3(Random.Range(-GameManager.instance.spawnX, GameManager.instance.spawnX), 50, Random.Range(-GameManager.instance.spawnZ, GameManager.instance.spawnZ));
        gun.currentAmmo = gun.maxAmmo;
        if (mapGenerator != null) mapGenerator.seed = Random.Range(0, 100000);
        timer.timeAmount = 300;
    }

    bool IsJaydenVisible()
    {
        RayPerceptionOutput.RayOutput[] rayOutput = RayPerceptionSensor.Perceive(rayPerception.GetRayPerceptionInput(), false).RayOutputs;

        for (int i = 0; i < rayOutput.Length; i++)
        {
            if (rayOutput[i].HitTaggedObject)
            {
                return true;
            }
        }
        return false;
    }

    // https://discussions.unity.com/t/clean-est-way-to-find-nearest-object-of-many-c/409917/4
    Transform GetClosestJayden()
    {
        Transform jayden = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;

        foreach (Transform potentialTarget in jaydens)
        {
            Vector3 directionToTarget = potentialTarget.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                jayden = potentialTarget;
            }
        }
        return jayden;
    }
}
