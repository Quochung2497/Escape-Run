using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using Watermelon.Outline;

namespace Watermelon.Outline
{
    public class OutlineRenderPass : ScriptableRenderPass
    {
        private static List<Outlinable> temporaryOutlinables = new List<Outlinable>();

        public ScriptableRenderer Renderer;

        public bool UseColorTargetForDepth;

        public Outliner Outliner;

        public OutlineParameters Parameters = new OutlineParameters();

        public OutlineRenderPass()
        {
            Parameters.CheckInitialization();
        }

        private bool IsDepthTextureAvailable => Renderer.cameraDepthTargetHandle.rt != null;
        

        private RenderTargetIdentifier DepthTarget => Renderer.cameraDepthTargetHandle;
        private RenderTargetIdentifier ColorTarget => Renderer.cameraColorTargetHandle;

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            if (Outliner == null || !Outliner.enabled) return;

            var data = renderingData.cameraData;
            var camera = data.camera;
            var descriptor = data.cameraTargetDescriptor;
#if UNITY_EDITOR
            Parameters.Buffer.name = camera.name;
#endif

            Outlinable.GetAllActiveOutlinables(camera, Parameters.OutlinablesToRender);

            Outliner.UpdateSharedParameters(Parameters, camera, data.isSceneViewCamera);

            RendererFilteringUtility.Filter(camera, Parameters);

            Parameters.TargetWidth = descriptor.width;
            Parameters.TargetHeight = descriptor.height;

            Parameters.Antialiasing = descriptor.msaaSamples;

            Parameters.Target = RTUtility.ComposeTarget(Parameters, ColorTarget);
            Parameters.DepthTarget = RTUtility.ComposeTarget(Parameters, IsDepthTextureAvailable ? DepthTarget : ColorTarget);

            Parameters.Buffer.Clear();
            if (Outliner.RenderingStrategy == OutlineRenderingStrategy.Default)
            {
                OutlineEffect.SetupOutline(Parameters);
                Parameters.BlitMesh = null;
                Parameters.MeshPool.ReturnEverythingToPool();
            }
            else
            {
                temporaryOutlinables.Clear();
                temporaryOutlinables.AddRange(Parameters.OutlinablesToRender);

                Parameters.OutlinablesToRender.Clear();
                Parameters.OutlinablesToRender.Add(null);

                foreach (var outlinable in temporaryOutlinables)
                {
                    Parameters.OutlinablesToRender[0] = outlinable;
                    OutlineEffect.SetupOutline(Parameters);
                    Parameters.BlitMesh = null;
                }

                Parameters.MeshPool.ReturnEverythingToPool();
            }

            context.ExecuteCommandBuffer(Parameters.Buffer);
        }
    }
}