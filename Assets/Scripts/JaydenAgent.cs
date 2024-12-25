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

    float lastHealth;

    int layerMask;

    CharacterController controller;
    Target target;
    RaycastHit hit;

    TargetJayden targetJayden;

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

        lastHealth = target.health;
        layerMask = ~LayerMask.GetMask("Player");
    }

    public override void OnEpisodeBegin()
    {
        transform.localPosition = new Vector3(Random.Range(-GameManager.instance.spawnX, GameManager.instance.spawnX), 50, Random.Range(-GameManager.instance.spawnZ, GameManager.instance.spawnZ));
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.position);
        sensor.AddObservation(transform.rotation);
        sensor.AddObservation(isGrounded);
        sensor.AddObservation(player.position);
        sensor.AddObservation(player.rotation);
        sensor.AddObservation((player.position - transform.position).normalized);
        sensor.AddObservation(Vector3.Distance(transform.position, player.position));
        sensor.AddObservation(rayPerception);
        sensor.AddObservation(target.health);
        sensor.AddObservation(timer.timeAmount);
        sensor.AddObservation(GetNorthWallDistance());
        sensor.AddObservation(GetEastWallDistance());
        sensor.AddObservation(GetSouthWallDistance());
        sensor.AddObservation(GetWestWallDistance());
    }
    public override void OnActionReceived(ActionBuffers actions)
    {
        discreteActions = actions.DiscreteActions;

        if (lastAction == discreteActions[0])
        {
            AddReward(-2f);
        }
        else
        {
            AddReward(0.75f);
        }

        lastAction = discreteActions[0];

        moveZ = actions.ContinuousActions[0];

        if (moveZ >= 0)
        {
            AddReward(0.75f);
        }
        else
        {
            AddReward(-1.75f);
        }

        Debug.Log(moveZ);

        // this is not ai pls don't flag me
        switch (discreteActions[0])
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

        move = transform.forward * moveZ;

        transform.Rotate(Vector3.up * rotationY);

        controller.Move(speed * Time.deltaTime * move.normalized);

        /**if (Vector3.Distance(transform.position, player.position) < 100)
        {
            AddReward(-5f);
        }
        else
        {
            AddReward(0.1f);
        }

        if (actions.DiscreteActions[1] == 1 && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            AddReward(0.1f);
        }

        if (GetNorthWallDistance() < 10 || GetEastWallDistance() < 10 || GetSouthWallDistance() < 10 || GetWestWallDistance() < 10)
        {
            AddReward(-1f);
            targetJayden.End();
            return;
        }

        if (target.health <= 0)
        {
            AddReward(-10f);
            targetJayden.End();
            target.health = 100f;
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
                AddReward(0.1f);
            }
        }

        if (target.health < lastHealth)
        {
            AddReward(-(lastHealth - target.health * 0.1f));

            lastHealth = target.health;
        }

        if (gun.hasMissed)
        {
            AddReward(1f);
            gun.hasMissed = false;
        }**/

        //AddReward(1f);
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        ActionSegment<int> discreteActions = actionsOut.DiscreteActions;

        continuousActions[0] = moveControls.ReadValue<Vector2>().y;

        discreteActions[0] = moveControls.ReadValue<Vector2>().x > 0 ? 5 : -5;
        discreteActions[1] = jump.IsPressed() ? 1 : 0;
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
            AddReward(10f);
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
