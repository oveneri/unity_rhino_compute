using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rhino.Compute;

static class LekoCore
{
    const double c_tolerance = 1.0e-8;
    const string c_defaulObjectNAme = "LekoObject";

    /// <summary>
    /// Convert mesh from Rhino to Unity
    /// </summary>
    /// <param name="rhinoMesh"></param>
    /// <returns></returns>
    public static void RhinoToUnityMesh(GameObject parent, Rhino.Geometry.Mesh[] rhinoMeshList)
    {
        foreach (Transform child in parent.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

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
            gb.name = "SubMesh";
            gb.AddComponent<MeshFilter>().mesh = meshObj;
            gb.AddComponent<MeshRenderer>().material = parent.GetComponent<MeshRenderer>().material;
            parent.GetComponent<MeshFilter>().mesh = null;

            gb.transform.parent = parent.transform;
        }
    }

    /// <summary>
    /// Convert matrix from  Unity to Rhino format
    /// </summary>
    /// <param name="unityMatrix"></param>
    /// <returns></returns>
    public static Rhino.Geometry.Transform UnityToRhinoTransform(Matrix4x4 unityMatrix)
    {
        var other = new Matrix4x4();

        other.SetColumn(0, new Vector4(1, 0, 0, 0));
        other.SetColumn(1, new Vector4(0, 0, 1, 0));
        other.SetColumn(2, new Vector4(0, -1, 0, 0));
        other.SetColumn(3, new Vector4(0, 0, 0, 1));

        Matrix4x4 tmp = other * unityMatrix;

        Rhino.Geometry.Transform rhinoTransform = new Rhino.Geometry.Transform();

        rhinoTransform[0, 0] = tmp[0, 0];
        rhinoTransform[1, 0] = tmp[1, 0];
        rhinoTransform[2, 0] = tmp[2, 0];
        rhinoTransform[3, 0] = tmp[3, 0];

        rhinoTransform[0, 1] = tmp[0, 1];
        rhinoTransform[1, 1] = tmp[1, 1];
        rhinoTransform[2, 1] = tmp[2, 1];
        rhinoTransform[3, 1] = tmp[3, 1];

        rhinoTransform[0, 2] = tmp[0, 2];
        rhinoTransform[1, 2] = tmp[1, 2];
        rhinoTransform[2, 2] = tmp[2, 2];
        rhinoTransform[3, 2] = tmp[3, 2];

        rhinoTransform[0, 3] = tmp[0, 3];
        rhinoTransform[1, 3] = tmp[1, 3];
        rhinoTransform[2, 3] = tmp[2, 3];
        rhinoTransform[3, 3] = tmp[3, 3];

        return rhinoTransform;
    }

    /// <summary>
    /// Compute the volume of the LekoObject
    /// </summary>
    /// <param name="lekoObj"></param>
    /// <returns></returns>
    public static double GetVolume(LekoObject lekoObj)
    {
        Rhino.Geometry.Brep tmpBrep = lekoObj.GetWithTransfornmApplied();
        if (tmpBrep == null)
        {
            return 0.0f;
        }

        return BrepCompute.GetVolume(tmpBrep);
    }

    /// <summary>
    /// LekoObject from Brep
    /// </summary>
    /// <param name="brep"></param>
    /// <returns></returns>
    private static LekoObject FromBrep(Rhino.Geometry.Brep brep)
    {
        GameObject gameObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
        gameObj.name = c_defaulObjectNAme;
        LekoObject lekoObj = gameObj.AddComponent<LekoObject>();
        lekoObj.m_internalRepresentation = brep;
        lekoObj.m_meshList = MeshCompute.CreateFromBrep(lekoObj.m_internalRepresentation);
        RhinoToUnityMesh(lekoObj.gameObject, lekoObj.m_meshList);

        return lekoObj;
    }

    /// <summary>
    /// Create Sphere
    /// </summary>
    /// <param name="radius"></param>
    /// <returns></returns>
    public static LekoObject CreateSphere(float radius)
    {
        var rhinoSphere = new Rhino.Geometry.Sphere(new Rhino.Geometry.Point3d(0,0,0), radius);
        return FromBrep(rhinoSphere.ToBrep());
    }

    /// <summary>
    /// Create cube
    /// </summary>
    /// <param name="halfSize"></param>
    /// <returns></returns>
    public static LekoObject CreateCube(float halfSize)
    {
        Rhino.Geometry.Point3d pt0 = new Rhino.Geometry.Point3d(-halfSize, -halfSize, -halfSize);
        Rhino.Geometry.Point3d pt1 = new Rhino.Geometry.Point3d(halfSize, halfSize, halfSize);
        Rhino.Geometry.Box rhinoBox = new Rhino.Geometry.Box(new Rhino.Geometry.BoundingBox(pt0, pt1));

        return FromBrep(rhinoBox.ToBrep());
    }

    /// <summary>
    /// Apply boolean intersection between 2 objects
    /// </summary>
    /// <param name="first"></param>
    /// <param name="second"></param>
    /// <returns></returns>
    public static LekoObject BooleanIntersection(LekoObject first, LekoObject second)
    {
        Rhino.Geometry.Brep[] intersection = Rhino.Compute.BrepCompute.CreateBooleanIntersection(new Rhino.Geometry.Brep[] { first.GetWithTransfornmApplied() }, new Rhino.Geometry.Brep[] { second.GetWithTransfornmApplied() }, c_tolerance);

        if (intersection == null || intersection.Length == 0)
        {
            return null;
        }

        return FromBrep(intersection[0]);
    }
}
