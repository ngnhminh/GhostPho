using UnityEngine;
using StarterAssets;

[RequireComponent(typeof(StarterAssetsInputs))]
[RequireComponent(typeof(FirstPersonController))]
public class PlayerInteraction : MonoBehaviour {
    private StarterAssetsInputs input;
    private SelectedCounterVisual selection;
    private FirstPersonController firstPersonController;

    private void Start() {
        input = GetComponent<StarterAssetsInputs>();
        selection = GetComponent<SelectedCounterVisual>();
        firstPersonController = GetComponent<FirstPersonController>();

        input.OnInteractPressed += HandleInteract;
    }

    private void OnDestroy() {
        input.OnInteractPressed -= HandleInteract;
    }

    private void HandleInteract() {
        selection.GetCurrentCounter()?.Interact(firstPersonController);
    }
}