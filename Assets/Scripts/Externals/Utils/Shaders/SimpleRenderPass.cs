using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Externals.Utils.Shaders
{
    public abstract class SimpleRenderPass : ScriptableRenderPass
    {
        private readonly Material _mat;

        private RenderTargetIdentifier _source;
        private RenderTexture _tmpRT;


        protected SimpleRenderPass(Material mat)
        {
            _mat = mat;
        }


        protected Material Mat => _mat;
        protected RenderTargetIdentifier Source => _source;
        protected RenderTexture TmpRT => _tmpRT;


        public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
        {
            _source = renderingData.cameraData.renderer.cameraColorTarget;
            var descriptor = renderingData.cameraData.cameraTargetDescriptor;
            _tmpRT = RenderTexture.GetTemporary(descriptor);
        }

        public override void OnCameraCleanup(CommandBuffer cmd)
        {
            RenderTexture.ReleaseTemporary(TmpRT);
        }


        protected void ExecutePasses(int passesCount, ref ScriptableRenderContext context)
        {
            var cmd = CommandBufferPool.Get();

            //assuming passesCount > 0

            var s = _source;
            var t = _tmpRT;
            var m = _mat;

            bool x = passesCount % 2 != 0;

            for (int i = -1; ++i < passesCount; x = !x)
            {
                Blit(cmd, x ? t : s, x ? s : t, m, i);
            }

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }

        protected void ExecutePasses(int passesCount, int repetitionsCount, ref ScriptableRenderContext context)
        {
            var cmd = CommandBufferPool.Get();

            //assuming passesCount > 0

            var s = _source;
            var t = _tmpRT;
            var m = _mat;

            bool x = passesCount % 2 != 0;

            for (int j = -1; ++j < repetitionsCount;)
            {
                for (int i = -1; ++i < passesCount; x = !x)
                {
                    Blit(cmd, x ? t : s, x ? s : t, m, i);
                }
            }
            

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }

    }
}