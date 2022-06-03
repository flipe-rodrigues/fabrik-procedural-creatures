using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEditor;

namespace ProceduralAnimation
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(ChainBhv))]
    public class ChainEditor : Editor
    {
        //public override void OnInspectorGUI()
        //{
        //    base.OnInspectorGUI();
        //}

        //public void OnSceneGUI()
        //{
        //    ChainBhv chain = target as ChainBhv;

        //    CommandBuffer commandBuffer = new CommandBuffer();

        //    for (int i = 0; i < chain.Joints.Length - 1; i++)
        //    {
        //        Handles.DrawLine(chain.Joints[i].Position, chain.Joints[i + 1].Position);
                
        //        commandBuffer.DrawMesh(this.Test(), Matrix4x4.TRS(chain.Links[i].Position, chain.Links[i].Rotation, chain.Links[i].Scale))
        //    }


        //    var tr = chain.transform;
        //    var pos = tr.position;

        //    // display an orange disc where the object is
        //    var color = new Color(1, 0.8f, 0.4f, 1);
        //    Handles.color = color;
        //    Handles.DrawWireDisc(pos, tr.up, 1.0f);

        //    // display object "value" in scene
        //    GUI.color = color;
        //    Handles.Label(pos, chain.Depth.ToString("F1"));
        //}

        //public Mesh Test()
        //{
        //    var mesh = new Mesh();
        //    mesh.name = "BoneRendererPyramidMesh";
        //    //mesh.subMeshCount = (int)BatchRenderer.SubMeshType.Count;
        //    mesh.hideFlags = HideFlags.DontSave;

        //    // Bone vertices
        //    Vector3[] vertices = new Vector3[]
        //    {
        //                new Vector3(0.0f, 1.0f, 0.0f),
        //                new Vector3(0.0f, 0.0f, -1.0f),
        //                new Vector3(-0.9f, 0.0f, 0.5f),
        //                new Vector3(0.9f, 0.0f, 0.5f),
        //    };

        //    mesh.vertices = vertices;

        //    // Build indices for different sub meshes
        //    int[] boneFaceIndices = new int[]
        //    {
        //                0, 2, 1,
        //                0, 1, 3,
        //                0, 3, 2,
        //                1, 2, 3
        //    };
        //    mesh.SetIndices(boneFaceIndices, MeshTopology.Triangles, (int)BatchRenderer.SubMeshType.BoneFaces);

        //    int[] boneWireIndices = new int[]
        //    {
        //                0, 1, 0, 2, 0, 3, 1, 2, 2, 3, 3, 1
        //    };
        //    mesh.SetIndices(boneWireIndices, MeshTopology.Lines, (int)BatchRenderer.SubMeshType.BoneWire);

        //    return mesh;
        //}

        //private static Matrix4x4 ComputeBoneMatrix(Vector3 start, Vector3 end, float length, float size)
        //{
        //    Vector3 direction = (end - start) / length;
        //    Vector3 tangent = Vector3.Cross(direction, Vector3.up);
        //    if (Vector3.SqrMagnitude(tangent) < 0.1f)
        //        tangent = Vector3.Cross(direction, Vector3.right);
        //    tangent.Normalize();
        //    Vector3 bitangent = Vector3.Cross(direction, tangent);

        //    float scale = length * k_BoneBaseSize * size;

        //    return new Matrix4x4(
        //        new Vector4(tangent.x * scale, tangent.y * scale, tangent.z * scale, 0f),
        //        new Vector4(direction.x * length, direction.y * length, direction.z * length, 0f),
        //        new Vector4(bitangent.x * scale, bitangent.y * scale, bitangent.z * scale, 0f),
        //        new Vector4(start.x, start.y, start.z, 1f));
        //}
    }
}