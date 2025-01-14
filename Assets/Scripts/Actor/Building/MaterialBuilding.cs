using UnityEngine;

public class MaterialBuilding : MonoBehaviour
{
    [SerializeField] private MeshRenderer meshRenderer;

    [SerializeField] private Color canPlace;
    [SerializeField] private Color notCanPlace;
    [SerializeField] private Color placed;

    public enum BuildColor
    {
        canPlace,
        notCanPlace,
        placed
    }
    
    public void StartPlace()
    {
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
                meshRenderer.material.color = canPlace;
                break;
            
            case BuildColor.notCanPlace:
                meshRenderer.material.color = notCanPlace;
                break;
            
            case BuildColor.placed:
                meshRenderer.material.color = placed;
                break;
        }

        meshRenderer.UpdateGIMaterials();
    }

    private void SetOpaqueMode()
    {
        meshRenderer.material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
        meshRenderer.material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
        meshRenderer.material.SetInt("_ZWrite", 1);
        meshRenderer.material.DisableKeyword("_ALPHATEST_ON");
        meshRenderer.material.DisableKeyword("_ALPHABLEND_ON");
        meshRenderer.material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        meshRenderer.material.renderQueue = -1;
    }

    private void SetTransparentMode()
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
