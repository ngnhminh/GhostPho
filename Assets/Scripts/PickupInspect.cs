using UnityEngine;
using UnityEngine.InputSystem;

public class PickupInspect : MonoBehaviour
{
    [Header("Refs")]
    public PlayerPickup pickup; // kéo component PlayerPickup vào đây

    [Header("Input")]
    public InputActionReference inspectAction;     // Player/Inspect (RMB)
    public InputActionReference inspectLookAction; // Player/InspectLook (Mouse delta)

    [Header("Settings")]
    public float rotateSpeed = 0.15f; // tăng/giảm theo cảm giác

    bool inspecting = false;
    Quaternion extraRotation = Quaternion.identity;

    public MouseLook mouseLook;

    void Awake()
    {
        if (pickup == null) pickup = GetComponent<PlayerPickup>();
        if (mouseLook == null) mouseLook = GetComponent<MouseLook>();
        if (pickup == null) pickup = GetComponent<PlayerPickup>();
    }

    void OnEnable()
    {
        if (inspectAction != null) inspectAction.action.Enable();
        if (inspectLookAction != null) inspectLookAction.action.Enable();
    }

    void OnDisable()
    {
        if (inspectAction != null) inspectAction.action.Disable();
        if (inspectLookAction != null) inspectLookAction.action.Disable();
    }

    void Update()
    {
        if (pickup == null || !pickup.IsHolding)
        {
            inspecting = false;
            return;
        }

        inspecting = inspectAction != null && inspectAction.action.IsPressed();

        if (mouseLook != null)
            mouseLook.lookEnabled = !inspecting;

        if (!inspecting) return;

        Vector2 delta = inspectLookAction != null
            ? inspectLookAction.action.ReadValue<Vector2>()
            : Vector2.zero;

        // Xoay theo chuột: kéo trái/phải = yaw, lên/xuống = pitch
        float yaw = delta.x * rotateSpeed;
        float pitch = -delta.y * rotateSpeed;

        // Xoay trong local space của camera để cảm giác “cầm trước mặt”
        Quaternion qYaw = Quaternion.AngleAxis(yaw, transform.up);
        Quaternion qPitch = Quaternion.AngleAxis(pitch, transform.right);

        extraRotation = qYaw * qPitch * extraRotation;

        // áp rotation này vào PlayerPickup (thông qua offset)
        pickup.SetInspectRotation(extraRotation);
    }
}
