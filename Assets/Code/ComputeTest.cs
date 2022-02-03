using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rhino.Compute;


public class ComputeTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        ComputeServer.WebAddress = "http://localhost:8081";

        //Rhino.Geometry.Box box1 = new Rhino.Geometry.Box();

        // Uses standard Rhino3dmIO methods locally to create a sphere.
        var sphere = new Rhino.Geometry.Sphere(Rhino.Geometry.Point3d.Origin, 12);
        var sphereAsBrep = sphere.ToBrep();

        // the following function calls compute.rhino3d.com to get access to something not
        // available in Rhino3dmIO. In this case send a Brep to Compute and get a Mesh back.
        var meshes = MeshCompute.CreateFromBrep(sphereAsBrep);

        // Use regular Rhino3dmIO local calls to count the vertices in the mesh.
        Debug.Log($"Got {meshes.Length} meshes");
        for (int i = 0; i < meshes.Length; i++)
        {
            Debug.Log($"  {i + 1} mesh has {meshes[i].Vertices.Count} vertices");
        }

        Debug.Log("press any key to exit");
       // Console.ReadKey();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
