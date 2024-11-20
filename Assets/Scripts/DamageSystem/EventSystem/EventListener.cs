using UnityEngine;
using UnityEngine.Events;

public abstract class EventListener<T> : MonoBehaviour {
    [SerializeField] EventChannel<T> eventChannel;
    [SerializeField] MonoEventChannel<T> monoEventChannel;
    [SerializeField] UnityEvent<T> unityEvent;

    protected void Awake()
    {
        if(eventChannel)
            eventChannel.Register(this);

        if(monoEventChannel)
            monoEventChannel.Register(this);
    }
    protected void OnDestroy()
    {
        if(eventChannel)
            eventChannel.Deregister(this);

        if(monoEventChannel)
            monoEventChannel.Deregister(this);
    }

    public void Raise(T value)
    {
        unityEvent?.Invoke(value);
    }
    
}

public class EventListener : EventListener<Empty> { }

