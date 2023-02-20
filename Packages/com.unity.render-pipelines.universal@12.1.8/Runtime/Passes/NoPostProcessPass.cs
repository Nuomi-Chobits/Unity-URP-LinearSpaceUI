using UnityEngine.Experimental.Rendering;

namespace UnityEngine.Rendering.Universal.Internal
{
    public class NoPostProcessPass : ScriptableRenderPass
    {
        RenderTargetHandle m_Source;
        RenderTargetHandle m_UguiTarget;

        Material m_BlitMaterial;

        public NoPostProcessPass(RenderPassEvent evt,Material mat)
        {
            renderPassEvent = evt;
            m_BlitMaterial = mat;
        }

        public void Setup(in RenderTargetHandle dest, in RenderTargetHandle source)
        {   
            m_UguiTarget = dest;
            m_Source = source;
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            ref CameraData cameraData = ref renderingData.cameraData;
            ref Camera camera = ref cameraData.camera;
            CommandBuffer cmd = CommandBufferPool.Get();

            using(new ProfilingScope(cmd, new ProfilingSampler("NoPostProcessPass")))
            {
                cmd.SetGlobalTexture(ShaderPropertyId.sourceTex, m_Source.Identifier());
                RenderTextureDescriptor desc = renderingData.cameraData.cameraTargetDescriptor;
                desc.depthBufferBits = 0;
                {
                    cmd.SetRenderTarget(m_UguiTarget.Identifier());
                    cmd.SetGlobalTexture(ShaderPropertyId.sourceTex, m_Source.Identifier());

                    // Conversion Gamma,and return to main Buffer
                    cmd.EnableShaderKeyword(ShaderKeywordStrings.LinearToSRGBConversion);
                    cmd.SetViewProjectionMatrices(Matrix4x4.identity, Matrix4x4.identity);
                    cmd.DrawMesh(RenderingUtils.fullscreenMesh, Matrix4x4.identity, m_BlitMaterial);
                    cmd.DisableShaderKeyword(ShaderKeywordStrings.LinearToSRGBConversion);
                }
            }

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }
    }
}