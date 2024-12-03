using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Watermelon.UI.Particle
{
    public class UIParticle : MonoBehaviour
    {
        [SerializeField] Image image;

        public float spawnTime;
        public float lifetime;

        public Color32 startColor;
        public Vector2 startSize;

        public Vector2 velocity;
        public float dumping;

        public Vector3 angularVelocity;

        public bool IsActive { get => gameObject.activeInHierarchy; set => gameObject.SetActive(value); }

        public Sprite Sprite { get => image.sprite; set => image.sprite = value; }

        public Vector2 Size { get => image.rectTransform.sizeDelta; set => image.rectTransform.sizeDelta = value; }
        public Vector2 AnchoredPosition { get => image.rectTransform.anchoredPosition; set => image.rectTransform.anchoredPosition = value; }
        public Vector3 EulerAngles { get => image.rectTransform.eulerAngles; set => image.rectTransform.eulerAngles = value; }
        public Color32 Color { get => image.color; set => image.color = value; }

        public UIParticleSettings Settings { get; private set; }

        public void Init(UIParticleSettings settings, float timeSpend, Vector2 targetAnchoredPosition, Vector2 normalizedVelocity)
        {
            Settings = settings;

            Sprite = settings.sprite;

            spawnTime = Time.time - timeSpend;

            lifetime = settings.lifetime.Random();

            if (settings.speed3d)
            {
                velocity = settings.speed3dValues.Random();
            }
            else
            {
                velocity = Quaternion.Euler(0, 0, settings.angle.Random()) * normalizedVelocity * settings.speed.Random();
            }

            if (targetAnchoredPosition != Vector2.zero)
            {
                velocity = Vector2.Lerp(velocity.normalized, targetAnchoredPosition.normalized, settings.spherizeDirection) * velocity.magnitude;
            }

            dumping = settings.dumping.Random();

            startColor = settings.startDuoColor.RandomBetween();
            Color = startColor;

            if (settings.colorOverLifetime)
            {
                Color *= settings.colorOverLifetimeGradient.Evaluate(0);
            }

            if (settings.rotationOverLifetime)
            {
                angularVelocity = new Vector3(0, 0, settings.angularSpeed.Random());
            }
            else
            {
                angularVelocity = Vector3.zero;
            }

            AnchoredPosition = targetAnchoredPosition + velocity * timeSpend;

            startSize = Vector2.one * settings.startSize.Random();
            Size = startSize;
        }

        public bool Tick()
        {
            var time = Time.time - spawnTime;

            if (lifetime <= time) return true;

            velocity += Vector2.down * Settings.gravityModifier * Time.deltaTime;

            AnchoredPosition += velocity * Time.deltaTime;
            EulerAngles += angularVelocity * Time.deltaTime;

            var t = time / lifetime;

            Size = startSize * Settings.scaleCurve.Evaluate(t);

            if (Settings.colorOverLifetime)
            {
                Color = startColor * Settings.colorOverLifetimeGradient.Evaluate(t);
            }

            return false;
        }
    }
}