using UnityEngine;
using UnityEngine.UI;

namespace Watermelon
{
    public class FloatingCloudSettings
    {
        public const float DEFAULT_RADIUS = 200;

        private string name;
        public string Name => name;

        private float cloudRadius;
        public float CloudRadius => cloudRadius;

        private GameObject prefab;
        public GameObject Prefab => prefab;

        private Sprite sprite;
        public Sprite Sprite => sprite;

        private AudioClip appearAudioClip;
        public AudioClip AppearAudioClip => appearAudioClip;

        private AudioClip collectAudioClip;
        public AudioClip CollectAudioClip => collectAudioClip;

        public FloatingCloudSettings(string name, GameObject prefab)
        {
            this.name = name;
            this.prefab = prefab;
            this.cloudRadius = DEFAULT_RADIUS;
        }

        public FloatingCloudSettings(string name, Sprite sprite, Vector2 size)
        {
            this.name = name;
            this.cloudRadius = DEFAULT_RADIUS;

            GameObject tempPrefab = new GameObject(name);
            tempPrefab.hideFlags = HideFlags.HideInHierarchy;

            Image image = tempPrefab.AddComponent<Image>();
            image.sprite = sprite;

            RectTransform rectTransform = (RectTransform)tempPrefab.transform;
            rectTransform.sizeDelta = size;

            this.prefab = tempPrefab;
        }

        public FloatingCloudSettings SetAudio(AudioClip appearAudioClip, AudioClip collectAudioClip)
        {
            this.appearAudioClip = appearAudioClip;
            this.collectAudioClip = collectAudioClip;

            return this;
        }

        public FloatingCloudSettings SetRadius(float cloudRadius)
        {
            this.cloudRadius = cloudRadius;

            return this;
        }
    }
}
