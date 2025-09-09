using UnityEngine;

/// <summary>
/// 個別ライトのオン/オフを管理する簡易スイッチ。
/// ・手動トグル（OnLight）
/// ・ブレーカー復旧時の状態反映（OnLightWakeUp）
/// </summary>
public class LightOnOff : MonoBehaviour
{
    [Header("対象ライト")]
    [SerializeField] private GameObject lightObj;

    [Header("効果音")]
    [SerializeField] private AudioClip audioClip;
    [SerializeField] private AudioSource source;

    [Header("現在状態（true=点灯）")]
    [SerializeField] private bool isTurn = false; // 既存名称を維持（外部互換）

    /// <summary>
    /// ブレーカー復旧時に、現在の状態フラグ（isTurn）に合わせて点灯/消灯を反映する。
    /// </summary>
    public void OnLightWakeUp()
    {
        if (lightObj == null) return;
        lightObj.SetActive(isTurn);
    }

    /// <summary>
    /// プレイヤー操作などで呼ばれるトグル。
    /// ・ブレーカー遮断中（isBreakerDown=true）は操作無効
    /// ・状態に応じてライトのアクティブを切り替え、SE を再生
    /// </summary>
    public void OnLight()
    {
        if (GameManager.instance != null && GameManager.instance.isFirstBreakerDown)
        {
            // 初回ブレーカー落ち演出の期間中は操作不可（現仕様踏襲）
            return;
        }

        if (GameManager.instance != null && GameManager.instance.isBreakerDown)
        {
            // ブレーカーが落ちている場合は操作不可
            return;
        }

        if (lightObj == null) return;

        // いまの状態に応じて反転
        lightObj.SetActive(!isTurn);

        // 効果音（任意）
        if (SoundManager.instance != null)
        {
            SoundManager.instance.PlaySE(audioClip, source);
        }

        // フラグを反転
        isTurn = !isTurn;
    }
}
