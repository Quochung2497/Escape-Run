using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowBase : MonoBehaviour
{
    public GameObject[] followPoints;
    public int numberOfPositions = 5;
    public float distanceBetweenPositions = 4f; 

    void Start()
    {
        GenerateFollowPositions();
    }

    void GenerateFollowPositions()
    {
        followPoints = new GameObject[numberOfPositions];

        for (int i = 0; i < numberOfPositions; i++)
        {
            GameObject followPoint = new GameObject($"FollowPosition_{i}");
            followPoint.transform.position = transform.position + Vector3.back * (i * distanceBetweenPositions);

            followPoint.transform.SetParent(transform);

            followPoints[i] = followPoint;
        }

        Debug.Log($"Generated {numberOfPositions} follow positions as children of {gameObject.name}");
    }

    private void OnDrawGizmos()
    {
        if (followPoints == null || followPoints.Length == 0) return;

        Gizmos.color = Color.green;
        foreach (GameObject point in followPoints)
        {
            if (point != null)
            {
                Gizmos.DrawWireSphere(point.transform.position,distanceBetweenPositions); 
            }
        }
    }
}
