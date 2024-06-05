using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Slot : MonoBehaviour
{
    // アイテムを受け取ったら画像をスロットに表示

    Item slotitem;

    Image image;
    
    private void Awake()
    {
        image = GetComponent<Image>();
    }

    public bool IsEmpty()
    {
        if(slotitem == null)
        {
            return true;
        }
        return false;
    }

    public void SetItem(Item item)
    {
        slotitem = item;
        UpdateImage(item);
    }

    void UpdateImage(Item item)
    {
        image.sprite = item.sprite;
    }
}
