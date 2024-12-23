using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;
using UnityEngine.InputSystem;

public class JaydenAgent : Agent
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
    public Transform player;
    public MapGenerator mapGenerator;
    public Timer timer;
    public RayPerceptionSensorComponent3D rayPerception;
    public Gun gun;

    Vector3 move;
    float moveZ;

    float rotationY;

    Vector3 velocity;
    Transform groundCheck;
    bool isGrounded;

    float lastHealth;

    int framesSinceLastRay;

    int layerMask;

    CharacterController controller;
    Target target;
    RaycastHit hit;

    TargetJayden targetJayden;
    Transform mainCamera;

    InputAction moveControls;
    InputAction jump;

    protected override void Awake()
    {
        moveControls = InputSystem.actions.FindAction("Move");
        jump = InputSystem.actions.FindAction("Jump");

        controller = GetComponent<CharacterController>();
        target = GetComponent<Target>();
        targetJayden = player.GetComponent<TargetJayden>();

        groundCheck = transform.Find("GroundCheck");

        mainCamera = Camera.main.transform;

        framesSinceLastRay = 0;
        lastHealth = target.health;
        layerMask = ~LayerMask.GetMask("Player");
    }

    public override void OnEpisodeBegin()
    {
        transform.position = new Vector3(Random.Range(-325, 325), 50, Random.Range(-325, 325));
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.position);
        sensor.AddObservation(transform.rotation);
        sensor.AddObservation(isGrounded);
        sensor.AddObservation(player.position);
        sensor.AddObservation(player.rotation);
        sensor.AddObservation((player.position - transform.position).normalized);
        sensor.AddObservation(rayPerception);
        sensor.AddObservation(target.health);
        sensor.AddObservation(timer.timeAmount);
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
            targetJayden.End();
        }

        if (target.health <= 0)
        {
            AddReward(-10f);
            targetJayden.End();
            target.health = 100f;
        }

        AddReward(0.02f);
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        ActionSegment<int> discreteActions = actionsOut.DiscreteActions;

        continuousActions[0] = moveControls.ReadValue<Vector2>().x;
        continuousActions[1] = moveControls.ReadValue<Vector2>().y;

        discreteActions[0] = jump.IsPressed() ? 1 : 0;
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

        if (target.health < lastHealth)
        {
            AddReward(-(lastHealth - target.health * 0.1f));

            lastHealth = target.health;
        }

        if (gun.hasMissed)
        {
            AddReward(1f);
            gun.hasMissed = false;
        }

        if (timer.timeAmount <= 0)
        {
            AddReward(-10f);
        }

        if (Vector3.Distance(transform.position, player.position) < 50)
        {
            AddReward(-2.5f);
        }


        if (Physics.Raycast(mainCamera.position, mainCamera.forward, out hit, 99999999, layerMask))
        {
            if (hit.transform.CompareTag("Jayden") || hit.transform.CompareTag("Head"))
            {
                AddReward(-0.5f);
                Debug.Log("hit");
            }
            else
            {
                AddReward(0.0001f);
            }
        }

    }

    public void End()
    {
        EndEpisode();
    }

    float GetNorthWallDistance() => northWall.position.z - transform.position.z;
    float GetEastWallDistance() => eastWall.position.x - transform.position.x;
    float GetSouthWallDistance() => Mathf.Abs(southWall.position.z - transform.position.z);
    float GetWestWallDistance() => Mathf.Abs(westWall.position.x - transform.position.x);
}
