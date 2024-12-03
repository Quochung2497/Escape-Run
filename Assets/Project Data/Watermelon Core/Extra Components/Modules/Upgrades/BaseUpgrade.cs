using System;
using UnityEngine;

namespace Watermelon.Upgrades
{
    public abstract class BaseUpgrade : ScriptableObject, IUpgrade
    {
        [SerializeField]
        protected UpgradeType upgradeType;
        public UpgradeType UpgradeType => upgradeType;

        [SerializeField]
        protected string title;
        public string Title => title;

        [SerializeField]
        protected Sprite icon;
        public Sprite Icon => icon;

        [SerializeField]
        protected Sprite disabledIcon;
        public Sprite DisabledIcon => disabledIcon;

        public int UpgradeLevel { get => save.UpgradeLevel; set => save.UpgradeLevel = value; }

        [NonSerialized]
        protected UpgradeSavableObject save;

        public abstract BaseUpgradeStage[] Upgrades { get; }
        public int UpgradesCount => Upgrades.Length;
        public bool IsMaxedOut => UpgradeLevel + 1 >= UpgradesCount;

        public BaseUpgradeStage CurrentStage => Upgrades[UpgradeLevel];
        public BaseUpgradeStage NextStage => Upgrades.Length > UpgradeLevel + 1 ? Upgrades[UpgradeLevel + 1] : null;

        public abstract void Initialise();
        public abstract void UpgradeStage();

        public event SimpleCallback OnUpgraded;

        protected void InvokeOnUpgraded()
        {
            OnUpgraded?.Invoke();
        }

        public int GetUpgradeCost(int level)
        {
            if (level >= Upgrades.Length)
                return -1;

            var upgradeStage = Upgrades[level];

            return upgradeStage.Price;
        }

        public virtual void SetSave(UpgradeSavableObject save)
        {
            this.save = save;
        }

        public Sprite GetPreviewImage(int level)
        {
            if (level >= Upgrades.Length)
                return null;

            var upgradeStage = Upgrades[level];

            return upgradeStage.PreviewSprite;
        }

        public virtual void ResetUpgrade()
        {
            UpgradeLevel = 0;

            InvokeOnUpgraded();
        }
    }
}