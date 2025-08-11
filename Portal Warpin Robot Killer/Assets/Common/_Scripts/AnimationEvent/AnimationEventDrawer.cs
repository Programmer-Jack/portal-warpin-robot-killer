#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

// https://www.youtube.com/watch?v=XEDi7fUCQos

[CustomPropertyDrawer(typeof(AnimationEvent))]
public class AnimationEventDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        SerializedProperty stateNameProperty = property.FindPropertyRelative("eventName");
        SerializedProperty stateEventProperty = property.FindPropertyRelative("OnAnimationEvent");

        Rect stateNameRect = new(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
        Rect stateEventRect = new(position.x, position.y + EditorGUIUtility.singleLineHeight + 2, position.width, EditorGUI.GetPropertyHeight(stateEventProperty));

        EditorGUI.PropertyField(stateNameRect, stateNameProperty);
        EditorGUI.PropertyField(stateEventRect, stateEventProperty);

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        SerializedProperty stateEventProperty = property.FindPropertyRelative("OnAnimationEvent");
        return EditorGUIUtility.singleLineHeight + EditorGUI.GetPropertyHeight(stateEventProperty) + 4;
    }
}

#endif