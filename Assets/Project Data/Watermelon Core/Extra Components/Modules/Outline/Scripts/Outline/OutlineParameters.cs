using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace Watermelon.Outline
{
    public class OutlineParameters
    {
        public readonly SimpleMeshPool MeshPool = new SimpleMeshPool();

        public Camera Camera { get; set; }
        public RenderTargetIdentifier Target { get; set; }
        public RenderTargetIdentifier DepthTarget { get; set; }
        public CommandBuffer Buffer { get; set; }
        public DilateQuality DilateQuality { get; set; }
        public int DilateIterations { get; set; } = 2;
        public int BlurIterations { get; set; } = 5;

        public Vector2 Scale { get; set; } = Vector2.one;

        public Rect? CustomViewport { get; set; }

        public long OutlineLayerMask { get; set; } = -1;

        public int TargetWidth { get; set; }
        public int TargetHeight { get; set; }

        public float BlurShift { get; set; } = 1.0f;

        public float DilateShift { get; set; } = 1.0f;

        public bool UseHDR { get; set; }

        public bool UseInfoBuffer { get; set; }

        public bool IsEditorCamera { get; set; }

        public BufferSizeMode PrimaryBufferSizeMode { get; set; }
        public int PrimaryBufferSizeReference { get; set; }

        public float PrimaryBufferScale { get; set; } = 0.1f;

        public StereoTargetEyeMask EyeMask { get; set; }

        public int Antialiasing { get; set; } = 1;

        public BlurType BlurType { get; set; } = BlurType.Gaussian13x13;

        public LayerMask Mask { get; set; } = -1;

        public Mesh BlitMesh { get; set; }

        public List<Outlinable> OutlinablesToRender { get; set; } = new List<Outlinable>();

        private bool isInitialized { get; set; }
        
        public Vector2Int MakeScaledVector(int x, int y)
        {
            var fx = (float)x;
            var fy = (float)y;

            return new Vector2Int(Mathf.FloorToInt(fx * Scale.x), Mathf.FloorToInt(fy * Scale.y));
        }

        public void CheckInitialization()
        {
            if (isInitialized)
                return;

            Buffer = new CommandBuffer();

            isInitialized = true;
        }

        public void Prepare()
        {
            if (OutlinablesToRender.Count == 0)
                return;
            
            UseInfoBuffer = OutlinablesToRender.Find(x => x != null && ((x.DrawingMode & (OutlinableDrawingMode.Obstacle | OutlinableDrawingMode.Mask)) != 0 || x.ComplexMaskingEnabled)) != null;
            if (UseInfoBuffer)
                return;

            foreach (var target in OutlinablesToRender)
            {
                if ((target.DrawingMode & OutlinableDrawingMode.Normal) == 0)
                    continue;

                if (!CheckDiffers(target))
                    continue;

                UseInfoBuffer = true;
                break;
            }
        }

        private static bool CheckDiffers(Outlinable outlinable)
        {
            if (outlinable.RenderStyle == RenderStyle.Single)
                return CheckIfNonOne(outlinable.DefaultOutlineProperty);
            else
                return CheckIfNonOne(outlinable.FrontOutlineProperty) || CheckIfNonOne(outlinable.BackOutlineProperty);
        }

        private static bool CheckIfNonOne(Outlinable.OutlineProperty parameters)
        {
            return parameters.BlurShift != 1.0f || parameters.DilateShift != 1.0f;
        }
    }
}