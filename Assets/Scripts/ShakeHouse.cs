using UnityEngine;

public class ShakeHouse : MonoBehaviour
{
    public float shakeDuration = 4f;       // 揺れの継続時間（全体）
    public float maxShakeMagnitude = 0.1f; // 最大揺れの強度
    public float dampingSpeed = 1.0f;      // 揺れの減衰スピード

    private Vector3 initialPosition;
    private bool isShaking = false;
    private float shakeTimeRemaining;
    private float shakeOffsetX;

    void Start()
    {
        initialPosition = transform.position;
    }

    void Update()
    {
        // 揺れを再現
        if (isShaking && shakeTimeRemaining > 0)
        {
            // 揺れの強度を時間に基づいて計算
            float halfDuration = shakeDuration / 3f;
            float elapsed = shakeDuration - shakeTimeRemaining;
            float shakeMagnitude;

            if (elapsed <= halfDuration)
            {
                // 揺れが強くなるフェーズ
                shakeMagnitude = Mathf.Lerp(0, maxShakeMagnitude, elapsed / halfDuration);
            }
            else if (elapsed <= 2 * halfDuration)
            {
                // 最大揺れのフェーズ
                shakeMagnitude = maxShakeMagnitude;
            }
            else
            {
                // 揺れが弱くなるフェーズ
                shakeMagnitude = Mathf.Lerp(maxShakeMagnitude, 0, (elapsed - 2 * halfDuration) / halfDuration);
            }

            // 左右に揺らす
            shakeOffsetX = Mathf.Sin(Time.time * 20f) * shakeMagnitude;
            Vector3 shakeOffset = new Vector3(shakeOffsetX, 0f, 0f);
            transform.position = initialPosition + shakeOffset;

            shakeTimeRemaining -= Time.deltaTime * dampingSpeed;
        }
        else if (shakeTimeRemaining <= 0 && isShaking)
        {
            isShaking = false;
            transform.position = initialPosition;  // 揺れが終了したら元の位置に戻す
        }
    }

    // 揺れを開始するメソッド
    public void StartShake()
    {
        isShaking = true;
        shakeTimeRemaining = shakeDuration;
    }
}
