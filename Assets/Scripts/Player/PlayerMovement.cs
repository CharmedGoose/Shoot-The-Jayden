//Tutorial Used: https://www.youtube.com/watch?v=_QajrabyTJc

using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Settings")]
    public float speed = 12f;
    public float sprintSpeed = 20f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;
    public bool canSprint = false;

    [Header("Ground Check")]
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    [Header("References")]
    public Animator animator;

    Vector3 velocity;
    Transform groundCheck;
    bool isGrounded;

    float x;
    float z;

    Vector3 move;

    CharacterController controller;

    InputAction moveControls;
    InputAction sprint;
    InputAction jump;

    void Awake()
    {
        moveControls = InputSystem.actions.FindAction("Move");
        sprint = InputSystem.actions.FindAction("Sprint");
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

        x = moveControls.ReadValue<Vector2>().x;
        z = moveControls.ReadValue<Vector2>().y;

        move = transform.right * x + transform.forward * z;

        if(move != Vector3.zero)
        {
            animator.SetBool("isWalking", true);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }

        controller.Move((sprint.IsPressed() ? sprintSpeed : speed) * Time.deltaTime * move.normalized);

        if (jump.IsPressed() && isGrounded)
        {
            animator.SetBool("isJumping", true);
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }
}
