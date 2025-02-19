using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemInfo : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField] private Image itemImage;
    [SerializeField] private TMP_Text itemNameText;
    [SerializeField] private TMP_Text itemCountText;

    private int itemCount;

    private Canvas canvas;
    private RectTransform itemRectTransform;
    private CanvasGroup canvasGroup;
    
    private Transform initParent; //El slot original al cual estoy anclado.
    private Vector3 initPosition; //Posición original con la cual nazco.

    private ItemSO currentData;
    public int ItemCount => itemCount;

    public Transform InitParent { get => initParent; }
    public Vector3 InitPosition { get => initPosition; }

    private void Awake()
    {
        canvas = transform.root.GetComponent<Canvas>();
        itemRectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }
    public void FeedData(ItemSO newItem)
    {
        itemImage.sprite = newItem.icon;
        itemNameText.text = newItem.itemName;
        currentData = newItem;
        itemCount = 1;
        itemCountText.text = "x" + itemCount;
    }

    //Se ejecuta cuando comenzamos a arrastrar el elemento.
    public void OnBeginDrag(PointerEventData eventData)
    {
        initParent = itemRectTransform.parent;
        initPosition = itemRectTransform.localPosition;

        //De forma momentanea me emparento al propio canvas.
        itemRectTransform.SetParent(canvas.transform);

        //Efecto de transparencia en el objeto que se va a mover.
        canvasGroup.alpha = 0.5f;
        //Dejo de tomar lecturas del click para con este objeto.
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        //Genero el movimiento del transform con lo que diga el ratón.
        itemRectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    //Se ejecuta cuando TERMINEMOS de arrastrar el elemento.
    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        //Dropeo FALLIDO
        if(itemRectTransform.parent == canvas.transform)
        {
            itemRectTransform.SetParent(initParent);
            itemRectTransform.localPosition = initPosition;
        }
    }

    public void UpdateStackItem()
    {
        itemCount++;
        itemCountText.text = "x" + itemCount;
    }

    public void UseItem()
    {
        if (currentData != null)
        {
            Debug.Log("Utilizo " + currentData.name);
        }
    }

    public void SetItemCount(int count)
    {
        itemCount = count;
        itemCountText.text = "x" + itemCount;
    }
}
