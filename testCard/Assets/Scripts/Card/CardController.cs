using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardController
{
    /// <summary>
    /// Card сharacteristics property
    /// </summary>
    public CardInfo CardInfo { get; set; }
    /// <summary>
    /// Card animation control property
    /// </summary>
    public CardAnimation CardAnimation { get; set; }

    /// <summary>
    /// Card controller initialization constructor
    /// </summary>
    /// <param name="cardInfo"></param>
    /// <param name="cardAnimation"></param>
    public CardController(CardInfo cardInfo, CardAnimation cardAnimation)
    {
        CardInfo = cardInfo;
        CardAnimation = cardAnimation;
    }

    /// <summary>
    /// Hiding enemy card data
    /// </summary>
    /// <param name="card"></param>
    public void HideEnemyCards(CardModel card)
    {
        CardInfo.card = card;

        CardInfo.HideCard.SetActive(true); // Visually hiding the enemy map

        CardInfo.manaImage.SetActive(false); // Visually hiding the enemy mana
    }

    /// <summary>
    /// Setting Card Characteristics for Display
    /// </summary>
    /// <param name="card"></param>
    public void SetCardInfo(CardModel card)
    {
        CardInfo.card = card;

        if (card != null) CardInfo.cardOutput.SetCardStats(card); // Writing Card Characteristics to Display Variables

        CardInfo.HideCard.SetActive(false); // Disable hide for player cards

        RefreshCardData();
    }

    /// <summary>
    /// Updating card data at the end of a turn
    /// </summary>
    public void RefreshCardData()
    {
        if (CardInfo.card != null) CardInfo.cardOutput.RefreshCardStats(CardInfo.card);
    }

    /// <summary>
    /// Turn on the card backlight
    /// </summary>
    public void HighLiteOn()
    {
        CardInfo.highLiteCard.SetActive(true);
    }

    /// <summary>
    /// Turn off the card backlight
    /// </summary>
    public void HighLiteOff()
    {
        CardInfo.highLiteCard.SetActive(false);
    }

    /// <summary>
    /// Launching the "death" card
    /// </summary>
    public void CardDeath()
    {
        CardAnimation.CardDeathAnimation();
    }

    /// <summary>
    /// Check for card availability depending on the player's remaining mana
    /// </summary>
    /// <param name="currentMana"></param>
    /// <param name="card"></param>
    public void CheckForAvalible(int currentMana, CardInfo card)
    {
        card.GetComponent<CanvasGroup>().alpha = currentMana >= card.card.Manacost ? 1 : 0.6f;
    }
}
