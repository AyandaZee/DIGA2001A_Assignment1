using UnityEngine;
using UnityEngine.UI;

public class EnergySystem : MonoBehaviour
{
    [Header("Timer Settings")]
    public float maxEnergy = 100f;
    public float currentEnergy = 100f;

    [Tooltip("Time in seconds for a full bar to drain. 120 = 2 full minutes.")]
    public float totalGameTimeInSeconds = 120f;

    [Header("UI References")]
    public Slider energySlider;
    public MainMenuManager mainMenuManager;

    [Header("State Control")]
    public bool isGameActive = false;
    private bool isGameOver = false;

    private void Start()
    {
        ResetEnergy();
        isGameActive = false;

        if (mainMenuManager == null)
        {
            mainMenuManager = Object.FindFirstObjectByType<MainMenuManager>();
        }
    }

    public void ResetEnergy()
    {
        isGameOver = false;
        currentEnergy = maxEnergy;

        if (energySlider == null)
        {
            // Auto-locate slider on scene if not explicitly assigned
            energySlider = Object.FindFirstObjectByType<Slider>();
        }

        if (energySlider != null)
        {
            energySlider.minValue = 0f;
            energySlider.maxValue = maxEnergy;
            energySlider.value = maxEnergy;
        }

        Debug.Log("ENERGY RESET TO 100%");
    }

    public void StartEnergyTimer()
    {
        ResetEnergy();
        isGameActive = true;
    }

    private void Update()
    {
        if (!isGameActive || isGameOver) return;

        if (currentEnergy > 0f)
        {
            // Calculate drain speed based on total desired game duration
            float drainRate = maxEnergy / totalGameTimeInSeconds;
            currentEnergy -= drainRate * Time.deltaTime;

            // Direct visual update to the UI slider every frame
            if (energySlider != null)
            {
                energySlider.value = currentEnergy;
            }
        }
        else if (currentEnergy <= 0f && !isGameOver)
        {
            currentEnergy = 0f;
            if (energySlider != null) energySlider.value = 0f;

            isGameOver = true;
            isGameActive = false;

            Debug.Log("<color=red>TIMER EXPIRED! GAME OVER.</color>");

            if (mainMenuManager != null)
            {
                mainMenuManager.TriggerGameOver();
            }
        }
    }
}