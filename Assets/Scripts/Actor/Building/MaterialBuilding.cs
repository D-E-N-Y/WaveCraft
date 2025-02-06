using UnityEngine;

public class MaterialBuilding : MonoBehaviour
{
    [SerializeField] private GameObject mesh;
    private MeshRenderer[] meshRenderers;

    [SerializeField] private Color canPlace;
    [SerializeField] private Color notCanPlace;
    [SerializeField] private Color placed;

    public enum BuildColor
    {
        canPlace,
        notCanPlace,
        placed
    }

    private void Awake() 
    {
        if (mesh.TryGetComponent(out MeshRenderer renderer))
        {
            meshRenderers = new MeshRenderer[] { renderer };
        }
        else
        {
            meshRenderers = mesh.GetComponentsInChildren<MeshRenderer>();
        }
    }
    
    public void StartPlace()
    {
        if (mesh.TryGetComponent(out MeshRenderer renderer))
        {
            meshRenderers = new MeshRenderer[] { renderer };
        }
        else
        {
            meshRenderers = mesh.GetComponentsInChildren<MeshRenderer>();
        }
        
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
    }

    private void SetColorMaterial(Color color)
    {
        foreach(MeshRenderer meshRenderer in meshRenderers)
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
    }
}
