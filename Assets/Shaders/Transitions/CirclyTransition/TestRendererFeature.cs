using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class TestRendererFeature : ScriptableRendererFeature
{
    class CustomRenderPass : ScriptableRenderPass
    {
        private readonly Material _mat;
        private RenderTargetIdentifier _source;
        private RenderTargetHandle _tmpTex;


        public CustomRenderPass(Material mat) : base()
        {
            _mat = mat;
            _tmpTex.Init("_TmpTestTex");
        }


        // This method is called before executing the render pass.
        // It can be used to configure render targets and their clear state. Also to create temporary render target textures.
        // When empty this render pass will render to the active camera render target.
        // You should never call CommandBuffer.SetRenderTarget. Instead call <c>ConfigureTarget</c> and <c>ConfigureClear</c>.
        // The render pipeline will ensure target setup and clearing happens in a performant manner.
        public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
        {

        }

        // Here you can implement the rendering logic.
        // Use <c>ScriptableRenderContext</c> to issue drawing commands or execute command buffers
        // https://docs.unity3d.com/ScriptReference/Rendering.ScriptableRenderContext.html
        // You don't have to call ScriptableRenderContext.submit, the render pipeline will call it at specific points in the pipeline.
        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            CommandBuffer cmdBuffer = CommandBufferPool.Get("TestRendererFeature");
            RenderTextureDescriptor camTexDesc = renderingData.cameraData.cameraTargetDescriptor;
            camTexDesc.depthBufferBits = 0;
            cmdBuffer.GetTemporaryRT(_tmpTex.id, camTexDesc, FilterMode.Bilinear);

            Blit(cmd: cmdBuffer, source: _source, _tmpTex.Identifier(), _mat, 0);
            Blit(cmd: cmdBuffer, source: _tmpTex.Identifier(), _source);

            context.ExecuteCommandBuffer(cmdBuffer);
            CommandBufferPool.Release(cmdBuffer);
        }


        public override void FrameCleanup(CommandBuffer cmd)
        {
            base.FrameCleanup(cmd);
            cmd.ReleaseTemporaryRT(_tmpTex.id);
        }
        // Cleanup any allocated resources that were created during the execution of this render pass.
        public override void OnCameraCleanup(CommandBuffer cmd)
        {
        }

        public void SetSource(RenderTargetIdentifier source)
        {
            _source = source;
        }
    }

    [SerializeField] private Material _mat;

    private CustomRenderPass _scriptablePass;

    /// <inheritdoc/>
    public override void Create()
    {
        _scriptablePass = new CustomRenderPass(_mat)
        {
            // Configures where the render pass should be injected.
            renderPassEvent = RenderPassEvent.AfterRendering,
        };
    }

    // Here you can inject one or multiple render passes in the renderer.
    // This method is called when setting up the renderer once per-camera.
    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        _scriptablePass.SetSource(renderer.cameraColorTarget);
        renderer.EnqueuePass(_scriptablePass);
    }
}


