using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class OutlineRenderFeature : ScriptableRendererFeature
{
    class OutlineRenderPass : ScriptableRenderPass
    {
        private Material material;
        private RenderTargetHandle tempRT;
        private string targetCameraName;

        public OutlineRenderPass(Material material, string targetCameraName)
        {
            this.material = material;
            this.targetCameraName = targetCameraName;
            tempRT.Init("_TempOutlineRT");
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            if (renderingData.cameraData.camera.name != targetCameraName || material == null)
                return;

            var cmd = CommandBufferPool.Get("OutlineEffect");

            var descriptor = renderingData.cameraData.cameraTargetDescriptor;
            var currentTarget = renderingData.cameraData.renderer.cameraColorTarget;

            cmd.GetTemporaryRT(tempRT.id, descriptor);
            Blit(cmd, currentTarget, tempRT.Identifier(), material);
            Blit(cmd, tempRT.Identifier(), currentTarget);

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }

        public override void OnCameraCleanup(CommandBuffer cmd)
        {
            cmd.ReleaseTemporaryRT(tempRT.id);
        }
    }

    public Material outlineMaterial;
    public string cameraName;

    private OutlineRenderPass outlineRenderPass;

    public override void Create()
    {
        outlineRenderPass = new OutlineRenderPass(outlineMaterial, cameraName)
        {
            renderPassEvent = RenderPassEvent.AfterRenderingTransparents
        };
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        renderer.EnqueuePass(outlineRenderPass);
    }
}
