using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AttackHero : MonoBehaviour, IDropHandler
{
    /// <summary>
    /// Character category enumeration
    /// </summary>
    public enum HeroType
    {
        ENEMY,
        PLAYER
    }

    /// <summary>
    /// Hero damage animation field
    /// </summary>
    [SerializeField] Animation heroDamageAnimation;

    /// <summary>
    /// Character category enumeration field
    /// </summary>
    public HeroType heroType;

    /// <summary>
    /// Current session management class field
    /// </summary>
    public GameManagerScript gameManager;

    public void OnDrop(PointerEventData eventData)
    {
        if (!gameManager.IsMyTurn)
            return;

        CardInfo card = eventData.pointerDrag.GetComponent<CardInfo>(); // Retrieving information of a dragged card

        // Enemy damage registration condition
        if (card && card.card.CanAttack && heroType == HeroType.ENEMY)
        {
            card.card.CanAttack = false; // Setting the card to be unable to attack until the next turn
            gameManager.HeroDamage(card, true); // Damage to the enemy
            heroDamageAnimation.Play(); // Start animation of damage on the hero
        }
    }
}
