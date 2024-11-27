using UnityEngine;

public class EventManager : MonoBehaviour
{
    public delegate void DisasterEventHandler(string eventType);
    public static event DisasterEventHandler OnDisasterEventTriggered;

    public void TriggerDisaster(string eventType)
    {
        Debug.Log($"Disaster triggered: {eventType}");
        OnDisasterEventTriggered?.Invoke(eventType);
    }
}
