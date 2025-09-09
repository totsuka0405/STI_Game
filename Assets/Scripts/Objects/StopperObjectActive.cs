using UnityEngine;

/// <summary>
/// 家具の転倒防止器具（止め具・耐震ストッパー）に関連するオブジェクト群を
/// まとめて有効/無効に切り替えるコンポーネント。
/// ・地震イベント前に無効、設置/使用後に有効化…といった表現に利用
/// ・表示用メッシュ/ガイド/固定用コライダ等の ON/OFF を想定
/// </summary>
public class StopperObjectActive : MonoBehaviour
{
    [Header("止め具として有効化する対象（メッシュ・コライダ・ガイド等）")]
    [SerializeField] private GameObject[] stopperObjects;

    [Header("初期状態（ON で開始するなら true）")]
    [SerializeField] private bool isStopperActive = false;

    private void Start()
    {
        // 起動時は設定フラグに合わせて一括切り替え
        SetObjectsActive(isStopperActive);
    }

    /// <summary>
    /// 止め具を「有効」にする（外部イベント/インタラクトから呼び出し）。
    /// 既存の StopperActive() と同義の挙動を保ちます。
    /// </summary>
    public void StopperActive()
    {
        if (isStopperActive)
        {
            // 既存仕様：true のときだけ ON。設計を変えないためこのまま残す。
            SetObjectsActive(true);
        }
    }

    /// <summary>
    /// 明示的に ON にしたい場合はこちらを使用（設置完了などのフローで呼ぶ想定）。
    /// ※既存シーンとの互換のため追加。無理に置換する必要はありません。
    /// </summary>
    public void ActivateStopper()
    {
        isStopperActive = true;
        SetObjectsActive(true);
    }

    /// <summary>
    /// 明示的に OFF にしたい場合（撤去・リセット・リトライなど）。
    /// </summary>
    public void DeactivateStopper()
    {
        isStopperActive = false;
        SetObjectsActive(false);
    }

    /// <summary>
    /// 登録されたオブジェクト群をまとめて切り替える内部処理。
    /// </summary>
    private void SetObjectsActive(bool active)
    {
        if (stopperObjects == null) return;

        foreach (var obj in stopperObjects)
        {
            if (obj != null) obj.SetActive(active);
        }
    }
}
