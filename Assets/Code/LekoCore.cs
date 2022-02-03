using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rhino.Compute;

static class LekoCore
{
    /// <summary>
    /// Convert mesh from Rhino to Unity
    /// </summary>
    /// <param name="rhinoMesh"></param>
    /// <returns></returns>
    public static Mesh RhinoToUnityMesh(Rhino.Geometry.Mesh rhinoMesh)
    {
        Mesh meshObj = new Mesh();

        var vertices = new List<Vector3>();
        foreach (var meshVertex in rhinoMesh.Vertices)
        {
            var vertex = new Vector3(meshVertex.X, meshVertex.Z, meshVertex.Y);
            vertices.Add(vertex);
        }

        var triangles = new List<int>();
        foreach (var meshFace in rhinoMesh.Faces)
        {
            if (meshFace.IsTriangle)
            {
                triangles.Add(meshFace.C);
                triangles.Add(meshFace.B);
                triangles.Add(meshFace.A);
            }
            else if (meshFace.IsQuad)
            {
                triangles.Add(meshFace.C);
                triangles.Add(meshFace.B);
                triangles.Add(meshFace.A);
                triangles.Add(meshFace.D);
                triangles.Add(meshFace.C);
                triangles.Add(meshFace.A);
            }
        }

        var normals = new List<Vector3>();
        foreach (var normal in rhinoMesh.Normals)
        {
            normals.Add(new Vector3(normal.X, normal.Z, normal.Y));
        }

        meshObj.vertices = vertices.ToArray();
        meshObj.triangles = triangles.ToArray();
        meshObj.normals = normals.ToArray();

        return meshObj;
    }

    /// <summary>
    /// Convert mesh from Unity to Rhino
    /// </summary>
    /// <param name="unityMesh"></param>
    /// <returns></returns>
    public static Rhino.Geometry.Mesh UnityToRhinoMesh(Mesh unityMesh)
    {
        Rhino.Geometry.Mesh meshObj = new Rhino.Geometry.Mesh();

        //Reserve vertices memory
        meshObj.Vertices.Capacity = unityMesh.vertices.Length;

        foreach (var meshVertex in unityMesh.vertices)
        {
            meshObj.Vertices.Add(meshVertex.x, meshVertex.z, meshVertex.y);
        }

        int triCount = unityMesh.triangles.Length / 3;
        meshObj.Faces.Capacity = triCount;

        for (int i = 0; i < triCount; i+=3)
        {
            meshObj.Faces.AddFace(unityMesh.triangles[i+2], unityMesh.triangles[i+1], unityMesh.triangles[i]);
        }

        int faceCount = unityMesh.normals.Length;
        meshObj.Normals.Capacity = faceCount;

        for (int i = 0; i < faceCount; i++)
        {
            Vector3 tmpNormal = unityMesh.normals[i];
            meshObj.Normals.Add(tmpNormal.x, tmpNormal.z, tmpNormal.y);
        }

        return meshObj;
    }
}
