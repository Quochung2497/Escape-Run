using UnityEngine;

namespace Watermelon
{
    [System.Serializable]
    public partial class Currency
    {
        [SerializeField] CurrencyType currencyType;
        public CurrencyType CurrencyType => currencyType;

        [SerializeField] Sprite icon;
        public Sprite Icon => icon;

        [SerializeField] bool displayAlways = false;
        public bool DisplayAlways => displayAlways;

        [SerializeField] FloatingCloudCase floatingCloud;

        [Space]
        [SerializeField] GameObject purchaseParticlePrefab;
        public GameObject PurchaseParticlePrefab => purchaseParticlePrefab;
        
        public int Amount { get => save.Amount; set => save.Amount = value; }

        public string AmountFormatted => CurrenciesHelper.Format(save.Amount);

        public event CurrencyChangeDelegate OnCurrencyChanged;

        private Save save;

        public void Initialise()
        {
            // Add element to cloud
            if(floatingCloud.AddToCloud)
            {
                FloatingCloudSettings floatingCloudSettings;

                if (floatingCloud.SpecialPrefab != null)
                {
                    floatingCloudSettings = new FloatingCloudSettings(currencyType.ToString(), floatingCloud.SpecialPrefab);
                }
                else
                {
                    floatingCloudSettings = new FloatingCloudSettings(currencyType.ToString(), icon, new Vector2(100, 100));
                }

                floatingCloudSettings.SetAudio(floatingCloud.AppearAudioClip, floatingCloud.CollectAudioClip);

                FloatingCloud.RegisterCase(floatingCloudSettings);
            }
        }

        public void SetSave(Save save)
        {
            this.save = save;
        }

        public void InvokeChangeEvent(int difference)
        {
            OnCurrencyChanged?.Invoke(this, difference);
        }

        [System.Serializable]
        public class Save : ISaveObject
        {
            [SerializeField] int amount;
            public int Amount { get => amount; set => amount = value; }

            public void Flush()
            {

            }
        }

        [System.Serializable]
        public class FloatingCloudCase
        {
            [SerializeField] bool addToCloud;
            public bool AddToCloud => addToCloud;

            [SerializeField] float radius = 200;
            public float Radius => radius;

            [SerializeField] GameObject specialPrefab;
            public GameObject SpecialPrefab => specialPrefab;

            [SerializeField] AudioClip appearAudioClip;
            public AudioClip AppearAudioClip => appearAudioClip;

            [SerializeField] AudioClip collectAudioClip;
            public AudioClip CollectAudioClip => collectAudioClip;
        }
    }
}