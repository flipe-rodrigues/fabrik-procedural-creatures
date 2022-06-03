using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ProceduralAnimation
{
    [CustomEditor(typeof(InverseKinematicsBhv))]
    public class InverseKinematicsEditor : Editor
    {
        private SerializedProperty _terrainLayerMask;
        private SerializedProperty _snapToTerrain;
        private SerializedProperty _terrainOffset;

        public void OnEnable()
        {
            _terrainLayerMask = serializedObject.FindProperty("_terrainLayerMask");

            _snapToTerrain = serializedObject.FindProperty("_snapToTerrain");

            _terrainOffset = serializedObject.FindProperty("_terrainOffset");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            //return;

            //InverseKinematicsBhv inverseKinematics = target as InverseKinematicsBhv;

            //serializedObject.Update();

            //EditorExtensions.ScriptHolder(target);

            //EditorGUILayout.Separator();

            //EditorExtensions.PropertyField("Terrain Layer Mask", _terrainLayerMask);

            //EditorExtensions.PropertyField("Snap To Terrain", _snapToTerrain);

            //if (_snapToTerrain.boolValue)
            //{
            //    EditorGUI.indentLevel++;

            //    EditorExtensions.PropertyField("Offset", _terrainOffset);

            //    EditorGUI.indentLevel--;
            //}

            //serializedObject.ApplyModifiedProperties();
        }
    }
}
