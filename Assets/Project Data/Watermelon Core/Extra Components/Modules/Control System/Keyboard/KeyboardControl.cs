using UnityEngine;

namespace Watermelon
{
    public class KeyboardControl : MonoBehaviour, IControlBehavior
    {
        private Vector3 formatInput;
        public Vector3 FormatInput => formatInput;

        private bool isInputActive;
        public bool IsInputActive => isInputActive;

        private bool isEnabled;

        public event SimpleCallback OnInputActivated;

        public void Initialise()
        {
            if (Control.InputType == InputType.Keyboard)
            {
                Control.SetControl(this);

                enabled = true;
                isEnabled = true;
            }
            else
            {
                enabled = false;
            }
        }

        public void DisableControl()
        {
            isEnabled = false;
        }

        public void EnableControl()
        {
            isEnabled = true;
        }

        private void Update()
        {
            if (!isEnabled) return;

            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");

            formatInput = Vector3.ClampMagnitude(new Vector3(horizontalInput, 0, verticalInput), 1);

            if(!isInputActive && formatInput.magnitude > 0.1f)
            {
                isInputActive = true;

                OnInputActivated?.Invoke();
            }

            isInputActive = formatInput.magnitude > 0.1f;
        }

        public void ResetControl()
        {
            isInputActive = false;
            formatInput = Vector3.zero;
        }
    }
}
