using UnityEngine;

/// <summary>
/// 手持ちアイテム「防災ブック」
/// - 装備中は GameManager に状態を通知
/// - 特殊な Update 処理は不要
/// </summary>
public class HandItemBousaiBook : MonoBehaviour
{
    private void OnEnable()
    {
        // 装備したらフラグを立てる
        GameManager.instance.isHandBousaiBook = true;
        GameManager.instance.isHandItemUse = true;
    }

    private void OnDisable()
    {
        // 装備解除したらフラグをリセット
        GameManager.instance.isHandBousaiBook = false;
        GameManager.instance.isHandItemUse = false;
    }
}
