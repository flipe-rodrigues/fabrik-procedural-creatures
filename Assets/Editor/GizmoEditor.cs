using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ProceduralAnimation
{
    //[CustomEditor(typeof(GizmoBhv), true)]
    //public class GizmoEditor : Editor
    //{
    //    private SerializedProperty _isActive;

    //    private void OnEnable()
    //    {
    //        _isActive = serializedObject.FindProperty("_isActive");
    //    }

    //    public override void OnInspectorGUI()
    //    {
    //        //base.OnInspectorGUI();

    //        Debug.Log(target.name);

    //        //serializedObject.Update();

    //        _isActive.boolValue = Selection.activeGameObject == target;

    //        serializedObject.ApplyModifiedProperties();
    //    }

    //    private void OnSceneGUI()
    //    {
    //        GizmoBhv gizmo = target as GizmoBhv;

    //        serializedObject.Update();

    //        _isActive.boolValue = Selection.Contains(target);

    //        serializedObject.ApplyModifiedProperties();
    //    }
    //}
}
