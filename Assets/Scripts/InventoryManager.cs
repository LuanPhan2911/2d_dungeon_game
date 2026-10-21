using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventoryItem
{
    public ItemSO dataSO;
    public int amount;

    public InventoryItem(ItemSO item, int amount)
    {
        dataSO = item;
        this.amount = amount;
    }
}
public class InventoryManager : MonoBehaviour
{

    public static InventoryManager Instance { get; private set; }
    [SerializeField] private List<InventoryItem> _inventoryItemList = new List<InventoryItem>();


    public event Action OnInventoryChanged;

    public InventoryItem[] InventoryItemArray => _inventoryItemList.ToArray();


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

   


    public bool AddItem(ItemSO itemData, int amount = 1)
    {
        if (itemData.isStackable)
        {
            InventoryItem exisitingItem = _inventoryItemList.Find(x => x.dataSO == itemData);

            if (exisitingItem != null)
            {
                exisitingItem.amount += amount;
                return true;
            }
        }

        _inventoryItemList.Add(new InventoryItem(itemData, amount));
        OnInventoryChanged?.Invoke();
        return true;
    }

    public bool HasItem(ItemSO itemData, int amount = 1)
    {
        InventoryItem exisitingItem = _inventoryItemList.Find(el => el.dataSO == itemData);

        if (exisitingItem == null) return false;


        return exisitingItem.amount >= amount;
    }
    public bool RemoveItem(ItemSO itemData, int amount = 1)
    {
        InventoryItem exisitingItem = _inventoryItemList.Find(el => el.dataSO == itemData);

        if (exisitingItem == null) return false;

        if (exisitingItem.amount < amount) return false;

        exisitingItem.amount -= amount;

        if (exisitingItem.amount == 0)
        {
            _inventoryItemList.Remove(exisitingItem);
        }

        OnInventoryChanged?.Invoke();
        return true;

    }



}
