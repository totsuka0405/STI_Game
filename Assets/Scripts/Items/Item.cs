using System;
using System.Runtime.Serialization;
using UnityEngine;

/// <summary>
/// ゲーム内で扱うアイテムの定義データ。
/// ・種類（enum）
/// ・表示用スプライト
/// ・手持ち時／フィールド配置用 Prefab
/// </summary>
[Serializable]
public class Item
{
    /// <summary>アイテムの分類（UIや判定で利用）。</summary>
    public enum ItemType
    {
        Bat,
        HandLight,
        Phone,
        Stopper,
        Game,
        Whistle,
        Gloves,
        Slippers,
        Bottle,
        Radio,
        FirstAidKit,
        BousaiBook,
        Step,
        Other
    }

    [Header("Basic Info")]
    [SerializeField] private string itemName;
    [SerializeField] private ItemType type;
    [SerializeField] private Sprite sprite;

    [Header("Prefab References")]
    [SerializeField] private GameObject handItemPrefab;  // 手に持ったときに表示するPrefab
    [SerializeField] private GameObject fieldItemPrefab; // フィールドに配置するPrefab

    // ====== プロパティ（読み取り用を基本とする） ======
    public string Name => itemName;
    public ItemType Type => type;
    public Sprite Sprite => sprite;
    public GameObject HandItemPrefab => handItemPrefab;
    public GameObject FieldItemPrefab => fieldItemPrefab;

    /// <summary>
    /// コンストラクタ。スクリプト生成時に利用可能。
    /// （Inspector 経由での設定を基本とし、手動生成は補助用途）
    /// </summary>
    public Item(string name, ItemType type, Sprite sprite, GameObject handPrefab, GameObject fieldPrefab)
    {
        this.itemName = name;
        this.type = type;
        this.sprite = sprite;
        this.handItemPrefab = handPrefab;
        this.fieldItemPrefab = fieldPrefab;
    }
}