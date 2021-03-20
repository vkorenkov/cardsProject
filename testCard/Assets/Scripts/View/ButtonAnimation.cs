using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonAnimation : MonoBehaviour
{
    /// <summary>
    /// Button animation field
    /// </summary>
    [SerializeField] Animation buttonAnimation;

    /// <summary>
    /// Method for triggering button animationы
    /// </summary>
    public void StartButtonAnimation()
    {
        buttonAnimation.Play();
    }
}
