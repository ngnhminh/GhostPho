using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PickupItem : MonoBehaviour, IInteractable
{
    Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Interact(PlayerPickup holder)
    {
        if (holder == null) return;
        holder.Pickup(rb);
    }
}
