using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ProceduralAnimation
{
    //[InitializeOnLoad]
    //public static class HierarchyMonitor
    //{
    //    static HierarchyMonitor()
    //    {
    //        EditorApplication.hierarchyChanged += OnHierarchyChanged;
    //    }

    //    private static void OnHierarchyChanged()
    //    {
    //        var all = Resources.FindObjectsOfTypeAll(typeof(GameObject));

    //        var numberVisible = all.Where(obj => (obj.hideFlags & HideFlags.HideInHierarchy) != HideFlags.HideInHierarchy).Count();

    //        Debug.LogFormat("There are currently {0} GameObjects visible in the hierarchy.", numberVisible);
    //    }
    //}
}
