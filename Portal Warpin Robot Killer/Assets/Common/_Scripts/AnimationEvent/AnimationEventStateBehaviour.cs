using UnityEngine;
using UnityEngine.Events;

// https://www.youtube.com/watch?v=XEDi7fUCQos

public class AnimationEventStateBehaviour : StateMachineBehaviour
{
    [SerializeField] private string _eventName;
    [Range(0f, 1f)] public float triggerTime;

    private bool _hasTriggered;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _hasTriggered = false;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float currTime = stateInfo.normalizedTime % 1f;

        if (!_hasTriggered && currTime >= triggerTime)
        {
            NotifyReceiver(animator);
            _hasTriggered = true;
        }
    }

    private void NotifyReceiver(Animator animator)
    {
        AnimationEventReceiver receiver = animator.GetComponent<AnimationEventReceiver>();
        if (receiver != null)
        {
            receiver.OnAnimationEventTriggered(_eventName);
        }
    }
}
