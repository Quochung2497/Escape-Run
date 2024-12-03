using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Watermelon.Store
{
    [CreateAssetMenu(fileName = "Store Database", menuName = "Content/Data/Store Database")]
    public class StoreDatabase : ScriptableObject
    {
        [SerializeField] TabData[] tabs;
        public TabData[] Tabs => tabs;

        [SerializeField] ProductData[] products;

        private Dictionary<TabType, SimpleStringSave> selectedProducts;

        public TabType[] TabTypes;


        [SerializeField] int coinsForAdsAmount;
        [SerializeField] CurrencyType currencyForAds;
        public int CoinsForAds => coinsForAdsAmount;
        public CurrencyType CurrencyForAds => currencyForAds;


        public string this[TabType type]
        {
            get => selectedProducts[type].Value;
            set => selectedProducts[type].Value = value;
        }

        public Dictionary<TabData, List<ProductData>> Init()
        {
            var sortedProducts = new Dictionary<TabData, List<ProductData>>();

            for(int i = 0; i < products.Length; i++)
            {
                var product = products[i];
                var tab = tabs[product.TabId];

                if (sortedProducts.ContainsKey(tab))
                {
                    sortedProducts[tab].Add(product); ;
                } else
                {
                    sortedProducts.Add(tab, new List<ProductData> { product });
                }

                product.Init();
            }

            selectedProducts = new Dictionary<TabType, SimpleStringSave>();

            TabTypes = (TabType[]) Enum.GetValues(typeof(TabType));

            for(int i = 0; i < TabTypes.Length; i++)
            {
                var type = (TabType)TabTypes.GetValue(i);
                var save = SaveController.GetSaveObject<SimpleStringSave>($"Store Type: {type}");
                selectedProducts.Add(type, save);
            }

            return sortedProducts;
        }
    }

    [System.Serializable]
    public class TabData
    {
        [SerializeField] string uniqueId;
        public string UniqueId => uniqueId;

        [SerializeField] string name;
        public string Name => name;

        [SerializeField] TabType type;
        public TabType Type => type;

        [Space]
        [SerializeField] PreviewType previewType;
        public PreviewType PreviewType => previewType;

        [Space]
        [SerializeField] GameObject previewPrefab;
        public GameObject PreviewPrefab => previewPrefab;

        [SerializeField] Color backgroundColor;
        public Color BackgroundColor => backgroundColor;

        [SerializeField] Color productBackgroundColor;
        public Color ProductBackgroundColor => productBackgroundColor;

        public delegate void SimpleTabDelegate(TabData tab);
    }

    public enum PreviewType
    {
        Preview_2D,
        Preview_3D
    }
}