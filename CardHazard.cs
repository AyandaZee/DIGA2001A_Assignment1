using UnityEngine;

public class CardHazard : MonoBehaviour
{
    public enum CardType { YellowCard, RedCard }
    public CardType cardType;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                if (cardType == CardType.YellowCard)
                {
                    player.ApplyYellowCard(5f); // Slow down 5s
                }
                else if (cardType == CardType.RedCard)
                {
                    player.ApplyRedCard(3f); // Freeze 3s
                }
            }
        }
    }
}