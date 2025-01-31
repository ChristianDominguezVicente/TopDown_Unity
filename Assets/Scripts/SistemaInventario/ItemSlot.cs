using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IDropHandler
{
    [SerializeField] private RectTransform itemBackground;

    private ItemInfo currentItemInfo;

    private void Awake()
    {
        currentItemInfo = GetComponentInChildren<ItemInfo>();
    }

    public void OnDrop(PointerEventData eventData)
    {
        Transform itemDroppedTransform = eventData.pointerDrag.transform;
        ItemInfo newItemInfo = eventData.pointerDrag.GetComponent<ItemInfo>();

        if (itemBackground.childCount > 0) //HAY DATOS EN ESTE SLOT. --> genero intercambio.
        {
            currentItemInfo.transform.SetParent(newItemInfo.InitParent);
            currentItemInfo.transform.localPosition = Vector3.zero;

            //Actualizar al slot destino con los datos del currentItemInfo.
            newItemInfo.InitParent.GetComponentInParent<ItemSlot>().currentItemInfo = currentItemInfo;
        }

        itemDroppedTransform.SetParent(itemBackground);
        itemDroppedTransform.localPosition = Vector3.zero; //Totalmente centrado al slot.

        currentItemInfo = newItemInfo;

    }
}
