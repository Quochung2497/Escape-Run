using UnityEngine;

namespace Watermelon
{
    public class SettingsNoAdsButton : SettingsButtonBase
    {
        public override bool IsActive()
        {
#if MODULE_IAP
            return AdsManager.IsForcedAdEnabled();
#else
            return false;
#endif
        }

        public override void OnClick()
        {
            IAPManager.BuyProduct(AdsManager.NO_ADS_PRODUCT_KEY);

            // Play button sound
            AudioController.PlaySound(AudioController.Sounds.buttonSound);

            settingsPanel.Hide(true);
            settingsPanel.InitPositions();
        }
    }
}

// -----------------
// Settings Panel v 0.3
// -----------------