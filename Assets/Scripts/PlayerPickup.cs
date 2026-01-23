using UnityEngine;

public class PlayerPickup : MonoBehaviour
{
    [Header("Hold settings")]
    public float holdDistance = 2.5f;
    public float holdUpOffset = -0.35f;
    public float holdSideOffset = 0.0f;

    [Header("Rotation")]
    public bool followCameraRotation = true; // vẫn quay theo camera
    public bool preservePickupAngle = true;  // giữ góc lúc nhặt

    Rigidbody heldRb;
    Collider[] heldCols;

    Quaternion rotOffset = Quaternion.identity;
    Quaternion inspectRotation = Quaternion.identity;

    public void SetInspectRotation(Quaternion rot)
    {
        inspectRotation = rot;
    }

    public bool IsHolding => heldRb != null;

    public void Pickup(Rigidbody rb)
    {
        if (rb == null || IsHolding) return;

        heldRb = rb;

        // tắt collider để không va vào người
        heldCols = heldRb.GetComponentsInChildren<Collider>();
        foreach (var c in heldCols) c.enabled = false;

        heldRb.isKinematic = true;
        heldRb.linearVelocity = Vector3.zero;
        heldRb.angularVelocity = Vector3.zero;

        // lưu offset góc tại thời điểm nhặt
        if (followCameraRotation && preservePickupAngle)
        {
            rotOffset = Quaternion.Inverse(transform.rotation) * heldRb.transform.rotation;
        }
        else
        {
            rotOffset = Quaternion.identity;
        }

        inspectRotation = Quaternion.identity;

    }

    public void Drop()
    {
        if (!IsHolding) return;

        if (heldCols != null)
            foreach (var c in heldCols) c.enabled = true;

        heldRb.isKinematic = false;

        heldRb = null;
        heldCols = null;
        rotOffset = Quaternion.identity;
    }

    void LateUpdate()
    {
        if (!IsHolding) return;

        // giữ vật trước mặt camera theo world space
        Vector3 targetPos =
            transform.position +
            transform.forward * holdDistance +
            transform.up * holdUpOffset +
            transform.right * holdSideOffset;

        heldRb.transform.position = targetPos;

        // quay
        if (followCameraRotation)
        {
            var baseRot = preservePickupAngle
                ? transform.rotation * rotOffset
                : transform.rotation;

            // cộng thêm rotation do inspect
            heldRb.transform.rotation = baseRot * inspectRotation;
        }
        // nếu followCameraRotation = false => giữ nguyên rotation world của vật
    }
}
