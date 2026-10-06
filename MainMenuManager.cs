using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenuManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject coverPanel;
    public GameObject menuPanel;
    public GameObject optionsPanel;

    [Header("Splash Screens")]
    public GameObject level1Splash;
    public GameObject level2Splash;

    void Start()
    {
        // Keep cursor unlocked and visible for UI menus
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Unfreeze time when a scene starts
        Time.timeScale = 1f;

        string currentScene = SceneManager.GetActiveScene().name;

        if (currentScene == "MainMenu")
        {
            ShowCover();
        }
        else if (currentScene == "Level 1")
        {
            ShowMainMenu();
        }
        else if (currentScene == "Level 2")
        {
            if (level2Splash) StartCoroutine(Level2SplashRoutine());
        }
    }

    public void ShowCover()
    {
        if (coverPanel) coverPanel.SetActive(true);
        if (menuPanel) menuPanel.SetActive(false);
        if (optionsPanel) optionsPanel.SetActive(false);
        if (level1Splash) level1Splash.SetActive(false);
        if (level2Splash) level2Splash.SetActive(false);
    }

    public void ShowMainMenu()
    {
        // Ensure cursor is visible when menu is shown
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (coverPanel) coverPanel.SetActive(false);
        if (menuPanel) menuPanel.SetActive(true);
        if (optionsPanel) optionsPanel.SetActive(false);
        if (level1Splash) level1Splash.SetActive(false);
        if (level2Splash) level2Splash.SetActive(false);
    }

    public void ShowOptions()
    {
        if (coverPanel) coverPanel.SetActive(false);
        if (menuPanel) menuPanel.SetActive(false);
        if (optionsPanel) optionsPanel.SetActive(true);
        if (level1Splash) level1Splash.SetActive(false);
        if (level2Splash) level2Splash.SetActive(false);
    }

    // Called from MainMenu cover art click
    public void LoadLevel1Scene()
    {
        SceneManager.LoadScene("Level 1");
    }

    // Called when clicking PLAY on Level 1 menu
    public void PlayGame()
    {
        // Hide all menus so gameplay is fully active and clickable
        if (coverPanel) coverPanel.SetActive(false);
        if (menuPanel) menuPanel.SetActive(false);
        if (optionsPanel) optionsPanel.SetActive(false);
        if (level1Splash) level1Splash.SetActive(false);
        if (level2Splash) level2Splash.SetActive(false);
    }

    // Called for Level 1 transition (with splash)
    public void LaunchLevel1()
    {
        StartCoroutine(SplashRoutine("Level 1", level1Splash));
    }

    // Called for Level 2 transition (with splash)
    public void LaunchLevel2()
    {
        StartCoroutine(SplashRoutine("Level 2", level2Splash));
    }

    private IEnumerator SplashRoutine(string sceneName, GameObject splashPanel)
    {
        if (menuPanel) menuPanel.SetActive(false);
        if (optionsPanel) optionsPanel.SetActive(false);
        if (splashPanel) splashPanel.SetActive(true);

        yield return new WaitForSeconds(3.5f);

        SceneManager.LoadScene(sceneName);
    }

    private IEnumerator Level2SplashRoutine()
    {
        if (level2Splash) level2Splash.SetActive(true);
        yield return new WaitForSeconds(3.5f);
        if (level2Splash) level2Splash.SetActive(false);
    }

    public void QuitGame()
    {
        Debug.Log("Quitting...");
        Application.Quit();
    }
}