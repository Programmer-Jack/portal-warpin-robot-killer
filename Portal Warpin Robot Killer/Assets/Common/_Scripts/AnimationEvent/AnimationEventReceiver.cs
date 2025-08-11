using System.Collections.Generic;
using UnityEngine;

// https://www.youtube.com/watch?v=XEDi7fUCQos

public class AnimationEventReceiver : MonoBehaviour
{
    [SerializeField] private List<AnimationEvent> _animationEvents = new();

    public void OnAnimationEventTriggered(string eventName)
    {
        AnimationEvent matchingEvent = _animationEvents.Find(se => se.eventName == eventName);
        matchingEvent?.OnAnimationEvent?.Invoke();
    }
}
