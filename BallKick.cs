using UnityEngine;

public class BallKick : MonoBehaviour
{
    private Rigidbody rb;
    public float kickForce = 35f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Press Spacebar to instantly kick forward
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ApplyKick(Vector3.forward);
        }
    }

    // Universal collision: kicks the second anything pushes into the ball
    private void OnCollisionEnter(Collision collision)
    {
        if (rb == null) return;

        // Get the push direction away from whatever hit it
        Vector3 pushDir = collision.contacts.Length > 0 ? -collision.contacts[0].normal : Vector3.forward;
        pushDir.y = 0.1f; // keep it low so it rolls on the grass

        rb.linearVelocity = Vector3.zero;
        rb.AddForce(pushDir.normalized * kickForce, ForceMode.Impulse);
    }

    void ApplyKick(Vector3 dir)
    {
        if (rb == null) return;
        dir.y = 0.1f;
        rb.linearVelocity = Vector3.zero;
        rb.AddForce(dir.normalized * kickForce, ForceMode.Impulse);
    }
}