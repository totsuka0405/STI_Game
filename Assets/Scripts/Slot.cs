using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Slot : MonoBehaviour
{
    // アイテムを受け取ったら画像をスロットに表示
    Item slotitem;
    Image image;
    Text text;
    
    private void Awake()
    {
        image = GetComponent<Image>();
        text = GetComponentInChildren<Text>();
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
        //slotitem = item;
        //UpdateImage(item);
        slotitem = item;
        UpdateText(item);
    }

    void UpdateImage(Item item)
    {
        if (item != null)
        {
            image.sprite = item.sprite;
            image.color = Color.white;
        }
        else
        {
            image.sprite = null;
            image.color = new Color(0, 0, 0, 0);
        }
    }

    void UpdateText(Item item)
    {
        if (item != null)
        {
            string slotItemName = item.name;
            text.text = slotItemName;
        }
        else
        {
            text.text = null;
        }
    }

    public Item GetItem()
    {
        return slotitem;
    }

    public void ClearItem()
    {
        text.text = null;
        slotitem = null;
        //UpdateImage(null); // スロットの画像をクリア
    }

    public void Highlight(bool highlight)
    {
        // ハイライトのためのエフェクトを追加します（例えば、色を変更）
        if (highlight)
        {
            image.color = Color.yellow;  // ハイライト色に変更
        }
        else
        {
            image.color = Color.white;   // 通常色に戻す
        }
    }
    
}
