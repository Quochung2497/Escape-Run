using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Serialization;

namespace Watermelon.Outline
{
    [ExecuteAlways]
    [RequireComponent(typeof(Camera))]
    public class Outliner : MonoBehaviour
    {
#if UNITY_EDITOR
        private static GameObject lastSelectedOutliner;

        private static List<Outliner> outliners = new List<Outliner>();
#endif

        private static List<Outlinable> temporaryOutlinables = new List<Outlinable>(); 

        private OutlineParameters parameters = new OutlineParameters();

#if UNITY_EDITOR
        private OutlineParameters editorPreviewParameters = new OutlineParameters();
#endif

        private Camera targetCamera;


        [SerializeField] RenderStage stage = RenderStage.AfterTransparents;
        public RenderStage RenderStage
        {
            get => stage;
            set => stage = value;
        }

        [SerializeField] OutlineRenderingStrategy renderingStrategy = OutlineRenderingStrategy.Default;
        public OutlineRenderingStrategy RenderingStrategy
        {
            get => renderingStrategy;
            set => renderingStrategy = value;
        }

        [SerializeField] RenderingMode renderingMode;
        private RenderingMode RenderingMode
        {
            get => renderingMode;
            set => renderingMode = value;
        }

        [SerializeField] int outlineLayerMask = int.MaxValue;
        public int OutlineLayerMask
        {
            get => outlineLayerMask;
            set => outlineLayerMask = value;
        }

        [SerializeField] BufferSizeMode primaryBufferSizeMode;
        public BufferSizeMode PrimaryBufferSizeMode
        {
            get => primaryBufferSizeMode;
            set => primaryBufferSizeMode = value;
        }

        [Range(0.15f, 1.0f)]
        [SerializeField] float primaryRendererScale = 0.75f;
        public float PrimaryRendererScale
        {
            get => primaryRendererScale;
            set => primaryRendererScale = Mathf.Clamp01(value);
        }

        [SerializeField] int primarySizeReference = 800;
        public int PrimarySizeReference
        {
            get => primarySizeReference;
            set => primarySizeReference = value;
        }

        [Range(0.0f, 2.0f)]
        [SerializeField] float blurShift = 1.0f;
        public float BlurShift
        {
            get => blurShift;
            set => blurShift = Mathf.Clamp(value, 0, 2.0f);
        }

        [Range(0.0f, 2.0f)]
        [SerializeField] float dilateShift = 1.0f;
        public float DilateShift
        {
            get => dilateShift;
            set => dilateShift = Mathf.Clamp(value, 0, 2.0f);
        }

        [SerializeField] int dilateIterations = 1;
        public int DilateIterations
        {
            get => dilateIterations;
            set => dilateIterations = value > 0 ? value : 0;
        }

        [SerializeField] DilateQuality dilateQuality;
        public DilateQuality DilateQuality
        {
            get => dilateQuality;
            set => dilateQuality = value;
        }

        [SerializeField] int blurIterations = 1;
        public int BlurIterations
        {
            get => blurIterations;
            set => blurIterations = value > 0 ? value : 0;
        }

        [SerializeField] BlurType blurType = BlurType.Box;
        public BlurType BlurType
        {
            get => blurType;
            set => blurType = value;
        }

        private CameraEvent Event => stage == RenderStage.BeforeTransparents? CameraEvent.AfterForwardOpaque : CameraEvent.BeforeImageEffects;

        private void OnValidate()
        {
            if (blurIterations < 0)
                blurIterations = 0;

            if (dilateIterations < 0)
                dilateIterations = 0;
        }

        private void OnEnable()
        {
            if (targetCamera == null)
                targetCamera = GetComponent<Camera>();

            targetCamera.forceIntoRenderTexture = targetCamera.stereoTargetEye == StereoTargetEyeMask.None || !UnityEngine.XR.XRSettings.enabled;

#if UNITY_EDITOR
            outliners.Add(this);
#endif

            parameters.CheckInitialization();
            parameters.Buffer.name = "Outline";

#if UNITY_EDITOR
            editorPreviewParameters.CheckInitialization();

            editorPreviewParameters.Buffer.name = "Editor outline";
#endif
        }

