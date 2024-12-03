using UnityEngine;

namespace Watermelon.Upgrades
{
    [System.Serializable]
    public class UpgradeSavableObject : ISaveObject
    {
        [SerializeField] int upgradeLevel;
        public int UpgradeLevel { get => upgradeLevel; set => upgradeLevel = value; }

        public UpgradeSavableObject(int upgradeLevel)
        {
            this.upgradeLevel = upgradeLevel;
        }

        public UpgradeSavableObject()
        {
            upgradeLevel = 0;
        }

        public virtual void Flush() { }
    }

}