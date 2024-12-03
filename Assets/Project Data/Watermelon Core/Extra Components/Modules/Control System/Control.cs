using UnityEngine;

namespace Watermelon
{
    public static class Control
    {
        private static InputType inputType;
        public static InputType InputType => inputType;

        private static IControlBehavior currentControl;
        public static IControlBehavior CurrentControl => currentControl;

        public static void Initialise(InputType inputType)
        {
            Control.inputType = inputType;
        }

        public static void SetControl(IControlBehavior controlBehavior)
        {
            currentControl = controlBehavior;
        }

        public static void Enable()
        {
#if UNITY_EDITOR
            if(currentControl == null)
            {
                Debug.LogError("[Control]: Control behavior isn't set!");

                return;
            }
#endif

            currentControl.EnableControl();
        }

        public static void Disable()
        {
#if UNITY_EDITOR
            if (currentControl == null)
            {
                Debug.LogError("[Control]: Control behavior isn't set!");

                return;
            }
#endif

            currentControl.DisableControl();
        }
    }
}