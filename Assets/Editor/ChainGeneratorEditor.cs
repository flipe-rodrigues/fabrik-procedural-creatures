using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ProceduralAnimation
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(ChainGenerator))]
    public class ChainGeneratorEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            foreach (ChainGenerator chainGenerator in targets as ChainGenerator[])
            {
                GUILayout.BeginHorizontal();

                if (GUILayout.Button("Sample Link Scales"))
                {
                    chainGenerator.SampleLinkScales();
                }

                GUI.enabled = !chainGenerator.HasChain && chainGenerator.HasInitializedScales;

                if (GUILayout.Button("Generate"))
                {
                    chainGenerator.InstantiateChain();
                }

                GUI.enabled = chainGenerator.HasChain;

                if (GUILayout.Button("Randomize Pose"))
                {
                    chainGenerator.RandomizePose();
                }

                GUI.enabled = chainGenerator.HasChain;

                if (GUILayout.Button("Reset"))
                {
                    chainGenerator.Reset();
                }

                GUILayout.EndHorizontal();
            }
        }
    }
}
