using System;
using UnityEngine.Events;

// https://www.youtube.com/watch?v=XEDi7fUCQos

[Serializable]
public class AnimationEvent
{
    public string eventName;
    public UnityEvent OnAnimationEvent;
}
