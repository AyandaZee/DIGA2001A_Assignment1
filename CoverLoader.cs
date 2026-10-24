using UnityEngine;
using UnityEngine.SceneManagement;

public class CoverLoader : MonoBehaviour
{
    // Call this when clicking the cover art
    public void LoadLevel1()
    {
        SceneManager.LoadScene("Level 1");
    }
}   