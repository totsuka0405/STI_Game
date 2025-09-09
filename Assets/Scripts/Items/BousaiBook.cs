using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 防災ブックを開いてページ送りする処理。
/// 所持状態（isHandBousaiBook）が true のときのみ有効。
/// </summary>
public class BousaiBook : MonoBehaviour
{
    [Header("防災ブックのページパネル群")]
    [SerializeField] private GameObject[] bousaiPanels;

    [Header("効果音再生用")]
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip clip;

    private int panelIndex = 0;

    private void Start()
    {
        panelIndex = 0;
        SetAllPanelsActive(false);
    }

    private void Update()
    {
        if (GameManager.instance.isHandBousaiBook)
        {
            // 右クリック または ゲームパッド East ボタンで次ページへ
            if (Input.GetMouseButtonDown(1) ||
                (Gamepad.current != null && Gamepad.current.buttonEast.wasPressedThisFrame))
            {
                OnNext();
            }
        }
        else
        {
            // ブックを閉じる → 全パネル非表示 & インデックスリセット
            SetAllPanelsActive(false);
            panelIndex = 0;
        }
    }

    /// <summary>
    /// 次のページを表示。末尾なら閉じる。
    /// </summary>
    public void OnNext()
    {
        if (bousaiPanels == null || bousaiPanels.Length == 0) return;

        if (panelIndex < bousaiPanels.Length)
        {
            // 現在のページを表示
            bousaiPanels[panelIndex].SetActive(true);

            // 前ページを閉じる
            if (panelIndex > 0)
            {
                bousaiPanels[panelIndex - 1].SetActive(false);
            }

            panelIndex++;
        }
        else
        {
            // 全ページ閉じて最初に戻る
            SetAllPanelsActive(false);
            panelIndex = 0;
        }

        if (clip != null && source != null)
        {
            SoundManager.instance.PlaySE(clip, source);
        }
    }

    /// <summary>
    /// 全ページを一括で ON/OFF。
    /// </summary>
    private void SetAllPanelsActive(bool active)
    {
        if (bousaiPanels == null) return;

        foreach (var panel in bousaiPanels)
        {
            if (panel != null)
            {
                panel.SetActive(active);
            }
        }
    }
}
