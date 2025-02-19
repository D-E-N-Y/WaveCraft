using UnityEngine;

[ExecuteAlways]
public class O_CustomImageEffect : MonoBehaviour
{
    public Material imageEffect;

    private void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (imageEffect != null)
        {
            Graphics.Blit(src, dest, imageEffect);
        }
        else
        {
            Graphics.Blit(src, dest);
        }
    }
}
