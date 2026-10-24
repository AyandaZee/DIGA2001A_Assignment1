using UnityEngine;
using UnityEngine.UI;

public class EnergyUI : MonoBehaviour
{
    public EnergySystem energySystem;
    public Slider energySlider;

    private void Start()
    {
        if (energySystem == null)
            energySystem = Object.FindFirstObjectByType<EnergySystem>();

        if (energySlider == null)
            energySlider = GetComponent<Slider>();

        if (energySlider != null && energySystem != null)
        {
            energySlider.minValue = 0f;
            energySlider.maxValue = energySystem.maxEnergy;
            energySlider.value = energySystem.currentEnergy;
        }
    }

    private void Update()
    {
        if (energySystem != null && energySlider != null)
        {
            energySlider.value = energySystem.currentEnergy;
        }
    }
}