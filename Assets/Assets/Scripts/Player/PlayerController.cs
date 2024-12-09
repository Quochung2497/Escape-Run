using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MovementBoundary))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float maxSpeed = 10f;
    public float minSpeed = 3f;
    [SerializeField] private float decreaseMinSpeed = 5f;
    [SerializeField] private float decreaseMaxSpeed = 15f;
    [SerializeField] private float rotationSpeed = 5f;


    [Header("References")]
    [SerializeField] private float height;
    [SerializeField] private GameObject playerModel;
    [SerializeField] private MovementBoundary boundary;
    [SerializeField] private FixedJoystick joystick;
    //public
    public static PlayerController instance {  get; private set; }
    [HideInInspector] public bool isRunning = false;
    [HideInInspector] public List<GameObject> BrownCats = new List<GameObject>();
    [HideInInspector] public List<GameObject> GrayCats = new List<GameObject>();
    [HideInInspector] public Transform stackBase;
    [HideInInspector] public bool isDecreased = false;
    [HideInInspector] public float currentSpeed;
    [HideInInspector] public bool catchingCat = false;

    //private
    private CharacterController characterController;
    private Animator anim;
    private int currentCatCount = 0;
    private Vector3 joystickMovement;

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
        anim = GetComponentInChildren<Animator>();
        stackBase = new GameObject("StackBase").transform;
        stackBase.SetParent(transform);
        stackBase.localPosition = Vector3.up * height;
    }

    
    void Update()
    {
        PlayerAnimation();
        PlayerModel();
        if (joystick != null && isRunning)
        { 
            JoystickControl();
            CarryCat();
        }
    }

    private void JoystickControl()
    {
        joystickMovement = new Vector3(joystick.Horizontal, 0, joystick.Vertical);

        if (joystickMovement.magnitude > 0.1f)
        {
            float joystickDistance = Mathf.Clamp01(joystickMovement.magnitude);
            currentSpeed = Mathf.Lerp(minSpeed, maxSpeed, joystickDistance);
            Vector3 moveDirection = joystickMovement.normalized * currentSpeed * Time.deltaTime;
            Vector3 targetPosition = transform.position + moveDirection;
            if (targetPosition.x >= boundary.LeftBoundary && targetPosition.x <= boundary.RightBoundary &&
                targetPosition.z >= boundary.BackBoundary)
            {
                characterController.Move(moveDirection);

                float rotation = Mathf.Atan2(joystickMovement.x, joystickMovement.z) * Mathf.Rad2Deg;
                Quaternion targetRotation = Quaternion.Euler(0, rotation, 0);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        }
        else
        {
            currentSpeed = 0;
        }
        Debug.Log(currentSpeed);
    }

    private void CarryCat()
    {
        int totalCats = BrownCats.Count + GrayCats.Count;
        if (totalCats > currentCatCount)
        {
            if (totalCats == 1)
            {
                minSpeed -= decreaseMinSpeed;
                maxSpeed -= decreaseMaxSpeed;
            }
            else if (totalCats >= 2)
            {
                minSpeed -= 0.3f;
                maxSpeed -= 2.5f;
            }
            minSpeed = Mathf.Clamp(minSpeed, 1, float.MaxValue);
            maxSpeed = Mathf.Clamp(maxSpeed, 2, float.MaxValue);
            currentCatCount++;
        }
    }
    
    public bool IsAtBoundary()
    {
        float tolerance = 0.1f;
        return Mathf.Abs(transform.position.x - boundary.LeftBoundary) <= tolerance ||
               Mathf.Abs(transform.position.x - boundary.RightBoundary) <= tolerance ||
               Mathf.Abs(transform.position.z - boundary.BackBoundary) <= tolerance;
    }
    private void PlayerAnimation()
    {
        if (!catchingCat)
        {
            if(anim.GetBool("WarmUp"))
            {
                anim.SetBool("WarmUp", false);
            }
            float avgSpeed = (minSpeed + maxSpeed) / 2;
            bool running = currentSpeed > avgSpeed;
            if (currentSpeed > 5f)
            {
                anim.SetBool("Run", running);
                if (!running)
                    anim.SetBool("Walk", true);
            }
            else if(currentSpeed <= 5f || !TouchFieldPanel.instance.joystickActive())
            {
                if (anim.GetBool("Walk"))
                    anim.SetBool("Walk", false);
                else if (anim.GetBool("Run"))
                    anim.SetBool("Run", false);
            }
        }
        else
        {
            if (anim.GetBool("Walk"))
                anim.SetBool("Walk", false);
            else if (anim.GetBool("Run"))
                anim.SetBool("Run", false);
            anim.SetBool("WarmUp", true);
        }
    }

    private void PlayerModel()
    {
        playerModel.transform.localPosition = new Vector3(0, -1, 0);
        playerModel.transform.localRotation = Quaternion.identity;
    }
}
