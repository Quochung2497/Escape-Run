using UnityEngine;

namespace Watermelon
{
    [CreateAssetMenu(fileName = "Define Settings", menuName = "Settings/Editor/Define Settings")]
    [HelpURL("https://docs.google.com/document/d/1uGv7AewHFS5ONmfSaZrGAxajTERczRw2PDVwgsO12AU")]
    public class DefinesSettings : ScriptableObject
    {
        public static readonly string[] STATIC_DEFINES = new string[]
        {
            "UNITY_POST_PROCESSING_STACK_V2",
            "PHOTON_UNITY_NETWORKING",
            "PUN_2_0_OR_NEWER",
            "PUN_2_OR_NEWER",
        };

        public string[] customDefines;
    }
}

// -----------------
// Define Manager v 0.2.1
// -----------------