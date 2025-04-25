using UnityEngine;

public class MaterialBuilding : MonoBehaviour
{
    [SerializeField] private GameObject mesh;
    private MeshRenderer[] meshRenderers;
    private SkinnedMeshRenderer[] skinnedMeshRenderers;

    [SerializeField] private Material transparentMaterial;
    [SerializeField] private Material normalMaterial;

    [SerializeField] private Color canPlace;
    [SerializeField] private Color notCanPlace;
    [SerializeField] private Color placed;

    public enum BuildColor
    {
        canPlace,
        notCanPlace,
        placed
    }

    private void GetRenderer()
    {
        if (mesh.TryGetComponent(out MeshRenderer renderer))
            meshRenderers = new MeshRenderer[] { renderer };
        else
            meshRenderers = mesh.GetComponentsInChildren<MeshRenderer>();

        if (mesh.TryGetComponent(out SkinnedMeshRenderer skinnedRenderer))
            skinnedMeshRenderers = new SkinnedMeshRenderer[] { skinnedRenderer };
        else
            skinnedMeshRenderers = mesh.GetComponentsInChildren<SkinnedMeshRenderer>();
    }

    private void Awake() 
    {
        GetRenderer();
    }
    
    public void StartPlace()
    {
        GetRenderer();
        SetTransparentMode();
    }

    public void Built()
    {
        SetOpaqueMode();
    }
    
    public void SetColor(BuildColor color)
    {
        switch(color)
        {
            case BuildColor.canPlace:
                SetColorMaterial(canPlace);
                break;
            
            case BuildColor.notCanPlace:
                SetColorMaterial(notCanPlace);
                break;
            
            case BuildColor.placed:
                SetColorMaterial(placed);
                break;
        }

        foreach(MeshRenderer meshRenderer in meshRenderers)
        {
            meshRenderer.UpdateGIMaterials();
        }

        foreach(SkinnedMeshRenderer meshRenderer in skinnedMeshRenderers)
        {
            meshRenderer.UpdateGIMaterials();
        }
    }

    private void SetColorMaterial(Color color)
    {
        foreach(MeshRenderer meshRenderer in meshRenderers)
        {
            meshRenderer.material.SetColor("_BaseColor", color);
        }

        foreach(SkinnedMeshRenderer meshRenderer in skinnedMeshRenderers)
        {
            meshRenderer.material.SetColor("_BaseColor", color);
        }
    }

    private void SetOpaqueMode()
    {
        foreach (var renderer in meshRenderers)
        {
            renderer.material = new Material(normalMaterial);
        }

        foreach (var renderer in skinnedMeshRenderers)
        {
            renderer.material = new Material(normalMaterial);
        }
    }

    private void SetTransparentMode()
    {
        foreach (var renderer in meshRenderers)
        {
            renderer.material = new Material(transparentMaterial);
        }

        foreach (var renderer in skinnedMeshRenderers)
        {
            renderer.material = new Material(transparentMaterial);
        }
    }
}
