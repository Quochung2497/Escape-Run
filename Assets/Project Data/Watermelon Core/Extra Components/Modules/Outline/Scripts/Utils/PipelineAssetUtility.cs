using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System.Collections.Generic;
using System;
using System.Reflection;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Watermelon.Outline
{
    public static class PipelineFetcher
    {
        public static RenderPipelineAsset CurrentAsset
        {
            get
            {
                var pipeline = QualitySettings.renderPipeline;
                if (pipeline == null)
                    pipeline = GraphicsSettings.renderPipelineAsset;
                return pipeline;
            }
        }
    }

#if UNITY_EDITOR
    public static class PipelineAssetUtility
    {
        public static RenderPipelineAsset CurrentAsset
        {
            get
            {
                return PipelineFetcher.CurrentAsset;
            }
        }

        public static HashSet<RenderPipelineAsset> ActiveAssets
        {
            get
            {
                var set = new HashSet<RenderPipelineAsset>();

                if (GraphicsSettings.renderPipelineAsset != null)
                    set.Add(GraphicsSettings.renderPipelineAsset);

                var qualitySettingNames = QualitySettings.names;
                for (var index = 0; index < qualitySettingNames.Length; index++)
                {
                    var assset = QualitySettings.GetRenderPipelineAssetAt(index);
                    if (assset == null)
                        continue;

                    set.Add(assset);
                }

                return set;
            }
        }

        public static object GetDefault(Type type)
        {
            if(type.IsValueType)
            {
                return Activator.CreateInstance(type);
            }
            return null;
        }
        
        public static RenderPipelineAsset CreateAsset(string path)
        {
            var method = typeof(UniversalRenderPipelineAsset)
                .GetMethod("CreateRendererAsset", BindingFlags.NonPublic | BindingFlags.Static);

            var methodParams = method.GetParameters();
            object[] parameters = new object[methodParams.Length];
            for (var index = 0; index < methodParams.Length; index++)
                parameters[index] = GetDefault(methodParams[index].ParameterType);

            var possibleParameters = new object[] {path + " renderer.asset", (RendererType)1, false, "Renderer"};
            for (var index = 0; index < Mathf.Min(possibleParameters.Length, parameters.Length); index++)
                parameters[index] = possibleParameters[index];
            
            var data = method.Invoke(null, parameters) as ScriptableRendererData;

            var pipeline = UniversalRenderPipelineAsset.Create(data);
            
            AssetDatabase.CreateAsset(pipeline, path + ".asset");

            return pipeline;
        }

        public static bool IsURPOrLWRP(RenderPipelineAsset asset)
        {
            return asset != null &&
                (asset.GetType().Name.Equals("LightweightRenderPipelineAsset") ||
                asset.GetType().Name.Equals("UniversalRenderPipelineAsset"));
        }

        public static ScriptableRendererData GetRenderer(RenderPipelineAsset asset)
        {
            using (var so = new SerializedObject(asset))
            {
                so.Update();

                var rendererDataList = so.FindProperty("m_RendererDataList");
                var assetIndex = so.FindProperty("m_DefaultRendererIndex");
                var item = rendererDataList.GetArrayElementAtIndex(assetIndex.intValue);

                return item.objectReferenceValue as ScriptableRendererData;
            }
        }

        public static bool IsAssetContainsSRPOutlineFeature(RenderPipelineAsset asset)
        {
            var data = GetRenderer(asset);

            return data.rendererFeatures.Find(x => x is OutlineFeature) != null;
        }

    }
#endif
}