using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
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

    Vector3 velocity;
    Transform groundCheck;
    bool isGrounded;

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

    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            animator.SetBool("isJumping", false);
            velocity.y = -2f;
        }

        float x = moveControls.ReadValue<Vector2>().x;
        float z = moveControls.ReadValue<Vector2>().y;

        Vector3 move = transform.right * x + transform.forward * z;

        if(move.magnitude > 0)
        {
            animator.SetBool("isWalking", true);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }

        controller.Move(speed * Time.deltaTime * move.normalized);

        if (jump.IsPressed() && isGrounded)
        {
            animator.SetBool("isJumping", true);
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }
}
