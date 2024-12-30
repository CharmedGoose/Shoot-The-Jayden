using UnityEngine;

public class TargetPlayer : MonoBehaviour
{
    [Header("Settings")]
    public float speed = 12f;
    public float sensitivity = 0.1f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;

    [Header("Ground Check")]
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    [Header("References")]
    public Transform player;

    Vector3 velocity;
    Transform groundCheck;
    bool isGrounded;

    Vector3 direction;

    Vector3 lastPosition;

    PlayerMovement playerMovement;

    CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        playerMovement = player.GetComponent<PlayerMovement>();
        groundCheck = transform.Find("GroundCheck");
    }

    void OnEnable()
    {
        playerMovement = player.GetComponent<PlayerMovement>();
        playerMovement.canSprint = true;
    }

    void OnDisable()
    {
        playerMovement.canSprint = false;
    }

    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        direction = (player.position - transform.position).normalized;

        direction.y = 0;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), sensitivity);

        controller.Move(speed * Time.deltaTime * transform.forward);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        if (((lastPosition == transform.position) || (Random.Range(0, 1001) == 0)) && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        lastPosition = transform.position;

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);

        if (Vector3.Distance(transform.position, player.position) < 1f)
        {
            GameManager.instance.SetEnding(0);
            UnityEngine.SceneManagement.SceneManager.LoadScene("End");
        }
    }
}
