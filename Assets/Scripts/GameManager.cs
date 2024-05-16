using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // Singleton instance

    public Row row1;
    public Row row2;
    public Row row3;
    public Row row4;
    public Transform closedDeck;
    public Transform baseCardTransform;
    public DeckManager deckManager; // —сылка на DeckManager
    public GameObject gameOverPanel;// ѕанель Game Over
    public GameObject CountClosedDeckPanel;
    public Text closedDeckCountText;

    private List<Card> deck;
    private Card currentBaseCard;

    void Awake()
    {
        Instance = this; // Set singleton instance
    }

    void Start()
    {
        deck = deckManager.InitializeDeck();
        deckManager.ShuffleDeck(deck);
        DealCards();
        gameOverPanel.SetActive(false); // —крыть панель Game Over в начале игры
        UpdateClosedDeckCount();
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

        // Base Card - последн€€ карта из колоды
        Card baseCard = deck[index + 23];
        baseCard.transform.SetParent(baseCardTransform);
        baseCard.transform.localPosition = Vector3.zero;
        baseCard.SetFaceUp(true);
        baseCard.gameObject.SetActive(true);
        currentBaseCard = baseCard;

        UpdateClosedDeckCount();
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

            // Check uncovered cards in rows
            CheckUncoveredCardsInRow(row4, null); // Row4 не провер€ет на перекрытие
            CheckUncoveredCardsInRow(row3, row4); // Row3 провер€ет на перекрытие с Row4
            CheckUncoveredCardsInRow(row2, row3); // Row2 провер€ет на перекрытие с Row3
            CheckUncoveredCardsInRow(row1, row2); // Row1 провер€ет на перекрытие с Row2

            // ≈сли закрыта€ колода пуста, проверить возможные ходы
            if (closedDeck.childCount == 0)
            {
                if (!CheckForPossibleMoves())
                {
                    // Ќет возможных ходов, игра окончена
                    GameOver();
                }
            }

            UpdateClosedDeckCount();
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

        // Check uncovered cards in rows
        CheckUncoveredCardsInRow(row4, null); // Row4 не провер€ет на перекрытие
        CheckUncoveredCardsInRow(row3, row4); // Row3 провер€ет на перекрытие с Row4
        CheckUncoveredCardsInRow(row2, row3); // Row2 провер€ет на перекрытие с Row3
        CheckUncoveredCardsInRow(row1, row2); // Row1 провер€ет на перекрытие с Row2

        // ≈сли закрыта€ колода пуста, проверить возможные ходы
        if (closedDeck.childCount == 0)
        {
            CountClosedDeckPanel.SetActive(false);
            if (!CheckForPossibleMoves())
            {
                // Ќет возможных ходов, игра окончена
                GameOver();
            }
        }

        UpdateClosedDeckCount();
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

        bool canPlace = (baseValue == 2 && (cardValue == 14 || cardValue == 3)) || // “уз и король или двойка
                        (baseValue == 14 && (cardValue == 13 || cardValue == 2)) || //  ороль и дама или туз
                        (cardValue == baseValue + 1 || cardValue == baseValue - 1); // ќбычные карты

        Debug.Log($"Can place card: {canPlace}");
        return canPlace;
    }

    public bool IsBaseCard(Card card)
    {
        return card == currentBaseCard;
    }

    private void CheckUncoveredCardsInRow(Row row, Row rowBelow)
    {
        Card[] cardsInRow = row.GetComponentsInChildren<Card>();
        foreach (Card card in cardsInRow)
        {
            if (rowBelow == null || !IsCardCovered(card, rowBelow))
            {
                card.IsCovered = false;
                card.CheckIfUncovered();
            }
        }
    }

    private bool IsCardCovered(Card card, Row rowBelow)
    {
        Bounds cardBounds = card.GetBounds();
        Card[] cardsInRowBelow = rowBelow.GetComponentsInChildren<Card>();

        foreach (Card otherCard in cardsInRowBelow)
        {
            if (otherCard != card)
            {
                if (cardBounds.Intersects(otherCard.GetBounds()))
                {
                    return true;
                }
            }
        }
        return false;
    }

    // ћетод дл€ проверки возможных ходов
    private bool CheckForPossibleMoves()
    {
        List<Card> allCards = new List<Card>();
        allCards.AddRange(row1.GetComponentsInChildren<Card>());
        allCards.AddRange(row2.GetComponentsInChildren<Card>());
        allCards.AddRange(row3.GetComponentsInChildren<Card>());
        allCards.AddRange(row4.GetComponentsInChildren<Card>());

        int baseValue = currentBaseCard.value;

        foreach (Card card in allCards)
        {
            if (card.isFaceUp && CanPlaceCardOnBase(card))
            {
                return true;
            }
        }
        return false;
    }

    // ћетод дл€ открыти€ всех оставшихс€ карт
    public void RevealAllCards()
    {
        List<Card> allCards = new List<Card>();
        allCards.AddRange(row1.GetComponentsInChildren<Card>());
        allCards.AddRange(row2.GetComponentsInChildren<Card>());
        allCards.AddRange(row3.GetComponentsInChildren<Card>());
        allCards.AddRange(row4.GetComponentsInChildren<Card>());

        foreach (Card card in allCards)
        {
            card.SetFaceUp(true);
            Debug.Log("Revealed card: " + card.cardName);
        }
    }

    private void UpdateClosedDeckCount()
    {
        closedDeckCountText.text = "Cards left in deck: " + closedDeck.childCount;
    }

    // ћетод дл€ завершени€ игры
    private void GameOver()
    {
        Debug.Log("Game Over");
        gameOverPanel.SetActive(true); // ѕоказать панель Game Over
        CountClosedDeckPanel.SetActive(false);
    }
}
