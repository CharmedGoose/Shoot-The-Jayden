using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAgent : Agent
{
    [Header("Settings")]
    public float speed = 12f;
    public float sensitivity = 100f;
    public float ADSSensitivity = 50f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;

    public float headRotationY;

    [Header("Ground Check")]
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    [Header("Walls")]
    public Transform northWall;
    public Transform eastWall;
    public Transform southWall;
    public Transform westWall;

    public List<Transform> jaydens;

    [Header("References")]
    public Transform head;
    public MapGenerator mapGenerator;
    public Gun gun;
    public Timer timer;
    public RayPerceptionSensorComponent3D rayPerception;

    Vector3 velocity;
    Transform groundCheck;
    bool isGrounded;

    Vector3 move;

    float rotationY;
    float rotationX;
    float moveZ;

    Vector2 mousePosition;

    CharacterController controller;

    InputAction moveControls;
    InputAction mouse;
    InputAction jump;
    InputAction shoot;
    InputAction reload;

    void Start()
    {
        moveControls = InputSystem.actions.FindAction("Move");
        jump = InputSystem.actions.FindAction("Jump");
        mouse = InputSystem.actions.FindAction("Look");
        shoot = InputSystem.actions.FindAction("Shoot");
        reload = InputSystem.actions.FindAction("Reload");

        controller = GetComponent<CharacterController>();
        groundCheck = transform.Find("GroundCheck");
    }

    public override void OnEpisodeBegin()
    {
        transform.position = new Vector3(Random.Range(-325, 325), 50, Random.Range(-325, 325));
        mapGenerator.seed = Random.Range(0, 100000);
        gun.currentAmmo = gun.maxAmmo;
        timer.timeAmount = 300;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.position);
        sensor.AddObservation(head.localRotation);
        sensor.AddObservation(transform.rotation);
        sensor.AddObservation(gun.currentAmmo);
        sensor.AddObservation(timer.timeAmount);
        sensor.AddObservation(GetNorthWallDistance());
        sensor.AddObservation(GetEastWallDistance());
        sensor.AddObservation(GetSouthWallDistance());
        sensor.AddObservation(GetWestWallDistance());

        for (int i = 0; i < jaydens.Count; i++)
        {
            sensor.AddObservation(jaydens[i].position);
        }
        sensor.AddObservation((GetClosestJayden().position - transform.position).normalized);
    }
    public override void OnActionReceived(ActionBuffers actions)
    {
        moveZ = actions.ContinuousActions[0];
        rotationY = actions.ContinuousActions[1] * sensitivity;

        move = transform.forward * moveZ;

        transform.Rotate(Vector3.up * rotationY);
        controller.Move(speed * Time.deltaTime * move.normalized);

        rotationX -= actions.ContinuousActions[2] * sensitivity;
        rotationX = Mathf.Clamp(rotationX, -90f, 90f);

        head.localRotation = Quaternion.Euler(rotationX, headRotationY, 0f);

        if ((actions.DiscreteActions[0] == 1) && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        if (actions.DiscreteActions[1] == 1)
        {
            gun.isShooting = true;
        }
        else
        {
            gun.isShooting = false;
        }

        if (actions.DiscreteActions[2] == 1)
        {
            StartCoroutine(gun.Reload());
        }

        if (GetNorthWallDistance() < 10 || GetEastWallDistance() < 10 || GetSouthWallDistance() < 10 || GetWestWallDistance() < 10)
        {
            AddReward(-5f);
            End();
        }

        for (int i = 0; i < jaydens.Count; i++)
        {
            if (Vector3.Distance(transform.position, jaydens[i].position) < 100)
            {
                AddReward(1f);
            }
        }

        if (!gun.hasMissed)
        {
            AddReward(10f);
        }
        else
        {
            AddReward(-3f);
        }

        AddReward(-2f);
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        ActionSegment<int> discreteActions = actionsOut.DiscreteActions;

        mousePosition = mouse.ReadValue<Vector2>();

        continuousActions[0] = moveControls.ReadValue<Vector2>().y;
        continuousActions[1] = mousePosition.x;
        continuousActions[2] = mousePosition.y;

        discreteActions[0] = jump.IsPressed() ? 1 : 0;
        discreteActions[1] = shoot.IsPressed() ? 1 : 0;
        discreteActions[2] = reload.IsPressed() ? 1 : 0;
    }

    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);

        if (timer.timeAmount <= 0)
        {
            AddReward(-10f);
            End();
        }

        RayPerceptionOutput.RayOutput rayOutput = RayPerceptionSensor.Perceive(rayPerception.GetRayPerceptionInput(), false).RayOutputs[0];

        if (rayOutput.HasHit)
        {
            AddReward(5f);
        }
        else
        {
            AddReward(-2f);
        }
    }

    public void End()
    {
        for (int i = 0; i < jaydens.Count; i++)
        {
            jaydens[i].GetComponent<JaydenAgent>().End();
        }
        EndEpisode();
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

    float GetNorthWallDistance() => northWall.position.z - transform.position.z;
    float GetEastWallDistance() => eastWall.position.x - transform.position.x;
    float GetSouthWallDistance() => Mathf.Abs(southWall.position.z - transform.position.z);
    float GetWestWallDistance() => Mathf.Abs(westWall.position.x - transform.position.x);
}
