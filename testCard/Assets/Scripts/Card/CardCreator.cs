using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class CardHolder
{
    public static List<CardModel> AllCardsInGame = new List<CardModel>();
}

public class CardCreator : MonoBehaviour
{
    private void Awake()
    {
        // Create cards with random characteristics
        for (int i = 0; i < 60; i++)
            CardHolder.AllCardsInGame.Add(new CardModel(Path.GetRandomFileName(), Random.Range(2, 6), Random.Range(2, 11), Random.Range(1, 6)));
    }
}
