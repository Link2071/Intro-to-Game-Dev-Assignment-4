using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> cardPrefabs = new List<GameObject>();
    [SerializeField] private List<float> cardXPositions;
    private List<GameObject> cards = new List<GameObject>();
    public static CardManager instance;
    void Awake()
    {
        if (instance != null && instance != this) Destroy(gameObject);
        instance = this;
    }

    public void DisplayCards()
    {
        PlayerController.instance.SetMovementState(false);
        GhostManager.instance.SetGhostsCanMove(false);
        for (int i = 0;  i < 3; i++)
        {
            GameObject card = Instantiate(cardPrefabs[Random.Range(0, cardPrefabs.Count)], transform);
            card.GetComponent<RectTransform>().anchoredPosition = new Vector3(cardXPositions[i], 0, 0);
            cards.Add(card);
        }
    }

    public void HideCards()
    {
        PlayerController.instance.SetMovementState(true);
        GhostManager.instance.SetGhostsCanMove(true);
        for (int i = 2; i >= 0; i--)
        {
            Destroy(cards[i]);
            cards.Remove(cards[i]);
        }
    }
}
