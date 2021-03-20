using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public static class CheckEventData
{
    /// <summary>
    /// (Extension) Checking PointerEventData for emptiness
    /// </summary>
    /// <param name="eventData"></param>
    /// <returns></returns>
    public static bool CheckForNull(this PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
            return true;
        else
            return false;
    }
}
