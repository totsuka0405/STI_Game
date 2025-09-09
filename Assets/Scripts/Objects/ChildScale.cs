using UnityEngine;

/// <summary>
/// 親のスケールに依存せず、子オブジェクトの見た目のスケールを一定に保つ補正コンポーネント。
/// </summary>
public class ChildScale : MonoBehaviour
{
    [Header("子オブジェクトの基準スケール")]
    [SerializeField] private Vector3 defaultScale = Vector3.one;

    private void Reset()
    {
        // 初期化時に現在のローカルスケールをデフォルト値として登録
        defaultScale = transform.localScale;
    }

    private void LateUpdate()
    {
        // 親のスケールに影響されないように補正
        Vector3 lossy = transform.lossyScale;

        if (lossy.x == 0f || lossy.y == 0f || lossy.z == 0f)
        {
            Debug.LogWarning("lossyScale が 0 を含んでいるため補正できません。", this);
            return;
        }

        transform.localScale = new Vector3(
            defaultScale.x / lossy.x * transform.localScale.x,
            defaultScale.y / lossy.y * transform.localScale.y,
            defaultScale.z / lossy.z * transform.localScale.z
        );
    }
}
