using StarterAssets;
using UnityEngine;

public class TrashCounter : BaseCounter
{
    public override void Interact(FirstPersonController player) {
        if (player.HasKitchenObject()) {
            player.GetKitchenObject().DestroySelf();
        }
    }
}
