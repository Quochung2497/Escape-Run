using UnityEngine;

namespace Watermelon
{
    public abstract class FloatingTextBaseBehaviour : MonoBehaviour
    {
        public abstract void Activate(string text, float scale = 1.0f);
    }
}