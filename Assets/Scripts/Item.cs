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
    }

    public ItemType type;       // 種類
    public Sprite sprite;       // 画像
    public Collider collider;
    public bool isHand;
    public bool isUse;

    public Item(ItemType type, Sprite sprite, Collider collider)
    {
        this.type = type;
        this.sprite = sprite;
        this.collider = collider;
        isHand = false;
        isUse = false;
    }

}
