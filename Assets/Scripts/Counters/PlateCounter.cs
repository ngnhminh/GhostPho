using StarterAssets;
using UnityEngine;

public class PlateCounter : BaseCounter
{

    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    public override void Interact(FirstPersonController player) {
        // Spawn object from container and give to player
        if (!player.HasKitchenObject()) {
            // Player is not carrying anything
            Transform kitchenObjectTransform = Instantiate(kitchenObjectSO.prefab);
            kitchenObjectTransform.GetComponent<KitchenObject>().SetKitchenObjectParent(player);
        }
    }
}
