using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("UI")]
    [SerializeField] private Image image;
    [SerializeField] private TMP_Text countText;
    [SerializeField] private GameObject InventorySlot;
    [SerializeField] public int count;
    [SerializeField] public bool inCraftMenu;

    [HideInInspector] public Item item;
    [HideInInspector] public Transform parentAfterDrag;

    public void InitialiseItem(Item newItem, int index = 0)
    {
        item = newItem;
        image.sprite = newItem.image;
        item.index = index;
        refreshCount();
    }
    public void refreshCount()
    {
        countText.text = count.ToString();
        countText.gameObject.SetActive(count>1);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        image.raycastTarget = false;
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
        countText.gameObject.SetActive(false);
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
        //Debug.Log($"{transform.position} = {Input.mousePosition}");
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        image.raycastTarget = true;
        transform.SetParent(parentAfterDrag);
        countText.gameObject.SetActive(count > 1);
        item.GetItemsAfterCraft();
    }


}
