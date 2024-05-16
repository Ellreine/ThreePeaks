using UnityEngine;

public class CardTrigger : MonoBehaviour
{
    public Card parentCard;
    public bool isBottomTrigger;
    private int overlapCount = 0;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isBottomTrigger && collision.CompareTag("TopTrigger"))
        {
            overlapCount++;
            parentCard.IsCovered = true;
            Debug.Log("Card is now covered: " + parentCard.cardName);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (isBottomTrigger && collision.CompareTag("TopTrigger"))
        {
            overlapCount--;
            if (overlapCount <= 0)
            {
                parentCard.IsCovered = false;
                parentCard.CheckIfUncovered();
                Debug.Log("Card is now uncovered: " + parentCard.cardName);
            }
        }
    }
}
