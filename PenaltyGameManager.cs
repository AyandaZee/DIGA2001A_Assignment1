using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class PenaltyGameManager : MonoBehaviour
{
    public static PenaltyGameManager Instance;

    [Header("Game State")]
    public int score = 0;
    public Text scoreText; // Drag your UI Text here to show goal count in corner

    [Header("Transforms for Reset")]
    public Transform playerTransform;
    public Transform ballTransform;
    public Transform keeperTransform;
    public Rigidbody ballRigidbody;

    private Vector3 initialPlayerPos;
    private Quaternion initialPlayerRot;
    private Vector3 initialBallPos;
    private Vector3 initialKeeperPos;

    private bool isHandlingSave = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        if (playerTransform != null)
        {
            initialPlayerPos = playerTransform.position;
            initialPlayerRot = playerTransform.rotation;
        }
        if (ballTransform != null)
        {
            initialBallPos = ballTransform.position;
            if (ballRigidbody == null) ballRigidbody = ballTransform.GetComponent<Rigidbody>();
        }
        if (keeperTransform != null)
        {
            initialKeeperPos = keeperTransform.position;
        }

        UpdateScoreUI();
    }

    private void Update()
    {
        // Out of Bounds Detection (Ball leaves pitch boundaries or falls below ground)
        if (ballTransform != null && !isHandlingSave)
        {
            if (ballTransform.position.y < -1f || Mathf.Abs(ballTransform.position.x) > 35f || Mathf.Abs(ballTransform.position.z) > 45f)
            {
                OnBallOutOfBounds();
            }
        }
    }

    public void OnGoalScored()
    {
        score++;
        UpdateScoreUI();

        if (score >= 2)
        {
            Debug.Log("PLAYER WINS! Moving to Level 2...");
            SceneManager.LoadScene("Level 2");
        }
        else
        {
            ResetToKickoff();
        }
    }

    public void OnBallOutOfBounds()
    {
        ResetToKickoff();
    }

    public void HandleKeeperSave()
    {
        if (!isHandlingSave)
        {
            StartCoroutine(KeeperHoldAndThrowRoutine());
        }
    }

    private IEnumerator KeeperHoldAndThrowRoutine()
    {
        isHandlingSave = true;

        if (ballRigidbody != null)
        {
            ballRigidbody.linearVelocity = Vector3.zero;
            ballRigidbody.angularVelocity = Vector3.zero;
            ballRigidbody.isKinematic = true; // Hold ball firmly
        }

        if (ballTransform != null && keeperTransform != null)
        {
            ballTransform.position = keeperTransform.position + keeperTransform.forward * 0.8f + Vector3.up * 0.5f;
        }

        // Wait 3 seconds holding ball
        yield return new WaitForSeconds(3.0f);

        if (ballRigidbody != null)
        {
            ballRigidbody.isKinematic = false;
            // Short throw away from player side
            Vector3 throwDirection = (keeperTransform.forward * 1.5f + keeperTransform.right * 1.0f + Vector3.up * 0.5f).normalized;
            ballRigidbody.AddForce(throwDirection * 12f, ForceMode.Impulse);
        }

        isHandlingSave = false;
    }

    public void ResetToKickoff()
    {
        if (ballRigidbody != null)
        {
            ballRigidbody.linearVelocity = Vector3.zero;
            ballRigidbody.angularVelocity = Vector3.zero;
            ballRigidbody.isKinematic = false;
        }

        if (ballTransform != null) ballTransform.position = initialBallPos;
        if (playerTransform != null)
        {
            playerTransform.position = initialPlayerPos;
            playerTransform.rotation = initialPlayerRot;
        }
        if (keeperTransform != null) keeperTransform.position = initialKeeperPos;
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = score.ToString();
        }
    }
}