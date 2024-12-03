using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class AdjustCameraOffSet : MonoBehaviour
{
    [Header("Camera Settings")]
    [SerializeField] private CinemachineVirtualCamera virtualCamera;

    [Header("Offset Values")]
    [SerializeField] private Vector3 defaultOffset = new Vector3(0, 50, -60);
    [SerializeField] private Vector3 obstacleOffset = new Vector3(0, 50, -80);
    [SerializeField] private Vector3 boundaryOffset = new Vector3(0, 50, -50);
    [SerializeField] private float zoomSpeed = 10f;

    [Header("Detection Settings")]
    [SerializeField] private LayerMask obstacleLayer;
    [SerializeField] private float rayDistance = 5f;
    [SerializeField] private Transform playerTransform;

    private CinemachineTransposer transposer;
    private Vector3 currentOffsetVelocity = Vector3.zero;

    void Start()
    {
        if (virtualCamera != null)
        {
            transposer = virtualCamera.GetCinemachineComponent<CinemachineTransposer>();
            if (transposer != null)
            {
                transposer.m_FollowOffset = defaultOffset;
            }
        }
    }
    void Update()
    {
        if (transposer == null || playerTransform == null) return;

        if (IsObstacleDetected())
        {
            SetCameraOffset(obstacleOffset);
        }
        else if (PlayerController.instance.IsAtBoundary())
        {
            SetCameraOffset(boundaryOffset);
        }
        else
        {
            SetCameraOffset(defaultOffset);
        }
    }
    private void SetCameraOffset(Vector3 offset)
    {
        if (transposer.m_FollowOffset != offset)
        {
            transposer.m_FollowOffset = Vector3.SmoothDamp(transposer.m_FollowOffset, offset, ref currentOffsetVelocity,0.2f);
        }
    }
    private bool IsObstacleDetected()
    {
        Ray ray = new Ray(playerTransform.position, playerTransform.forward);
        return Physics.Raycast(ray, rayDistance, obstacleLayer);
    }
}
