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
        whistle,
        gloves,
        slippers,
        bottle,
        radio,
        firstaidkit,
        bousaibook,
        step,
        other
    }

    public String name;
    public ItemType type;       // 種類
    public Sprite sprite;       // 画像
    public GameObject handItemPrefab; // アイテムのPrefab参照
    public GameObject fieldItemPrefab;


    public Item(String name, ItemType type, Sprite sprite, GameObject itemPrefab, GameObject fieldItemPrefab)
    {
        this.name = name;
        this.type = type;
        this.sprite = sprite;
        this.handItemPrefab = itemPrefab;
        this.fieldItemPrefab = fieldItemPrefab;
    }
}
