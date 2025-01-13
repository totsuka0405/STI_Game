using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class OpenCloseAnim : MonoBehaviour
{
    [SerializeField] private Animation animations;
    [SerializeField] private AnimationClip openAnimation;  // 開くアニメーション
    [SerializeField] private AnimationClip closeAnimation; // 閉じるアニメーション
    [SerializeField] AudioClip openSE;

    private bool isOpen = false;
    private bool isAnimating = false;

    private void Start()
    {
        animations = GetComponent<Animation>();
    }

    public void ToggleDoor()
    {
        if (isAnimating) return;  // アニメーション中に別のアクションを実行しない

        if (isOpen)
        {
            StartCoroutine(PlayAnimationAndWait(closeAnimation));
            Debug.Log("tozita");  // 閉じた
        }
        else
        {
            StartCoroutine(PlayAnimationAndWait(openAnimation));
            Debug.Log("hiraita");  // 開いた
        }
    }

    private IEnumerator PlayAnimationAndWait(AnimationClip animationClip)
    {
        isAnimating = true;  // アニメーション中フラグを立てる

        // アニメーションを再生
        animations.Play(animationClip.name);
        SoundManager.instance.PlaySE(openSE, transform.position, this.transform);
        // アニメーションの長さを取得
        float animationDuration = animationClip.length;

        // アニメーションの再生が終わるまで待機
        yield return new WaitForSeconds(animationDuration);

        isOpen = !isOpen;  // ドアの状態を反転
        isAnimating = false;  // アニメーション終了後、フラグを元に戻す
    }
}
