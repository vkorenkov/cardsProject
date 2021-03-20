using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class CardDrop : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    /// <summary>
    /// Main camera field
    /// </summary>
    public new Camera camera;

    /// <summary>
    /// Variable offset of the cursor relative to the dragged object
    /// </summary>
    Vector3 offset;

    /// <summary>
    /// Object parent field
    /// </summary>
    public Transform defaultParent, defaultTempCardParent;

    /// <summary>
    /// Temporary card field
    /// </summary>
    public GameObject TempCardGameObj;

    /// <summary>
    /// Current session management class field
    /// </summary>
    public GameManagerScript GameManager;

    /// <summary>
    /// The field indicates that the card is on the player's field
    /// </summary>
    public bool inField;

    /// <summary>
    /// The field indicates whether the player can move the card
    /// </summary>
    public bool isMovable;

    private void Awake()
    {
        camera = Camera.main;

        TempCardGameObj = GameObject.Find("TempCard");

        GameManager = FindObjectOfType<GameManagerScript>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        offset = transform.position - camera.ScreenToWorldPoint(eventData.position); // Calculating the offset of the cursor relative to the center of the dragged object

        defaultParent = defaultTempCardParent = transform.parent; // Initializing the fields of the card's parents

        // Conditions under which a player can move a card
        isMovable = GameManager.IsMyTurn && 
            ((defaultParent.GetComponent<PlayField>().fieldTipe == FieldTipe.MY_HAND && GameManager.player.Mana >= GetComponent<CardInfo>().card.Manacost) || 
            (defaultParent.GetComponent<PlayField>().fieldTipe == FieldTipe.MY_FIELD && GetComponent<CardInfo>().card.CanAttack));

        if (!isMovable)
            return;

        TempCardGameObj.transform.SetParent(defaultParent); // Setting the parent of the temporary card when moving the card

        TempCardGameObj.transform.SetSiblingIndex(transform.GetSiblingIndex()); // Setting the index of the temporary card when moving the card

        transform.SetParent(defaultParent.parent); // Setting the new parent of the draggable card

        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isMovable)
            return;

        Vector3 newPosition = camera.ScreenToWorldPoint(eventData.position); // Setting coordinates relative to the coordinates of the game world

        transform.position = newPosition + offset; // Setting a new position of the map with the calculation of the cursor offset relative to the center of the dragged object

        // Setting the parent of the temporary card when dragging the card
        if (TempCardGameObj.transform.parent != defaultTempCardParent) 
            TempCardGameObj.transform.SetParent(defaultTempCardParent);

        // Checking the position of the card, provided that it is not in the player's field
        if (defaultParent.GetComponent<PlayField>().fieldTipe != FieldTipe.MY_FIELD)
            CheckCardPosition();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!isMovable)
            return;

        transform.SetParent(defaultParent); // Assigning a parent to the card when finished dragging

        GetComponent<CanvasGroup>().blocksRaycasts = true;

        transform.SetSiblingIndex(TempCardGameObj.transform.GetSiblingIndex()); // Assigning the Map Index to the Temporary Map When You Finish Dragging
        TempCardGameObj.transform.SetParent(GameObject.Find("TableCanvas").transform); // Assigning a Standard Parent to a temporary card
        TempCardGameObj.transform.localPosition = new Vector3(0, 2000); // Setting the position of the temporary map outside the field of view of the camera
    }

    /// <summary>
    /// Checking the position of the map while dragging
    /// </summary>
    void CheckCardPosition()
    {
        // Getting the number of child objects
        int index = defaultTempCardParent.childCount;

        for (int i = 0; i < defaultTempCardParent.childCount; i++)
        {
            if (transform.position.x < defaultTempCardParent.GetChild(i).position.x)
            {
                index = i;

                if (TempCardGameObj.transform.GetSiblingIndex() < index)
                    index--;

                break;
            }
        }

        TempCardGameObj.transform.SetSiblingIndex(index); // Setting the index to the temporary card
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="field"></param>
    public void MoveToField(Transform field)
    {
        transform.DOMove(field.position, 0.3f);
    }

    /// <summary>
    /// Smooth card movement on the field (used to move enemy cards)
    /// </summary>
    /// <param name="target"></param>
    public void MoveToTarget(Transform target)
    {
        StartCoroutine(MoveToTargetCor(target));
    }

    /// <summary>
    /// Coroutine for smooth movement of cards on the field
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    IEnumerator MoveToTargetCor(Transform target)
    {
        Vector3 position = transform.position; // Getting the initial position of the card

        transform.DOMove(target.position, 0.2f); // Move to the target 

        yield return new WaitForSeconds(0.2f);

        transform.DOMove(position, 0.2f); // Move to original position

        yield return new WaitForSeconds(0.2f);
    }
}
