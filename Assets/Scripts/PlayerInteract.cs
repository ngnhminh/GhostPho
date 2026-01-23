using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Camera))]
public class PlayerInteract : MonoBehaviour
{
    public float interactDistance = 3f;
    public LayerMask interactLayer;
    public InputActionReference interactAction;

    Camera cam;
    OutlineHighlight currentOutline;

    public PlayerPickup holder; // kéo PlayerPickup vào

    void Awake()
    {
        cam = GetComponent<Camera>();
        if (holder == null) holder = GetComponent<PlayerPickup>();
    }

    void OnEnable() => interactAction.action.Enable();
    void OnDisable() => interactAction.action.Disable();

    void Update()
    {
        UpdateOutline();

        if (interactAction.action.WasPressedThisFrame())
            TryInteract();
    }

    void UpdateOutline()
    {
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);

        OutlineHighlight newOutline = null;

        if (Physics.Raycast(ray, out RaycastHit hit, interactDistance, interactLayer))
        {
            newOutline = hit.collider.GetComponentInParent<OutlineHighlight>();
        }

        if (newOutline == currentOutline) return;

        if (currentOutline != null) currentOutline.SetHighlighted(false);

        currentOutline = newOutline;

        if (currentOutline != null) currentOutline.SetHighlighted(true);
    }

    void TryInteract()
    {
        // Nếu đang cầm đồ rồi -> bấm E để thả
        if (holder != null && holder.IsHolding)
        {
            holder.Drop();
            return;
        }

        // Nếu chưa cầm gì -> raycast để nhặt/tương tác
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, interactDistance, interactLayer))
        {
            var interactable = hit.collider.GetComponentInParent<IInteractable>();
            interactable?.Interact(holder);
        }
    }
}
