using UnityEngine;
using UnityEngine.UI;

public class EnergyUI : MonoBehaviour
{
    public EnergySystem energySystem;
    public Slider energySlider;

    private void Update()
    {
        if (energySystem != null && energySlider != null)
        {
            energySlider.value = energySystem.currentEnergy;
        }
    }
}