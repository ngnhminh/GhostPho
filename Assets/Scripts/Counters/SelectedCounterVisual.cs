using UnityEngine;

public class SelectedCounterVisual : MonoBehaviour {
    [SerializeField] private float interactDistance = 2f;
    [SerializeField] private LayerMask interactLayer;

    private Camera mainCamera;
    private BaseCounter currentCounter;

    private void Awake() {
        mainCamera = Camera.main;
    }

    private void Update() {
        HandleSelection();
    }

    private void HandleSelection() {
        Ray ray = new Ray(mainCamera.transform.position, mainCamera.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, interactDistance, interactLayer)) {
            BaseCounter counter = hit.transform.GetComponent<BaseCounter>();

            if (counter != currentCounter) {
                ClearCurrent();

                currentCounter = counter;
                currentCounter?.ShowSelected();
            }
        } else {
            ClearCurrent();
        }
    }

    private void ClearCurrent() {
        currentCounter?.HideSelected();
        currentCounter = null;
    }

    public BaseCounter GetCurrentCounter() {
        return currentCounter;
    }
}