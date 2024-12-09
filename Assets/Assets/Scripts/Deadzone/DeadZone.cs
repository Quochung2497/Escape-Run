using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DeadZone : MonoBehaviour
{
    [Header("Deadzone Settings")]
    public float phase1Speed = 10f; 
    public float phase1Duration = 40f; 
    public float phase2Speed = 40f;
    public float phase2Duration = 25f; 
    public float startDistance = 150f; 
    public float deadzoneWidth = 100f; 

    //References
    private Transform hero; 
    private float currentSpeed;
    private float deadzonePositionZ; 
    private float timer; 

    private int currentPhase = 1;
    private bool isHeroCaught = false; 


    private void Start()
    {
        hero = PlayerController.instance.transform;
        deadzonePositionZ = hero.position.z - startDistance;
        transform.position = new Vector3(transform.position.x, transform.position.y, deadzonePositionZ);
        currentSpeed = phase1Speed;
    }

    private void Update()
    {
        if (isHeroCaught) return;
        if(currentPhase == 1)
        {
            timer += Time.deltaTime;
            if (timer >= phase1Duration)
            {
                currentPhase = 2;
                currentSpeed = phase2Speed;
                timer = 0;
            }
        }
        deadzonePositionZ += currentSpeed * Time.deltaTime;
        transform.position = new Vector3(transform.position.x, transform.position.y, deadzonePositionZ);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isHeroCaught = true;
            OnHeroCaught();
        }
        else if (other.CompareTag("Cat"))
        {
            CatBehavior catBehavior = other.GetComponent<CatBehavior>();
            if (catBehavior != null)
            {
                Debug.Log($"Catch Cat: {catBehavior.name}");
            }
        }
    }

    private void OnHeroCaught()
    {
        Debug.Log("Game Over!");
    }
}

