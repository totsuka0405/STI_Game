using System;
using UnityEngine;

/// <summary>
/// フィールド上の「拾えるアイテム」を制御するコンポーネント。
/// プレイヤーがクリック／インタラクトした際に ItemBox へ追加され、
/// GameManager に所持フラグを反映した後、自身を破棄する。
/// </summary>
public class PickupObj : MonoBehaviour
{
    [SerializeField] private Item.ItemType itemType; // このオブジェクトが持つアイテム種別
    private Item item;                               // 実際に生成されたアイテムデータ

    private void Start()
    {
        item = ItemGenerator.instance.Spawn(itemType);

        // 初期状態として GameManager 側の所持フラグを false にしておく
        SetGameManagerFlag(false);
    }

    /// <summary>
    /// プレイヤーによってクリックされた際の処理。
    /// ItemBox へ追加し、GameManager に所持フラグを立て、自身を破棄する。
    /// </summary>
    public void OnClickObj()
    {
        if (item == null)
        {
            Debug.LogWarning($"PickupObj: itemType {itemType} の生成に失敗しました");
            return;
        }

        ItemBox.instance.SetItem(item);
        SetGameManagerFlag(true);

        Destroy(gameObject);
    }

    /// <summary>
    /// GameManager の「所持フラグ」を itemType に応じて更新する。
    /// </summary>
    private void SetGameManagerFlag(bool value)
    {
        switch (itemType)
        {
            case Item.ItemType.HandLight:
                GameManager.instance.isHaveHandLight = value;
                break;
            case Item.ItemType.Phone:
                GameManager.instance.isHavePhone = value;
                break;
            case Item.ItemType.Whistle:
                GameManager.instance.isHaveWhistle = value;
                break;
            case Item.ItemType.Gloves:
                GameManager.instance.isHaveGloves = value;
                break;
            case Item.ItemType.Slippers:
                GameManager.instance.isHaveSlipper = value;
                break;
            case Item.ItemType.Bottle:
                GameManager.instance.isHaveBottle = value;
                break;
            case Item.ItemType.Radio:
                GameManager.instance.isHaveRadio = value;
                break;
            case Item.ItemType.FirstAidKit:
                GameManager.instance.isHaveFirstAidKit = value;
                break;
            default:
                // それ以外はフラグ不要
                break;
        }
    }
}
