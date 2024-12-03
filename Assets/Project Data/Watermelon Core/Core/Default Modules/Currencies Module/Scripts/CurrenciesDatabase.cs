using UnityEngine;

namespace Watermelon
{
    [CreateAssetMenu(fileName = "Currencies Database", menuName = "Content/Currencies/Currencies Database")]
    public class CurrenciesDatabase : ScriptableObject
    {
        [SerializeField] Currency[] currencies;
        public Currency[] Currencies => currencies;

        public void Initialise()
        {
            // Initialise currencies
            for(int i = 0; i < currencies.Length; i++)
            {
                currencies[i].Initialise();
            }
        }
    }
}