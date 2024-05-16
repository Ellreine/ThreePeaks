using System.Collections.Generic;
using UnityEngine;

public class Row : MonoBehaviour
{
    public int groupSize = 2; // ���������� ���� � ������
    public float groupOffset = 2.0f; // ������ ����� �������� ����
    public float defaultOffset = 1.5f; // ����������� ������ ����� ������� ������ ������
    public bool isFaceUp = false; // ����������, ������ �� ����� ���� ������� ������� ��������

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
                card.gameObject.SetActive(true); // ���������� �����
                positionX += defaultOffset;
                cardIndex++;
            }
            positionX += groupOffset; // ��������� ������ ����� ������
        }
    }
}
