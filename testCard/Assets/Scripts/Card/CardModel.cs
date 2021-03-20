using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardModel
{
    /// <summary>
    /// Card name property
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// Card attack property
    /// </summary>
    public int Attack { get; set; }
    /// <summary>
    /// Card health property
    /// </summary>
    public int Defence { get; set; }
    /// <summary>
    /// Card mana cost property
    /// </summary>
    public int Manacost { get; set; }
    /// <summary>
    /// Can the card attack
    /// </summary>
    public bool CanAttack { get; set; }
    /// <summary>
    /// Is the card alive
    /// </summary>
    public bool isAlive
    {
        get => Defence > 0;
    }
    /// <summary>
    /// Is the card played
    /// </summary>
    public bool isPlayed;

    /// <summary>
    /// Card model initialization constructor
    /// </summary>
    /// <param name="name"></param>
    /// <param name="attack"></param>
    /// <param name="defence"></param>
    /// <param name="manacost"></param>
    public CardModel(string name, int attack, int defence, int manacost)
    {
        Name = name;
        Attack = attack;
        Defence = defence;
        Manacost = manacost;
        CanAttack = false;
    }

    /// <summary>
    /// Setting the Attack Opportunity
    /// </summary>
    /// <param name="attack"></param>
    public void AttackState(bool attack)
    {
        CanAttack = attack;
    }

    /// <summary>
    /// Damage taken by the card
    /// </summary>
    /// <param name="damage"></param>
    public void GetDamage(int damage)
    {
        Defence -= damage;
    }
}
