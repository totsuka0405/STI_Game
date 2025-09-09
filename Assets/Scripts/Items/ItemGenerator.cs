using UnityEngine;

/// <summary>
/// Item を種類ごとに生成するファクトリークラス。
/// ・Inspector で参照する ItemListEmpty の定義をもとに Item インスタンスを複製
/// ・シングルトンとしてどこからでも利用可能
/// </summary>
public class ItemGenerator : MonoBehaviour
{
    [Header("アイテムリスト（全アイテム定義を保持する ScriptableObject 等）")]
    [SerializeField] private ItemListEmpty itemListEntity;

    /// <summary>どこからでも利用可能なシングルトンインスタンス。</summary>
    public static ItemGenerator instance { get; private set; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject); // 重複インスタンスを防止
        }

        if (itemListEntity == null)
        {
            Debug.LogError("ItemListEntity が設定されていません。ItemGenerator が正常に動作しません。");
        }
    }

    /// <summary>
    /// 指定された ItemType に一致する Item を新規生成して返す。
    /// 一致する Item が存在しない場合は null を返す。
    /// </summary>
    public Item Spawn(Item.ItemType type)
    {
        if (itemListEntity == null || itemListEntity.itemList == null)
        {
            Debug.LogWarning("ItemListEntity が空です。");
            return null;
        }

        foreach (Item item in itemListEntity.itemList)
        {
            if (item.Type == type)
            {
                return new Item(item.Name, item.Type, item.Sprite, item.HandItemPrefab, item.FieldItemPrefab);
            }
        }

        Debug.LogWarning($"指定されたアイテムタイプ {type} が見つかりませんでした。");
        return null;
    }
}
