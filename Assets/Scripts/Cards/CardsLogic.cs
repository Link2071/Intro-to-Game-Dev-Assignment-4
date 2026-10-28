using UnityEngine;
using UnityEngine.UI;
public enum CardTypes
{
    increasePlayerMovementSpeed,
    decreaseEnemySpeed,
    increaseScoreMultiplier,
    posScoreMultiplierNegGhostSpeed,
    negScoreMultiplierPosGhostSpeed,
}

public class CardsLogic : MonoBehaviour
{
    [SerializeField] private CardTypes cardType;

    public void HandleClick()
    {
        switch (cardType)
        {
            case CardTypes.decreaseEnemySpeed:
                GhostManager.instance.SetGhostsMoveSpeedMult(0.8f);
                break;
            case CardTypes.increasePlayerMovementSpeed:
                PlayerController.instance.SetMoveSpeedMult(1.3f);
                break;
            case CardTypes.increaseScoreMultiplier:
                PlayerController.instance.SetScoreMult(1.3f);
                break;
            case CardTypes.negScoreMultiplierPosGhostSpeed:
                PlayerController.instance.SetScoreMult(0.6f);
                GhostManager.instance.SetGhostsMoveSpeedMult(0.4f);
                break;
            case CardTypes.posScoreMultiplierNegGhostSpeed:
                PlayerController.instance.SetScoreMult(1.6f);
                GhostManager.instance.SetGhostsMoveSpeedMult(1.4f);
                break;
        }
        CardManager.instance.HideCards();
    }
}
