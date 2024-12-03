#pragma warning disable 0649

using UnityEngine;

namespace Watermelon
{
    [SetupTab("Init Settings", priority = 1, texture = "icon_puzzle")]
    [CreateAssetMenu(fileName = "Project Init Settings", menuName = "Settings/Project Init Settings")]
    [HelpURL("https://docs.google.com/document/d/1ORNWkFMZ5_Cc-BUgu9Ds1DjMjR4ozMCyr6p_GGdyCZk")]
    public class ProjectInitSettings : ScriptableObject
    {
        [SerializeField] InitModule[] initModules;
        public InitModule[] InitModules => initModules;

        public void Init(Initialiser initialiser)
        {
            for(int i = 0; i < initModules.Length; i++)
            {
                initModules[i].CreateComponent(initialiser);
            }
        }

        public void StartInit(Initialiser initialiser)
        {
            for (int i = 0; i < initModules.Length; i++)
            {
                initModules[i].StartInit(initialiser);
            }
        }
    }
}

// -----------------
// Initialiser v 0.4.2
// -----------------