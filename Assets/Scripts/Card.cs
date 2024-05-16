using UnityEngine;

public class Card : MonoBehaviour
{
    public int value;
    public string suit;
    public string cardName;
    public Sprite frontSprite;
    public Sprite backSprite;
    public bool isFaceUp;

    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateCardAppearance();
    }

    public void SetCard(int value, string suit, Sprite frontSprite, Sprite backSprite, bool isFaceUp)
    {
        this.value = value;
        this.suit = suit;
        this.cardName = $"{value}_{suit}";
        this.frontSprite = frontSprite;
        this.backSprite = backSprite;
        this.isFaceUp = isFaceUp;
        UpdateCardAppearance();
    }

    public void SetFaceUp(bool isFaceUp)
    {
        this.isFaceUp = isFaceUp;
        UpdateCardAppearance();
    }

    public void FlipCard()
    {
        isFaceUp = !isFaceUp;
        UpdateCardAppearance();
    }

    private void UpdateCardAppearance()
    {
        spriteRenderer.sprite = isFaceUp ? frontSprite : backSprite;
    }
}