        private void OnDestroy()
        {
#if UNITY_EDITOR
            DestroyImmediate(editorPreviewParameters.BlitMesh);
            if (editorPreviewParameters.Buffer != null)
                editorPreviewParameters.Buffer.Dispose();
#endif

            DestroyImmediate(parameters.BlitMesh);

            if (parameters.Buffer != null)
                parameters.Buffer.Dispose();
        }

        private void OnDisable()
        {
            if (targetCamera != null)
                UpdateBuffer(targetCamera, parameters.Buffer, true);

#if UNITY_EDITOR
            RemoveFromAllSceneViews();

            outliners.Remove(this);

            foreach (var view in UnityEditor.SceneView.sceneViews)
            {
                var viewToUpdate = (UnityEditor.SceneView)view;

                viewToUpdate.camera.RemoveCommandBuffer(CameraEvent.BeforeImageEffects, editorPreviewParameters.Buffer);
                viewToUpdate.camera.RemoveCommandBuffer(CameraEvent.AfterForwardOpaque, editorPreviewParameters.Buffer);
            }
#endif
        }

        private void UpdateBuffer(Camera targetCamera, CommandBuffer buffer, bool removeOnly)
        {
            targetCamera.RemoveCommandBuffer(CameraEvent.BeforeImageEffects, buffer);
            targetCamera.RemoveCommandBuffer(CameraEvent.AfterForwardOpaque, buffer);
            if (removeOnly)
                return;

            targetCamera.AddCommandBuffer(Event, buffer);
        }

        private void OnPreRender()
        {
            if (GraphicsSettings.renderPipelineAsset != null)
                return;

            parameters.OutlinablesToRender.Clear();
            SetupOutline(targetCamera, parameters, false);
        }

        private void SetupOutline(Camera cameraToUse, OutlineParameters parametersToUse, bool isEditor)
        {
            UpdateBuffer(cameraToUse, parametersToUse.Buffer, false);
            UpdateParameters(parametersToUse, cameraToUse, isEditor);

            parametersToUse.Buffer.Clear();
            if (renderingStrategy == OutlineRenderingStrategy.Default)
            {
                OutlineEffect.SetupOutline(parametersToUse);
                parametersToUse.BlitMesh = null;
                parametersToUse.MeshPool.ReturnEverythingToPool();
            }
            else
            {
                temporaryOutlinables.Clear();
                temporaryOutlinables.AddRange(parametersToUse.OutlinablesToRender);

                parametersToUse.OutlinablesToRender.Clear();
                parametersToUse.OutlinablesToRender.Add(null);

                foreach (var outlinable in temporaryOutlinables)
                {
                    parametersToUse.OutlinablesToRender[0] = outlinable;
                    OutlineEffect.SetupOutline(parametersToUse);
                    parametersToUse.BlitMesh = null;
                }

                parametersToUse.MeshPool.ReturnEverythingToPool();
            }
        }

#if UNITY_EDITOR
        private void RemoveFromAllSceneViews()
        {
            foreach (var view in UnityEditor.SceneView.sceneViews)
            {
                var viewToUpdate = (UnityEditor.SceneView)view;
                var eventTransferer = viewToUpdate.camera.GetComponent<OnPreRenderEventTransferer>();
                if (eventTransferer != null)
                    eventTransferer.OnPreRenderEvent -= UpdateEditorCamera;

                viewToUpdate.camera.RemoveCommandBuffer(CameraEvent.BeforeImageEffects, editorPreviewParameters.Buffer);
                viewToUpdate.camera.RemoveCommandBuffer(CameraEvent.AfterForwardOpaque, editorPreviewParameters.Buffer);
            }
        }

