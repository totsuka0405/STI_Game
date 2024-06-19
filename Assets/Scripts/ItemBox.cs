using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBox : MonoBehaviour
{
    [SerializeField] Slot[] slots;
    //どこでも実行できるように
    public static ItemBox instance;
    private int selectedIndex = -1;
   

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
    }

    public void SelectItem(int index)
    {
        if(index >= 0 && index < slots.Length)
        {
            selectedIndex = index;
            //アイテムの表示を更新するなどの処理を追加
            Debug.Log("Selected item at index: " + selectedIndex);
            UpdateSelectedItemDisplay();
            DisplaySelectedItem();
        }
    }

    public Item GetSelectedItem()
    {
        if (selectedIndex >= 0 && selectedIndex < slots.Length)
        {
            return slots[selectedIndex].GetItem();
        }
        return null;
    }

    public int GetNextIndex(int direction)
    {
        if (slots.Length == 0) return -1;

        int newIndex = selectedIndex + direction;
        if (newIndex < 0) newIndex = slots.Length - 1;
        if (newIndex >= slots.Length) newIndex = 0;

        return newIndex;
    }

    private void UpdateSelectedItemDisplay()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].Highlight(i == selectedIndex);
        }
    }

    private void DisplaySelectedItem()
    {
        CharacterMove.instance.DisplaySelectedItemInHand(GetSelectedItem());
    }


}
