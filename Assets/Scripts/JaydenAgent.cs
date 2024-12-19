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
    public Gun gun;
    
    float moveX;
    float moveZ;

    float lastHealth;

    Vector3 velocity;
    Transform groundCheck;
    bool isGrounded;

    CharacterController controller;
    Target target;

    InputAction moveControls;
    InputAction jump;

    void Start()
    {
        moveControls = InputSystem.actions.FindAction("Move");
        jump = InputSystem.actions.FindAction("Jump");
        controller = GetComponent<CharacterController>();
        target = GetComponent<Target>();
        groundCheck = transform.Find("GroundCheck");

        lastHealth = target.health;
    }

    public override void OnEpisodeBegin()
    {
        transform.position = new Vector3(Random.Range(-325, 325), 50, Random.Range(-325, 325));
        player.position = new Vector3(Random.Range(-325, 325), 50, Random.Range(-325, 325));
        mapGenerator.seed = Random.Range(0, 100000);
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.position);
        sensor.AddObservation(player.position);

    }
    public override void OnActionReceived(ActionBuffers actions)
    {
        moveX = actions.ContinuousActions[0];
        moveZ = actions.ContinuousActions[1];

        Vector3 move = transform.right * moveX + transform.forward * moveZ;

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

        if (Vector3.Distance(transform.position, player.position) < 50)
        {
            AddReward(-2.5f);
            EndEpisode();
        }

        AddReward(0.1f);
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

        if (target.health < lastHealth)
        {
            AddReward(-(lastHealth - target.health * 0.1f));
            if (target.health <= 0)
            {
                EndEpisode();
            }

            lastHealth = target.health;
        }

        if (gun.hasMissed) 
        {
            AddReward(1f);
            gun.hasMissed = false;
        }

        if (gun.currentAmmo <= 0)
        {
            AddReward(0.5f);
        }
    }

    float GetNorthWallDistance() => northWall.position.z - player.position.z;
    float GetEastWallDistance() => eastWall.position.x - player.position.x;
    float GetSouthWallDistance() => Mathf.Abs(southWall.position.z - player.position.z);
    float GetWestWallDistance() => Mathf.Abs(westWall.position.x - player.position.x);
}
