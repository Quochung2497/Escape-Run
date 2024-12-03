using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

namespace Watermelon
{
    public class ItemPanelScript : MonoBehaviour
    {
        [SerializeField] TMP_Text nameText;
        [SerializeField] TMP_Text purchasedText;
        [SerializeField] TMP_Text typeText;
        [SerializeField] Text priceText;

        ProductKeyType item;

        public ProductKeyType Item { get => item; set => item = value; }
        public string Name { get => nameText.text; set => nameText.text = value; }
        public string Purchased { get => purchasedText.text; set => purchasedText.text = value; }
        public string Type { get => typeText.text; set => typeText.text = value; }
        public string Price { get => priceText.text; set => priceText.text = value; }

        public void SetPurchasedTextActive(bool isActive)
        {
            purchasedText.gameObject.SetActive(isActive);
        }

        public void BuyButton()
        {
            IAPManager.BuyProduct(item);
            IAPManager.OnPurchaseComplete += HandlePurchaseComplete;
        }

        private void HandlePurchaseComplete(ProductKeyType productKeyType)
        {
            if(productKeyType == item)
            {
                SetPurchasedTextActive(true);
            }
        }
    }
}
