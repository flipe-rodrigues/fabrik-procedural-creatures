using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ProceduralAnimation
{
    [CustomPropertyDrawer(typeof(MonoBehaviourPair<JointBhv, TargetGizmoBhv>))]
    public class MonoBehaviourPairPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            SerializedProperty key = property.FindPropertyRelative("_key");

            float width = EditorGUIUtility.currentViewWidth - 57;

            Rect rect = new Rect(position.x, position.y, width, EditorGUIUtility.singleLineHeight);

            EditorGUI.MultiPropertyField(rect, new GUIContent[] { new GUIContent(""), new GUIContent("") }, key);

            EditorGUI.EndProperty();
        }
    }
}
