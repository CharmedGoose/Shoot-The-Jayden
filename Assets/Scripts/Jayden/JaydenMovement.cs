using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class JaydenMovement : MonoBehaviour
{
    [Header("Settings")]
    public float speed = 12f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;

    [Header("Ground Check")]
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    [Header("References")]
    public Animator animator;

    float z;
    float y;

    Vector3 move;

    Vector3 velocity;
    Transform groundCheck;
    bool isGrounded;

    CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        groundCheck = transform.Find("GroundCheck");
    }

    void Update()
    {
        animator.SetBool("isWalking", true);

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        z = 1f;

        if (Random.Range(0, 200) == 0)
        {
            // not ai
            switch (Random.Range(0, 5))
            {
                case 0:
                    y = transform.eulerAngles.y + 22.5f;
                    break;
                case 1:
                    y = transform.eulerAngles.y + 45f;
                    break;
                case 2:
                    y = transform.eulerAngles.y + -22.5f;
                    break;
                case 3:
                    y = transform.eulerAngles.y + -45f;
                    break;
                case 4:
                    y = transform.eulerAngles.y + 0f;
                    break;
            }
        }

        move = transform.forward * z;

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, y, 0), 1f - Mathf.Exp(-Time.deltaTime));

        controller.Move(speed * Time.deltaTime * move.normalized);

        if (Random.Range(0, 1000) == 0 && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }
}
