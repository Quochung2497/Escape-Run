using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatchAnimal : MonoBehaviour
{
    [Header("Cone Settings")]
    [SerializeField] private float fov;
    [SerializeField] private int rayCount;
    [SerializeField] private float angle;
    [SerializeField] private float viewDistance;
    [SerializeField] private Vector3 origin;
    [Header("Catch Settings")]
    [SerializeField] private float catCatchDuration;
    [SerializeField] private LayerMask cat;

    private MeshRenderer meshRender;
    private MeshCollider meshCollider;

    private HashSet<CatBehavior> catsInCone = new HashSet<CatBehavior>();


    private void Start()
    {
        InitializeComponents();
        CreateConeMesh();
        HideConeMesh();
    }

    private void LateUpdate()
    {
        CreateConeMesh();
        CheckConeWithRaycast();
    }

    private void InitializeComponents()
    {
        meshRender = GetComponent<MeshRenderer>();
        meshCollider = GetComponent<MeshCollider>();
    }

    private void CreateConeMesh()
    {
        Mesh mesh = new Mesh();

        Vector3[] vertices = CalculateConeVertices();
        int[] triangles = CalculateConeTriangles();

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        GetComponent<MeshFilter>().mesh = mesh;

        if (meshCollider != null)
        {
            meshCollider.sharedMesh = mesh;
        }
    }

    private Vector3[] CalculateConeVertices()
    {
        Vector3[] vertices = new Vector3[rayCount + 2];
        vertices[0] = origin;

        float angleStep = fov / rayCount;
        float currentAngle = -fov / 2 + angle;

        for (int i = 0; i <= rayCount; i++)
        {
            vertices[i + 1] = origin + GetVectorFromAngle(currentAngle) * viewDistance;
            currentAngle += angleStep;
        }

        return vertices;
    }

    private int[] CalculateConeTriangles()
    {
        int[] triangles = new int[rayCount * 3];

        for (int i = 0; i < rayCount; i++)
        {
            triangles[i * 3] = 0;
            triangles[i * 3 + 1] = i + 1;
            triangles[i * 3 + 2] = i + 2;
        }

        return triangles;
    }

    private void CheckConeWithRaycast()
    {
        Vector3 coneOrigin = transform.position + transform.TransformVector(origin);
        float angleStep = fov / rayCount;
        float currentAngle = -fov / 2 + angle;

        bool anyCatInCone = false;

        HashSet<CatBehavior> currentCatsInCone = new HashSet<CatBehavior>();

        for (int i = 0; i <= rayCount; i++)
        {
            Vector3 rayDirection = transform.TransformDirection(GetVectorFromAngle(currentAngle));

            if (Physics.Raycast(coneOrigin, rayDirection, out RaycastHit hit, viewDistance, cat))
            {
                Debug.DrawLine(coneOrigin, hit.point, Color.red);

                CatBehavior catBehavior = hit.collider.GetComponent<CatBehavior>();
                if (catBehavior != null && !catBehavior.isCatched)
                {
                    anyCatInCone = true;

                    currentCatsInCone.Add(catBehavior);
                    if (!catsInCone.Contains(catBehavior))
                    {
                        catBehavior.StartCatching();
                        catsInCone.Add(catBehavior); 
                    }
                }
            }

            currentAngle += angleStep;

        }
        HashSet<CatBehavior> catsToRemove = new HashSet<CatBehavior>(catsInCone);
        catsToRemove.ExceptWith(currentCatsInCone);

        foreach (var cat in catsToRemove)
        {
            cat.StopCatching();
            catsInCone.Remove(cat);
        }
        if (!anyCatInCone)
        {
            HideConeMesh();
        }
        else
        {
            ShowConeMesh();
        }
    }

    private void ShowConeMesh()
    {
        if (meshRender != null)
        {
            meshRender.enabled = true;
        }
    }

    private void HideConeMesh()
    {
        if (meshRender != null)
        {
            meshRender.enabled = false;
        }
    }

    private Vector3 GetVectorFromAngle(float angle)
    {
        float rad = angle * Mathf.Deg2Rad;
        return new Vector3(Mathf.Cos(rad), 0, Mathf.Sin(rad));
    }
}
