using Cinemachine;
using System.Collections.Generic;
using UnityEngine;

namespace Watermelon
{
    [DefaultExecutionOrder(100)]
    public sealed partial class CameraController : MonoBehaviour
    {
        private const int ACTIVE_CAMERA_PRIORITY = 100;
        private const int UNACTIVE_CAMERA_PRIORITY = 0;

        private static CameraController cameraController;

        [SerializeField] CinemachineBrain cameraBrain;
        [SerializeField] CameraType firstCamera;

        [Space]
        [SerializeField] VirtualCameraCase[] virtualCameras;

        private static Dictionary<CameraType, int> virtualCamerasLink;

        private static Camera mainCamera;
        public static Camera MainCamera => mainCamera;

        private static Transform mainTarget;
        public static Transform MainTarget => mainTarget;

        private static VirtualCameraCase activeVirtualCamera;
        public static VirtualCameraCase ActiveVirtualCamera => activeVirtualCamera;

        private void Awake()
        {
            cameraController = this;

            // Get camera component
            mainCamera = GetComponent<Camera>();

            // Initialise cameras link
            virtualCamerasLink = new Dictionary<CameraType, int>();
            for(int i = 0; i < virtualCameras.Length; i++)
            {
                virtualCameras[i].Initialise();
                virtualCamerasLink.Add(virtualCameras[i].CameraType, i);
            }

            // Disable camera brain
            cameraController.cameraBrain.enabled = false;

            EnableCamera(firstCamera);
        }

        public static void SetMainTarget(Transform target)
        {
            // Link target
            mainTarget = target;

            cameraController.cameraBrain.enabled = false;

            for (int i = 0; i < cameraController.virtualCameras.Length; i++)
            {
                cameraController.virtualCameras[i].VirtualCamera.Follow = target;
                cameraController.virtualCameras[i].VirtualCamera.LookAt = target;
            }

            cameraController.cameraBrain.transform.position = target.position;
            cameraController.cameraBrain.enabled = true;
        }

        public static VirtualCameraCase GetCamera(CameraType cameraType)
        {
            return cameraController.virtualCameras[virtualCamerasLink[cameraType]];
        }

        public static void EnableCamera(CameraType cameraType)
        {
            if (activeVirtualCamera != null && activeVirtualCamera.CameraType == cameraType)
                return;

            for (int i = 0; i < cameraController.virtualCameras.Length; i++)
            {
                cameraController.virtualCameras[i].VirtualCamera.Priority = UNACTIVE_CAMERA_PRIORITY;
            }

            activeVirtualCamera = cameraController.virtualCameras[virtualCamerasLink[cameraType]];
            activeVirtualCamera.VirtualCamera.Priority = ACTIVE_CAMERA_PRIORITY;
        }
    }
}