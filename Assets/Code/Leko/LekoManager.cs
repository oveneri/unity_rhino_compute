using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rhino.Compute;

public class LekoManager : MonoBehaviour
{
    public static LekoManager Instance { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
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
            LekoObject cube = LekoCore.CreateCube();
            cube.transform.parent = transform;
        }
    }


}
