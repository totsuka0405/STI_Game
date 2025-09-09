using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// BGM・SE 音量調整スライダーの制御。
/// ・Start 時に初期値をセット
/// ・スライダーの変更イベントで SoundManager に値を渡す
/// </summary>
public class SliderController : MonoBehaviour
{
    [Header("音量スライダー")]
    [SerializeField] private Slider seSlider;
    [SerializeField] private Slider bgmSlider;

    [Header("サウンド管理")]
    [SerializeField] private SoundManager soundManager;

    private void Start()
    {
        // 初期値を設定
        if (seSlider != null)
        {
            seSlider.value = 0.5f;
            seSlider.onValueChanged.AddListener(HandleSEVolumeChange);
        }

        if (bgmSlider != null)
        {
            bgmSlider.value = 0.5f;
            bgmSlider.onValueChanged.AddListener(HandleBGMVolumeChange);
        }
    }

    /// <summary>
    /// SE音量変更
    /// </summary>
    private void HandleSEVolumeChange(float value)
    {
        if (soundManager != null)
        {
            soundManager.SetSEVolume(value);
        }
    }

    /// <summary>
    /// BGM音量変更
    /// </summary>
    private void HandleBGMVolumeChange(float value)
    {
        if (soundManager != null)
        {
            soundManager.SetBGMVolume(value);
        }
    }
}
