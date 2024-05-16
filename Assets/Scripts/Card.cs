using UnityEngine;

public class Card : MonoBehaviour
{
    public int value;
    public string suit;
    public string cardName;
    public Sprite frontSprite;
    public Sprite backSprite;
    public bool isFaceUp;
    public bool IsCovered { get; set; } // Используется для другой механики

    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateCardAppearance();
    }

    public void SetCard(int value, string suit, string cardName, Sprite frontSprite, Sprite backSprite, bool isFaceUp)
    {
        this.value = value;
        this.suit = suit;
        this.cardName = cardName;
        this.frontSprite = frontSprite;
        this.backSprite = backSprite;
        this.isFaceUp = isFaceUp;
        IsCovered = true;
        UpdateCardAppearance();
    }

    public void SetFaceUp(bool isFaceUp)
    {
        this.isFaceUp = isFaceUp;
        UpdateCardAppearance();
        Debug.Log("SetFaceUp called: " + cardName + " isFaceUp: " + isFaceUp);
    }

    public void FlipCard()
    {
        isFaceUp = !isFaceUp;
        UpdateCardAppearance();
    }

    public void CheckIfUncovered()
    {
        if (!IsCovered)
        {
            SetFaceUp(true);
            Debug.Log("Card uncovered: " + cardName);
        }
    }

    private void UpdateCardAppearance()
    {
        spriteRenderer.sprite = isFaceUp ? frontSprite : backSprite;
    }

    void OnMouseDown()
    {
        Debug.Log("Card clicked: " + cardName);
        if (isFaceUp && !GameManager.Instance.IsBaseCard(this))
        {
            Debug.Log("Card is face up and not the base card: " + cardName);
            GameManager.Instance.OnCardClicked(this);
        }
        else if (transform.parent == GameManager.Instance.closedDeck)
        {
            Debug.Log("Card clicked from closed deck: " + cardName);
            GameManager.Instance.OnClosedDeckCardClicked(this);
        }
        else
        {
            Debug.Log("Card cannot be clicked: " + cardName + " isFaceUp: " + isFaceUp);
        }
    }
}
