using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace Watermelon.Outline
{
    [Serializable]
    public class OutlineTarget
    {
        public bool IsVisible = false;
        public ColorMask CutoutMask = ColorMask.A;

        [SerializeField] float edgeDilateAmount = 5.0f;
        public float EdgeDilateAmount
        {
            get => edgeDilateAmount;
            set => edgeDilateAmount = Math.Clamp(value, 0, float.MaxValue);
        }

        [SerializeField] float frontEdgeDilateAmount = 5.0f;
        public float FrontEdgeDilateAmount
        {
            get => frontEdgeDilateAmount;
            set => frontEdgeDilateAmount = Math.Clamp(value, 0, float.MaxValue);
        }

        [SerializeField] float backEdgeDilateAmount = 5.0f;
        public float BackEdgeDilateAmount
        {
            get => backEdgeDilateAmount;
            set => backEdgeDilateAmount = Math.Clamp(value, 0, float.MaxValue);
        }

        public Renderer renderer;
        public Renderer Renderer => renderer;

        [SerializeField] int submeshIndex;
        public int SubmeshIndex => submeshIndex;

        [SerializeField] BoundsMode boundsMode = BoundsMode.Default;
        public BoundsMode BoundsMode => boundsMode;

        [SerializeField] Bounds bounds = new Bounds(Vector3.zero, Vector3.one);
        public Bounds Bounds => bounds;

        [Range(0.0f, 1.0f)] public float CutoutThreshold = 0.5f;

        [SerializeField] CullMode cullMode;
        public CullMode CullMode => cullMode;

        [SerializeField] string cutoutTextureName;
        public string CutoutTextureName
        {
            get => cutoutTextureName;
            set{
                cutoutTextureName = value;
                cutoutTextureId = null;
            }
        }

        public DilateRenderMode dilateRenderingMode;

        [SerializeField] int cutoutTextureIndex;
        public int CutoutTextureIndex
        {
            get => cutoutTextureIndex;
            set => cutoutTextureIndex = Math.Clamp(value, 0, int.MaxValue);
        }

        public bool UsesCutout => !string.IsNullOrEmpty(cutoutTextureName);

        private int? cutoutTextureId;
        public int CutoutTextureId
        {
            get
            {
                if (!cutoutTextureId.HasValue)
                    cutoutTextureId = Shader.PropertyToID(cutoutTextureName);

                return cutoutTextureId.Value;
            }
        }

        public OutlineTarget()
        {

        }

        public OutlineTarget(Renderer renderer, int submesh = 0)
        {
            submeshIndex = submesh;
            this.renderer = renderer;

            CutoutThreshold = 0.5f;
            cutoutTextureId = null;
            cutoutTextureName = string.Empty;
            cullMode = renderer is SpriteRenderer ? CullMode.Off : CullMode.Back;
            dilateRenderingMode = DilateRenderMode.PostProcessing;
            frontEdgeDilateAmount = 5.0f;
            backEdgeDilateAmount = 5.0f;
            edgeDilateAmount = 5.0f;
        }

        public OutlineTarget(Renderer renderer, string cutoutTextureName, float cutoutThreshold = 0.5f)
        {
            submeshIndex = 0;
            this.renderer = renderer;

            cutoutTextureId = Shader.PropertyToID(cutoutTextureName);
            CutoutThreshold = cutoutThreshold;
            this.cutoutTextureName = cutoutTextureName;
            cullMode = renderer is SpriteRenderer ? CullMode.Off : CullMode.Back;
            dilateRenderingMode = DilateRenderMode.PostProcessing;
            frontEdgeDilateAmount = 5.0f;
            backEdgeDilateAmount = 5.0f;
            edgeDilateAmount = 5.0f;
        }
    }

    [Flags]
    public enum ColorMask
    {
        None = 0,
        R = 1,
        G = 2,
        B = 4,
        A = 8
    }
}