using UnityEngine;

public class CardHazard : MonoBehaviour
{
    public enum CardType { YellowCard, RedCard }
    public CardType cardType = CardType.YellowCard;

    [Header("Ball Reset Settings")]
    public Vector3 ballStartPosition = new Vector3(0f, 1f, 5f); // Y set to 1f so it spawns safely above the ground

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the ball touches the Referee
        if (collision.gameObject.CompareTag("Ball") || collision.gameObject.name == "sharp.shooter ball")
        {
            Rigidbody ballRb = collision.gameObject.GetComponent<Rigidbody>();
            if (ballRb != null)
            {
                // 1. Unfreeze physics components
                ballRb.isKinematic = false;

                // 2. Clear all linear and spin velocities completely
                ballRb.linearVelocity = Vector3.zero;
                ballRb.angularVelocity = Vector3.zero;

                // 3. Reposition the ball safely above the kickoff spot
                collision.transform.position = ballStartPosition;

                // 4. Wake up physics engine to make sure ball is rendered and active
                ballRb.WakeUp();

                Debug.Log("REFEREE INTERCEPTED THE BALL! Safely reset to kickoff position.");
            }
        }

        // Check if Player touches the Referee (Card Penalty System)
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            if (player != null)
            {
                if (cardType == CardType.YellowCard)
                {
                    player.ApplyYellowCard(3f); // Slow movement for 3s
                    Debug.Log("YELLOW CARD ISSUED TO PLAYER!");
                }
                else if (cardType == CardType.RedCard)
                {
                    player.ApplyRedCard(2f); // Freeze movement for 2s
                    Debug.Log("RED CARD ISSUED TO PLAYER!");
                }
            }
        }
    }
}