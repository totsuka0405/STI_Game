using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UIElements;

public class ShakeHouse : MonoBehaviour
{
    public class EarthquakeEvent
    {
        public float duration;
        public float maxMagnitude;

        public EarthquakeEvent(float duration, float maxMagnitude)
        {
            this.duration = duration;
            this.maxMagnitude = maxMagnitude;
        }
    }

    [SerializeField] float dampingSpeed = 1.0f; // 揺れの減衰スピード
    [SerializeField] float shakeFrequency = 20f; // 振動の周波数
    [SerializeField] float shakeAmplitude = 3.0f; // 振動の振幅
    [SerializeField] AudioClip shakeSound;
    [SerializeField] AudioSource shakeSource;

    private Rigidbody rb;
    private bool isShaking = false;
    private float shakeTimeRemaining;
    private float currentMaxShakeMagnitude;
    private int currentEventIndex = 0;
    private List<EarthquakeEvent> earthquakeEvents = new List<EarthquakeEvent>();
    private Vector3 position;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;
    }

    void Update()
    {
        if (isShaking && shakeTimeRemaining > 0)
        {
            float halfDuration = earthquakeEvents[currentEventIndex].duration / 3f;
            float elapsed = earthquakeEvents[currentEventIndex].duration - shakeTimeRemaining;
            float shakeMagnitude;

            if (elapsed <= halfDuration)
            {
                shakeMagnitude = Mathf.Lerp(0, currentMaxShakeMagnitude, elapsed / halfDuration);
            }
            else if (elapsed <= 2 * halfDuration)
            {
                shakeMagnitude = currentMaxShakeMagnitude;
            }
            else
            {
                shakeMagnitude = Mathf.Lerp(currentMaxShakeMagnitude, 0, (elapsed - 2 * halfDuration) / halfDuration);
                SoundManager.instance.StopLoopSEWithFadeOut(shakeSource, 8);
            }

            float shakeOffsetX = Mathf.Sin(Time.time * shakeFrequency * 1.0f) * shakeMagnitude * shakeAmplitude;
            float shakeOffsetZ = Mathf.Cos(Time.time * shakeFrequency * 0.5f) * shakeMagnitude * shakeAmplitude;
            Vector3 shakeForce = new Vector3(shakeOffsetX, 0f, shakeOffsetZ);

            rb.AddForce(shakeForce * Time.deltaTime, ForceMode.Impulse);

            shakeTimeRemaining -= Time.deltaTime * dampingSpeed;
        }
        else if (shakeTimeRemaining <= 0 && isShaking)
        {
            StopShake(); // Shake停止処理を呼び出す

            currentEventIndex++;
            if (currentEventIndex < earthquakeEvents.Count)
            {
                StartShake();
            }
        }
    }

    public void AddEarthquakeEvent(float duration, float maxMagnitude)
    {
        earthquakeEvents.Add(new EarthquakeEvent(duration, maxMagnitude));
    }

    public void StartShake()
    {
        if (currentEventIndex < earthquakeEvents.Count)
        {
            isShaking = true;
            shakeTimeRemaining = earthquakeEvents[currentEventIndex].duration;
            currentMaxShakeMagnitude = earthquakeEvents[currentEventIndex].maxMagnitude;
            SoundManager.instance.PlayLoopSEWithFadeIn(shakeSound,shakeSource, 8);
        }
    }

    private void StopShake()
    {
        isShaking = false;
        rb.linearVelocity = Vector3.zero; // 移動速度をリセット
        rb.angularVelocity = Vector3.zero; // 回転速度をリセット
    }
}
