using UnityEngine;

public class CardTrigger : MonoBehaviour
{
    public Card parentCard;
    private int overlapCount = 0;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Card"))
        {
            overlapCount++;
            parentCard.IsCovered = true;
            Debug.Log("Card is now covered: " + parentCard.cardName + " by " + collision.transform.parent.GetComponent<Card>().cardName);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Card"))
        {
            overlapCount--;
            if (overlapCount <= 0)
            {
                parentCard.IsCovered = false;
                parentCard.CheckIfUncovered();
                Debug.Log("Card is now uncovered: " + parentCard.cardName + " after " + collision.transform.parent.GetComponent<Card>().cardName + " moved away");
            }
        }
    }
}
