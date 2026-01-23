using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class OutlineHighlight : MonoBehaviour
{
    public Material outlineMaterial;
    [Range(1.001f, 1.2f)]
    public float outlineScale = 1.03f;

    GameObject outlineObj;
    MeshRenderer outlineRenderer;

    void Awake()
    {
        CreateOutline();
        SetHighlighted(false);
    }

    void CreateOutline()
    {
        var mf = GetComponent<MeshFilter>();
        if (mf == null || mf.sharedMesh == null) return;

        outlineObj = new GameObject("OutlineShell");
        outlineObj.transform.SetParent(transform, false);
        outlineObj.transform.localPosition = Vector3.zero;
        outlineObj.transform.localRotation = Quaternion.identity;
        outlineObj.transform.localScale = Vector3.one * outlineScale;

        var outlineMF = outlineObj.AddComponent<MeshFilter>();
        outlineMF.sharedMesh = mf.sharedMesh;

        outlineRenderer = outlineObj.AddComponent<MeshRenderer>();
        outlineRenderer.sharedMaterial = outlineMaterial;

        // Đảm bảo outline vẽ “đè” nhẹ lên (tùy bạn)
        outlineRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        outlineRenderer.receiveShadows = false;
    }

    public void SetHighlighted(bool on)
    {
        if (outlineObj != null)
            outlineObj.SetActive(on);
    }
}
