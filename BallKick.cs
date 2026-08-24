using UnityEngine;

public class BallKick : MonoBehaviour
{
    private Vector3 initialPosition;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        initialPosition = transform.position; // Save starting location
    }

    private void Update()
    {
        // Reset if the ball falls off the pitch (underground or past bounds)
        if (transform.position.y < -1f || Mathf.Abs(transform.position.x) > 25f || transform.position.z > 35f || transform.position.z < -10f)
        {
            ResetBall();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Entering the goal net
        if (other.gameObject.name == "GoalArea")
        {
            Debug.Log("GOAL! YOU WIN THE MATCH!");
            ResetBall();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Touching the goalkeeper resets the ball back to start
        if (collision.gameObject.name == "Goalkeeper")
        {
            Debug.Log("SAVED BY THE KEEPER!");
            ResetBall();
        }
    }

    public void ResetBall()
    {
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        transform.position = initialPosition;
    }
}