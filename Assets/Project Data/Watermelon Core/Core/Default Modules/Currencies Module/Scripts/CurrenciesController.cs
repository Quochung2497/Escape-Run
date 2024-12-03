using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Watermelon
{
    public class CurrenciesController : MonoBehaviour
    {
        private static CurrenciesController currenciesController;

        [SerializeField] CurrenciesDatabase currenciesDatabase;
        public CurrenciesDatabase CurrenciesDatabase => currenciesDatabase;

        private static Currency[] currencies;
        public static Currency[] Currencies => currencies;

        private static Dictionary<CurrencyType, int> currenciesLink;

        public virtual void Initialise()
        {
            currenciesController = this;

            // Initialsie database
            currenciesDatabase.Initialise();

            // Store active currencies
            currencies = currenciesDatabase.Currencies;

            // Link currencies by the type
            currenciesLink = new Dictionary<CurrencyType, int>();
            for (int i = 0; i < currencies.Length; i++)
            {
                if (!currenciesLink.ContainsKey(currencies[i].CurrencyType))
                {
                    currenciesLink.Add(currencies[i].CurrencyType, i);
                }
                else
                {
                    Debug.LogError(string.Format("[Currency Syste]: Currency with type {0} added to database twice!", currencies[i].CurrencyType));
                }

                var save = SaveController.GetSaveObject<Currency.Save>("currency" + ":" + (int)currencies[i].CurrencyType);
                currencies[i].SetSave(save);
            }
        }

        public static bool HasAmount(CurrencyType currencyType, int amount)
        {
            return currencies[currenciesLink[currencyType]].Amount >= amount;
        }

        public static int Get(CurrencyType currencyType)
        {
            return currencies[currenciesLink[currencyType]].Amount;
        }

        public static Currency GetCurrency(CurrencyType currencyType)
        {
            return currencies[currenciesLink[currencyType]];
        }

        public static void Set(CurrencyType currencyType, int amount)
        {
            Currency currency = currencies[currenciesLink[currencyType]];

            currency.Amount = amount;

            // Change save state to required
            SaveController.MarkAsSaveIsRequired();

            // Invoke currency change event
            currency.InvokeChangeEvent(0);
        }

        public static void Add(CurrencyType currencyType, int amount)
        {
            Currency currency = currencies[currenciesLink[currencyType]];

            currency.Amount += amount;

            // Change save state to required
            SaveController.MarkAsSaveIsRequired();

            // Invoke currency change event;
            currency.InvokeChangeEvent(amount);
        }

        public static void Substract(CurrencyType currencyType, int amount)
        {
            Currency currency = currencies[currenciesLink[currencyType]];

            currency.Amount -= amount;

            // Change save state to required
            SaveController.MarkAsSaveIsRequired();

            // Invoke currency change event
            currency.InvokeChangeEvent(-amount);
        }

        public static void SubscribeGlobalCallback(CurrencyChangeDelegate currencyChange)
        {
            for(int i = 0; i < currencies.Length; i++)
            {
                currencies[i].OnCurrencyChanged += currencyChange;
            }
        }

        public static void UnsubscribeGlobalCallback(CurrencyChangeDelegate currencyChange)
        {
            for (int i = 0; i < currencies.Length; i++)
            {
                currencies[i].OnCurrencyChanged -= currencyChange;
            }
        }
    }

    public delegate void CurrencyChangeDelegate(Currency currency, int difference);
}