using StarterAssets;
using UnityEngine;

public class DeliveryCounter : BaseCounter
{
    public override void Interact(FirstPersonController player) {
        if (player.HasKitchenObject()) {
            if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plate)) {
                // Only accepts plates

                DeliveryManager.Instance.DeliverRecipe(plate);
                player.GetKitchenObject().DestroySelf();
            }
        }
    }
}
