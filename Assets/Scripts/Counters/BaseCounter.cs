using UnityEngine;
using StarterAssets;

public abstract class BaseCounter : MonoBehaviour, IKitchenObjectParent {

    [SerializeField] private GameObject[] selectedVisualArray;

    [SerializeField] private Transform counterTopPoint;

    private KitchenObject kitchenObject;

    public abstract void Interact(FirstPersonController player);


    protected virtual void Start() {
        HideSelected();
    }
    public void ShowSelected() {
        foreach (GameObject selectedVisual in selectedVisualArray) { 
            if (selectedVisual != null)
                selectedVisual.SetActive(true);
        }
    }

    public void HideSelected() {
        foreach (GameObject selectedVisual in selectedVisualArray) {
            if (selectedVisual != null)
                selectedVisual.SetActive(false);
        }
    }

    public Transform GetKitchenObjectFollowTransform() {
        return counterTopPoint;
    }

    public void SetKitchenObject(KitchenObject kitchenObject) {
        this.kitchenObject = kitchenObject;
    }

    public KitchenObject GetKitchenObject() {
        return kitchenObject;
    }

    public void ClearKitchenObject() {
        kitchenObject = null;
    }

    public bool HasKitchenObject() {
        return kitchenObject != null;
    }
}