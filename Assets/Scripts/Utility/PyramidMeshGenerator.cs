using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ProceduralAnimation
{
    public class PyramidMeshGenerator : MonoBehaviour
    {
        public int[] faceIndices = new int[]
   {
                        1, 2, 0,
                        3, 1, 0,
                        2, 3, 0,
                        3, 2, 1
   };

        private Mesh CreatePyramidMesh()
        {
            Mesh mesh = new Mesh();

            mesh.name = "PyramidMesh";

            mesh.subMeshCount = 2;

            mesh.hideFlags = HideFlags.DontSave;

            Vector3[] vertices = new Vector3[]
            {
                        new Vector3(0.0f, 0.0f, 1.0f),
                        new Vector3(0.0f, -1.0f, 0.0f) / 2,
                        new Vector3(-0.9f, 0.5f, 0.0f) / 2,
                        new Vector3(0.9f, 0.5f, 0.0f) / 2,
            };

            Vector3 offset = vertices.MidPoint();

            //offset.z = vertices[0].z / 2f;

            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i] -= offset;
            }

            mesh.vertices = vertices;

            mesh.SetIndices(faceIndices, MeshTopology.Triangles, 0);

            mesh.RecalculateNormals();

            mesh.RecalculateBounds();

            int[] wireIndices = new int[]
            {
                        0, 1, 0, 2, 0, 3, 1, 2, 2, 3, 3, 1
            };
            mesh.SetIndices(wireIndices, MeshTopology.Lines, 1);

            return mesh;
        }

        [ContextMenu("Create And Save Mesh:")]
        public void CreateAndSavePyramidMesh()
        {
            string assetPath = "Assets/Shapes/Pyramid.asset";

            AssetDatabase.DeleteAsset(assetPath);

            AssetDatabase.CreateAsset(this.CreatePyramidMesh(), assetPath);
        }
    }
}
