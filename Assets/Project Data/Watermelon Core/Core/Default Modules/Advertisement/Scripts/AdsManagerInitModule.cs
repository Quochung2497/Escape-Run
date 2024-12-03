using System.Collections;
using UnityEngine;

#if UNITY_IOS
using Unity.Advertisement.IosSupport;
#endif

namespace Watermelon
{
    [RegisterModule("Services/Ads Manager")]
    public class AdsManagerInitModule : InitModule
    {
        public AdsData settings;
        public GameObject dummyCanvasPrefab;
        public GameObject gdprPrefab;

        [Space]
        public bool loadAdOnStart = true;

        public AdsManagerInitModule()
        {
            moduleName = "Ads Manager";
        }

        public override void CreateComponent(Initialiser Initialiser)
        {
            AdsManager.Init(this, loadAdOnStart);
        }

        public override void StartInit(Initialiser Initialiser)
        {
#if UNITY_IOS
            if (settings.IsIDFAEnabled && !AdsManager.IsIDFADetermined())
            {
                if (settings.SystemLogs)
                    Debug.Log("[Ads Manager]: Requesting IDFA..");

                ATTrackingStatusBinding.RequestAuthorizationTracking();
            }
#endif
        }
    }
}

// -----------------
// Advertisement v 1.3
// -----------------