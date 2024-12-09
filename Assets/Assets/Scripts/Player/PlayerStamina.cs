using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerStamina : MonoBehaviour
{
    [Header("Stamina Settings")]
    [SerializeField] private int maxStamina = 100;
    [SerializeField] private float staminaDecreasePerTap = 10f;
    [SerializeField] private float staminaRecoveryRate = 5f;
    [SerializeField] private Slider staminaSlider;

    [Header("Game Settings")]
    [SerializeField] private float maxRunningTime = 10f;
    [SerializeField] private float speedIncreasePerTap = 0.3f;
    [SerializeField] private float minSpeedIncreasePerTap = 0.1f;

    private float currentStamina;
    private float runningTimer;
    private bool isTapPhaseActive;

    private PlayerController playerController;

    void Start()
    {
        currentStamina = maxStamina;
        runningTimer = maxRunningTime;
        isTapPhaseActive = true;

        if (staminaSlider != null)
        {
            staminaSlider.maxValue = maxStamina;
            staminaSlider.value = currentStamina;
        }

        playerController = PlayerController.instance;
    }

    void Update()
    {
        if (isTapPhaseActive)
        {
            runningTimer -= Time.deltaTime;
            if (runningTimer <= 0 || currentStamina <= 0)
            {
                EndTapPhase();
            }
        }
        else
        {
            if (currentStamina < maxStamina)
            {
                RecoverStamina();
            }
        }
    }

    public void OnTap()
    {
        if (!isTapPhaseActive || currentStamina <= 0) return;

        currentStamina = Mathf.Max(currentStamina - staminaDecreasePerTap, 0);

        if (playerController != null)
        {
            playerController.maxSpeed += speedIncreasePerTap;
            playerController.minSpeed += minSpeedIncreasePerTap;
        }

        if (staminaSlider != null)
        {
            staminaSlider.value = currentStamina;
        }
    }

    private void RecoverStamina()
    {
        currentStamina = Mathf.Min(currentStamina + staminaRecoveryRate * Time.deltaTime, maxStamina);

        if (staminaSlider != null)
        {
            staminaSlider.value = currentStamina;
        }
    }

    private void EndTapPhase()
    {
        isTapPhaseActive = false;
        if (playerController != null)
        {
            playerController.isRunning = true;
        }
        TouchFieldPanel.instance.CompleteStaminaPhase();
    }
}

