using UnityEngine;

public class MaterialBuilding : MonoBehaviour
{
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private Material M_Building;
    [SerializeField] private Material M_Build;

    [SerializeField] private Color canPlace;
    [SerializeField] private Color notCanPlace;

    public enum BuildColor
    {
        canPlace,
        notCanPlace
    }
    
    public void StartPlace()
    {
        meshRenderer.material = M_Build;
    }

    public void EndPlace()
    {
        meshRenderer.material = M_Building;
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
        }

        meshRenderer.UpdateGIMaterials();
    }

    private void SetOpaqueMode()
    {
        M_Building.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
        M_Building.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
        M_Building.SetInt("_ZWrite", 1);
        M_Building.DisableKeyword("_ALPHATEST_ON");
        M_Building.DisableKeyword("_ALPHABLEND_ON");
        M_Building.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        M_Building.renderQueue = -1;
    }

    private void SetTransparentMode()
    {
        M_Building.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
        M_Building.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        M_Building.SetInt("_ZWrite", 0);
        M_Building.DisableKeyword("_ALPHATEST_ON");
        M_Building.DisableKeyword("_ALPHABLEND_ON");
        M_Building.EnableKeyword("_ALPHAPREMULTIPLY_ON");
        M_Building.renderQueue = 3000;
    }
}
