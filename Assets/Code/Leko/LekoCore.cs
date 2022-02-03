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
    public static void RhinoToUnityMesh(GameObject parent, Rhino.Geometry.Mesh[] rhinoMeshList)
    {
        foreach (var rhinoMesh in rhinoMeshList)
        {
            Mesh meshObj = new Mesh();

            var vertices = new List<Vector3>();
            var triangles = new List<int>();
            var normals = new List<Vector3>();

            foreach (var meshVertex in rhinoMesh.Vertices)
            {
                var vertex = new Vector3(meshVertex.X, meshVertex.Z, meshVertex.Y);
                vertices.Add(vertex);
            }
            
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
            
            foreach (var normal in rhinoMesh.Normals)
            {
                normals.Add(new Vector3(normal.X, normal.Z, normal.Y));
            }

            meshObj.vertices = vertices.ToArray();
            meshObj.triangles = triangles.ToArray();
            meshObj.normals = normals.ToArray();

            GameObject gb = new GameObject();
            gb.AddComponent<MeshFilter>().mesh = meshObj;
            gb.AddComponent<MeshRenderer>().material = parent.GetComponent<MeshRenderer>().material;
            parent.GetComponent<MeshFilter>().mesh = null;

            gb.transform.parent = parent.transform;
        }
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

        int faceCount = unityMesh.triangles.Length / 3;
        meshObj.Faces.Capacity = faceCount;

        for (int i = 0; i < unityMesh.triangles.Length; i+=3)
        {
            meshObj.Faces.AddFace(unityMesh.triangles[i+2], unityMesh.triangles[i+1], unityMesh.triangles[i]);
        }

        int normalCount = unityMesh.normals.Length;
        meshObj.Normals.Capacity = normalCount;

        for (int i = 0; i < normalCount; i++)
        {
            Vector3 tmpNormal = unityMesh.normals[i];
            meshObj.Normals.Add(tmpNormal.x, tmpNormal.z, tmpNormal.y);
        }

        meshObj.Compact();

        return meshObj;
    }

    /// <summary>
    /// Convert matrix from Rhino to Unity format
    /// </summary>
    /// <param name="rhinoMatrix"></param>
    /// <returns></returns>
    public static Matrix4x4 RhinoToUnityTransform(Rhino.Geometry.Transform rhinoMatrix)
    {
        return Matrix4x4.identity;
    }

    /// <summary>
    /// Convert matrix from  Unity to Rhino format
    /// </summary>
    /// <param name="unityMatrix"></param>
    /// <returns></returns>
    public static Rhino.Geometry.Transform UnityToRhinoTransform(Matrix4x4 unityMatrix)
    {
        return new Rhino.Geometry.Transform();
    }

    /// <summary>
    /// Compute the volume of the LekoObject
    /// </summary>
    /// <param name="lekoObj"></param>
    /// <returns></returns>
    public static double GetVolume(LekoObject lekoObj)
    {
        Rhino.Geometry.Brep tmpBrep = lekoObj.GetRhinoBrep();
        if (tmpBrep == null)
        {
            return 0.0f;
        }

        return BrepCompute.GetVolume(tmpBrep); 
    }

    public static void RefreshRhinoRepresentation(LekoObject lekoObj)
    {
        var meshList = MeshCompute.CreateFromBrep(lekoObj.m_internalRepresentation);
        RhinoToUnityMesh(lekoObj.gameObject, meshList);
    }

    /// <summary>
    /// Creates a cube
    /// </summary>
    /// <returns></returns>
    public static LekoObject CreateCube()
    {
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        LekoObject lekoObj = cube.AddComponent<LekoObject>();


        Rhino.Geometry.Point3d pt0 = new Rhino.Geometry.Point3d(-0.5, -0.5, -0.5);
        Rhino.Geometry.Point3d pt1 = new Rhino.Geometry.Point3d(0.5, 0.5, 0.5);
        Rhino.Geometry.BoundingBox rhinoBox = new Rhino.Geometry.BoundingBox(pt0, pt1);

        lekoObj.m_internalRepresentation = rhinoBox.ToBrep();

        var meshList = MeshCompute.CreateFromBrep(lekoObj.m_internalRepresentation);
        RhinoToUnityMesh(lekoObj.gameObject, meshList);

        return lekoObj;
    }
}
