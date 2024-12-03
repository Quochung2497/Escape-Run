using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Watermelon
{
    public class WebGLSaveWrapper : BaseSaveWrapper
    {
        public override GlobalSave Load(string fileName)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            string jsonObject = load(fileName);
            if(!string.IsNullOrEmpty(jsonObject))
            {
                try
                {
                    GlobalSave deserializedObject = JsonUtility.FromJson<GlobalSave>(jsonObject);

                    return deserializedObject;
                }
                catch (System.Exception ex)
                {
                    Debug.LogError(ex.Message);
                }
            }
#endif

            return new GlobalSave();
        }

        public override void Save(GlobalSave globalSave, string fileName)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            string jsonObject = JsonUtility.ToJson(globalSave);

            save(fileName, jsonObject);
#endif
        }

        public override void Delete(string fileName)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            deleteItem(fileName);
#endif
        }

#if UNITY_WEBGL && !UNITY_EDITOR
        [DllImport("__Internal")]
        private static extern string load(string keyName);

        [DllImport("__Internal")]
        private static extern void save(string keyName, string data);

        [DllImport("__Internal")]
        private static extern void deleteItem(string keyName);
#endif
    }
}
