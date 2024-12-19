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
    public float gravity = -9.81f;
    public float jumpHeight = 3f;

    [Header("Ground Check")]
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    [Header("Walls")]
    public Transform northWall;
    public Transform eastWall;
    public Transform southWall;
    public Transform westWall;

    [Header("References")]
    public List<Transform> jaydens;
    public MapGenerator mapGenerator;
    public Gun gun;

    Vector3 velocity;
    Transform groundCheck;
    bool isGrounded;

    Vector3 move;

    float rotationY;
    float moveZ;

    CharacterController controller;

    InputAction moveControls;
    InputAction jump;

    void Start()
    {
        moveControls = InputSystem.actions.FindAction("Move");
        jump = InputSystem.actions.FindAction("Jump");
        controller = GetComponent<CharacterController>();
        groundCheck = transform.Find("GroundCheck");
    }

    public override void OnEpisodeBegin()
    {
        transform.position = new Vector3(Random.Range(-325, 325), 50, Random.Range(-325, 325));
        for (int i = 0; i < jaydens.Count; i++)
        {
            jaydens[i].position = new Vector3(Random.Range(-325, 325), 50, Random.Range(-325, 325));
        }
        mapGenerator.seed = Random.Range(0, 100000);
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.position);
        for (int i = 0; i < jaydens.Count; i++)
        {
            sensor.AddObservation(jaydens[i].position);
        }
    }
    public override void OnActionReceived(ActionBuffers actions)
    {
        rotationY = actions.ContinuousActions[0];
        moveZ = actions.ContinuousActions[1];

        move = transform.forward * moveZ;

        transform.Rotate(Vector3.up * rotationY);
        controller.Move(speed * Time.deltaTime * move.normalized);

        if (actions.DiscreteActions[0] == 1 && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        if (GetNorthWallDistance() < 10 || GetEastWallDistance() < 10 || GetSouthWallDistance() < 10 || GetWestWallDistance() < 10)
        {
            AddReward(-1f);
            EndEpisode();
        }

        for (int i = 0; i < jaydens.Count; i++)
        {
            if (Vector3.Distance(transform.position, jaydens[i].position) < 100)
            {
                AddReward(5f);
            }
        }

        AddReward(-0.1f);
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        ActionSegment<int> discreteActions = actionsOut.DiscreteActions;

        continuousActions[0] = moveControls.ReadValue<Vector2>().x;
        continuousActions[1] = moveControls.ReadValue<Vector2>().y;

        discreteActions[0] = jump.IsPressed() ? 0 : 1;
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
    }

    float GetNorthWallDistance() => northWall.position.z - transform.position.z;
    float GetEastWallDistance() => eastWall.position.x - transform.position.x;
    float GetSouthWallDistance() => Mathf.Abs(southWall.position.z - transform.position.z);
    float GetWestWallDistance() => Mathf.Abs(westWall.position.x - transform.position.x);
}
