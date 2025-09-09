using UnityEngine;

/// <summary>
/// リトライボタンの処理を担当するコンポーネント。
/// UI ボタンの OnClick イベントから呼び出して利用する。
/// </summary>
public class RetryButton : MonoBehaviour
{
    /// <summary>
    /// リトライ処理を実行。
    /// </summary>
    public void OnRetry()
    {
        if (GameManager.instance != null)
        {
            GameManager.instance.Retry();
        }
        else
        {
            Debug.LogError("GameManager.instance が見つかりません。Retry を実行できません。");
        }
    }
}
