using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace Watermelon {
    public class AdmobBuildHandler : IPreprocessBuildWithReport
    {
        public int callbackOrder => -5;
        private const string SETTINGS_FILE_PATH = "Assets/GoogleMobileAds/Resources/GoogleMobileAdsSettings.asset";
        private const string ANDROID_APP_ID_PROPERTY_PATH = "adMobAndroidAppId";
        private const string IOS_APP_ID_PROPERTY_PATH = "adMobIOSAppId";
        private const string OUR_ANDROID_APP_ID_PROPERTY_PATH = "androidAppId";
        private const string OUR_IOS_APP_ID_PROPERTY_PATH = "iosAppId";
        private const string ADMOB_CONTAINER_PROPERTY_PATH = "adMobContainer";

        public void OnPreprocessBuild(BuildReport report)
        {
#if MODULE_ADMOB
            AdsData adsData = EditorUtils.GetAsset<AdsData>();

            if(adsData == null)
            {
                Debug.LogError("AdsData don`t exist.");
                return;
            }

            Object settingsFile = AssetDatabase.LoadMainAssetAtPath(SETTINGS_FILE_PATH);

            if(settingsFile == null)
            {
                GoogleMobileAds.Editor.GoogleMobileAdsSettingsEditor.OpenInspector(); // Creates addmob settings file
                settingsFile = AssetDatabase.LoadMainAssetAtPath(SETTINGS_FILE_PATH);

                if(settingsFile == null)
                {
                    Debug.LogError("Failed to create instance of GoogleMobileAdsSettings.");
                }
            }

            SerializedObject adsDataObject = new SerializedObject(adsData);
            SerializedObject settingsObject = new SerializedObject(settingsFile);
            settingsObject.FindProperty(ANDROID_APP_ID_PROPERTY_PATH);
            settingsObject.FindProperty(IOS_APP_ID_PROPERTY_PATH);

            bool changed = false;

            if (settingsObject.FindProperty(ANDROID_APP_ID_PROPERTY_PATH).stringValue.Length == 0)
            {
                if (adsDataObject.FindProperty(ADMOB_CONTAINER_PROPERTY_PATH).FindPropertyRelative(OUR_ANDROID_APP_ID_PROPERTY_PATH).stringValue.Length != 0)
                {
                    settingsObject.FindProperty(ANDROID_APP_ID_PROPERTY_PATH).stringValue = adsDataObject.FindProperty(ADMOB_CONTAINER_PROPERTY_PATH).FindPropertyRelative(OUR_ANDROID_APP_ID_PROPERTY_PATH).stringValue;
                    changed = true;
                }
            }

            if (settingsObject.FindProperty(IOS_APP_ID_PROPERTY_PATH).stringValue.Length == 0)
            {
                if (adsDataObject.FindProperty(ADMOB_CONTAINER_PROPERTY_PATH).FindPropertyRelative(OUR_IOS_APP_ID_PROPERTY_PATH).stringValue.Length != 0)
                {
                    settingsObject.FindProperty(IOS_APP_ID_PROPERTY_PATH).stringValue = adsDataObject.FindProperty(ADMOB_CONTAINER_PROPERTY_PATH).FindPropertyRelative(OUR_IOS_APP_ID_PROPERTY_PATH).stringValue;
                    changed = true;
                }
            }

            if (changed)
            {
                settingsObject.ApplyModifiedProperties();
            }

#endif
        }
    }
}
