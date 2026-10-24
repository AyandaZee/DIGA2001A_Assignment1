using UnityEngine;
using System.Collections;

public class RulesManager : MonoBehaviour
{
    [Header("Rules UI Panel")]
    public GameObject rulesPanel;
    public float displayDuration = 4f; // Display duration in seconds

    public void ShowRulesAndStartGame()
    {
        StartCoroutine(RulesRoutine());
    }

    private IEnumerator RulesRoutine()
    {
        // 1. Show the rules overlay
        if (rulesPanel != null)
        {
            rulesPanel.SetActive(true);
        }

        // 2. Wait 4 seconds using unscaled real time
        yield return new WaitForSecondsRealtime(displayDuration);

        // 3. Hide the rules panel
        if (rulesPanel != null)
        {
            rulesPanel.SetActive(false);
        }

        // 4. Trigger active energy bar timer
        EnergySystem energySys = Object.FindFirstObjectByType<EnergySystem>();
        if (energySys != null)
        {
            energySys.StartEnergyTimer();
        }
    }
}