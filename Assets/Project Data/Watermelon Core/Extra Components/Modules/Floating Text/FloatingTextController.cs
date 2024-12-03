using System.Collections.Generic;
using UnityEngine;

namespace Watermelon
{
    public class FloatingTextController : MonoBehaviour
    {
        private static FloatingTextController floatingTextController;

        [SerializeField] FloatingTextCase[] floatingTextCases;
        private Dictionary<int, FloatingTextCase> floatingTextLink;

        public void Inititalise()
        {
            floatingTextController = this;

            floatingTextLink = new Dictionary<int, FloatingTextCase>();
            for (int i = 0; i < floatingTextCases.Length; i++)
            {
                floatingTextCases[i].Initialise();

                floatingTextLink.Add(floatingTextCases[i].Name.GetHashCode(), floatingTextCases[i]);
            }
        }

        public static FloatingTextBaseBehaviour SpawnFloatingText(string floatingTextName, string text, Vector3 position, Quaternion rotation, float scale)
        {
            return SpawnFloatingText(floatingTextName.GetHashCode(), text, position, rotation, scale);
        }

        public static FloatingTextBaseBehaviour SpawnFloatingText(int floatingTextNameHash, string text, Vector3 position, Quaternion rotation, float scale)
        {
            if (floatingTextController.floatingTextLink.ContainsKey(floatingTextNameHash))
            {
                FloatingTextCase floatingTextCase = floatingTextController.floatingTextLink[floatingTextNameHash];

                GameObject floatingTextObject = floatingTextCase.FloatingTextPool.GetPooledObject();
                floatingTextObject.transform.position = position;
                floatingTextObject.transform.rotation = rotation;
                floatingTextObject.SetActive(true);

                FloatingTextBaseBehaviour floatingTextBehaviour = floatingTextObject.GetComponent<FloatingTextBaseBehaviour>();
                floatingTextBehaviour.Activate(text, scale);

                return floatingTextBehaviour;
            }

            return null;
        }

        public static void Unload()
        {
            FloatingTextCase[] floatingTextCases = floatingTextController.floatingTextCases;
            for(int i = 0; i < floatingTextCases.Length; i++)
            {
                floatingTextCases[i].FloatingTextPool.ReturnToPoolEverything(true);
            }
        }

        public static int GetHash(string textStyleName)
        {
            return textStyleName.GetHashCode();
        }
    }
}