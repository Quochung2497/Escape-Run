using System.Collections.Generic;
using UnityEngine;

namespace Watermelon
{
    public class CurrenciesUIController : MonoBehaviour
    {
        private const float DISALBE_PANEL_IN_SECONDS = 10.0f;

        [SerializeField] StaticCurrencyUI[] staticPanels;

        [SerializeField] GameObject panelObject;
        [SerializeField] Transform parentTrasnform;

        private Pool panelPool;

        private Dictionary<CurrencyType, CurrencyUI> activePanelsUI;

        public void Initialise(Currency[] currencies)
        {
            panelPool = new Pool(new PoolSettings("Currency Panel", panelObject, 1, true, parentTrasnform));

            activePanelsUI = new Dictionary<CurrencyType, CurrencyUI>();

            for(int i = 0; i < staticPanels.Length; i++)
            {
                Currency currency = CurrenciesController.GetCurrency(staticPanels[i].CurrencyType);

                CurrencyUI currencyUI = staticPanels[i].CurrencyPanel;
                currencyUI.Initialise(currency);
                currencyUI.Show();

                activePanelsUI.Add(staticPanels[i].CurrencyType, staticPanels[i].CurrencyPanel);
            }

            for (int i = 0; i < currencies.Length; i++)
            {
                if (!activePanelsUI.ContainsKey(currencies[i].CurrencyType) && (currencies[i].DisplayAlways || currencies[i].Amount > 0))
                {
                    GameObject currencyObject = panelPool.GetPooledObject();
                    currencyObject.transform.SetParent(parentTrasnform);
                    currencyObject.transform.ResetLocal();
                    currencyObject.transform.SetAsLastSibling();
                    currencyObject.SetActive(true);

                    CurrencyUI currencyUI = currencyObject.GetComponent<CurrencyUI>();
                    currencyUI.Initialise(currencies[i]);
                    currencyUI.Show();

                    activePanelsUI.Add(currencies[i].CurrencyType, currencyUI);
                }

                currencies[i].OnCurrencyChanged += RedrawCurrency;
            }
        }

        public CurrencyUI GetCurrencyUI(CurrencyType type)
        {
            if (activePanelsUI.ContainsKey(type))
            {
                return activePanelsUI[type];
            }
            else
            {
                return ActivateCurrency(type);
            }
        }

        public void ActivateAllExistingCurrencies()
        {
            Currency[] activeCurrencies = CurrenciesController.Currencies;
            for (int i = 0; i < activeCurrencies.Length; i++)
            {
                if (activeCurrencies[i].Amount > 0)
                    ActivateCurrency(activeCurrencies[i].CurrencyType);
            }
        }

        public void RedrawCurrency(Currency currency, int amount)
        {
            CurrencyType type = currency.CurrencyType;

            if (activePanelsUI.ContainsKey(type))
            {
                activePanelsUI[type].Redraw();

                if(amount == 0)
                {
                    if (!currency.DisplayAlways)
                    {
                        activePanelsUI[type].DisableAfter(DISALBE_PANEL_IN_SECONDS, delegate
                        {
                            if (activePanelsUI.ContainsKey(type))
                                activePanelsUI.Remove(type);
                        });
                    }
                }
                else
                {
                    activePanelsUI[type].KillDisable();
                }
            }
            else
            {
                ActivateCurrency(type);
            }
        }

        // doNotHide will prevent currency from auto hide - call DisableCurrency to hide it manually
        public CurrencyUI ActivateCurrency(CurrencyType type, bool doNotHide = false)
        {
            // Check if panel is disabled
            if (!activePanelsUI.ContainsKey(type))
            {
                // Get object from pool
                GameObject currencyObject = panelPool.GetPooledObject();
                currencyObject.transform.SetParent(parentTrasnform);
                currencyObject.transform.ResetLocal();
                currencyObject.transform.SetAsLastSibling();
                currencyObject.SetActive(true);

                // Get currency from database
                Currency currency = CurrenciesController.GetCurrency(type);

                // Get UI panel component
                CurrencyUI currencyUI = currencyObject.GetComponent<CurrencyUI>();
                currencyUI.Initialise(currency);
                currencyUI.Show();

                // Check if panel require disable
                if (!currency.DisplayAlways && !doNotHide)
                {
                    currencyUI.DisableAfter(DISALBE_PANEL_IN_SECONDS, delegate
                    {
                        if (activePanelsUI.ContainsKey(type))
                            activePanelsUI.Remove(type);
                    });
                }

                activePanelsUI.Add(type, currencyUI);

                return currencyUI;
            }
            // Rewrite panel state
            else
            {
                CurrencyUI currencyUI = activePanelsUI[type];

                // Check if panel require disable reset
                if (!currencyUI.Currency.DisplayAlways)
                {
                    currencyUI.ShowImmediately();
                    currencyUI.ResetDisable();

                    if (doNotHide)
                        currencyUI.KillDisable();
                }

                // Redraw
                currencyUI.Redraw();

                return currencyUI;
            }
        }

        public void DisableCurrency(CurrencyType type)
        {
            // check if panel is active state
            if (activePanelsUI.ContainsKey(type) && !CurrenciesController.GetCurrency(type).DisplayAlways)
            {
                activePanelsUI[type].Hide();
                activePanelsUI.Remove(type);
            }
        }

        [System.Serializable]
        private class StaticCurrencyUI
        {
            public CurrencyType CurrencyType;
            public CurrencyUI CurrencyPanel;
        }
    }
}