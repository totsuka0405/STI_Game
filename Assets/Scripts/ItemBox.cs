using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemBox : MonoBehaviour
{
    [SerializeField] GameObject slotPrefab;
    [SerializeField] Transform slotParent;
    [SerializeField] Image itemImage;
    List<Slot> slots = new List<Slot>();
    public int handItemCount = 0;
    public int handItemLimit = 0;

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

    private void Start()
    {
        InitializeSlots(5);
        handItemCount = 0;
    }

    void InitializeSlots(int count)
    {
        for (int i = 0; i < count; i++)
        {
            AddSlot();
            handItemLimit++;
        }
    }

    public void AddManySlot(int count)
    {
        for (int i = 0; i < count; i++)
        {
            AddSlot();
            handItemLimit++;
        }
    }

    public void AddSlot()
    {
        GameObject newSlotObject = Instantiate(slotPrefab, slotParent); // プレハブから生成
        Slot newSlot = newSlotObject.GetComponent<Slot>();
        if (newSlot != null)
        {
            slots.Add(newSlot);
        }
    }

    // PickupObjがクリックされたら、スロットにアイテムを入れる

    public void SetItem(Item item)
    {
        handItemCount++;
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
        if(index >= 0 && index < slots.Count)
        {
            selectedIndex = index;
            //アイテムの表示を更新するなどの処理を追加
            //Debug.Log("Selected item at index: " + selectedIndex);
            UpdateSelectedItemDisplay();
            DisplaySelectedItem();
            UpdateItemImage();
        }
    }

    public Item GetSelectedItem()
    {
        if (selectedIndex >= 0 && selectedIndex < slots.Count)
        {
            return slots[selectedIndex].GetItem();
        }
        return null;
    }

    public int GetNextIndex(int direction)
    {
        if (slots.Count == 0) return -1;

        int newIndex = selectedIndex + direction;
        if (newIndex < 0) newIndex = slots.Count - 1;
        if (newIndex >= slots.Count) newIndex = 0;

        return newIndex;
    }

    private void UpdateSelectedItemDisplay()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            slots[i].Highlight(i == selectedIndex);
        }
    }

    private void DisplaySelectedItem()
    {
        CharacterMove.instance.DisplaySelectedItemInHand(GetSelectedItem());
    }

    void UpdateItemImage()
    {
        if (selectedIndex >= 0 && selectedIndex < slots.Count)
        {
            Item selectedItem = slots[selectedIndex].GetItem();
            if (selectedItem != null)
            {
                itemImage.sprite = selectedItem.sprite; // 選択中のアイテムのスプライトを設定
                itemImage.color = Color.white;          // 画像を表示可能にする
            }
            else
            {
                itemImage.sprite = null;                // スプライトをクリア
                itemImage.color = new Color(0, 0, 0, 0); // 透明にする
            }
        }
        else
        {
            itemImage.sprite = null;                    // スプライトをクリア
            itemImage.color = new Color(0, 0, 0, 0);     // 透明にする
        }
    }

    public void RemoveSelectedItem()
    {
        if (selectedIndex >= 0 && selectedIndex < slots.Count)
        {
            CharacterMove.instance.DropItemgenerate(GetSelectedItem());
            slots[selectedIndex].ClearItem(); // 現在選択中のスロットのアイテムをクリア
            handItemCount--;
            // 画像をクリアする
            UpdateItemImage();
            CharacterMove.instance.DisplaySelectedItemInHand(GetSelectedItem());
            //Debug.Log($"Removed item from slot {selectedIndex}");
        }
        else
        {
            Debug.LogWarning("No valid slot selected to remove an item.");
        }
    }
}
