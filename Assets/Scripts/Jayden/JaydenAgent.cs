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
    public Transform playerShootPoint;
    public Timer timer;
    public RayPerceptionSensorComponent3D rayPerception;
    public Gun gun;

    Vector3 move;
    float moveZ;

    float rotationY;

    ActionSegment<int> discreteActions;
    int lastAction;

    Vector3 velocity;
    Transform groundCheck;
    bool isGrounded;

    int layerMask;

    CharacterController controller;
    Target target;
    RaycastHit hit;

    //TargetJayden targetJayden;

    InputAction moveControls;
    InputAction jump;

    protected override void Awake()
    {
        moveControls = InputSystem.actions.FindAction("Move");
        jump = InputSystem.actions.FindAction("Jump");

        controller = GetComponent<CharacterController>();
        target = GetComponent<Target>();
        //targetJayden = player.GetComponent<TargetJayden>();

        groundCheck = transform.Find("GroundCheck");

        layerMask = ~LayerMask.GetMask("Player");
    }

    public override void OnEpisodeBegin()
    {
        //transform.localPosition = new Vector3(Random.Range(-GameManager.instance.spawnX, GameManager.instance.spawnX), 50, Random.Range(-GameManager.instance.spawnZ, GameManager.instance.spawnZ));
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(isGrounded ? 1 : 0);
        sensor.AddObservation(transform.localRotation);
        sensor.AddObservation(player.localPosition);
        sensor.AddObservation((transform.position - player.position).normalized);
        sensor.AddObservation(Vector3.Distance(transform.position, player.position));
        sensor.AddObservation(Physics.Raycast(playerShootPoint.position, playerShootPoint.forward, out hit, 99999999, layerMask) ? 1 : 0);
        sensor.AddObservation(gun.currentAmmo);
    }
    public override void OnActionReceived(ActionBuffers actions)
    {
        discreteActions = actions.DiscreteActions;

        // this is not ai pls don't flag me
        switch (discreteActions[0])
        {
            case 0:
                moveZ = 1;
                break;
            case 1:
                moveZ = -1;
                break;
            case 2:
                moveZ = 0;
                break;
        }

        // also not ai
        switch (discreteActions[1])
        {
            case 0:
                rotationY = 1;
                break;
            case 1:
                rotationY = 3;
                break;
            case 2:
                rotationY = 5;
                break;
            case 3:
                rotationY = 10;
                break;
            case 4:
                rotationY = 15;
                break;
            case 5:
                rotationY = -1;
                break;
            case 6:
                rotationY = -3;
                break;
            case 7:
                rotationY = -5;
                break;
            case 8:
                rotationY = -10;
                break;
            case 9:
                rotationY = -15;
                break;
            case 10:
                rotationY = 0;
                break;
        }

        if (discreteActions[1] == lastAction)
        {
            AddReward(-1f);
        }
        else
        {
            AddReward(1f);
        }
        lastAction = discreteActions[0];

        move = transform.forward * moveZ;

        transform.Rotate(Vector3.up * rotationY);

        controller.Move(speed * Time.deltaTime * move.normalized);

        AddReward(Vector3.Distance(transform.position, player.position) * 0.01f);

        if (actions.DiscreteActions[2] == 1 && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            AddReward(0.1f);
        }

        if (target.health <= 0)
        {
            AddReward(-10f);
            //targetJayden.End();
            //target.health = 100f;
            return;
        }

        if (gun.currentAmmo <= 0)
        {
            AddReward(10f);
            //targetJayden.End();
            //gun.currentAmmo = gun.maxAmmo;
            return;
        }

        if (timer.timeAmount <= 0)
        {
            AddReward(10f);
            enabled = false;
            GetComponent<TargetPlayer>().enabled = true;
            GetComponent<Target>().isInvincible = true;
            //targetJayden.End();
            //timer.timeAmount = 300;
            return;
        }

        if (Physics.Raycast(playerShootPoint.position, playerShootPoint.forward, out hit, 99999999, layerMask))
        {
            if (hit.transform.CompareTag("Jayden") || hit.transform.CompareTag("Head"))
            {
                AddReward(-1f);
            }
            else
            {
                AddReward(1f);
            }
        }

        AddReward(-1.5f);
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<int> discreteActions = actionsOut.DiscreteActions;

        discreteActions[0] = (int)moveControls.ReadValue<Vector2>().y;
        discreteActions[1] = moveControls.ReadValue<Vector2>().x > 0 ? 5 : -5;
        discreteActions[2] = jump.IsPressed() ? 1 : 0;
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

    public void End()
    {
        EndEpisode();
    }
}
