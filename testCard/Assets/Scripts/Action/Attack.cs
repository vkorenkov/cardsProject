using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Attack : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        // Check for the possibility of a player move 
        if (GetComponent<CardDrop>().GameManager.IsMyTurn)
        {
            CardInfo card = eventData.pointerDrag.GetComponent<CardInfo>(); // Getting player's choice the card

            if (card && card.card.CanAttack && transform.parent == GetComponent<CardDrop>().GameManager.enemyField)
            {
                card.card.AttackState(false); // Change card attack state
                card.cardController.HighLiteOff();
                GetComponent<CardDrop>().GameManager.CardsAttack(card, GetComponent<CardInfo>()); // Initialize card attack
            }
        }
    }
}
