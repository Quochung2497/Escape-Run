using UnityEngine;

namespace Watermelon
{
    [RegisterModule("Modules/Floating Message")]
    public class FloatingMessageInitModule : InitModule
    {
        [SerializeField] GameObject canvas;

        public override void CreateComponent(Initialiser Initialiser)
        {
            GameObject canvasGameObject = Instantiate(canvas);
            canvasGameObject.transform.SetParent(Initialiser.InitialiserGameObject.transform);
            canvasGameObject.transform.localScale = Vector3.one;
            canvasGameObject.transform.localPosition = Vector3.zero;
            canvasGameObject.transform.localRotation = Quaternion.identity;
            canvasGameObject.GetComponent<FloatingMessage>().Init();
        }

        public FloatingMessageInitModule()
        {
            moduleName = "Floating Message";
        }
    }
}

// -----------------
// Floating Message v 0.1
// -----------------