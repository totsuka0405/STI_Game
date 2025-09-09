using System.Collections;
using UnityEngine;

/// <summary>
/// ドアや開閉可能オブジェクトのアニメーション制御。
/// ・ToggleDoor() 呼び出しで開閉をトグル
/// ・アニメーション再生中は次の入力を受け付けない
/// ・開閉時にSEを再生
/// </summary>
[RequireComponent(typeof(Animation))]
public class OpenCloseAnim : MonoBehaviour
{
    [Header("Animation Clips")]
    [SerializeField] private AnimationClip openAnimation;   // 開くアニメーション
    [SerializeField] private AnimationClip closeAnimation;  // 閉じるアニメーション

    [Header("SE")]
    [SerializeField] private AudioClip openSE;              // 開閉共通のSE
    [SerializeField] private AudioSource source;

    private Animation animations;   // Unityの旧Animationコンポーネント
    private bool isOpen = false;    // 現在の状態
    private bool isAnimating = false; // 再生中かどうか

    private void Awake()
    {
        animations = GetComponent<Animation>();
    }

    /// <summary>
    /// ドア開閉トグル。現在の状態に応じて開アニメ or 閉アニメを再生する。
    /// </summary>
    public void ToggleDoor()
    {
        if (isAnimating) return; // アニメーション中は無視

        if (isOpen)
        {
            StartCoroutine(PlayAnimationAndWait(closeAnimation));
            Debug.Log("ドアを閉じた");
        }
        else
        {
            StartCoroutine(PlayAnimationAndWait(openAnimation));
            Debug.Log("ドアを開いた");
        }
    }

    /// <summary>
    /// 指定したアニメーションを再生し、完了を待って状態を反転。
    /// </summary>
    private IEnumerator PlayAnimationAndWait(AnimationClip animationClip)
    {
        if (animationClip == null)
        {
            Debug.LogWarning("AnimationClip が設定されていません");
            yield break;
        }

        isAnimating = true;

        // アニメーション再生
        animations.Play(animationClip.name);

        // SE再生
        if (SoundManager.instance != null)
        {
            SoundManager.instance.PlaySE(openSE, source);
        }

        // アニメーションの長さ分待機
        yield return new WaitForSeconds(animationClip.length);

        // 状態反転と終了処理
        isOpen = !isOpen;
        isAnimating = false;
    }
}
