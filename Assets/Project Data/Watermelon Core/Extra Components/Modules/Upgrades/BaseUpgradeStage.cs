using UnityEngine;

namespace Watermelon.Upgrades
{
    [System.Serializable]
    public abstract class BaseUpgradeStage
    {
        [SerializeField] int price;
        public int Price => price;

        [SerializeField] CurrencyType currencyType;
        public CurrencyType CurrencyType => currencyType;

        [SerializeField] Sprite previewSprite;
        public Sprite PreviewSprite => previewSprite;
    }
}