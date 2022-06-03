using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ProceduralAnimation
{
    [CustomPropertyDrawer(typeof(ScaleCurves))]
    public class ScaleCurvesPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label, EditorStyles.boldLabel);

            SerializedProperty separateAxes = property.FindPropertyRelative("_separateAxes");

            SerializedProperty xScaleCurve = property.FindPropertyRelative("_xScaleCurve");
            SerializedProperty yScaleCurve = property.FindPropertyRelative("_yScaleCurve");
            SerializedProperty zScaleCurve = property.FindPropertyRelative("_zScaleCurve");

            SerializedProperty curveOffsets = property.FindPropertyRelative("_curveOffsets");
            SerializedProperty curveGains = property.FindPropertyRelative("_curveGains");

            SerializedProperty scaleCurve = property.FindPropertyRelative("_scaleCurve");

            SerializedProperty curveOffset = property.FindPropertyRelative("_curveOffset");
            SerializedProperty curveGain = property.FindPropertyRelative("_curveGain");

            EditorGUI.indentLevel++;

            EditorExtensions.PropertyField("Separate Axes", separateAxes);

            if (separateAxes.boolValue)
            {
                float defaultLabelWidth = EditorGUIUtility.labelWidth;

                EditorGUILayout.LabelField("Curves");
                GUILayout.Space(-EditorGUIUtility.singleLineHeight);

                GUILayout.BeginHorizontal();
                GUILayout.Space(EditorGUIUtility.labelWidth);
                EditorGUIUtility.labelWidth = 28f;

                GUILayout.Space(-EditorGUIUtility.labelWidth / 2 + 1);
                EditorExtensions.PropertyField("X", xScaleCurve);
                GUILayout.Space(-EditorGUIUtility.labelWidth / 2);
                EditorExtensions.PropertyField("Y", yScaleCurve);
                GUILayout.Space(-EditorGUIUtility.labelWidth / 2);
                EditorExtensions.PropertyField("Z", zScaleCurve);

                EditorGUIUtility.labelWidth = defaultLabelWidth;
                GUILayout.EndHorizontal();

                EditorExtensions.PropertyField("Offsets", curveOffsets);

                EditorExtensions.PropertyField("Gains", curveGains);
            }
            else
            {
                EditorExtensions.PropertyField("Curve", scaleCurve, true, GUILayout.ExpandWidth(true));

                GUILayout.Space(2);

                EditorExtensions.PropertyField("Offset", curveOffset);

                EditorExtensions.PropertyField("Gain", curveGain);
            }

            EditorGUI.indentLevel--;

            EditorGUI.EndProperty();
        }
    }
}
