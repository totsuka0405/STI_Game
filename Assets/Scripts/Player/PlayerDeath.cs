using UnityEngine;
using System.Collections;

/// <summary>
/// プレイヤー死亡時の演出（カメラ転倒＋フェード）と、死亡フラグの確定を担当するコンポーネント。
/// ・Update でゲーム側フラグ（地震死）を監視し、死亡演出を開始
/// ・当たり判定での即死（火災）にも対応
/// ・演出完了時に GameManager の isPlayerDead を確定
/// </summary>
public class PlayerDeath : MonoBehaviour
{
    [Header("演出対象")]
    [SerializeField] private Camera playerCamera;     // 演出で回転/移動させるプレイヤーカメラ
    [SerializeField] private CanvasGroup deathScreen; // フェード用キャンバス（α制御）

    [Header("カメラ転倒")]
    [SerializeField] private float fallDuration = 1f;                  // 転倒アニメの所要時間
    [SerializeField] private Vector3 fallRotation = new Vector3(90f, 0f, 0f); // 目標回転（オイラー角）
    [SerializeField] private Vector3 fallPositionOffset = new Vector3(0f, -1f, 0f); // 目標位置オフセット

    [Header("フェード")]
    [SerializeField] private float fadeDuration = 1f; // 画面を黒にするまでの時間

    /// <summary>死亡演出が開始済みか（多重起動防止）。</summary>
    private bool isDead = false;

    private void Update()
    {
        // 既に死亡演出中なら監視不要
        if (isDead) return;

        // 地震による死亡フラグを監視し、演出を開始
        if (GameManager.instance.isFirstEarthDie) Die();
        if (GameManager.instance.isSecondEarthDie) Die();
    }

    /// <summary>
    /// 死亡演出を開始する外部API。
    /// ・多重起動を防ぎ、統合的な死亡シーケンスを進める
    /// </summary>
    public void Die()
    {
        if (isDead) return;
        isDead = true;
        StartCoroutine(DeathSequence());
    }

    /// <summary>
    /// 死亡アニメ → フェードの順に進め、最後にゲーム側の死亡確定フラグを立てる。
    /// </summary>
    private IEnumerator DeathSequence()
    {
        StartCoroutine(DeathAnimation());
        yield return StartCoroutine(FadeToBlack());

        // ここでゲームオーバーUIなど後続処理へハンドオフする想定
        GameManager.instance.isPlayerDead = true;
    }

    /// <summary>
    /// カメラを転倒させるアニメーション。
    /// ・位置は現在位置＋オフセットへ補間
    /// ・回転は現在角から目標角へ球面線形補間
    /// </summary>
    private IEnumerator DeathAnimation()
    {
        Vector3 initialPosition = playerCamera.transform.localPosition;
        Quaternion initialRotation = playerCamera.transform.localRotation;

        Vector3 finalPosition = initialPosition + fallPositionOffset;
        Quaternion finalRotation = Quaternion.Euler(fallRotation);

        float elapsed = 0f;
        while (elapsed < fallDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / fallDuration;

            playerCamera.transform.localPosition = Vector3.Lerp(initialPosition, finalPosition, t);
            playerCamera.transform.localRotation = Quaternion.Slerp(initialRotation, finalRotation, t);

            yield return null;
        }

        playerCamera.transform.localPosition = finalPosition;
        playerCamera.transform.localRotation = finalRotation;
    }

    /// <summary>
    /// 画面全体を黒にフェードさせる。
    /// ・CanvasGroup の α を 0→1 へ補間
    /// </summary>
    private IEnumerator FadeToBlack()
    {
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            if (deathScreen != null)
            {
                deathScreen.alpha = Mathf.Lerp(0f, 1f, elapsed / fadeDuration);
            }
            yield return null;
        }
        if (deathScreen != null) deathScreen.alpha = 1f;
    }

    /// <summary>
    /// 火災に触れた瞬間の即死処理、および安全位置（FirstEarthEventPos）進入時の非致死フラグ設定。
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Fire"))
        {
            Die();
            GameManager.instance.isFireDie = true;
        }

        if (other.CompareTag("FirstEarthEventPos"))
        {
            GameManager.instance.isFirstEarthDontDie = true;
        }
    }

    /// <summary>
    /// 安全位置（FirstEarthEventPos）から出た際に、非致死フラグを解除する。
    /// </summary>
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("FirstEarthEventPos"))
        {
            GameManager.instance.isFirstEarthDontDie = false;
        }
    }
}
