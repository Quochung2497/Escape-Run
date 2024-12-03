using System.Collections.Generic;
using UnityEngine;
using System;

namespace Watermelon.Outline
{
    [ExecuteAlways]
    public class Outlinable : MonoBehaviour
    {
        private static List<VisibilityListener> tempListeners = new List<VisibilityListener>();
        
        private static HashSet<Outlinable> outlinables = new HashSet<Outlinable>();

        [Serializable]
        public class OutlineProperty
        {
#pragma warning disable CS0649

            [SerializeField] bool enabled = true;
            public bool Enabled 
            {
                get => enabled;
                set => enabled = value;
            }

            [SerializeField] Color color = Color.black;
            public Color Color
            {
                get => color;
                set => color = value;
            }

            [Range(0.0f, 1.0f)]
            [SerializeField] float dilateShift = 1.0f;

            public float DilateShift
            {
                get => dilateShift;
                set => dilateShift = value;
            }

            [Range(0.0f, 1.0f)]
            [SerializeField] float blurShift = 1.0f;
            public float BlurShift
            {
                get => blurShift;
                set => blurShift = value;
            }

            [SerializeField] private SerializedPass fillPass = new SerializedPass();
            public SerializedPass FillPass => fillPass;

            public bool IsNonOne => BlurShift != 1.0f || DilateShift != 1.0f;

#pragma warning restore CS0649
        }

        [SerializeField] ComplexMaskingMode complexMaskingMode;
        public ComplexMaskingMode ComplexMaskingMode
        {
            get => complexMaskingMode;
            set => complexMaskingMode = value;
        }
        public bool ComplexMaskingEnabled => complexMaskingMode != ComplexMaskingMode.None;

        [SerializeField] OutlinableDrawingMode drawingMode = OutlinableDrawingMode.Normal;
        public OutlinableDrawingMode DrawingMode
        {
            get => drawingMode;
            set => drawingMode = value;
        }

        [SerializeField] int outlineLayer = 0;
        public int OutlineLayer
        {
            get => outlineLayer;
            set => outlineLayer = value;
        }

        [SerializeField] List<OutlineTarget> outlineTargets = new List<OutlineTarget>();
        public IReadOnlyList<OutlineTarget> OutlineTargets => outlineTargets;

        [SerializeField] RenderStyle renderStyle = RenderStyle.Single;
        public RenderStyle RenderStyle
        {
            get => renderStyle;
            set => renderStyle = value;
        }

#pragma warning disable CS0649

        [SerializeField] OutlineProperty defaultOutlineProperty = new OutlineProperty();
        public OutlineProperty DefaultOutlineProperty => defaultOutlineProperty;

        [SerializeField] OutlineProperty backOutlineProperty = new OutlineProperty();
        public OutlineProperty BackOutlineProperty => backOutlineProperty;

        [SerializeField] OutlineProperty frontOutlineProperty = new OutlineProperty();
        public OutlineProperty FrontOutlineProperty => frontOutlineProperty;

        private bool shouldValidateTargets = false;
        
#pragma warning restore CS0649

        public bool NeedFillMask
        {
            get
            {
                if ((drawingMode & OutlinableDrawingMode.Normal) == 0)
                    return false;

                if (renderStyle == RenderStyle.FrontBack)
                    return (frontOutlineProperty.Enabled || backOutlineProperty.Enabled) && (frontOutlineProperty.FillPass.Material != null || backOutlineProperty.FillPass.Material != null);
                else
                    return false;
            }
        }

        public bool IsObstacle => (drawingMode & OutlinableDrawingMode.Obstacle) != 0;


        public bool TryAddTarget(OutlineTarget target)
        {
            outlineTargets.Add(target);
            ValidateTargets();

            return true;
        }

        public void RemoveTarget(OutlineTarget target)
        {
            outlineTargets.Remove(target);
            if (target.renderer != null)
            {
                var listener = target.renderer.GetComponent<VisibilityListener>();
                if (listener == null)
                    return;
                
                listener.RemoveCallback(this, UpdateVisibility);
            }
        }
        
        public OutlineTarget this[int index]
        {
            get => outlineTargets[index];
            set
            {
                outlineTargets[index] = value;
                ValidateTargets();
            }
        }

        private void Reset()
        {
            AddAllChildRenderersToRenderingList(RenderersAddingMode.SkinnedMeshRenderer | RenderersAddingMode.MeshRenderer | RenderersAddingMode.SpriteRenderer);
        }

        private void OnValidate()
        {
            outlineLayer = Mathf.Clamp(outlineLayer, 0, 63);
            shouldValidateTargets = true;
        }

        private void SubscribeToVisibilityChange(GameObject go)
        {
            var listener = go.GetComponent<VisibilityListener>();
            if (listener == null)
            {
                listener = go.AddComponent<VisibilityListener>();
#if UNITY_EDITOR
                UnityEditor.EditorUtility.SetDirty(listener);
                UnityEditor.EditorUtility.SetDirty(go);
#endif
            }

            listener.RemoveCallback(this, UpdateVisibility);
            listener.AddCallback(this, UpdateVisibility);
        }

