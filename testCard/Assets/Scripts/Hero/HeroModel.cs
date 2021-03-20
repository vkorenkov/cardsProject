using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroModel
{
    /// <summary>
    /// Character health property
    /// </summary>
    public int Health { get; set; }

    /// <summary>
    /// Character mana property
    /// </summary>
    public int Mana { get; set; }

    /// <summary>
    /// Character initialization constructor
    /// </summary>
    /// <param name="health"></param>
    /// <param name="mana"></param>
    public HeroModel(int health, int mana)
    {
        Health = health;
        Mana = mana;
    }
}
