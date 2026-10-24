using UnityEngine;

public class OutOfBounds : MonoBehaviour
{
    [Header("Reset Position")]
    public Vector3 kickOffSpot = new Vector3(0f, 1f, 5f); // Kickoff spot coordinates

    private void OnTriggerEnter(Collider other)
    {
        // If the ball enters this out-of-bounds area, reset it to kickoff
        if (other.CompareTag("Ball") || other.name == "sharp.shooter ball")
        {
            Rigidbody ballRb = other.GetComponent<Rigidbody>();
            if (ballRb != null)
            {
                ballRb.linearVelocity = Vector3.zero;
                ballRb.angularVelocity = Vector3.zero;
                other.transform.position = kickOffSpot;
                ballRb.WakeUp();

                Debug.Log("BALL OUT OF BOUNDS! Resetting to kickoff spot.");
            }
        }
    }
}