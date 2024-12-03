using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Watermelon.UI.Particle
{
    [CreateAssetMenu(menuName = "Content/Data/Custom UI Particle Settings", fileName = "Custom UI Particle Settings")]
    public class UIParticleSettings : ScriptableObject
    {
        public GameObject uiParticlePrefab;
        public Sprite sprite;

        [Space]
        public bool playOnAwake;
        public float startDelay;
        public DuoFloat startSize;
        public DuoColor startDuoColor;
        public float gravityModifier;

        [Space]
        public Shape shape;
        public float circleRadius;
        public Vector2 rectSize;
        [Range(0f, 1f)] public float spherizeDirection;

        [Space]
        public DuoFloat lifetime;
        public int emissionPerSecond;
        public BurstSettings[] bursts;

        [Space]
        public DuoFloat angle;

        [Space]
        public bool speed3d;
        public DuoFloat speed;
        public DuoVector3 speed3dValues;
        public DuoFloat dumping;

        [Space]
        public AnimationCurve scaleCurve;

        [Space]
        public bool rotationOverLifetime;
        public DuoFloat angularSpeed;

        [Space]
        public bool colorOverLifetime;
        public Gradient colorOverLifetimeGradient;


        public enum Shape
        {
            Point = 0,
            Circle = 1,
            Rect = 2,
        }

        [System.Serializable]
        public class BurstSettings
        {
            public int count;
            public int loopsCount;
            public float interval;
            public float delay;
        }


    }
}