using UnityEngine;
using UnityEngine.InputSystem;

public class MouseLook : MonoBehaviour
{
    public bool lookEnabled = true;
    [Header("Refs")]
    public Transform playerBody;

    [Header("Settings")]
    public float sensitivity = 0.12f;
    public float pitchMin = -85f;
    public float pitchMax = 85f;

    [Header("Input")]
    public InputActionReference lookAction;

    private float xRotation = 0f;

    void OnEnable()
    {
        if (lookAction != null) lookAction.action.Enable();
    }

    void OnDisable()
    {
        if (lookAction != null) lookAction.action.Disable();
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (!lookEnabled) return;

        Vector2 look = lookAction.action.ReadValue<Vector2>();

        float mouseX = look.x * sensitivity;
        float mouseY = look.y * sensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, pitchMin, pitchMax);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
    }
}
