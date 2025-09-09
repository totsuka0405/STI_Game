using UnityEngine;

/// <summary>
/// フィールド上に配置される懐中電灯（取得可能アイテム）
/// - 配置中は GameManager に「未所持」として通知
/// - 無効化（取得後など）時に「所持済み」に切り替える
/// </summary>
public class FieldHandLight : MonoBehaviour
{
    private void OnEnable()
    {
        // フィールドに存在している間は未所持
        GameManager.instance.isHaveHandLight = false;
    }

    private void OnDisable()
    {
        // 拾われて無効化されたら所持状態に切り替え
        GameManager.instance.isHaveHandLight = true;
    }
}
