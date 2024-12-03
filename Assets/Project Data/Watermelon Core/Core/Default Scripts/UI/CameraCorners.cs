using UnityEngine;

namespace Watermelon
{
    public class CameraCorners
    {
        private float left;
        public float Left
        {
            get { return left; }
        }

        private float right;
        public float Right
        {
            get { return right; }
        }

        private float top;
        public float Top
        {
            get { return top; }
        }

        private float bottom;
        public float Bottom
        {
            get { return bottom; }
        }

        public float Width
        {
            get { return right - left; }
        }

        public float Height
        {
            get { return top - bottom; }
        }

        public CameraCorners(Camera camera)
        {
            Vector3 leftPoint = camera.ScreenToWorldPoint(new Vector3(0, 0, 0));
            Vector3 rightPoint = camera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));

            left = leftPoint.x;
#if UNITY_EDITOR
            top = Camera.main.orthographicSize;
#else
            top = leftPoint.y;
#endif

#if UNITY_EDITOR
            right = Camera.main.orthographicSize * 2 * Camera.main.aspect * 0.5f;
#else
            right = rightPoint.x;
#endif
            bottom = rightPoint.y;
        }
    }
}