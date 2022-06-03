using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ProceduralAnimation
{
    [RequireComponent(typeof(LinkBhv))]
    public class LinkGizmoBhv : GizmoBhv
    {
        // Public properties
        public override bool IsActive { get => Selection.Contains(this.Parent.gameObject); }
    }
}
