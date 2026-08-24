using UnityEngine;

public class GoalkeeperAI : MonoBehaviour
{
    public float moveDistance = 14f; // Covers the 16-wide goal
    public float moveSpeed = 5f;

    private Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        float offset = Mathf.PingPong(Time.time * moveSpeed, moveDistance) - (moveDistance / 2f);
        transform.position = new Vector3(startPosition.x + offset, startPosition.y, startPosition.z);
    }
}