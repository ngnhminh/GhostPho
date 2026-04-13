using UnityEngine;
using UnityEngine.InputSystem;

public class HeadBob : MonoBehaviour
{
    [Header("References")]
    public Transform cameraHolder;

    [Header("Movement")]
    public CharacterController controller;
    public float movementThreshold = 0.1f;

    [Header("Bob Settings")]
    public float baseBobSpeed = 8f;
    public float verticalBobAmount = 0.05f;
    public float horizontalBobAmount = 0.025f;

    [Header("Run Settings")]
    public float runBobMultiplier = 1.35f;
    public float runAmountMultiplier = 1.2f;

    [Header("Smoothing")]
    public float positionLerpSpeed = 10f;
    public float returnLerpSpeed = 8f;

    [Header("Options")]
    public bool enableHeadBob = true;

    private Vector3 defaultLocalPos;
    private float bobTimer;

    private void Start()
    {
        if (controller == null)
            controller = GetComponent<CharacterController>();

        if (cameraHolder == null)
        {
            Debug.LogError("HeadBob: chưa gán cameraHolder trong Inspector.");
            enabled = false;
            return;
        }

        if (controller == null)
        {
            Debug.LogError("HeadBob: không tìm thấy CharacterController.");
            enabled = false;
            return;
        }

        defaultLocalPos = cameraHolder.localPosition;
    }

    private void Update()
    {
        if (!enableHeadBob)
        {
            ReturnToDefault();
            return;
        }

        Vector3 horizontalVelocity = new Vector3(controller.velocity.x, 0f, controller.velocity.z);
        float speed = horizontalVelocity.magnitude;

        bool isMoving = controller.isGrounded && speed > movementThreshold;
        bool isRunning = Keyboard.current != null && Keyboard.current.leftShiftKey.isPressed;

        if (isMoving)
        {
            ApplyHeadBob(speed, isRunning);
        }
        else
        {
            ReturnToDefault();
        }
    }

    private void ApplyHeadBob(float moveSpeed, bool isRunning)
    {
        float speedMultiplier = Mathf.Clamp(moveSpeed, 0.5f, 2f);

        float bobSpeed = baseBobSpeed * speedMultiplier;
        float vAmount = verticalBobAmount;
        float hAmount = horizontalBobAmount;

        if (isRunning)
        {
            bobSpeed *= runBobMultiplier;
            vAmount *= runAmountMultiplier;
            hAmount *= runAmountMultiplier;
        }

        bobTimer += Time.deltaTime * bobSpeed;

        float xOffset = Mathf.Cos(bobTimer * 0.5f) * hAmount;
        float yOffset = Mathf.Sin(bobTimer) * vAmount;

        Vector3 targetPos = new Vector3(
            defaultLocalPos.x + xOffset,
            defaultLocalPos.y + yOffset,
            defaultLocalPos.z
        );

        cameraHolder.localPosition = Vector3.Lerp(
            cameraHolder.localPosition,
            targetPos,
            Time.deltaTime * positionLerpSpeed
        );
    }

    private void ReturnToDefault()
    {
        bobTimer = 0f;

        cameraHolder.localPosition = Vector3.Lerp(
            cameraHolder.localPosition,
            defaultLocalPos,
            Time.deltaTime * returnLerpSpeed
        );
    }
}