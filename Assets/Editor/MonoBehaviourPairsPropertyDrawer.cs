using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ProceduralAnimation
{
    [CustomPropertyDrawer(typeof(MonoBehaviourPairs<JointBhv, TargetGizmoBhv>))]
    public class MonoBehaviourPairsPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            SerializedProperty pairs = property.FindPropertyRelative("_pairs");

            float width = EditorGUIUtility.currentViewWidth - 22;

            Rect rect = new Rect(position.x, position.y, width, EditorGUIUtility.singleLineHeight);

            EditorGUI.PropertyField(rect, pairs, new GUIContent(label));

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            SerializedProperty pairs = property.FindPropertyRelative("_pairs");

            return base.GetPropertyHeight(property, label) * (pairs.isExpanded ? pairs.arraySize + 3 : 1);
        }
    }
}
