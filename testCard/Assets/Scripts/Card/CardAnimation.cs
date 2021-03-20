using UnityEngine;
using UnityEngine.EventSystems;

public class CardAnimation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    /// <summary>
    /// Card animator variable
    /// </summary>
    public Animator cardAnimator;

    /// <summary>
    /// The position of the card before the player selects
    /// </summary>
    Vector3 DefaultPosition;

    /// <summary>
    /// Card move class variable
    /// </summary>
    CardDrop cardDrop;

    /// <summary>
    /// Game fields class variable
    /// </summary>
    PlayField playField;

    bool moveUp;

    void Awake()
    {
        cardDrop = GetComponent<CardDrop>();
    }

    private void Update()
    {
        // Move the card higher for easier viewing
        if (moveUp) transform.position = Vector3.Lerp(transform.position, DefaultPosition + Vector3.up, 0.05f);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        playField = GetComponentInParent<PlayField>(); // Getting the field where the card is currently placed

        if (!cardDrop.inField && playField.fieldTipe != FieldTipe.ENEMY_HAND && playField.fieldTipe != FieldTipe.ENEMY_FIELD)
        {
            DefaultPosition = transform.position; // Getting the initial position of the card in the player's hand

            moveUp = true;

            cardAnimator.SetBool("IsMouseOver", true); // Starting a map view animation
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!cardDrop.inField && playField.fieldTipe != FieldTipe.ENEMY_HAND && playField.fieldTipe != FieldTipe.ENEMY_FIELD)
        {
            transform.position = DefaultPosition; // Applying the original card position

            moveUp = false;

            cardAnimator.SetBool("IsMouseOver", false); // Start the animation for exiting the card view
        }
    }

    /// <summary>
    /// Method for starting the animation of taking damage
    /// </summary>
    public void DamageAnimation()
    {
        cardAnimator.SetTrigger("IsAttacked");
    }

    /// <summary>
    /// Method for starting the animation of the "death" of the card
    /// </summary>
    public void CardDeathAnimation()
    {
        cardAnimator.SetTrigger("Death");
    }
}
