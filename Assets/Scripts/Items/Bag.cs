using UnityEngine;

/// <summary>
/// バッグを拾ったときの処理を管理するコンポーネント。
/// ・ItemBox の所持スロット数を増加
/// ・自分自身を非表示化
/// </summary>
public class Bag : MonoBehaviour
{
    [Header("追加されるスロット数")]
    [SerializeField] private int addSlotCount = 5;

    /// <summary>
    /// バッグを取得したときのイベント処理。
    /// </summary>
    public void OnBagEvent()
    {
        if (ItemBox.instance == null)
        {
            Debug.LogError("Bag: ItemBox.instance が見つかりません。");
            return;
        }

        ItemBox.instance.AddManySlot(addSlotCount);
        gameObject.SetActive(false);
    }
}
