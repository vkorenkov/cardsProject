using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManagerScript : MonoBehaviour
{
    /// <summary>
    /// text output field 
    /// </summary>
    TextOutput textOutput;
    /// <summary>
    /// Current game deck field 
    /// </summary>
    public CardLists currentGame;

    public Transform myHand, enemyHand, myField, enemyField; // Variables of playing fields 
    /// <summary>
    /// Card template
    /// </summary>
    public GameObject CardPrefab; 
    public AttackHero playerHero, enemyHero; // Variable player hero 
    /// <summary>
    /// Animation of game result
    /// </summary>
    [SerializeField] Animation resultAnimation;

    float turnTime, turn; // Variables to turn timer and turn count

    /// <summary>
    /// The enemy data model
    /// </summary>
    public HeroModel enemy;
    /// <summary>
    /// The player data model
    /// </summary>
    public HeroModel player; 

    [SerializeField] Button endTurnButton;

    int defaultManaCount = 10;

    /// <summary>
    /// Player turn property 
    /// </summary>
    public bool IsMyTurn
    {
        get => turn % 2 == 0;
    }

    private void Awake()
    {
        enemy = new HeroModel(30, defaultManaCount);
        player = new HeroModel(30, defaultManaCount);
    }

    private void Start()
    {
        currentGame = new CardLists();

        textOutput = GetComponent<TextOutput>();

        turnTime = 30;

        FillHand(currentGame.myDeckList, myHand);
        FillHand(currentGame.enemyDeckList, enemyHand);

        textOutput.SetOutMana(player.Mana, enemy.Mana);
    }

    /// <summary>
    /// Filling the enemy's and player's hand with cards 
    /// </summary>
    /// <param name="deck"></param>
    /// <param name="hand"></param>
    void FillHand(List<CardModel> deck, Transform hand)
    {
        int i = 0;
        while (i++ < 6)
            GetCard(deck, hand);
    }

    /// <summary>
    /// Get new card from deck
    /// </summary>
    /// <param name="deck"></param>
    /// <param name="hand"></param>
    void GetCard(List<CardModel> deck, Transform hand)
    {
        if (deck.Count == 0)
            return;

        CardModel card = deck[0]; // Get first card from deck

        GameObject newCard = Instantiate(CardPrefab, hand, false); // Creating an empty map in the scene 

        // Set and hide enemy cards
        if (hand == enemyHand)
        {
            newCard.GetComponent<CardInfo>().cardController.HideEnemyCards(card); // Hide an enemy's card
            currentGame.enemyHandCards.Add(newCard.GetComponent<CardInfo>()); // Add card to enemy's hand
        }
        else // Set player cards
        {
            newCard.GetComponent<CardInfo>().cardController.SetCardInfo(card); // Setting the characteristics of a player's card 
            currentGame.myHandCards.Add(newCard.GetComponent<CardInfo>()); // Add card to a player's hand
            newCard.GetComponent<Attack>().enabled = false;
        }

        deck.RemoveAt(0); // remove card from deck
    }

    /// <summary>
    /// Turn coroutine
    /// </summary>
    /// <returns></returns>
    public IEnumerator TurnCor()
    {
        turnTime = 30;
        textOutput.SetOutTurnTime(turnTime); // Set timer view

        // Disable cards highLite
        foreach (var card in currentGame.myFieldCards)
            card.cardController.HighLiteOff();

        CheckAvalible();

        if (IsMyTurn)
        {
            // Change elayer cards attack state
            foreach (var card in currentGame.myFieldCards)
            {
                card.card.AttackState(true); 
                card.cardController.HighLiteOn(); // Enable cards highLite
            }

            // Change timer every second 
            while (turnTime-- > 0)
            {
                textOutput.SetOutTurnTime(turnTime);
                yield return new WaitForSeconds(1);
            }

            ChangeTurn();
        }
        else
        {
            // Change enemy cards attack state
            foreach (var card in currentGame.enemyFieldCards)
                card.card.AttackState(true);

            if (currentGame.enemyHandCards.Count > 0)
                StartCoroutine(EnemyTurn(currentGame.enemyHandCards)); // Start enemy turn coroutine
        }
      
    }

    /// <summary>
    /// Emulates the opponent's turn 
    /// </summary>
    /// <param name="cards"></param>
    /// <returns></returns>
    IEnumerator EnemyTurn(List<CardInfo> cards)
    {
        textOutput.SetOutTurnTime(0); // Set the егкт timer to 0, as this method only emulates the opponent's turn 

        yield return new WaitForSeconds(1);

        int count = Random.Range(0, cards.Count + 1); // Choose a random number of cards 

        for (int i = 0; i < count; i++)
        {
            // exit the method if there are more than 5 cards on the opponent's field, or there is not enough mana, or there are no cards 
            if (currentGame.enemyFieldCards.Count > 5 || enemy.Mana == 0 || currentGame.enemyHandCards.Count == 0)
                break;

            List<CardInfo> enemyCardList = cards.FindAll(c => enemy.Mana >= c.card.Manacost); // Get all cards, that have enough mana 

            if (enemyCardList.Count == 0)
                break;

            cards[0].GetComponent<CardDrop>().MoveToField(enemyField); // Move an opponent's card from hand to the field 

            ReduceMana(false, enemyCardList[0].card.Manacost);

            yield return new WaitForSeconds(0.31f);

            enemyCardList[0].cardController.SetCardInfo(enemyCardList[0].card); // Setting the characteristics of an enemy's card 
            enemyCardList[0].transform.SetParent(enemyField);

            currentGame.enemyFieldCards.Add(enemyCardList[0]);
            currentGame.enemyHandCards.Remove(enemyCardList[0]);
        }

        yield return new WaitForSeconds(1);

        // Get cards, that can attack 
        foreach (var card in currentGame.enemyFieldCards.FindAll(c => c.card.CanAttack))
        {
            // Attack the player's cards if there is 0 and there are more than 0 cards on the player's field 
            if (Random.Range(0, 2) == 0 && currentGame.myFieldCards.Count > 0)
            {
                var enemy = currentGame.myFieldCards[Random.Range(0, currentGame.myFieldCards.Count)]; // Random card attacked 

                card.card.AttackState(false);

                card.GetComponent<CardDrop>().MoveToTarget(enemy.transform); // Movement of an opponent's card to a player's card 

                yield return new WaitForSeconds(0.2f);

                CardsAttack(enemy, card); // Performing a card attack 
            }
            else
            {
                card.card.AttackState(false);

                card.GetComponent<CardDrop>().MoveToTarget(playerHero.transform); // Movement of an opponent's card to a player's hero 

                textOutput.myDamageText.text = $"-{card.card.Attack}";

                yield return new WaitForSeconds(0.2f);

                playerHero.GetComponent<Animation>().Play(); // Play animation of taking damage 

                yield return new WaitForSeconds(0.75f);

                HeroDamage(card, false); // Taking hero damage 
            }

            yield return new WaitForSeconds(0.2f);
        }

        yield return new WaitForSeconds(1);

        ChangeTurn();
    }

    /// <summary>
    /// Change of turn between players 
    /// </summary>
    public void ChangeTurn()
    {
        foreach (var card in currentGame.myFieldCards)
            card.cardController.HighLiteOff();

        StopAllCoroutines();

        turn++;
        endTurnButton.interactable = IsMyTurn; // Setting the button mode depending on the sequence of turn 

        if (IsMyTurn)
        {
            TakeNewCard();

            player.Mana = defaultManaCount; // Setting the default mana value for the next turn 
            enemy.Mana = defaultManaCount;  //

            textOutput.SetOutMana(player.Mana, enemy.Mana); // Setting the display of mana 

            textOutput.turnText.text = "Your turn";
        }
        else
        {
            textOutput.turnText.text = "Enemy turn";            
        }

        TurnAnimation.StartTurnAnim(); // Start change turn animation
    }

    /// <summary>
    /// Take a new card to hand 
    /// </summary>
    void TakeNewCard()
    {
        GetCard(currentGame.enemyDeckList, enemyHand);
        GetCard(currentGame.myDeckList, myHand);
    }

    /// <summary>
    /// Method of performing card attack 
    /// </summary>
    /// <param name="myCard"></param>
    /// <param name="enemyCard"></param>
    public void CardsAttack(CardInfo myCard, CardInfo enemyCard)
    {
        myCard.card.GetDamage(enemyCard.card.Attack); // Player card damage 
        myCard.cardOutput.damage.text = $"-{enemyCard.card.Attack}"; // display of damage done 
        myCard.GetComponent<CardAnimation>().DamageAnimation(); // Starting damage animation to the card 
        enemyCard.card.GetDamage(myCard.card.Attack); // Enemy card damage 
        enemyCard.cardOutput.damage.text = $"-{myCard.card.Attack}"; // display of damage done 
        enemyCard.GetComponent<CardAnimation>().DamageAnimation(); // Starting damage animation to the card 

        if (!myCard.card.isAlive)
            DisableCard(myCard);
        else
            myCard.cardController.RefreshCardData(); // Refresh card characteristics 

        if (!enemyCard.card.isAlive)
            DisableCard(enemyCard);
        else
            enemyCard.cardController.RefreshCardData();// Refresh card characteristics 
    }

    /// <summary>
    /// Deleting an object upon "death" 
    /// </summary>
    /// <param name="card"></param>
    void DisableCard(CardInfo card)
    {
        card.GetComponent<CardDrop>().OnEndDrag(null);

        if (currentGame.myFieldCards.Exists(c => c == card))
            currentGame.myFieldCards.Remove(card); // Removing a card from the list of a player's field

        if (currentGame.enemyFieldCards.Exists(c => c == card))
            currentGame.enemyFieldCards.Remove(card);// Removing a card from the list of a enemy's field

        card.cardController.CardDeath();
    }

    /// <summary>
    /// Mana value change 
    /// </summary>
    /// <param name="isMyMana"></param>
    /// <param name="manacost"></param>
    public void ReduceMana(bool isMyMana, int manacost)
    {
        if (isMyMana)
            player.Mana = Mathf.Clamp(player.Mana - manacost, 0, int.MaxValue);
        else
            enemy.Mana = Mathf.Clamp(enemy.Mana - manacost, 0, int.MaxValue);

        textOutput.SetOutMana(player.Mana, enemy.Mana);
    }

    /// <summary>
    /// Taking damage by the player's hero 
    /// </summary>
    /// <param name="card"></param>
    /// <param name="isEnemy"></param>
    public void HeroDamage(CardInfo card, bool isEnemy)
    {
        if (isEnemy)
        {
            textOutput.enemyDamageText.text = $"-{card.card.Attack}";
            enemy.Health = Mathf.Clamp(enemy.Health - card.card.Attack, 0, int.MaxValue);
        }
        else
        {
            player.Health = Mathf.Clamp(player.Health - card.card.Attack, 0, int.MaxValue);
        }

        textOutput.SetOutHealth(player.Health, enemy.Health);

        card.cardController.HighLiteOff();

        BattleResult();
    }

    /// <summary>
    /// Method of checking battle results 
    /// </summary>
    void BattleResult()
    {
        if(player.Health == 0 || enemy.Health == 0)
        {
            StopAllCoroutines();

            DisableFields();

            TurnAnimation.turnAnimation.enabled = false; // turn off animation of change of course 

            if (enemy.Health <= 0)
                textOutput.SetOutBattleResult("You Win");
            else
                textOutput.SetOutBattleResult("You lose");

            resultAnimation.Play(); // Running Result Screen Animation ы
        }
    }

    /// <summary>
    /// The method checks if there is enough mana to select a card 
    /// </summary>
    public void CheckAvalible()
    {
        foreach (var card in currentGame.myHandCards)
            card.cardController.CheckForAvalible(player.Mana, card);
    }

    /// <summary>
    /// Disabling all fields in the game 
    /// </summary>
    void DisableFields()
    {
        myField.gameObject.SetActive(false);
        enemyField.gameObject.SetActive(false);
        myHand.gameObject.SetActive(false);
        enemyHand.gameObject.SetActive(false);
    }
        
}
