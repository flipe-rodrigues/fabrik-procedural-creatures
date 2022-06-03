using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProceduralAnimation
{
    [CreateAssetMenu(fileName = "To Do", menuName = "ScriptableObjects/To Do", order = 1)]
    public class ToDoScriptableObject : ScriptableObject
    {
        // Public fields
        [TextArea]
        public string item1;
        [TextArea]
        public string item2;
        [TextArea]
        public string item3;
        [TextArea]
        public string item4;
        [TextArea]
        public string item5;
    }
}
