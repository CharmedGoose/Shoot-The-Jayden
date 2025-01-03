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
    public Animator animator;
    public Transform player;
    public GameObject eyes;
    public Timer timer;

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

        eyes.SetActive(false);
    }

    void OnEnable()
    {
        animator.SetBool("isWalking", true);
        animator.SetFloat("speed", 1f);
        playerMovement = player.GetComponent<PlayerMovement>();
        playerMovement.canSprint = true;
    }

    void OnDisable()
    {
        animator.SetBool("isWalking", false);
        playerMovement.canSprint = false;

        eyes.SetActive(false);
    }

    void Update()
    {
        eyes.SetActive(true);
        
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

        if (timer.timeAmount <= -60)
        {
            GetComponent<Target>().isInvincible = false;
            speed = 10f;
        }
    }
}
