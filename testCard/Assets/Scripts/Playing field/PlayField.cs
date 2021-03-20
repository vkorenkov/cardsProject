using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Field category enumeration
/// </summary>
public enum FieldTipe
{
    MY_HAND,
    MY_FIELD,
    ENEMY_HAND,
    ENEMY_FIELD
}

public class PlayField : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    /// <summary>
    /// Field category enumeration field
    /// </summary>
    public FieldTipe fieldTipe;

    /// <summary>
    /// Card move class field
    /// </summary>
    CardDrop cardDrop;

    public void OnDrop(PointerEventData eventData)
    {
        // Checking that the card is over the player's field
        if (fieldTipe != FieldTipe.ENEMY_FIELD &&
            fieldTipe != FieldTipe.ENEMY_HAND &&
            fieldTipe != FieldTipe.MY_HAND)
        {
            GetCardDrop(eventData);

            if (cardDrop)
            {
                var manager = cardDrop.GameManager;

                // Checking that it is the player's turn now, 
                // there are no more than 6 cards on the player's field, 
                // enough mana, and the card is not played
                if (cardDrop.GameManager.IsMyTurn &&
                manager.currentGame.myFieldCards.Count < 6 &&
                manager.player.Mana >= cardDrop.GetComponent<CardInfo>().card.Manacost &&
                !cardDrop.GetComponent<CardInfo>().card.isPlayed)
                {
                    manager.currentGame.myHandCards.Remove(cardDrop.GetComponent<CardInfo>()); // Removing a card from a player's hand
                    manager.currentGame.myFieldCards.Add(cardDrop.GetComponent<CardInfo>()); // Adding a card to the player's field
                    cardDrop.defaultParent = transform; // Assigning a new parent to the transferred card

                    manager.ReduceMana(true, cardDrop.GetComponent<CardInfo>().card.Manacost); // Changing the player's mana value
                    cardDrop.GetComponent<CardInfo>().card.isPlayed = true; // card playedы

                    cardDrop.GameManager.CheckAvalible();
                    cardDrop.GetComponent<CardInfo>().cardController.HighLiteOff();

                    if (fieldTipe == FieldTipe.MY_FIELD) cardDrop.inField = true; // An indication that the card is now on the player's board
                }
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Check that the cursor is dragging the card, 
        // the field is not the opponent's field, 
        // the opponent's hand, the player's handы
        if (eventData.CheckForNull() &&
            fieldTipe != FieldTipe.ENEMY_FIELD &&
            fieldTipe != FieldTipe.ENEMY_HAND &&
            fieldTipe != FieldTipe.MY_HAND)
        {
            GetCardDrop(eventData);

            if (cardDrop && cardDrop.GameManager.IsMyTurn)
                cardDrop.defaultTempCardParent = transform; // Assigning a parent to a temporary card
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (eventData.CheckForNull())
        {
            GetCardDrop(eventData);

            if (cardDrop &&
                cardDrop.GameManager.IsMyTurn &&
                cardDrop.defaultTempCardParent == transform)
            {
                cardDrop.defaultTempCardParent = cardDrop.defaultParent;
            }
        }
    }

    /// <summary>
    /// Method for getting the card move field
    /// </summary>
    /// <param name="eventData"></param>
    void GetCardDrop(PointerEventData eventData)
    {
        cardDrop = eventData.pointerDrag.GetComponent<CardDrop>();
    }
}
