using UnityEngine;
using System.Collections.Generic;

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

    public float dampingSpeed = 1.0f; // 揺れの減衰スピード

    private Vector3 initialPosition;
    private bool isShaking = false;
    private float shakeTimeRemaining;
    private float currentMaxShakeMagnitude;
    private float shakeOffsetX;
    private int currentEventIndex = 0;
    private List<EarthquakeEvent> earthquakeEvents = new List<EarthquakeEvent>();

    void Start()
    {
        initialPosition = transform.position;
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

            shakeOffsetX = Mathf.Sin(Time.time * 20f) * shakeMagnitude;
            Vector3 shakeOffset = new Vector3(shakeOffsetX, 0f, 0f);
            transform.position = initialPosition + shakeOffset;

            shakeTimeRemaining -= Time.deltaTime * dampingSpeed;
        }
        else if (shakeTimeRemaining <= 0 && isShaking)
        {
            isShaking = false;
            transform.position = initialPosition;

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
        }
    }
}
