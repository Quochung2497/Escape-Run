#pragma warning disable 0649 

using UnityEngine;

namespace Watermelon
{
    public class SettingsLinkButton : SettingsButtonBase
    {
        [SerializeField] bool isActive = true;
        [SerializeField] string url;

        public override bool IsActive()
        {
            return isActive;
        }

        public override void OnClick()
        {
            Application.OpenURL(url);

            // Play button sound
            AudioController.PlaySound(AudioController.Sounds.buttonSound);
        }
    }
}

// -----------------
// Settings Panel v 0.3
// -----------------