        private void LateUpdate()
        {
            if (lastSelectedOutliner == null && outliners.Count > 0)
                lastSelectedOutliner = outliners[0].gameObject;

            var isSelected = Array.Find(UnityEditor.Selection.gameObjects, x => x == gameObject) ?? lastSelectedOutliner != null;
            if (isSelected)
                lastSelectedOutliner = gameObject;

            foreach (var view in UnityEditor.SceneView.sceneViews)
            {
                var viewToUpdate = (UnityEditor.SceneView)view;
                var eventTransferer = viewToUpdate.camera.GetComponent<OnPreRenderEventTransferer>();
                if (eventTransferer != null)
                    eventTransferer.OnPreRenderEvent -= UpdateEditorCamera;

                UpdateBuffer(viewToUpdate.camera, editorPreviewParameters.Buffer, true);
            }

            if (!isSelected)
                return;

            foreach (var view in UnityEditor.SceneView.sceneViews)
            {
                var viewToUpdate = (UnityEditor.SceneView)view;
                if (!viewToUpdate.sceneViewState.showImageEffects)
                    continue;

                var eventTransferer = viewToUpdate.camera.GetComponent<OnPreRenderEventTransferer>();
                if (eventTransferer == null)
                    eventTransferer = viewToUpdate.camera.gameObject.AddComponent<OnPreRenderEventTransferer>();

                eventTransferer.OnPreRenderEvent += UpdateEditorCamera;
            }
        }

        private void UpdateEditorCamera(Camera camera)
        {
            SetupOutline(camera, editorPreviewParameters, true);
        }
#endif

        public void UpdateSharedParameters(OutlineParameters parameters, Camera camera, bool editorCamera)
        {
            parameters.DilateQuality = DilateQuality;
            parameters.Camera = camera;
            parameters.IsEditorCamera = editorCamera;
            parameters.PrimaryBufferScale = primaryRendererScale;

            parameters.PrimaryBufferSizeMode = primaryBufferSizeMode;
            parameters.PrimaryBufferSizeReference = primarySizeReference;

            parameters.BlurIterations = blurIterations;
            parameters.BlurType = blurType;
            parameters.DilateIterations = dilateIterations;
            parameters.BlurShift = blurShift;
            parameters.DilateShift = dilateShift;
            parameters.UseHDR = camera.allowHDR && (RenderingMode == RenderingMode.HDR);
            parameters.EyeMask = camera.stereoTargetEye;

            parameters.OutlineLayerMask = outlineLayerMask;

            parameters.Prepare();
        }

        private void UpdateParameters(OutlineParameters parameters, Camera camera, bool editorCamera)
        {
            parameters.DepthTarget = RTUtility.ComposeTarget(parameters, BuiltinRenderTextureType.CameraTarget);

            var targetTexture = camera.targetTexture == null ? camera.activeTexture : camera.targetTexture;

            if (UnityEngine.XR.XRSettings.enabled
                && !parameters.IsEditorCamera
                && parameters.EyeMask != StereoTargetEyeMask.None)
            {
                var descriptor = UnityEngine.XR.XRSettings.eyeTextureDesc;
                parameters.TargetWidth = descriptor.width;
                parameters.TargetHeight = descriptor.height;
            }
            else
            {
                parameters.TargetWidth = targetTexture != null ? targetTexture.width : camera.scaledPixelWidth;
                parameters.TargetHeight = targetTexture != null ? targetTexture.height : camera.scaledPixelHeight;
            }

            parameters.Antialiasing = editorCamera ? (targetTexture == null ? 1 : targetTexture.antiAliasing) : CameraUtility.GetMSAA(targetCamera);

            parameters.Target = RTUtility.ComposeTarget(parameters, BuiltinRenderTextureType.CameraTarget);

            parameters.Camera = camera;

            Outlinable.GetAllActiveOutlinables(parameters.Camera, parameters.OutlinablesToRender);
            RendererFilteringUtility.Filter(parameters.Camera, parameters);
            UpdateSharedParameters(parameters, camera, editorCamera);
        }
    }

    public enum DilateQuality
    {
        Base,
        High,
        Ultra
    }

    public enum RenderingMode
    {
        LDR,
        HDR
    }

    public enum OutlineRenderingStrategy
    {
        Default,
        PerObject
    }

    public enum RenderStage
    {
        BeforeTransparents,
        AfterTransparents
    }

    public enum BufferSizeMode
    {
        WidthControllsHeight,
        HeightControlsWidth,
        Scaled,
        Native
    }
}