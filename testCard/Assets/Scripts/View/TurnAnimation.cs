using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnAnimation : MonoBehaviour
{
    /// <summary>
    /// Change turn animation field
    /// </summary>
    public static Animation turnAnimation;

    /// <summary>
    /// Current session management class field
    /// </summary>
    [SerializeField] GameManagerScript gameManager;

    private void Awake()
    {
        turnAnimation = GetComponent<Animation>();
    }

    /// <summary>
    /// Start change turn animation
    /// </summary>
    public static void StartTurnAnim()
    {
        turnAnimation.Play();
    }

    /// <summary>
    /// Start the turn coroutine
    /// </summary>
    public void StartCor()
    {
        StopAllCoroutines();
        StartCoroutine(gameManager.TurnCor());
    }
}
