using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CatBehavior : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] protected float minSpeed = 10f;
    [SerializeField] protected float maxSpeed = 20f;
    [SerializeField] protected MovementBoundary boundary;
    [SerializeField] protected float Range = 15f;
    [SerializeField] protected float waitDuration = 2f;

    [Header("Catch Setting")]
    [SerializeField] private float catchDuration = 2f;

    //private
    private float catchTimer = 0f;
    protected float waitTimer;
    protected float currentSpeed;
    protected Vector3 walkPosition;
    protected Vector3 spawnPosition;
    protected NavMeshAgent navMeshAgent;
    protected Rigidbody rb;
    protected Animator anim;

    //State
    private bool isBeingCaught = false;
    protected bool walkPointSet;
    protected bool isWalking = false;
    public bool isCatched = false;
    // Initialize
    protected virtual void Start()
    {
        spawnPosition = transform.position;
        navMeshAgent = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();
        navMeshAgent.angularSpeed = 180f;
        navMeshAgent.speed = Random.Range(minSpeed, maxSpeed);
    }

    protected void HandleCatching()
    {
        if (isBeingCaught)
        {
            catchTimer += Time.deltaTime;

            if (IsCatched())
            {
                StartCoroutine(OnCaught());
            }
        }
    }
    public bool IsCatched()
    {
        return catchTimer >= catchDuration;
    }
    protected virtual IEnumerator OnCaught()
    { 
        yield return null;
    }

    protected void HandleMovement()
    {
        if (!walkPointSet)
        {
            GetRandomMovement();
        }
        else
        {
            navMeshAgent.SetDestination(walkPosition);
            isWalking = true;

            Vector3 distanceToWalkPos = walkPosition - transform.position;

            if (distanceToWalkPos.magnitude < 0.1f)
            {
                waitTimer += Time.deltaTime;
                if (waitTimer >= waitDuration)
                {
                    walkPointSet = false;
                    isWalking = false;
                }
            }
        }
    }

    protected virtual void GetRandomMovement()
    {
        float randomX = Random.Range(-Range, Range);
        float randomZ = Random.Range(-Range, Range);

        walkPosition = spawnPosition + new Vector3(randomX, 0, randomZ);
        if (walkPosition.x >= boundary.LeftBoundary && walkPosition.x <= boundary.RightBoundary &&
            walkPosition.z >= boundary.BackBoundary)
        {
            walkPointSet = true;
            waitTimer = 0;
            navMeshAgent.speed = Random.Range(minSpeed, maxSpeed);
        }
        else
        {
            GetRandomMovement();
        }
    }

    public virtual void StartCatching()
    {
        isBeingCaught = true;
    }

    public virtual void StopCatching()
    {
        isBeingCaught = false;
        catchTimer = 0f;
    }

    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, Range);
    }

    protected void HandleAnimation()
    {
        float speed = navMeshAgent.velocity.magnitude;
        bool isRunning = speed > 0.1f;
        anim.SetBool("Run", isRunning);
    }
}
