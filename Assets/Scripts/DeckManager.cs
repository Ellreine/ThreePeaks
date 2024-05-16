using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    public GameObject cardPrefab;
    private List<Card> deck = new List<Card>();

    public List<Card> InitializeDeck()
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
        return deck;
    }

    public void ShuffleDeck(List<Card> deck)
    {
        for (int i = 0; i < deck.Count; i++)
        {
            Card temp = deck[i];
            int randomIndex = Random.Range(0, deck.Count);
            deck[i] = deck[randomIndex];
            deck[randomIndex] = temp;
        }
    }
}
