using UnityEngine;

namespace Watermelon.Upgrades
{
    public abstract class Upgrade<T> : BaseUpgrade where T : BaseUpgradeStage
    {
        [SerializeField]
        protected T[] upgrades;
        public override BaseUpgradeStage[] Upgrades => upgrades;

        public T GetCurrentStage()
        {
            if (upgrades.IsInRange(UpgradeLevel))
                return upgrades[UpgradeLevel];

            UpgradeLevel = upgrades.Length - 1;
            Debug.Log("[Perks]: Perk level is out of range!");

            return upgrades[UpgradeLevel];
        }

        public T GetNextStage()
        {
            if (upgrades.IsInRange(UpgradeLevel + 1))
                return upgrades[UpgradeLevel + 1];

            return null;
        }

        public override void UpgradeStage()
        {
            if (upgrades.IsInRange(UpgradeLevel + 1))
            {
                UpgradeLevel += 1;

                InvokeOnUpgraded();
            }
        }

        public T GetStage(int i)
        {
            if (upgrades.IsInRange(i))
                return upgrades[i];
            return null;
        }

        [Button("Info")]
        public void TestInfo()
        {
            Debug.Log(upgradeType + ": " + UpgradeLevel);
        }

        [Button("Upgrade")]
        public void TestUpgrade()
        {
            UpgradeStage();
        }

        [Button("Max")]
        public virtual void TestMaxUpgrade()
        {
            UpgradeLevel = upgrades.Length - 1;

            InvokeOnUpgraded();
        }

        [Button("Reset")]
        public override void ResetUpgrade()
        {
            UpgradeLevel = 0;
            InvokeOnUpgraded();
        }
    }
}