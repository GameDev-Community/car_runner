using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Testing.Shaders
{
    public class CircleWipeRendererFeature : ScriptableRendererFeature
    {
        private sealed class RenderPass : Externals.Utils.Shaders.SimpleRenderPass
        {
            public RenderPass(Material mat) : base(mat)
            {
            }

            public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
            {
                ExecutePasses(1, ref context);
            }
        }


        [SerializeField] private RenderPassEvent _renderPassEvent = RenderPassEvent.AfterRenderingTransparents;
        [SerializeField] private Material _mat;
        private RenderPass _pass;


        public override void Create()
        {
            _pass = new RenderPass(_mat)
            {
                renderPassEvent = _renderPassEvent
            };
        }


        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            renderer.EnqueuePass(_pass);
        }

        
    }
}