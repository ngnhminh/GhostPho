using UnityEngine;
using UnityEngine.InputSystem;

public class HeadBob : MonoBehaviour
{
    [Header("Input (New Input System)")]
    public InputActionReference moveAction; // kéo Controls/Player/Move vào

    [Header("Bobbing")]
    public float walkBobSpeed = 12f;
    public float walkBobAmount = 0.05f;

    public float sprintBobSpeed = 16f;
    public float sprintBobAmount = 0.08f;

    [Header("Smoothing")]
    public float returnSpeed = 10f;

    [Header("Optional")]
    public InputActionReference sprintAction; // kéo Controls/Player/Sprint (Shift) nếu có

    private Vector3 startLocalPos;
    private float bobTimer;

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

    void Start()
    {
        startLocalPos = transform.localPosition;
    }

    void Update()
    {
        Vector2 move = moveAction != null ? moveAction.action.ReadValue<Vector2>() : Vector2.zero;
        bool isMoving = move.sqrMagnitude > 0.01f;

        bool isSprinting = sprintAction != null && sprintAction.action.ReadValue<float>() > 0.5f;

        if (!isMoving)
        {
            // Không di chuyển -> camera trở về vị trí gốc mượt
            bobTimer = 0f;
            transform.localPosition = Vector3.Lerp(transform.localPosition, startLocalPos, Time.deltaTime * returnSpeed);
            return;
        }

        float speed = isSprinting ? sprintBobSpeed : walkBobSpeed;
        float amount = isSprinting ? sprintBobAmount : walkBobAmount;

        bobTimer += Time.deltaTime * speed;

        // Nhấp nhả theo sin: lên xuống là chính, kèm lắc nhẹ trái phải cho tự nhiên
        float y = Mathf.Sin(bobTimer) * amount;
        float x = Mathf.Cos(bobTimer * 0.5f) * (amount * 0.5f);

        Vector3 target = startLocalPos + new Vector3(x, y, 0f);
        transform.localPosition = Vector3.Lerp(transform.localPosition, target, Time.deltaTime * returnSpeed);
    }
}
