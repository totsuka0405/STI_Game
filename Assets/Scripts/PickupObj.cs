using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupObj : MonoBehaviour
{
    // [SerializeField] Item item;
    [SerializeField] Item.ItemType itemType;
    Item item;

    private void Start()
    {
        item = ItemGenerator.instance.Spawn(itemType);
    }
    public void OnClickObj()
    {
        ItemBox.instance.SetItem(item);
        gameObject.SetActive(false);
    }
}
