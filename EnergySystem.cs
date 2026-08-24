using UnityEngine;

public class EnergySystem : MonoBehaviour
{
    [Header("Energy Settings")]
    public float maxEnergy = 100f;
    public float currentEnergy;
    public float baseDrainRate = 1f;
    public float sprintDrainRate = 4f;

    private PlayerController playerController;
    public bool isFainted = false;

    private void Start()
    {
        currentEnergy = maxEnergy;
        playerController = GetComponent<PlayerController>();
    }

    private void Update()
    {
        if (isFainted) return;

        // Drain energy faster if sprinting, otherwise normal rate
        bool isSprinting = UnityEngine.InputSystem.Keyboard.current != null &&
                           UnityEngine.InputSystem.Keyboard.current.leftShiftKey.isPressed;

        float currentDrain = isSprinting ? sprintDrainRate : baseDrainRate;
        currentEnergy -= currentDrain * Time.deltaTime;

        // Clamp energy between 0 and max
        currentEnergy = Mathf.Clamp(currentEnergy, 0f, maxEnergy);

        // Check for Faint (Game Over)
        if (currentEnergy <= 0f)
        {
            Faint();
        }
    }

    private void Faint()
    {
        isFainted = true;
        Debug.Log("GAME OVER: Energy reached 0! Player fainted.");
        // We will trigger the Game Over UI screen here later!
    }
}