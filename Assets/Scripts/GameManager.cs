using UnityEngine;
using System.Collections.Generic;
using System;

public class GameManager : MonoBehaviour
{
    public GameObject cardPrefab;
    public Row row1;
    public Row row2;
    public Row row3;
    public Row row4;
    public Transform closedDeck;
    public Transform baseCard;

    private List<Card> deck = new List<Card>();

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
            foreach (string rank in ranks)
            {
                string cardName = $"{suit}_{rank}";
                Sprite frontSprite = Resources.Load<Sprite>($"Sprites/{cardName}");
                if (frontSprite != null)
                {
                    GameObject cardObject = Instantiate(cardPrefab);
                    Card card = cardObject.GetComponent<Card>();
                    int value = Array.IndexOf(ranks, rank) + 1;
                    card.SetCard(value, suit, frontSprite, backSprite, false);
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

        // Closed Deck
        int closedDeckOrder = 1;
        for (int i = index; i < deck.Count - 1; i++)
        {
            deck[i].transform.SetParent(closedDeck);
            deck[i].transform.localPosition = Vector3.zero;
            deck[i].SetFaceUp(false);
            deck[i].GetComponent<SpriteRenderer>().sortingOrder = closedDeckOrder;
            deck[i].gameObject.SetActive(true);
            closedDeckOrder++;
        }

        // Base Card
        deck[deck.Count - 1].transform.SetParent(baseCard);
        deck[deck.Count - 1].transform.localPosition = Vector3.zero;
        deck[deck.Count - 1].SetFaceUp(true);
        deck[deck.Count - 1].gameObject.SetActive(true);
    }
}
