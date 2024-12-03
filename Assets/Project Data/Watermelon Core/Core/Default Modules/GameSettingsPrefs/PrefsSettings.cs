using UnityEngine;

namespace Watermelon
{
    [CreateAssetMenu(fileName = "Prefs Settings", menuName = "Settings/Prefs Settings")]
    public partial class PrefsSettings : ScriptableObject
    {
        private static PrefsSettings prefsSettings;

        private const string PREFS_PREFIX = "settings_";
        private const string INCORRECT_TYPE_ERROR_MESSAGE = "Incorrect settings type";
        [SerializeField] SettingsContainer settings;

#if UNITY_EDITOR
        public static void InitEditor()
        {
            prefsSettings = RuntimeEditorUtils.GetAssetByName<PrefsSettings>("Prefs Settings");

            if (prefsSettings != null)
            {
                prefsSettings.Init();
            }
            else
            {
                Debug.LogError("[Prefs Settings]: Settings file is missing!");
            }
        }
#endif

        public void Init()
        {
            prefsSettings = this;

            Load();
        }

        public void Save()
        {
            SettingInfo settingInfo;

            foreach (Key key in System.Enum.GetValues(typeof(Key)))
            {
                settingInfo = GetSettingInfo(key);

                switch (settingInfo.fieldType)
                {
                    case FieldType.Bool:
                        SetBool(key, prefsSettings.settings.boolSettings[settingInfo.index].value);
                        break;
                    case FieldType.Float:
                        SetFloat(key, prefsSettings.settings.floatSettings[settingInfo.index].value);
                        break;
                    case FieldType.Int:
                        SetInt(key, prefsSettings.settings.intSettings[settingInfo.index].value);
                        break;
                    case FieldType.Long:
                        SetLong(key, prefsSettings.settings.longSettings[settingInfo.index].value);
                        break;
                    case FieldType.String:
                        SetString(key, prefsSettings.settings.stringSettings[settingInfo.index].value);
                        break;
                    case FieldType.DateTime:
                        SetDateTime(key, prefsSettings.settings.dateTimeSettings[settingInfo.index].value);
                        break;
                    case FieldType.Double:
                        SetDouble(key, prefsSettings.settings.doubleSettings[settingInfo.index].value);
                        break;
                }
            }
        }

        public void Load()
        {
            SettingInfo settingInfo;

            foreach (Key key in System.Enum.GetValues(typeof(Key)))
            {
                settingInfo = GetSettingInfo(key);

                if (HasKey(key))
                {
                    switch (settingInfo.fieldType)
                    {
                        case FieldType.Bool:
                            prefsSettings.settings.boolSettings[settingInfo.index].value = (bool)System.Convert.ChangeType(PlayerPrefs.GetString(PREFS_PREFIX + key, null), typeof(bool));
                            break;
                        case FieldType.Float:
                            prefsSettings.settings.floatSettings[settingInfo.index].value = PlayerPrefs.GetFloat(PREFS_PREFIX + key);
                            break;
                        case FieldType.Int:
                            prefsSettings.settings.intSettings[settingInfo.index].value = PlayerPrefs.GetInt(PREFS_PREFIX + key);
                            break;
                        case FieldType.Long:
                            prefsSettings.settings.longSettings[settingInfo.index].value = (long)System.Convert.ChangeType(PlayerPrefs.GetString(PREFS_PREFIX + key, null), typeof(long));
                            break;
                        case FieldType.String:
                            prefsSettings.settings.stringSettings[settingInfo.index].value = PlayerPrefs.GetString(PREFS_PREFIX + key);
                            break;
                        case FieldType.DateTime:
                            prefsSettings.settings.dateTimeSettings[settingInfo.index].value = PlayerPrefs.GetString(PREFS_PREFIX + key);
                            break;
                        case FieldType.Double:
                            prefsSettings.settings.doubleSettings[settingInfo.index].value = (double)System.Convert.ChangeType(PlayerPrefs.GetString(PREFS_PREFIX + key, null), typeof(double));
                            break;
                    }
                }
                else
                {
                    switch (settingInfo.fieldType)
                    {
                        case FieldType.Bool:
                            prefsSettings.settings.boolSettings[settingInfo.index].value = prefsSettings.settings.boolSettings[settingInfo.index].defaultValue;
                            break;
                        case FieldType.Float:
                            prefsSettings.settings.floatSettings[settingInfo.index].value = prefsSettings.settings.floatSettings[settingInfo.index].defaultValue;
                            break;
                        case FieldType.Int:
                            prefsSettings.settings.intSettings[settingInfo.index].value = prefsSettings.settings.intSettings[settingInfo.index].defaultValue;
                            break;
                        case FieldType.Long:
                            prefsSettings.settings.longSettings[settingInfo.index].value = prefsSettings.settings.longSettings[settingInfo.index].defaultValue;
                            break;
                        case FieldType.String:
                            prefsSettings.settings.stringSettings[settingInfo.index].value = prefsSettings.settings.stringSettings[settingInfo.index].defaultValue;
                            break;
                        case FieldType.DateTime:
                            prefsSettings.settings.dateTimeSettings[settingInfo.index].value = prefsSettings.settings.dateTimeSettings[settingInfo.index].defaultValue;
                            break;
                        case FieldType.Double:
                            prefsSettings.settings.doubleSettings[settingInfo.index].value = prefsSettings.settings.doubleSettings[settingInfo.index].defaultValue;
                            break;
                    }
                }
            }

        }
        private static SettingInfo GetSettingInfo(Key key)
        {
            return prefsSettings.settings.settingInfos[(int)key];
        }

