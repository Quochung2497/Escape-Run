#pragma warning disable 0649 

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Watermelon
{
    public class GDPRPanel : MonoBehaviour
    {
        [SerializeField] GameObject termsOfUseObject;
        [SerializeField] GameObject privacyPolicyObject;

        [SerializeField] Button acceptButton;

        private GDPRLoadingTask gdprLoadingTask;

        public void Initialise(GDPRLoadingTask gdprLoadingTask)
        {
            this.gdprLoadingTask = gdprLoadingTask;

            // Inititalise panel
            EventTrigger termsTrigger = termsOfUseObject.AddComponent<EventTrigger>();

            EventTrigger.Entry termsEntry = new EventTrigger.Entry();
            termsEntry.eventID = EventTriggerType.PointerDown;
            termsEntry.callback.AddListener((eventData) => { OpenTermsOfUseLinkButton(); });
            termsTrigger.triggers.Add(termsEntry);

            EventTrigger privacyTrigger = privacyPolicyObject.AddComponent<EventTrigger>();

            EventTrigger.Entry privacyEntry = new EventTrigger.Entry();
            privacyEntry.eventID = EventTriggerType.PointerDown;
            privacyEntry.callback.AddListener((eventData) => { OpenPrivacyLinkButton(); });
            privacyTrigger.triggers.Add(privacyEntry);

            acceptButton.onClick.AddListener(() => SetGDPRStateButton(false));

            DontDestroyOnLoad(gameObject);

            gameObject.SetActive(true);
        }

        public void OpenPrivacyLinkButton()
        {
            Application.OpenURL(AdsManager.Settings.PrivacyLink);
        }

        public void OpenTermsOfUseLinkButton()
        {
            Application.OpenURL(AdsManager.Settings.TermsOfUseLink);
        }

        public void SetGDPRStateButton(bool state)
        {
            AdsManager.SetGDPR(state);

            CloseWindow();

            gdprLoadingTask.CompleteTask();
        }

        public void CloseWindow()
        {
            gameObject.SetActive(false);
        }
    }
}

// -----------------
// Advertisement v 1.3
// -----------------