using UnityEngine;

public class MaterialBuilding : MonoBehaviour
{
    [SerializeField] private GameObject mesh;
    private MeshRenderer[] meshRenderers;
    private SkinnedMeshRenderer[] skinnedMeshRenderers;

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
            meshRenderer.material.color = color;
        }

        foreach(SkinnedMeshRenderer meshRenderer in skinnedMeshRenderers)
        {
            meshRenderer.material.color = color;
        }
    }

    private void SetOpaqueMode()
    {
        foreach(MeshRenderer meshRenderer in meshRenderers)
        {
            meshRenderer.material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
            meshRenderer.material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
            meshRenderer.material.SetInt("_ZWrite", 1);
            meshRenderer.material.DisableKeyword("_ALPHATEST_ON");
            meshRenderer.material.DisableKeyword("_ALPHABLEND_ON");
            meshRenderer.material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            meshRenderer.material.renderQueue = -1;
        }

        foreach(SkinnedMeshRenderer meshRenderer in skinnedMeshRenderers)
        {
            meshRenderer.material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
            meshRenderer.material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
            meshRenderer.material.SetInt("_ZWrite", 1);
            meshRenderer.material.DisableKeyword("_ALPHATEST_ON");
            meshRenderer.material.DisableKeyword("_ALPHABLEND_ON");
            meshRenderer.material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            meshRenderer.material.renderQueue = -1;
        }
    }

    private void SetTransparentMode()
    {
        foreach(MeshRenderer meshRenderer in meshRenderers)
        {
            meshRenderer.material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
            meshRenderer.material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            meshRenderer.material.SetInt("_ZWrite", 0);
            meshRenderer.material.DisableKeyword("_ALPHATEST_ON");
            meshRenderer.material.DisableKeyword("_ALPHABLEND_ON");
            meshRenderer.material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
            meshRenderer.material.renderQueue = 3000;
        }

        foreach(SkinnedMeshRenderer meshRenderer in skinnedMeshRenderers)
        {
            meshRenderer.material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
            meshRenderer.material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            meshRenderer.material.SetInt("_ZWrite", 0);
            meshRenderer.material.DisableKeyword("_ALPHATEST_ON");
            meshRenderer.material.DisableKeyword("_ALPHABLEND_ON");
            meshRenderer.material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
            meshRenderer.material.renderQueue = 3000;
        }
    }
}
