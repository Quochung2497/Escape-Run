using UnityEngine;
using UnityEditor;
using System;

namespace Watermelon
{
    public static class EditorExtraMenus
    {
        [MenuItem("Help/Open Persistent Folder", priority = 151)]
        public static void OpenPersistentFolder()
        {
            EditorUtility.RevealInFinder(Application.persistentDataPath + "/" + Application.productName);
        }

        [MenuItem("Help/Open Project Folder", priority = 152)]
        public static void OpenProjectFolder()
        {
            EditorUtility.RevealInFinder(Application.dataPath.Replace("/Assets", "/"));
        }

        [MenuItem("Help/Open Editor Folder", priority = 153)]
        public static void OpenEditorFolder()
        {
            EditorUtility.RevealInFinder(EditorApplication.applicationPath);
        }

#if UNITY_EDITOR_WIN
        [MenuItem("Help/Open Register Path", priority = 154)]
        public static void OpenRegisterPath()
        {
            //Project location path
            string registryLocation = @"HKEY_CURRENT_USER\Software\Unity\UnityEditor\" + Application.companyName + @"\" + Application.productName + @"\";
            //Last location path
            string registryLastKey = @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Applets\Regedit";

            try
            {
                //Set LastKey value that regedit will go directly to
                Microsoft.Win32.Registry.SetValue(registryLastKey, "LastKey", registryLocation);
                System.Diagnostics.Process.Start("regedit.exe");
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
#endif
    }
}
