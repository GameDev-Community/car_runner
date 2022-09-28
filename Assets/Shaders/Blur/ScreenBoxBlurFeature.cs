using Externals.Utils.Shaders;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Testing.Shaders
{
    public sealed class ScreenBoxBlurFeature : ScriptableRendererFeature
    {
        private sealed class CustomRenderPass : SimpleRenderPass
        {
            public CustomRenderPass(Material mat) : base(mat)
            {
            }


            public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
            {
                ExecutePasses(2, 2, ref context);
            }
        }


        private static readonly int _blurAmountParamID = Shader.PropertyToID("_BlurStrength");

        [SerializeField] private RenderPassEvent _renderPassEvent = RenderPassEvent.AfterRenderingTransparents;
        [SerializeField] private Material _mat;
        [SerializeField, Range(0, 20)] private int _blurAmount = 1;

        private CustomRenderPass _renderPass;


        public override void Create()
        {
            _mat.SetInteger(_blurAmountParamID, _blurAmount);

            _renderPass = new(_mat)
            {
                renderPassEvent = _renderPassEvent
            };
        }


        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            renderer.EnqueuePass(_renderPass);
        }
    }
}