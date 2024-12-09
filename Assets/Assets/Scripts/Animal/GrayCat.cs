using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GrayCat : CatBehavior
{
    [Header("Flee Settings")]
    [SerializeField] private float fleeSpeed = 15f;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private float detectionRadius = 10f;
    [SerializeField] private Transform eyePosition; 
    [SerializeField] private float fieldOfView = 120f; 
    [SerializeField] private int rayCount = 10;
    [SerializeField] private float fleeDuration = 5f;
    [SerializeField] private Transform followBase;
    //private
    private float timer;
    private bool isFleeing;
    private void Update()
    {
        HandleCatching();
        HandleAnimation();
        if (!isCatched)
        {
            DetectPlayer();

            if (isFleeing)
            {
                FleeFromPlayer();
            }
            else
            {
                HandleMovement();
            }
        }
    }
    private void DetectPlayer()
    {
        float angleStep = fieldOfView / rayCount;
        float startAngle = -fieldOfView / 2;

        for (int i = 0; i <= rayCount; i++)
        {
            float currentAngle = startAngle + (angleStep * i);

            Vector3 rayDirection = Quaternion.Euler(0, currentAngle, 0) * transform.forward;

            if (Physics.Raycast(eyePosition.position, rayDirection, out RaycastHit hit, detectionRadius, playerLayer))
            {
                Debug.DrawLine(eyePosition.position, hit.point, Color.red);
                isFleeing = true;
                timer = 0;
                break; 
            }
            else
            {
                Debug.DrawRay(eyePosition.position, rayDirection * detectionRadius, Color.green);
            }
        }
    }
    protected override IEnumerator OnCaught()
    {
        isCatched = true;
        Destroy(rb);
        navMeshAgent.enabled = false;
        int followIndex = PlayerController.instance.GrayCats.Count;
        PlayerController.instance.GrayCats.Add(gameObject);
        PlayerController.instance.catchingCat = true;
        yield return new WaitForSeconds(0.5f);
        if (followBase != null && followIndex < followBase.childCount)
        {
            Transform followPoint = followBase.GetChild(followIndex);
            transform.position = followPoint.position;
            transform.SetParent(followPoint);
            transform.localRotation = Quaternion.identity;
        }
        PlayerController.instance.catchingCat = false;
    }

    private void FleeFromPlayer()
    {
        if (timer <= fleeDuration)
        {
            timer += Time.deltaTime;
            Vector3 fleeTarget = new Vector3(transform.position.x, transform.position.y, transform.position.z + 20f);
            navMeshAgent.speed = fleeSpeed;
            navMeshAgent.SetDestination(fleeTarget);
        }
        else
        {
            isFleeing = false;
        }
    }
}
