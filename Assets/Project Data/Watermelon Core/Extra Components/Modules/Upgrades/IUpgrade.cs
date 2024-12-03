using UnityEngine;

namespace Watermelon.Upgrades
{
    public interface IUpgrade
    {
        public UpgradeType UpgradeType { get; }
        public string Title { get; }
        public BaseUpgradeStage[] Upgrades { get; }

        public void Initialise();
        public void UpgradeStage();
    }
}