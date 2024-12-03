using UnityEditor;
using UnityEditor.Callbacks;
#if UNITY_IOS
using UnityEditor.iOS.Xcode;
#endif

using System.IO;
using System.Collections.Generic;

namespace Watermelon
{
    public static class IOSBuildPostProcessor
    {
        [PostProcessBuild]
        public static void ChangeXcodePlist(BuildTarget buildTarget, string pathToBuiltProject)
        {
#if UNITY_IOS
        if (buildTarget == BuildTarget.iOS)
        {
            // Get plist
            string plistPath = pathToBuiltProject + "/Info.plist";
            PlistDocument plist = new PlistDocument();
            plist.ReadFromString(File.ReadAllText(plistPath));

            // Get root
            PlistElementDict rootDict = plist.root;

            // Path info.plist
            rootDict.SetString("ITSAppUsesNonExemptEncryption", "NO");

            // Write to file
            File.WriteAllText(plistPath, plist.WriteToString());
        }
#endif
        }
    }
}