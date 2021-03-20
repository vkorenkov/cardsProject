using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardLists
{
    /// <summary>
    /// Player deck
    /// </summary>
    public List<CardModel> myDeckList;

    /// <summary>
    /// Enemy deck
    /// </summary>
    public List<CardModel> enemyDeckList;

    /// <summary>
    /// List of player hand cards
    /// </summary>
    public List<CardInfo> myHandCards = new List<CardInfo>();
    /// <summary>
    /// List of player field cards
    /// </summary>
    public List<CardInfo> myFieldCards = new List<CardInfo>();
    /// <summary>
    /// List of enemy hand cards
    /// </summary>
    public List<CardInfo> enemyHandCards = new List<CardInfo>();
    /// <summary>
    /// List of enemy field cards
    /// </summary>
    public List<CardInfo> enemyFieldCards = new List<CardInfo>();

    public CardLists()
    {
        myDeckList = GetDeckCard();
        enemyDeckList = GetDeckCard();
    }

    /// <summary>
    /// Deck filling method
    /// </summary>
    /// <returns></returns>
    List<CardModel> GetDeckCard()
    {
        List<CardModel> TempCardList = new List<CardModel>();

        for(int i = 0; i < 10; i++)
            TempCardList.Add(CardHolder.AllCardsInGame[Random.Range(0, CardHolder.AllCardsInGame.Count)]);

        return TempCardList;
    }
}
