using UnityEngine;
using System.Collections.Generic;

public class EarthquakeEvent_1 : MonoBehaviour
{
    public class EarthquakeEvent_2
    {
        public float duration;
        public float maxMagnitude;

        public EarthquakeEvent_2(float duration, float maxMagnitude)
        {
            this.duration = duration;
            this.maxMagnitude = maxMagnitude;
        }
    }

    [SerializeField] float dampingSpeed = 1.0f; // 揺れの減衰スピード
    [SerializeField] float shakeFrequency = 20f; // 振動の周波数
    [SerializeField] float shakeAmplitude = 3.0f; // 振動の振幅

    private Rigidbody rb;
    private bool isShaking = false;
    private float shakeTimeRemaining;
    private float currentMaxShakeMagnitude;
    private int currentEventIndex = 0;
    private List<EarthquakeEvent_2> earthquakeEvents = new List<EarthquakeEvent_2>();

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;
    }

    private void OnEnable()
    {
        EventManager.OnDisasterEventTriggered += HandleEvent;
    }

    private void OnDisable()
    {
        EventManager.OnDisasterEventTriggered -= HandleEvent;
    }

    private void HandleEvent(string eventType)
    {
        if (eventType == "Earthquake")
        {
            Debug.Log("Handling earthquake...");
            // 地震の処理
        }
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
            }

            float shakeOffsetX = Mathf.Sin(Time.time * shakeFrequency * 1.0f) * shakeMagnitude * shakeAmplitude;
            float shakeOffsetZ = Mathf.Cos(Time.time * shakeFrequency * 0.5f) * shakeMagnitude * shakeAmplitude;
            Vector3 shakeForce = new Vector3(shakeOffsetX, 0f, shakeOffsetZ);

            rb.AddForce(shakeForce * Time.deltaTime, ForceMode.Impulse);

            shakeTimeRemaining -= Time.deltaTime * dampingSpeed;
        }
        else if (shakeTimeRemaining <= 0 && isShaking)
        {
            isShaking = false;
            rb.velocity = Vector3.zero; // 加わっている力を0にする
            rb.angularVelocity = Vector3.zero;

            currentEventIndex++;
            if (currentEventIndex < earthquakeEvents.Count)
            {
                StartShake();
            }
        }
    }

    public void AddEarthquakeEvent(float duration, float maxMagnitude)
    {
        earthquakeEvents.Add(new EarthquakeEvent_2(duration, maxMagnitude));
    }

    public void StartShake()
    {
        if (currentEventIndex < earthquakeEvents.Count)
        {
            isShaking = true;
            shakeTimeRemaining = earthquakeEvents[currentEventIndex].duration;
            currentMaxShakeMagnitude = earthquakeEvents[currentEventIndex].maxMagnitude;
        }
    }
}
