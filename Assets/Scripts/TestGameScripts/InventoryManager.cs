using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    private List<Item> inventory = new List<Item>();

    public void AddItem(Item item)
    {
        inventory.Add(item);
        //Debug.Log($"{item.name} added to inventory.");
    }

    public List<Item> GetInventory()
    {
        return inventory;
    }
}
