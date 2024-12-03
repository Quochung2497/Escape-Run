using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MovementBoundary))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float maxSpeed = 10f;
    public float minSpeed = 3f;  
    [SerializeField] private float rotationSpeed = 5f;

    [SerializeField] private MovementBoundary boundary;
    [SerializeField] private FixedJoystick joystick;
    //public
    public static PlayerController instance {  get; private set; }
    public bool isRunning = false;

    //private
    private CharacterController characterController;
    private float currentSpeed;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    
    void Update()
    {
        if (joystick != null && isRunning)
            JoystickControl();
    }

    private void JoystickControl()
    {
        Vector3 movement = new Vector3(joystick.Horizontal, 0, joystick.Vertical);

        if (movement.magnitude > 0.1f)
        {
            float joystickDistance = Mathf.Clamp01(movement.magnitude);
            currentSpeed = Mathf.Lerp(minSpeed, maxSpeed, joystickDistance);

            Vector3 moveDirection = movement.normalized * currentSpeed * Time.deltaTime;
            Vector3 targetPosition = transform.position + moveDirection;

            if (targetPosition.x >= boundary.LeftBoundary && targetPosition.x <= boundary.RightBoundary &&
                targetPosition.z >= boundary.BackBoundary)
            {
                characterController.Move(moveDirection);

                float rotation = Mathf.Atan2(movement.x, movement.z) * Mathf.Rad2Deg;
                Quaternion targetRotation = Quaternion.Euler(0, rotation, 0);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
            Debug.Log(currentSpeed);
        }
    }
    
    public bool IsAtBoundary()
    {
        float tolerance = 0.1f;
        return Mathf.Abs(transform.position.x - boundary.LeftBoundary) <= tolerance ||
               Mathf.Abs(transform.position.x - boundary.RightBoundary) <= tolerance ||
               Mathf.Abs(transform.position.z - boundary.BackBoundary) <= tolerance;
    }

}
