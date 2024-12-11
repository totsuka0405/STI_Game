using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;

public class BrightnessController : MonoBehaviour
{
    [SerializeField] private PostProcessVolume postProcessVolume; // Post-Processing Volumeをアタッチ
    [SerializeField] private Slider exposureSlider; // スライダーUIをアタッチ

    private AutoExposure autoExposure;

    void Start()
    {
        // ProfileからAuto Exposureエフェクトを取得
        if (postProcessVolume.profile.TryGetSettings(out autoExposure))
        {
            Debug.Log("Auto Exposure found!");
        }
        else
        {
            Debug.LogWarning("Auto Exposure not found in the profile.");
        }

        // スライダーの初期設定
        if (exposureSlider != null && autoExposure != null)
        {
            // スライダーの値を初期化
            exposureSlider.minValue = 0.05f; // 最小値
            exposureSlider.maxValue = 3.0f; // 最大値
            exposureSlider.value = autoExposure.keyValue.value; // 現在の値を設定

            // スライダーの値変更イベントを登録
            exposureSlider.onValueChanged.AddListener(SetExposureCompensation);
        }
    }

    // スライダーで変更された値をAuto Exposureに反映
    public void SetExposureCompensation(float value)
    {
        Debug.Log("Slider Value: " + value);
        if (autoExposure != null)
        {
            autoExposure.keyValue.value = value;
            Debug.Log("Exposure Compensation set to: " + autoExposure.keyValue.value);
        }
    }
}
