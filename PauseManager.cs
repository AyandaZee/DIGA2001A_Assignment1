using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseButtonUI; // Drag PauseButtonUI here
    private bool isPaused = false;

    public void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            Time.timeScale = 0f; // Freezes all physics, ball, and character movement
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Debug.Log("GAME PAUSED");
        }
        else
        {
            Time.timeScale = 1f; // Resumes gameplay speed
            Debug.Log("GAME RESUMED");
        }
    }

    private void Update()
    {
        // Pressing 'P' or 'Escape' on keyboard also pauses
        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }
}