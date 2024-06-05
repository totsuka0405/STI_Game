using System;
using UnityEngine;

[Serializable]
public class Item
{
    public enum ItemType
    {
        bat,
        hand_light,
        phone,
    }

    public ItemType type;       // 種類
    public Sprite sprite;       // 画像

    public Item(ItemType type, Sprite sprite)
    {
        this.type = type;
        this.sprite = sprite;
    }

}
