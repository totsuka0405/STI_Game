using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBox : MonoBehaviour
{
    [SerializeField] Slot[] slots;
    //どこでも実行できるように
    public static ItemBox instance;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }
    // PickupObjがクリックされたら、スロットにアイテムを入れる

    public void SetItem(Item item)
    {
        foreach (Slot slot in slots)
        {
            if (slot.IsEmpty())
            {
                slot.SetItem(item);
                break;
            }
        }

        /*
        for (int i=0; i< slots.Length; i++)
        {
            if (slots[i].IsEmpty())
            {
                slots[i].SetItem(item);
                break;
            }
        }
        */
    }
}
