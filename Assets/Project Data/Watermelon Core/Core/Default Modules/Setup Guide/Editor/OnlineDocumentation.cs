using UnityEngine;
using System.IO;
using UnityEditor;

namespace Watermelon
{
    public class OnlineDocumentation
    {
        private readonly string template = "=====================================================================\n Thank you for using our template!\n Your experience is important for us. Feel free to leave a review or\n join our Discord community - https://discord.gg/xEGUnBg \n=====================================================================\n\n𝗢𝗡𝗟𝗜𝗡𝗘 𝗗𝗢𝗖𝗨𝗠𝗘𝗡𝗧𝗔𝗧𝗜𝗢𝗡 - {0}";

        private string documentationURL;

        public OnlineDocumentation(string documentationURL)
        {
            this.documentationURL = documentationURL;
        }

        public void SaveToFile()
        {
            string documentationString = string.Format(template, documentationURL);

            File.WriteAllText(Application.dataPath + "/Project Data/DOCUMENTATION.txt", documentationString);

            Debug.Log("Documentation file successfully saved to the Project Data folder");

            AssetDatabase.Refresh();

            Selection.activeObject = null;
        }
    }
}

// -----------------
// Setup Guide v 1.0.2
// -----------------