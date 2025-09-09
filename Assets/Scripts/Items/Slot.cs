using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// インベントリ内の1マス（スロット）を表すUIコンポーネント。
/// ・受け取った Item を内部に保持
/// ・スロット内テキストの更新
/// ・選択中ハイライト表示
/// ※スロット内画像の更新は現状未使用（ItemBox 側で大画像を管理）。
/// </summary>
public class Slot : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Image image; // スロット背景等に使用（ハイライト色変更に利用）
    [SerializeField] private Text text;  // アイテム名表示（子に Text がある前提。未設定なら自動取得）

    /// <summary>このスロットに格納されているアイテム（null なら空）。</summary>
    private Item slotitem;

    private void Awake()
    {
        // Inspector 未設定時のフォールバック
        if (image == null) image = GetComponent<Image>();
        if (text == null) text = GetComponentInChildren<Text>();
    }

    /// <summary>このスロットが空かどうか。</summary>
    public bool IsEmpty() => slotitem == null;

    /// <summary>
    /// スロットへアイテムを設定し、表示（テキスト）を更新する。
    /// </summary>
    public void SetItem(Item item)
    {
        slotitem = item;
        UpdateText(item);
        // UpdateImage(item);  // 現状は未使用。必要ならコメントアウト解除。
    }

    /// <summary>スロットに設定されているアイテムを取得する（null 可）。</summary>
    public Item GetItem() => slotitem;

    /// <summary>
    /// スロットを空にし、表示をクリアする。
    /// </summary>
    public void ClearItem()
    {
        slotitem = null;
        if (text != null) text.text = string.Empty;
        // UpdateImage(null); // スロット内の画像を使う設計にする場合は有効化
    }

    /// <summary>
    /// 選択中ハイライトの見た目を切り替える（背景色で簡易表現）。
    /// </summary>
    public void Highlight(bool highlight)
    {
        if (image == null) return;
        image.color = highlight ? Color.yellow : Color.white;
    }

    // ====== 表示更新（必要に応じて利用） ======

    /// <summary>スロット内テキスト（アイテム名）を更新する。</summary>
    private void UpdateText(Item item)
    {
        if (text == null) return;

        if (item != null)
        {
            text.text = item.Name;
        }
        else
        {
            text.text = string.Empty;
        }
    }

    /// <summary>
    /// スロット内画像を更新する（使う場合のみ有効化）。
    /// 現状は背景 Image をハイライト用途に使用しているため未使用。
    /// </summary>
    private void UpdateImage(Item item)
    {
        if (image == null) return;

        if (item != null && item.Sprite != null)
        {
            image.sprite = item.Sprite;
            image.color = Color.white;
        }
        else
        {
            image.sprite = null;
            image.color = new Color(0f, 0f, 0f, 0f);
        }
    }
}
