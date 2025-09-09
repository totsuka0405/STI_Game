using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 手持ちインベントリ（スロット）の生成・選択・表示切替・破棄を管理する。
/// ・スロットの初期化/追加
/// ・選択中スロットの強調表示と手元表示の更新
/// ・選択アイテムの画像更新/投下/消去
/// </summary>
public class ItemBox : MonoBehaviour
{
    [Header("UI/Prefab")]
    [SerializeField] private GameObject slotPrefab; // スロットのプレハブ
    [SerializeField] private Transform slotParent;  // スロットを並べる親
    [SerializeField] private Image itemImage;       // 選択中アイテムのサムネ

    [Header("Slot Runtime")]
    [SerializeField] private int handItemCount = 0; // 所持中アイテム数（表示/監視用）
    [SerializeField] private int handItemLimit = 0; // スロット数（上限）

    /// <summary>現在の所持アイテム数を取得</summary>
    public int HandItemCount => handItemCount;

    /// <summary>所持可能なスロット数を取得</summary>
    public int HandItemLimit => handItemLimit;

    /// <summary>シーン内で単一利用を想定した参照用インスタンス。</summary>
    public static ItemBox instance;

    private readonly List<Slot> slots = new List<Slot>();
    private int selectedIndex = -1;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        InitializeSlots(5);
        handItemCount = 0;
    }

    /// <summary>指定数のスロットを生成し、上限を更新する。</summary>
    private void InitializeSlots(int count)
    {
        for (int i = 0; i < count; i++)
        {
            AddSlot();
            handItemLimit++;
        }
    }

    /// <summary>スロットを複数追加する（上限も連動）。</summary>
    public void AddManySlot(int count)
    {
        for (int i = 0; i < count; i++)
        {
            AddSlot();
            handItemLimit++;
        }
    }

    /// <summary>スロットを1つ生成し、リストへ登録する。</summary>
    public void AddSlot()
    {
        GameObject newSlotObject = Instantiate(slotPrefab, slotParent);
        Slot newSlot = newSlotObject.GetComponent<Slot>();
        if (newSlot != null)
        {
            slots.Add(newSlot);
        }
    }

    /// <summary>
    /// 空きスロットにアイテムを格納する（最初に見つかった空きへ）。
    /// </summary>
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

    /// <summary>
    /// 指定インデックスのスロットを選択状態にし、UI/手持ち表示を更新する。
    /// </summary>
    public void SelectItem(int index)
    {
        if (index >= 0 && index < slots.Count)
        {
            selectedIndex = index;
            UpdateSelectedItemDisplay(); // ハイライト
            DisplaySelectedItem();       // 手元（3D）反映
            UpdateItemImage();           // サムネ反映
        }
    }

    /// <summary>現在選択中のアイテムを返す（未選択/空なら null）。</summary>
    public Item GetSelectedItem()
    {
        if (selectedIndex >= 0 && selectedIndex < slots.Count)
        {
            return slots[selectedIndex].GetItem();
        }
        return null;
    }

    /// <summary>
    /// ホイール/ショルダー用：現在選択から方向（±1）に応じた次インデックスを返す（循環）。
    /// </summary>
    public int GetNextIndex(int direction)
    {
        if (slots.Count == 0) return -1;

        int newIndex = selectedIndex + direction;
        if (newIndex < 0) newIndex = slots.Count - 1;
        if (newIndex >= slots.Count) newIndex = 0;

        return newIndex;
    }

    /// <summary>選択中スロットのみ強調表示する。</summary>
    private void UpdateSelectedItemDisplay()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            slots[i].Highlight(i == selectedIndex);
        }
    }

    /// <summary>選択中アイテムの手元表示（3Dプレハブ）を更新する。</summary>
    private void DisplaySelectedItem()
    {
        CharacterMove.instance.DisplaySelectedItemInHand(GetSelectedItem());
    }

    /// <summary>選択中アイテムのサムネイル画像を更新する。</summary>
    private void UpdateItemImage()
    {
        if (selectedIndex >= 0 && selectedIndex < slots.Count)
        {
            Item selectedItem = slots[selectedIndex].GetItem();
            if (selectedItem != null)
            {
                itemImage.sprite = selectedItem.Sprite;
                itemImage.color = Color.white;
            }
            else
            {
                itemImage.sprite = null;
                itemImage.color = new Color(0, 0, 0, 0);
            }
        }
        else
        {
            itemImage.sprite = null;
            itemImage.color = new Color(0, 0, 0, 0);
        }
    }

    /// <summary>
    /// 選択中スロットのアイテムを投下/消去し、所持数と表示を更新する。
    /// </summary>
    public void RemoveSelectedItem()
    {
        if (selectedIndex >= 0 && selectedIndex < slots.Count)
        {
            CharacterMove.instance.DropItemgenerate(GetSelectedItem());
            slots[selectedIndex].ClearItem();
            handItemCount--;

            UpdateItemImage();
            CharacterMove.instance.DisplaySelectedItemInHand(GetSelectedItem());
        }
        else
        {
            Debug.LogWarning("No valid slot selected to remove an item.");
        }
    }
}