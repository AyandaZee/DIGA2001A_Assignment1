using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenuManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject coverPanel;
    public GameObject menuPanel;
    public GameObject optionsPanel;
    public GameObject gameOverPanel;

    [Header("Splash Screens")]
    public GameObject level1Splash;
    public GameObject level2Splash;

    void Start()
    {
        Time.timeScale = 1f;

        if (menuPanel != null)
        {
            ShowMainMenu();
        }
    }

    public void ShowMainMenu()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (coverPanel) coverPanel.SetActive(false);
        if (menuPanel) menuPanel.SetActive(true);
        if (optionsPanel) optionsPanel.SetActive(false);
        if (gameOverPanel) gameOverPanel.SetActive(false);
        if (level1Splash) level1Splash.SetActive(false);
        if (level2Splash) level2Splash.SetActive(false);
    }

    public void PlayGame()
    {
        // Hide panels
        if (coverPanel) coverPanel.SetActive(false);
        if (menuPanel) menuPanel.SetActive(false);
        if (optionsPanel) optionsPanel.SetActive(false);
        if (gameOverPanel) gameOverPanel.SetActive(false);
        if (level1Splash) level1Splash.SetActive(false);
        if (level2Splash) level2Splash.SetActive(false);

        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Trigger 4-second rules sequence first
        RulesManager rulesManager = Object.FindFirstObjectByType<RulesManager>();
        if (rulesManager != null)
        {
            rulesManager.ShowRulesAndStartGame();
        }
        else
        {
            // Fallback: Start energy timer directly if no RulesManager is found
            EnergySystem energySys = Object.FindFirstObjectByType<EnergySystem>();
            if (energySys != null)
            {
                energySys.StartEnergyTimer();
            }
        }
    }

    public void TriggerGameOver()
    {
        if (gameOverPanel != null) gameOverPanel.SetActive(true);
        StartCoroutine(GameOverSequence());
    }

    private IEnumerator GameOverSequence()
    {
        // Display GAME OVER screen for 2.5 seconds, then safely return to menu
        yield return new WaitForSecondsRealtime(2.5f);

        if (gameOverPanel) gameOverPanel.SetActive(false);
        ShowMainMenu();
    }

    public void RestartToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        Debug.Log("QUIT BUTTON PRESSED");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}