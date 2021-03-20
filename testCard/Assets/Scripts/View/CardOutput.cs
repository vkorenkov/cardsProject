using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CardOutput : MonoBehaviour
{
    /// <summary>
    /// Fields for displaying card information on the screen
    /// </summary>
    public TextMeshProUGUI cardName, attack, defence, manacost, damage;

    /// <summary>
    /// Setting text fields for displaying name and mana values
    /// </summary>
    /// <param name="card"></param>
    public void SetCardStats(CardModel card)
    {
        cardName.text = card.Name;
        manacost.text = card.Manacost.ToString();
    }

    /// <summary>
    /// Updating card informationы
    /// </summary>
    /// <param name="card"></param>
    public void RefreshCardStats(CardModel card)
    {
        attack.text = card.Attack.ToString();
        defence.text = card.Defence.ToString();
    }
}
