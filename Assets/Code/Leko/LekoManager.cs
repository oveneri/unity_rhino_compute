using UnityEngine;

public class LekoManager : MonoBehaviour
{
    public FlyCamera m_flyCamera;

    public static LekoManager Instance { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        m_flyCamera.enabled = false;

        Rhino.Compute.ComputeServer.WebAddress = "http://localhost:8081";
    }


    void OnGUI()
    {
        GUILayout.BeginArea(new Rect(10, 10, 200, 1000));
        if (!m_flyCamera.enabled)
        {
            if (GUILayout.Button("Activate Fly Cam"))
            {
                m_flyCamera.enabled = true;
            }
        }
        else
        {
            GUILayout.Label("Fly Cam is On! . WASD for lateral movement, Space & Ctrl for vertical movement. Press Esc to turn it off. ");

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                m_flyCamera.enabled = false;
            }
        }

        

        if (GUILayout.Button("Create Cube"))
        {
            LekoObject cube = LekoCore.CreateCube(0.5f);
            cube.transform.parent = transform;
        }

        if (GUILayout.Button("Create Sphere"))
        {
            LekoObject cube = LekoCore.CreateSphere(0.707f);
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

        GUILayout.EndArea();
    }


}
