#pragma warning disable 0649 

using UnityEngine;
using UnityEngine.UI;

namespace Watermelon
{
    public class SettingsVibrationToggleButton : SettingsButtonBase
    {
        [SerializeField] Image imageRef;

        [Space]
        [SerializeField] Sprite activeSprite;
        [SerializeField] Sprite disableSprite;

        private bool isActive = true;

        private void Start()
        {
            isActive = AudioController.IsVibrationEnabled();

            if (isActive)
                imageRef.sprite = activeSprite;
            else
                imageRef.sprite = disableSprite;
        }

        public override bool IsActive()
        {
            return true;
        }

        public override void OnClick()
        {
            isActive = !isActive;

            if (isActive)
            {
                imageRef.sprite = activeSprite;

                AudioController.SetVibrationState(true);
            }
            else
            {
                imageRef.sprite = disableSprite;

                AudioController.SetVibrationState(false);
            }

            // Play button sound
            AudioController.PlaySound(AudioController.Sounds.buttonSound);
        }
    }
}

// -----------------
// Settings Panel v 0.3
// -----------------