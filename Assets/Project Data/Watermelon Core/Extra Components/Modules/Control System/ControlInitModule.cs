using UnityEngine;

namespace Watermelon
{
    [RegisterModule("Modules/Control Manager")]
    public class ControlInitModule : InitModule
    {
        [SerializeField] bool selectAutomatically = true;

        [HideIf("selectAutomatically")]
        [SerializeField] InputType inputType;

        public ControlInitModule()
        {
            moduleName = "Control Manager";
        }

        public override void CreateComponent(Initialiser Initialiser)
        {
            if (selectAutomatically)
                inputType = ControlUtils.GetCurrentInputType();

            Control.Initialise(inputType);

            if(inputType == InputType.Keyboard)
            {
                KeyboardControl keyboardControl = Initialiser.InitialiserGameObject.AddComponent<KeyboardControl>();
                keyboardControl.Initialise();
            }
        }
    }
}