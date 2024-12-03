using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Watermelon.Outline
{
    public static class CameraUtility
    {
        public static int GetMSAA(Camera camera)
        {
            if (camera.targetTexture != null)
                return camera.targetTexture.antiAliasing;

            var antialiasing = GetRenderPipelineMSAA();

            var msaa = Mathf.Max(antialiasing, 1);
            if (!camera.allowMSAA)
                msaa = 1;

            if (camera.actualRenderingPath != RenderingPath.Forward &&
                camera.actualRenderingPath != RenderingPath.VertexLit)
                msaa = 1;

            return msaa;
        }
        
        private static int GetRenderPipelineMSAA()
        {
            if (PipelineFetcher.CurrentAsset is UniversalRenderPipelineAsset)
            {
                return (PipelineFetcher.CurrentAsset as UniversalRenderPipelineAsset).msaaSampleCount;
            }

            return QualitySettings.antiAliasing;
        }
    }
}