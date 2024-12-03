using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Rendering.Universal;

namespace Watermelon.Outline
{
    public class OutlineFeature : ScriptableRendererFeature
    {
        private GameObject lastSelectedCamera;

        private SimplePool<OutlineRenderPass> outlinePool = new SimplePool<OutlineRenderPass>();

        private List<Outliner> outliners = new List<Outliner>();

        private bool GetOutlinersToRenderWith(RenderingData renderingData, List<Outliner> outliners)
        {
            outliners.Clear();
            var camera = renderingData.cameraData.camera.gameObject;
            camera.GetComponents(outliners);
            if (outliners.Count == 0)
            {
#if UNITY_EDITOR
                if (renderingData.cameraData.isSceneViewCamera)
                {
                    for(int i = 0; i < UnityEditor.Selection.gameObjects.Length; i++)
                    {
                        var obj = UnityEditor.Selection.gameObjects[i];
                        if(obj != null && obj.TryGetComponent<Outliner>(out var outliner) && outliner != null)
                        {
                            camera = outliner?.gameObject ?? lastSelectedCamera;
                        }
                    }                  

                    if (camera == null)
                        return false;
                    else
                        camera.GetComponents(outliners);
                }
                else
                    return false;
#else
                return false;
#endif
            }

            lastSelectedCamera = camera;

            return outliners.Count > 0;
        }

        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            if (!GetOutlinersToRenderWith(renderingData, outliners))
                return;

            foreach (var outliner in outliners)
            {
                var outline = outlinePool.GetPooledObject();

                outline.Outliner = outliner;

                outline.Renderer = renderer;

                outline.renderPassEvent = outliner.RenderStage == RenderStage.AfterTransparents ? RenderPassEvent.AfterRenderingTransparents : RenderPassEvent.AfterRenderingOpaques;

                renderer.EnqueuePass(outline);
            }

            outlinePool.ReturnEverythingToPool();
        }

        public override void Create()
        {
        }
    }
}