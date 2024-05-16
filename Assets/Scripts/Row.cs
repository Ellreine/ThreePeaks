using System.Collections.Generic;
using UnityEngine;

public class Row : MonoBehaviour
{
    public int groupSize = 2; // Количество карт в группе
    public float groupOffset = 2.0f; // Отступ между группами карт
    public float defaultOffset = 1.5f; // Стандартный отступ между картами внутри группы
    public bool isFaceUp = false; // Определяет, должны ли карты быть открыты лицевой стороной

    public void PlaceCards(List<Card> cards, int orderInLayer)
    {
        float positionX = 0;
        int cardIndex = 0;

        while (cardIndex < cards.Count)
        {
            for (int i = 0; i < groupSize && cardIndex < cards.Count; i++)
            {
                Card card = cards[cardIndex];
                card.transform.SetParent(transform);
                card.transform.localPosition = new Vector3(positionX, 0, 0);
                card.SetFaceUp(isFaceUp);
                card.GetComponent<SpriteRenderer>().sortingOrder = orderInLayer;
                card.gameObject.SetActive(true); // Активируем карту
                positionX += defaultOffset;
                cardIndex++;
            }
            positionX += groupOffset; // Добавляем отступ после группы
        }
    }
}
