using UnityEngine;

/// <summary>
/// 手持ちアイテム「スマートフォン」
/// - 装備中は GameManager に状態を通知
/// - 通話イベントが開始された際に SE を一度だけ再生
/// </summary>
public class HandItemPhone : MonoBehaviour
{
    [Header("通話イベント時に再生する SE")]
    [SerializeField] private AudioClip clip;
    [SerializeField] private AudioSource source;

    private bool isCallEventEnd = false;

    private void OnEnable()
    {
        // 手に持ったタイミングでフラグを立てる
        GameManager.instance.isHandPhone = true;
        GameManager.instance.isHandItemUse = true;

        // すでに通話イベントが発生していた場合は即フラグを更新
        if (GameManager.instance.isCallPhone)
        {
            isCallEventEnd = true;
        }
    }

    private void Update()
    {
        // 通話イベントが開始されたが、まだSEを再生していない場合のみ処理
        if (GameManager.instance.isCallPhone && !isCallEventEnd)
        {
            isCallEventEnd = true;
            if (clip != null && source != null)
            {
                SoundManager.instance.PlaySE(clip, source);
            }
        }
    }

    private void OnDisable()
    {
        // 装備解除時にフラグをリセット
        GameManager.instance.isHandPhone = false;
        GameManager.instance.isHandItemUse = false;
    }
}
