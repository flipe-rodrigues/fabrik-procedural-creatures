using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ProceduralAnimation
{
    public static class EditorExtensions
    {
        public static void ScriptHolder(Object target)
        {
            GUI.enabled = false;

            EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour((MonoBehaviour)target), target.GetType(), false);

            GUI.enabled = true;
        }

        public static void Header(string text)
        {
            EditorGUILayout.Separator();

            GUILayout.Label(text, EditorStyles.boldLabel);
        }

        public static void PropertyField(string label, SerializedProperty property, bool enabled = true, params GUILayoutOption[] options)
        {
            GUI.enabled = enabled;

            GUIContent content = new GUIContent(label);

            EditorGUILayout.PropertyField(property, content, options);

            GUI.enabled = true;
        }

        public static void PropertyFieldWithTooltip(string label, string tooltip, SerializedProperty property, bool enabled = true, params GUILayoutOption[] options)
        {
            GUI.enabled = enabled;

            GUIContent content = new GUIContent(label, tooltip);

            EditorGUILayout.PropertyField(property, content, options);

            GUI.enabled = true;
        }
    }
}