        #region Set methods
        public static void SetBool(Key key, bool value)
        {
            SettingInfo settingInfo = GetSettingInfo(key);

            if (settingInfo.fieldType == FieldType.Bool)
            {
                prefsSettings.settings.boolSettings[settingInfo.index].value = value;
                PlayerPrefs.SetString(PREFS_PREFIX + key.ToString(), value.ToString());
            }
            else
            {
                throw new System.Exception(INCORRECT_TYPE_ERROR_MESSAGE);
            }
        }

        public static void SetFloat(Key key, float value)
        {
            SettingInfo settingInfo = GetSettingInfo(key);

            if (settingInfo.fieldType == FieldType.Float)
            {
                prefsSettings.settings.floatSettings[settingInfo.index].value = value;
                PlayerPrefs.SetFloat(PREFS_PREFIX + key.ToString(), value);
            }
            else
            {
                throw new System.Exception(INCORRECT_TYPE_ERROR_MESSAGE);
            }
        }

        public static void SetInt(Key key, int value)
        {
            SettingInfo settingInfo = GetSettingInfo(key);

            if (settingInfo.fieldType == FieldType.Int)
            {
                prefsSettings.settings.intSettings[settingInfo.index].value = value;
                PlayerPrefs.SetInt(PREFS_PREFIX + key.ToString(), value);
            }
            else
            {
                throw new System.Exception(INCORRECT_TYPE_ERROR_MESSAGE);
            }
        }

        public static void SetLong(Key key, long value)
        {
            SettingInfo settingInfo = GetSettingInfo(key);

            if (settingInfo.fieldType == FieldType.Long)
            {
                prefsSettings.settings.longSettings[settingInfo.index].value = value;
                PlayerPrefs.SetString(PREFS_PREFIX + key.ToString(), value.ToString());
            }
            else
            {
                throw new System.Exception(INCORRECT_TYPE_ERROR_MESSAGE);
            }
        }

        public static void SetString(Key key, string value)
        {
            SettingInfo settingInfo = GetSettingInfo(key);

            if (settingInfo.fieldType == FieldType.String)
            {
                prefsSettings.settings.stringSettings[settingInfo.index].value = value;
                PlayerPrefs.SetString(PREFS_PREFIX + key.ToString(), value);
            }
            else
            {
                throw new System.Exception(INCORRECT_TYPE_ERROR_MESSAGE);
            }
        }

        public static void SetDateTime(Key key, System.DateTime value)
        {
            SettingInfo settingInfo = GetSettingInfo(key);

            if (settingInfo.fieldType == FieldType.DateTime)
            {
                prefsSettings.settings.dateTimeSettings[settingInfo.index].value = value.ToString();
                PlayerPrefs.SetString(PREFS_PREFIX + key.ToString(), value.ToString());
            }
            else
            {
                throw new System.Exception(INCORRECT_TYPE_ERROR_MESSAGE);
            }
        }

        public static void SetDateTime(Key key, string value)
        {
            SettingInfo settingInfo = GetSettingInfo(key);

            if (settingInfo.fieldType == FieldType.DateTime)
            {
                prefsSettings.settings.dateTimeSettings[settingInfo.index].value = value;
                PlayerPrefs.SetString(PREFS_PREFIX + key.ToString(), value);
            }
            else
            {
                throw new System.Exception(INCORRECT_TYPE_ERROR_MESSAGE);
            }
        }

        public static void SetDouble(Key key, double value)
        {
            SettingInfo settingInfo = GetSettingInfo(key);

            if (settingInfo.fieldType == FieldType.Double)
            {
                prefsSettings.settings.doubleSettings[settingInfo.index].value = value;
                PlayerPrefs.SetString(PREFS_PREFIX + key.ToString(), value.ToString());
            }
            else
            {
                throw new System.Exception(INCORRECT_TYPE_ERROR_MESSAGE);
            }
        }

