using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextOutput : MonoBehaviour
{
    /// <summary>
    /// Fields for displaying game information on the screen
    /// </summary>
    public TextMeshProUGUI turnTimeOut,
                            myHealthText,
                            myDamageText,
                            enemyHealthText,
                            enemyDamageText,
                            myManaText,
                            enemyManaText,
                            ResultText,
                            turnText;

    /// <summary>
    /// Setting the value of an output timer text box
    /// </summary>
    /// <param name="time"></param>
    public void SetOutTurnTime(float time)
    {
        // With an incoming value of 0, the timer displays an empty string, since there is an imitation of the opponent's turn
        turnTimeOut.text = time != 0 ? time.ToString() : string.Empty;
    }

    /// <summary>
    /// Setting the value of the text field of player's and enemy's health
    /// </summary>
    /// <param name="playerHealth"></param>
    /// <param name="enemyHealth"></param>
    public void SetOutHealth(int playerHealth, int enemyHealth)
    {
        myHealthText.text = playerHealth.ToString();
        enemyHealthText.text = enemyHealth.ToString();
    }

    /// <summary>
    /// Setting the value of the text field of player's and enemy's mana
    /// </summary>
    /// <param name="playerMana"></param>
    /// <param name="enemyMana"></param>
    public void SetOutMana(int playerMana, int enemyMana)
    {
        myManaText.text = playerMana.ToString();
        enemyManaText.text = enemyMana.ToString();
    }

    /// <summary>
    /// Setting the value of the text field of game result
    /// </summary>
    /// <param name="result"></param>
    public void SetOutBattleResult(string result)
    {
        // Setting the color depending on the result of the game
        if (result.ToLower().Contains("lose"))
            ResultText.color = Color.red;
        else
            ResultText.color = Color.yellow;

        ResultText.text = result;
    }
}
