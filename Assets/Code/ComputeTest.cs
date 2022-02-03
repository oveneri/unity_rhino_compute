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
        // Set Rhino Compute server info
        ComputeServer.WebAddress = "http://localhost:8081";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnGUI()
    {
        if (GUILayout.Button("Create Cube"))
        {
            LekoObject cube = LekoCore.CreateCube(1);
            cube.transform.parent = transform.parent;
        }
    }
}
