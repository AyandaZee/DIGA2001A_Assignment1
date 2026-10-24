using UnityEngine;
using UnityEngine.SceneManagement;

public class GoalTrigger : MonoBehaviour
{
    [Header("UI Image Settings")]
    public GameObject scoreImage; // Reference to ScoreImage UI object

    [Header("Goal Chip Settings")]
    public float forwardChipForce = 18f;
    public float upwardChipForce = 10f;

    private int scoreCount = 0;
    private bool isProcessingGoal = false;

    private void Start()
    {
        // Ensure image is hidden when game begins
        if (scoreImage != null)
        {
            scoreImage.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isProcessingGoal) return;

        // Check if the ball enters the goal area
        if (other.CompareTag("Ball") || other.name == "sharp.shooter ball")
        {
            isProcessingGoal = true;
            scoreCount++;

            Debug.Log("GOAL SCORED! Current Count: " + scoreCount);

            Rigidbody ballRb = other.GetComponent<Rigidbody>();
            if (ballRb != null)
            {
                // Reset incoming speed completely
                ballRb.linearVelocity = Vector3.zero;
                ballRb.angularVelocity = Vector3.zero;

                // Find player position or default to pitch center (Z = 0)
                GameObject player = GameObject.FindWithTag("Player");
                Vector3 targetDirection;

                if (player != null)
                {
                    // Direction pointing back toward the player from the goal
                    targetDirection = (player.transform.position - other.transform.position).normalized;
                }
                else
                {
                    // Fallback: push along world Z-axis toward pitch center
                    targetDirection = Vector3.back;
                }

                // Combine horizontal push toward pitch with upward arc
                Vector3 chipForceVector = new Vector3(targetDirection.x * forwardChipForce, upwardChipForce, targetDirection.z * forwardChipForce);
                ballRb.AddForce(chipForceVector, ForceMode.Impulse);

                Debug.Log("BALL CHIPPED BACK OUT TOWARD PITCH!");
            }

            if (scoreCount == 1)
            {
                // Show the "1" image in the top-right corner upon scoring 1st goal
                if (scoreImage != null)
                {
                    scoreImage.SetActive(true);
                }

                Invoke(nameof(ResetGoalLock), 1.5f);
            }
            else if (scoreCount >= 2)
            {
                // Load Level 2 upon scoring 2nd goal
                Debug.Log("Goal 2 Scored - Moving to Level 2!");
                Invoke(nameof(LoadNextLevel), 2.0f);
            }
        }
    }

    private void ResetGoalLock()
    {
        isProcessingGoal = false;
    }

    private void LoadNextLevel()
    {
        SceneManager.LoadScene("Level 2");
    }
}