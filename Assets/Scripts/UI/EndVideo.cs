using UnityEngine;
using UnityEngine.Video;

/// <summary>
/// エンディング用の動画再生と終了後のUI切り替え制御。
/// </summary>
public class EndVideo : MonoBehaviour
{
    [Header("参照コンポーネント")]
    [SerializeField] private VideoPlayer videoPlayer;

    [Header("動画終了後に表示するUI")]
    [SerializeField] private GameObject endUI;

    private void Start()
    {
        if (videoPlayer == null)
        {
            Debug.LogError("VideoPlayer が設定されていません", this);
            return;
        }

        videoPlayer.loopPointReached += OnVideoFinished;
        videoPlayer.Play();
    }

    /// <summary>
    /// 動画が最後まで再生されたときに呼ばれる。
    /// </summary>
    private void OnVideoFinished(VideoPlayer vp)
    {
        gameObject.SetActive(false);

        if (endUI != null)
        {
            endUI.SetActive(true);
        }
    }
}
