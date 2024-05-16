using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // Singleton instance

    public GameObject cardPrefab;
    public Row row1;
    public Row row2;
    public Row row3;
    public Row row4;
    public Transform closedDeck;
    public Transform baseCardTransform;

    private List<Card> deck = new List<Card>();
    private Card currentBaseCard;

    void Awake()
    {
        Instance = this; // Set singleton instance
    }

    void Start()
    {
        InitializeDeck();
        ShuffleDeck();
        DealCards();
    }

    void InitializeDeck()
    {
        string[] suits = { "Hearts", "Diamonds", "Clubs", "Spades" };
        string[] ranks = { "2", "3", "4", "5", "6", "7", "8", "9", "10", "j", "q", "k", "a" };
        Sprite backSprite = Resources.Load<Sprite>("Sprites/back_red");

        foreach (string suit in suits)
        {
            for (int i = 0; i < ranks.Length; i++)
            {
                string rank = ranks[i];
                string cardName = $"{suit}_{rank}";
                Sprite frontSprite = Resources.Load<Sprite>($"Sprites/{cardName}");
                if (frontSprite != null)
                {
                    GameObject cardObject = Instantiate(cardPrefab);
                    Card card = cardObject.GetComponent<Card>();
                    int value = i + 2; // Значение карты соответствует её индексу в массиве ranks плюс 2 (для 2-ки значение должно быть 2)
                    card.SetCard(value, suit, cardName, frontSprite, backSprite, false);
                    deck.Add(card);
                    cardObject.SetActive(false);
                }
                else
                {
                    Debug.LogError($"Could not find sprite for {cardName}");
                }
            }
        }
    }

    void ShuffleDeck()
    {
        for (int i = 0; i < deck.Count; i++)
        {
            Card temp = deck[i];
            int randomIndex = UnityEngine.Random.Range(0, deck.Count);
            deck[i] = deck[randomIndex];
            deck[randomIndex] = temp;
        }
    }

    void DealCards()
    {
        int index = 0;

        // Row1
        List<Card> row1Cards = new List<Card>();
        for (int i = 0; i < 3; i++)
        {
            row1Cards.Add(deck[index++]);
        }
        row1.PlaceCards(row1Cards, 9);

        // Row2
        List<Card> row2Cards = new List<Card>();
        for (int i = 0; i < 6; i++)
        {
            row2Cards.Add(deck[index++]);
        }
        row2.PlaceCards(row2Cards, 10);

        // Row3
        List<Card> row3Cards = new List<Card>();
        for (int i = 0; i < 9; i++)
        {
            row3Cards.Add(deck[index++]);
        }
        row3.PlaceCards(row3Cards, 11);

        // Row4
        List<Card> row4Cards = new List<Card>();
        for (int i = 0; i < 10; i++)
        {
            row4Cards.Add(deck[index++]);
        }
        row4.PlaceCards(row4Cards, 12);

        // Closed Deck - 23 карты
        int closedDeckOrder = 1;
        for (int i = index; i < index + 23; i++)
        {
            deck[i].transform.SetParent(closedDeck);
            deck[i].transform.localPosition = Vector3.zero;
            deck[i].SetFaceUp(false);
            deck[i].GetComponent<SpriteRenderer>().sortingOrder = closedDeckOrder;
            deck[i].gameObject.SetActive(true);
            closedDeckOrder++;
        }

        // Base Card - последняя карта из колоды
        Card baseCard = deck[index + 23];
        baseCard.transform.SetParent(baseCardTransform);
        baseCard.transform.localPosition = Vector3.zero;
        baseCard.SetFaceUp(true);
        baseCard.gameObject.SetActive(true);
        currentBaseCard = baseCard;
    }

    public void OnCardClicked(Card clickedCard)
    {
        Debug.Log("Card clicked in GameManager: " + clickedCard.cardName);
        if (CanPlaceCardOnBase(clickedCard))
        {
            Debug.Log("Card can be placed on base: " + clickedCard.cardName);

            // Remove current base card
            if (currentBaseCard != null)
            {
                currentBaseCard.gameObject.SetActive(false);
            }

            // Update base card
            clickedCard.transform.SetParent(baseCardTransform);
            clickedCard.transform.localPosition = Vector3.zero;
            clickedCard.SetFaceUp(true);
            currentBaseCard = clickedCard;

            Debug.Log("New base card: " + currentBaseCard.cardName);
        }
        else
        {
            Debug.Log("Card cannot be placed on base: " + clickedCard.cardName);
        }
    }

    public void OnClosedDeckCardClicked(Card clickedCard)
    {
        Debug.Log("Closed deck card clicked in GameManager: " + clickedCard.cardName);

        // Remove current base card
        if (currentBaseCard != null)
        {
            currentBaseCard.gameObject.SetActive(false);
        }

        // Update base card
        clickedCard.transform.SetParent(baseCardTransform);
        clickedCard.transform.localPosition = Vector3.zero;
        clickedCard.SetFaceUp(true);
        currentBaseCard = clickedCard;

        Debug.Log("New base card: " + currentBaseCard.cardName);
    }

    private bool CanPlaceCardOnBase(Card card)
    {
        if (currentBaseCard == null)
        {
            return true;
        }

        int baseValue = currentBaseCard.value;
        int cardValue = card.value;

        Debug.Log($"Checking if card {card.cardName} can be placed on base {currentBaseCard.cardName}");
        Debug.Log($"Base card value: {baseValue}, clicked card value: {cardValue}");

        bool canPlace = (baseValue == 1 && (cardValue == 13 || cardValue == 2)) || // Туз и король или двойка
                        (baseValue == 13 && (cardValue == 12 || cardValue == 1)) || // Король и дама или туз
                        (cardValue == baseValue + 1 || cardValue == baseValue - 1); // Обычные карты

        Debug.Log($"Can place card: {canPlace}");
        return canPlace;
    }

    public bool IsBaseCard(Card card)
    {
        return card == currentBaseCard;
    }
}
