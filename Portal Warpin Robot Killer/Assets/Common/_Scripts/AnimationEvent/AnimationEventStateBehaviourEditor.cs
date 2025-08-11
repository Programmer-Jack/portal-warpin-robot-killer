#if UNITY_EDITOR

using System;
using System.Linq;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

// https://www.youtube.com/watch?v=XEDi7fUCQos

[CustomEditor(typeof(AnimationEventStateBehaviour))]
public class AnimationEventStateBehaviourEditor : Editor
{
    private AnimationClip _previewClip;
    private float _previewTime;
    private bool _isPreviewing;

    [MenuItem("GameObject/Enfore T-Pose", false, 0)]
    private static void EnforceTPose()
    {
        GameObject selected = Selection.activeGameObject;
        if (!selected || !selected.TryGetComponent(out Animator animator) || !animator.avatar) return;

        SkeletonBone[] skeletonBones = animator.avatar.humanDescription.skeleton;

        foreach (HumanBodyBones hbb in Enum.GetValues(typeof(HumanBodyBones)))
        {
            if (hbb == HumanBodyBones.LastBone) continue;

            Transform boneTransform = animator.GetBoneTransform(hbb);
            if (!boneTransform) continue;

            SkeletonBone skeletonBone = skeletonBones.FirstOrDefault(sb => sb.name == boneTransform.name);
            if (skeletonBone.name == null) continue;

            if (hbb == HumanBodyBones.Hips) boneTransform.localPosition = skeletonBone.position;
            boneTransform.localRotation = skeletonBone.rotation;
        }

        Debug.Log("T-Pose enforced successfully on " + selected.name);
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        AnimationEventStateBehaviour stateBehaviour = (AnimationEventStateBehaviour)target;

        if (Validate(stateBehaviour, out string errorMessage))
        {
            GUILayout.Space(10);

            if (_isPreviewing)
            {
                if (GUILayout.Button("Stop Preview"))
                {
                    EnforceTPose();
                    _isPreviewing = false;
                }
                else
                {
                    PreviewAnimationClip(stateBehaviour);
                }   
            }
            else if (GUILayout.Button("Preview"))
            {
                _isPreviewing = true;
            }

            GUILayout.Label($"Previewing at {_previewTime:F2}s", EditorStyles.helpBox);
        }
        else
        {
            EditorGUILayout.HelpBox(errorMessage, MessageType.Info);
        }
    }

    private void PreviewAnimationClip(AnimationEventStateBehaviour stateBehaviour)
    {
        if (_previewClip == null) return;

        _previewTime = stateBehaviour.triggerTime * _previewClip.length;

        AnimationMode.StartAnimationMode();
        AnimationMode.SampleAnimationClip(Selection.activeGameObject, _previewClip, _previewTime);
        AnimationMode.StopAnimationMode();
    }

    private bool Validate(AnimationEventStateBehaviour stateBehaviour, out string errorMessage)
    {
        AnimatorController animatorController = GetValidAnimatorController(out errorMessage);
        if (animatorController == null) return false;

        ChildAnimatorState matchingState = animatorController.layers
            .SelectMany(layer => layer.stateMachine.states)
            .FirstOrDefault(state => state.state.behaviours.Contains(stateBehaviour));

        _previewClip = matchingState.state?.motion as AnimationClip;
        if (_previewClip == null)
        {
            errorMessage = "No valid AnimationClip found for the current state";
            return false;
        }

        return true;
    }

    private AnimatorController GetValidAnimatorController(out string errorMessage)
    {
        errorMessage = string.Empty;

        GameObject targetGameObject = Selection.activeGameObject;
        if (targetGameObject == null)
        {
            errorMessage = "Please select a GameObject with an Animator to preview";
            return null;
        }

        Animator animator = targetGameObject.GetComponent<Animator>();
        if (animator == null)
        {
            errorMessage = "The selected GameObject does not have an Animator component";
            return null;
        }

        AnimatorController animatorController = animator.runtimeAnimatorController as AnimatorController;
        if (animatorController == null)
        {
            errorMessage = "The selected Animator does not have a valid AnimatorController";
            return null;
        }

        return animatorController;
    }
}

#endif