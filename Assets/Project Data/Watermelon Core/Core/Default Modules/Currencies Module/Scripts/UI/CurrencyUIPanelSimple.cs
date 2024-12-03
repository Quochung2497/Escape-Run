using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Watermelon
{
    [System.Serializable]
    public class CurrencyUIPanelSimple : MonoBehaviour
    {
        [SerializeField] CurrencyType currencyType;

        [Space]
        [SerializeField] bool updateOnChange = true;
        [SerializeField] bool useFormattedAmount = true;

        [Space]
        [SerializeField] TextMeshProUGUI text;
        [SerializeField] Image icon;
        [SerializeField] Button addButton;

        public string Text { get => text.text; set => text.text = value; }
        public Sprite Icon { get => icon.sprite; set => icon.sprite = value; }
        public Button AddButton => addButton;

        private Currency currency;
        public Currency Currency => currency;
        
        private RectTransform rectTransformRef;
        public RectTransform RectTransform => rectTransformRef;

        private void Awake()
        {
            rectTransformRef = GetComponent<RectTransform>();
        }

        public void Initialise()
        {
            currency = CurrenciesController.GetCurrency(currencyType);

            icon.sprite = currency.Icon;

            Redraw();
            Activate();
        }

        public void Redraw()
        {
            text.text = useFormattedAmount ? currency.AmountFormatted : currency.Amount.ToString();
        }

        public void SetAmount(int amount, bool format = true)
        {
            text.text = format ? CurrenciesHelper.Format(amount) : amount.ToString();
        }

        public void Activate()
        {
            if(updateOnChange)
            {
                currency.OnCurrencyChanged += OnCurrencyAmountChanged;
            }
        }

        public void Disable()
        {
            if(updateOnChange)
            {
                currency.OnCurrencyChanged -= OnCurrencyAmountChanged;
            }
        }

        private void OnCurrencyAmountChanged(Currency currency, int amountDifference)
        {
            text.text = useFormattedAmount ? currency.AmountFormatted : currency.Amount.ToString();
        }
    }
}