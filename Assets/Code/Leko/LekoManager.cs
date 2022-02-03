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


    void OnGUI()
    {
        if (GUILayout.Button("Create Cube 1"))
        {
            LekoObject cube = LekoCore.CreateCube(0.5f);
            cube.transform.parent = transform;
        }

        if (GUILayout.Button("Create Sphere"))
        {
            LekoObject cube = LekoCore.CreateSphere(1);
            cube.transform.parent = transform;
        }

        if (GUILayout.Button("Boolean Intersection"))
        {
            LekoObject[] lekoObjects = GetComponentsInChildren<LekoObject>();

            if (lekoObjects.Length > 1)
            {
                LekoObject intersectionResult = LekoCore.BooleanIntersection(lekoObjects[0], lekoObjects[1]);

                if (intersectionResult != null)
                {
                    intersectionResult.transform.parent = transform;
                    GameObject.Destroy(lekoObjects[0].gameObject);
                    GameObject.Destroy(lekoObjects[1].gameObject);
                }
            }
        }
    }


}
