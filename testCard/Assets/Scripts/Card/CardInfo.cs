using UnityEngine;

public class CardInfo : MonoBehaviour
{
    /// <summary>
    /// Card characteristics field
    /// </summary>
    public CardModel card;
    /// <summary>
    /// Class field for displaying map characteristics on the screen
    /// </summary>
    public CardOutput cardOutput;
    /// <summary>
    /// mana image field
    /// </summary>
    public GameObject manaImage;
    /// <summary>
    /// Image field hiding the card
    /// </summary>
    public GameObject HideCard;
    /// <summary>
    /// Map highlight object field
    /// </summary>
    public GameObject highLiteCard;
    /// <summary>
    /// Card controller variable
    /// </summary>
    public CardController cardController;
    public CardAnimation cardAnimation;

    private void Awake()
    {
        cardOutput = GetComponent<CardOutput>();
        cardAnimation = GetComponent<CardAnimation>();

        cardController = new CardController(this, cardAnimation);
    }

    /// <summary>
    /// Destroying an object in the scene
    /// </summary>
    public void DestroyThisCard()
    {
        Destroy(gameObject);
    }
}
