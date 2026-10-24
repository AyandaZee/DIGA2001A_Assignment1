using UnityEngine;

public class OpponentAI : MonoBehaviour
{
    [Header("Zigzag Movement Settings")]
    public float forwardDistance = 15f; // How far forward/back the opponent travels
    public float zigzagWidth = 5f;       // How far side-to-side the zigzag swings
    public float moveSpeed = 4f;        // Speed of movement
    public float zigzagFrequency = 3f;  // Speed of the side-to-side swing

    [Header("Opponent Kick/Chip Settings")]
    public float forwardChipForce = 18f;
    public float upwardChipForce = 10f;

    private Vector3 startPosition;
    private bool movingForward = true;
    private float currentZOffset = 0f;

    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        HandleZigzagMovement();
    }

    private void HandleZigzagMovement()
    {
        // 1. Calculate forward/backward progression
        if (movingForward)
        {
            currentZOffset += moveSpeed * Time.deltaTime;
            if (currentZOffset >= forwardDistance)
            {
                movingForward = false;
            }
        }
        else
        {
            currentZOffset -= moveSpeed * Time.deltaTime;
            if (currentZOffset <= 0f)
            {
                movingForward = true;
            }
        }

        // 2. Calculate smooth sine-wave side-to-side (zigzag) offset
        float xOffset = Mathf.Sin(Time.time * zigzagFrequency) * zigzagWidth;

        // 3. Update opponent position relative to starting spot
        Vector3 targetPosition = startPosition + (transform.forward * currentZOffset) + (transform.right * xOffset);
        transform.position = targetPosition;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check if ball touches Opponent (chips ball away like Keeper & Referee)
        if (collision.gameObject.CompareTag("Ball") || collision.gameObject.name == "sharp.shooter ball")
        {
            Rigidbody ballRb = collision.gameObject.GetComponent<Rigidbody>();
            if (ballRb != null)
            {
                // Reset incoming ball speed
                ballRb.linearVelocity = Vector3.zero;
                ballRb.angularVelocity = Vector3.zero;

                // Calculate chip direction back downfield
                Vector3 chipDirection = (transform.forward * forwardChipForce) + (Vector3.up * upwardChipForce);

                // Launch ball away
                ballRb.AddForce(chipDirection, ForceMode.Impulse);

                Debug.Log("OPPONENT INTERCEPTED AND CHIPPED THE BALL!");
            }
        }
    }
}