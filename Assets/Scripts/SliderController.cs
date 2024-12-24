using UnityEngine;
using UnityEngine.UI;

public class SliderController : MonoBehaviour
{
    [SerializeField] private Slider seSlider;
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private SoundManager soundManager;

    private void Start()
    {
        // Sliderの初期値を設定
        seSlider.value = 0.5f;
        bgmSlider.value = 0.5f;

        // Sliderの値変更イベントを追加
        seSlider.onValueChanged.AddListener(HandleSEVolumeChange);
        bgmSlider.onValueChanged.AddListener(HandleBGMVolumeChange);
    }

    // SE音量変更用
    private void HandleSEVolumeChange(float value)
    {
        soundManager.SetSEVolume(value);
    }

    // BGM音量変更用
    private void HandleBGMVolumeChange(float value)
    {
        soundManager.SetBGMVolume(value);
    }
}
