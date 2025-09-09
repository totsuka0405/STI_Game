using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;

/// <summary>
/// スライダーUIを通じて PostProcessing の AutoExposure を調整するコンポーネント。
/// </summary>
public class BrightnessController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PostProcessVolume postProcessVolume; // PostProcessVolume（AutoExposure含む）
    [SerializeField] private Slider exposureSlider;               // 明るさ調整用スライダー

    private AutoExposure autoExposure;

    private void Start()
    {
        InitializeAutoExposure();
        InitializeSlider();
    }

    /// <summary>
    /// PostProcessVolume から AutoExposure を取得する。
    /// </summary>
    private void InitializeAutoExposure()
    {
        if (postProcessVolume == null || postProcessVolume.profile == null)
        {
            Debug.LogWarning("PostProcessVolume or profile is not assigned.");
            return;
        }

        if (postProcessVolume.profile.TryGetSettings(out autoExposure))
        {
            Debug.Log("AutoExposure found and linked.");
        }
        else
        {
            Debug.LogWarning("AutoExposure not found in the profile.");
        }
    }

    /// <summary>
    /// スライダーの初期化とリスナー登録。
    /// </summary>
    private void InitializeSlider()
    {
        if (exposureSlider == null || autoExposure == null) return;

        exposureSlider.minValue = 0.05f;
        exposureSlider.maxValue = 3.0f;
        exposureSlider.value = autoExposure.keyValue.value;

        exposureSlider.onValueChanged.AddListener(SetExposureCompensation);
    }

    /// <summary>
    /// スライダーの値を AutoExposure.keyValue に反映。
    /// </summary>
    public void SetExposureCompensation(float value)
    {
        if (autoExposure == null) return;

        autoExposure.keyValue.value = value;
        Debug.Log($"Exposure Compensation set to: {autoExposure.keyValue.value}");
    }
}