        #endregion

        #region Get methods
        public static bool GetBool(Key key)
        {
            SettingInfo settingInfo = GetSettingInfo(key);

            if (settingInfo.fieldType == FieldType.Bool)
            {
                return prefsSettings.settings.boolSettings[settingInfo.index].value;
            }
            else
            {
                throw new System.Exception(INCORRECT_TYPE_ERROR_MESSAGE);
            }
        }

        public static float GetFloat(Key key)
        {
            SettingInfo settingInfo = GetSettingInfo(key);

            if (settingInfo.fieldType == FieldType.Float)
            {
                return prefsSettings.settings.floatSettings[settingInfo.index].value;
            }
            else
            {
                throw new System.Exception(INCORRECT_TYPE_ERROR_MESSAGE);
            }
        }

        public static int GetInt(Key key)
        {
            SettingInfo settingInfo = GetSettingInfo(key);

            if (settingInfo.fieldType == FieldType.Int)
            {
                return prefsSettings.settings.intSettings[settingInfo.index].value;
            }
            else
            {
                throw new System.Exception(INCORRECT_TYPE_ERROR_MESSAGE);
            }
        }

        public static long GetLong(Key key)
        {
            SettingInfo settingInfo = GetSettingInfo(key);

            if (settingInfo.fieldType == FieldType.Long)
            {
                return prefsSettings.settings.longSettings[settingInfo.index].value;
            }
            else
            {
                throw new System.Exception(INCORRECT_TYPE_ERROR_MESSAGE);
            }
        }

        public static string GetString(Key key)
        {
            SettingInfo settingInfo = GetSettingInfo(key);

            if (settingInfo.fieldType == FieldType.String)
            {
                return prefsSettings.settings.stringSettings[settingInfo.index].value;
            }
            else
            {
                throw new System.Exception(INCORRECT_TYPE_ERROR_MESSAGE);
            }
        }

        public static System.DateTime GetDateTime(Key key)
        {
            SettingInfo settingInfo = GetSettingInfo(key);

            if (settingInfo.fieldType == FieldType.DateTime)
            {
                return System.DateTime.Parse(prefsSettings.settings.dateTimeSettings[settingInfo.index].value);
            }
            else
            {
                throw new System.Exception(INCORRECT_TYPE_ERROR_MESSAGE);
            }
        }

        public static double GetDouble(Key key)
        {
            SettingInfo settingInfo = GetSettingInfo(key);

            if (settingInfo.fieldType == FieldType.Double)
            {
                return prefsSettings.settings.doubleSettings[settingInfo.index].value;
            }
            else
            {
                throw new System.Exception(INCORRECT_TYPE_ERROR_MESSAGE);
            }
        }
        #endregion

        public static bool HasKey(Key key)
        {
            return PlayerPrefs.HasKey(PREFS_PREFIX + key);
        }

        public static void RemoveKey(Key key)
        {
            PlayerPrefs.DeleteKey(PREFS_PREFIX + key);
        }

        [System.Serializable]
        private class SettingsContainer
        {
            public bool needsUpdate;
            public SettingInfo[] settingInfos;
            public BoolSetting[] boolSettings;
            public FloatSetting[] floatSettings;
            public IntSetting[] intSettings;
            public LongSetting[] longSettings;
            public StringSetting[] stringSettings;
            public DateTimeSetting[] dateTimeSettings;
            public DoubleSetting[] doubleSettings;
        }

        [System.Serializable]
        private class SettingInfo
        {
            public Key key; //key can be used as index in settingInfos array
            public FieldType fieldType; // points at value array
            public int index;// index in value array
        }

        [System.Serializable]
        private abstract class GenericSetting<T>
        {
            public T defaultValue;
            public T value;
        }

        [System.Serializable]
        private class BoolSetting : GenericSetting<bool> { }

        [System.Serializable]
        private class FloatSetting : GenericSetting<float> { }

        [System.Serializable]
        private class IntSetting : GenericSetting<int> { }

        [System.Serializable]
        private class LongSetting : GenericSetting<long> { }

        [System.Serializable]
        private class StringSetting : GenericSetting<string> { }

        [System.Serializable]
        private class DateTimeSetting : GenericSetting<string> { }

        [System.Serializable]
        private class DoubleSetting : GenericSetting<double> { }

        public enum FieldType
        {
            Bool = 0,
            Float = 1,
            Int = 2,
            Long = 3,
            String = 4,
            DateTime = 5,
            Double = 6
        }
    }
}

// -----------------
// Prefs Settings v1.0
// -----------------

// Changelog
// v 1.0
// • Basic version