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
        stopper,
        game,
    }

    public ItemType type;       // 種類
    public Sprite sprite;       // 画像
    public GameObject itemPrefab; // アイテムのPrefab参照

    public Item(ItemType type, Sprite sprite, GameObject itemPrefab)
    {
        this.type = type;
        this.sprite = sprite;
        this.itemPrefab = itemPrefab;
    }
}
