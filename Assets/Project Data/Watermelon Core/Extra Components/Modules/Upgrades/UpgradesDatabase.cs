using UnityEngine;

namespace Watermelon.Upgrades
{
    [CreateAssetMenu(fileName = "Upgrades Database", menuName = "Content/Upgrades/Upgrades Database")]
    public class UpgradesDatabase : ScriptableObject
    {
        [SerializeField] BaseUpgrade[] upgrades;
        public BaseUpgrade[] Upgrades => upgrades;
    }
}