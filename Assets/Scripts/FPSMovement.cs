using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class FPSMovement : MonoBehaviour
{
    [Header("Speeds")]
    public float walkSpeed = 3.5f;
    public float runSpeed = 6f;

    [Header("Gravity")]
    public float gravity = -9.81f;

    [Header("Input")]
    public InputActionReference moveAction;   // Player/Move
    public InputActionReference sprintAction; // optional (hold Shift)

    private CharacterController controller;
    private Vector3 velocity;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    void OnEnable()
    {
        if (moveAction != null) moveAction.action.Enable();
        if (sprintAction != null) sprintAction.action.Enable();
    }

    void OnDisable()
    {
        if (moveAction != null) moveAction.action.Disable();
        if (sprintAction != null) sprintAction.action.Disable();
    }

    void Update()
    {
        Vector2 move2D = moveAction != null ? moveAction.action.ReadValue<Vector2>() : Vector2.zero;

        bool isSprinting = false;
        if (sprintAction != null)
        {
            // Sprint action dạng Button (Shift)
            isSprinting = sprintAction.action.ReadValue<float>() > 0.5f;
        }

        float speed = isSprinting ? runSpeed : walkSpeed;

        Vector3 move = transform.right * move2D.x + transform.forward * move2D.y;
        controller.Move(move * speed * Time.deltaTime);

        // gravity
        if (controller.isGrounded && velocity.y < 0f)
            velocity.y = -2f;

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
        Debug.Log(move2D);
    }
}
