using Unity.VisualScripting;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    [SerializeField] public InventorySlot[] inventorySlots;
    [SerializeField] private GameObject inventoryItemPrefab;
    [SerializeField] private int maxStackItems = 20;
    [SerializeField] public int selectedSlot;
    [SerializeField] private Item[] defaultItems;
    [SerializeField] private Transform MainInventoryWindow;
    [SerializeField] private GameObject ShowInvenoryButton;
    [SerializeField] private GameObject HideInvenoryButton;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        changeSelectedSlot(0);
        foreach(Item item in defaultItems)
        {
            AddItem(item);
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            MainInvenoryWindowChangeShowState();
            return;
        }

        if (Input.inputString != null)
        {
            bool isNumber = int.TryParse(Input.inputString, out int number);
            if (isNumber && number > 0 && number < 8)
            {
                changeSelectedSlot(number-1);
            }
        }
        float mouseWheel = Input.GetAxis("Mouse ScrollWheel");
        int roundMouseWheel = mouseWheel > 0 ? -1 : mouseWheel < 0 ? 1 : 0;
        int newSelectedSlot = selectedSlot + roundMouseWheel;

        if (newSelectedSlot >= 0 && newSelectedSlot <= 6)
        {
            changeSelectedSlot(newSelectedSlot);
        }else if (newSelectedSlot < 0)
        {
            changeSelectedSlot(7 + newSelectedSlot);
        }
        else if (newSelectedSlot > 6)
        {
            changeSelectedSlot(newSelectedSlot - 7);
        }
    }

    public void MainInvenoryWindowChangeShowState()
    {
        Animator MIWanimator = MainInventoryWindow.GetComponent<Animator>();
        bool state = MainInventoryWindow.position.x < 0;
        MIWanimator.SetTrigger(state ? "Open" : "Close");
        ShowInvenoryButton.SetActive(!state);
        HideInvenoryButton.SetActive(state);
    }
    private void changeSelectedSlot(int newValue)
    {
        if (selectedSlot >= 0)
        {
            inventorySlots[selectedSlot].Deselect();
        }
        inventorySlots[newValue].Select();
        selectedSlot = newValue;
    }
    public bool AddItem(Item item)
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot != null &&
                itemInSlot.item == item &&
                itemInSlot.count < maxStackItems &&
                itemInSlot.item.stackable)
            {
                itemInSlot.count++;
                itemInSlot.refreshCount();
                return true;
            }
        }

        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot == null)
            {
                spawnNewItem(item, slot);
                return true;
            }
        }
        return false;
    }
    private void spawnNewItem(Item item, InventorySlot slot)
    {
        GameObject newItemGo = Instantiate(inventoryItemPrefab, slot.transform);
        InventoryItem inventoryItem = newItemGo.GetComponent<InventoryItem>();
        inventoryItem.InitialiseItem(item);
    }
    public Item getSelectedItem(bool use)
    {
        InventorySlot slot = inventorySlots[selectedSlot];
        InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
        if (itemInSlot)
        {
            Item item = itemInSlot.item;
            if (use)
            {
                itemInSlot.count--;
                if (itemInSlot.count == 0)
                {
                    Destroy(itemInSlot.gameObject);
                }
                else
                {
                    itemInSlot.refreshCount();
                }
                Debug.Log("pass");
            }
            return item;
        }
        return null;
    }
}