        private void UpdateVisibility()
        {
            if (!enabled)
            {
                outlinables.Remove(this);
                return;
            }

            outlineTargets.RemoveAll(x => x.renderer == null);
            foreach (var target in OutlineTargets)
                target.IsVisible = target.renderer.isVisible;

            outlineTargets.RemoveAll(x => x.renderer == null);

            foreach (var target in outlineTargets)
            {
                if (target.IsVisible)
                {
                    outlinables.Add(this);
                    return;
                }
            }

            outlinables.Remove(this);
        }

        private void OnEnable()
        {
            UpdateVisibility();
        }

        private void OnDisable()
        {
            outlinables.Remove(this);
        }

        private void Awake()
        {
            ValidateTargets();
        }

        private void ValidateTargets()
        {
            outlineTargets.RemoveAll(x => x.renderer == null);
            foreach (var target in outlineTargets)
                SubscribeToVisibilityChange(target.renderer.gameObject);
        }

        private void OnDestroy()
        {
            outlinables.Remove(this);
        }
        
        public static void GetAllActiveOutlinables(Camera camera, List<Outlinable> outlinablesList)
        {
            outlinablesList.Clear();
            foreach (var outlinable in outlinables)
                outlinablesList.Add(outlinable);
        }

        private int GetSubmeshCount(Renderer renderer)
        {
            if (renderer is MeshRenderer)
                return renderer.GetComponent<MeshFilter>().sharedMesh.subMeshCount;
            else if (renderer is SkinnedMeshRenderer)
                return (renderer as SkinnedMeshRenderer).sharedMesh.subMeshCount;
            else
                return 1;
        }

        public void AddAllChildRenderersToRenderingList(RenderersAddingMode renderersAddingMode = RenderersAddingMode.All)
        {
            outlineTargets.Clear();
            var renderers = GetComponentsInChildren<Renderer>(true);
            foreach (var renderer in renderers)
            {
                if (!MatchingMode(renderer, renderersAddingMode))
                    continue;

                var submeshesCount = GetSubmeshCount(renderer);
                for (var index = 0; index < submeshesCount; index++)
                    TryAddTarget(new OutlineTarget(renderer, index));
            }
        }

        private void Update()
        {
            if (!shouldValidateTargets)
                return;

            shouldValidateTargets = false;
            ValidateTargets();
        }

        private bool MatchingMode(Renderer renderer, RenderersAddingMode mode)
        {
            return 
                (!(renderer is MeshRenderer) && !(renderer is SkinnedMeshRenderer) && !(renderer is SpriteRenderer) && (mode & RenderersAddingMode.Others) != RenderersAddingMode.None) ||
                (renderer is MeshRenderer && (mode & RenderersAddingMode.MeshRenderer) != RenderersAddingMode.None) ||
                (renderer is SpriteRenderer && (mode & RenderersAddingMode.SpriteRenderer) != RenderersAddingMode.None) ||
                (renderer is SkinnedMeshRenderer && (mode & RenderersAddingMode.SkinnedMeshRenderer) != RenderersAddingMode.None);
        }

#if UNITY_EDITOR
        public void OnDrawGizmosSelected()
        {
            foreach (var target in outlineTargets)
            {
                if (target.Renderer == null || target.BoundsMode != BoundsMode.Manual)
                    continue;

                Gizmos.matrix = target.Renderer.transform.localToWorldMatrix;

                Gizmos.color = new Color(1.0f, 0.5f, 0.0f, 0.2f);
                var size = target.Bounds.size;
                var scale = target.Renderer.transform.localScale;
                size.x /= scale.x;
                size.y /= scale.y;
                size.z /= scale.z;

                Gizmos.DrawCube(target.Bounds.center, size);
                Gizmos.DrawWireCube(target.Bounds.center, size);
            }
        }
#endif
    }

    public enum DilateRenderMode
    {
        PostProcessing,
        EdgeShift
    }

    public enum RenderStyle
    {
        Single = 1,
        FrontBack = 2
    }

    [Flags]
    public enum OutlinableDrawingMode
    {
        Normal = 1,
        ZOnly = 2,
        GenericMask = 4,
        Obstacle = 8,
        Mask = 16
    }

    [Flags]
    public enum RenderersAddingMode
    {
        All = -1,
        None = 0,
        MeshRenderer = 1,
        SkinnedMeshRenderer = 2,
        SpriteRenderer = 4,
        Others = 4096
    }

    public enum BoundsMode
    {
        Default,
        ForceRecalculate,
        Manual
    }

    public enum ComplexMaskingMode
    {
        None,
        ObstaclesMode,
        MaskingMode
    }
}