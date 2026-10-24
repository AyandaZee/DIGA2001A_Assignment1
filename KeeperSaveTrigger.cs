using UnityEngine;

public class KeeperSaveTrigger : MonoBehaviour
{
    [Header("Keeper Chip Settings")]
    public float forwardChipForce = 18f; // Strength of chip towards opponent side
    public float upwardChipForce = 12f;  // Height of chip arc

    private void OnCollisionEnter(Collision collision)
    {
        // Detect if the object touching the keeper is the ball
        if (collision.gameObject.CompareTag("Ball") || collision.gameObject.name == "sharp.shooter ball")
        {
            Rigidbody ballRb = collision.gameObject.GetComponent<Rigidbody>();
            if (ballRb != null)
            {
                // 1. Reset incoming ball speed instantly
                ballRb.linearVelocity = Vector3.zero;
                ballRb.angularVelocity = Vector3.zero;

                // 2. Calculate chip direction (Keeper's forward direction + upward arc)
                Vector3 chipDirection = (transform.forward * forwardChipForce) + (Vector3.up * upwardChipForce);

                // 3. Launch ball away toward the opponent side
                ballRb.AddForce(chipDirection, ForceMode.Impulse);

                Debug.Log("GOALKEEPER CHIPPED THE BALL AWAY!");
            }

            // Trigger game manager logic if active
            if (PenaltyGameManager.Instance != null)
            {
                PenaltyGameManager.Instance.HandleKeeperSave();
            }
        }
    }
}