using UnityEngine;

public class LekoObject : MonoBehaviour
{
    public Rhino.Geometry.Brep GetWithTransfornmApplied()
    {
        Rhino.Geometry.Brep tmpBrep = m_internalRepresentation.DuplicateBrep();
        if (tmpBrep != null)
        {
            Rhino.Geometry.Transform rhinoTransform = LekoCore.UnityToRhinoTransform(transform.localToWorldMatrix);
            tmpBrep.Transform(rhinoTransform);
        }

        return tmpBrep;
    }

    void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Volume is: " + LekoCore.GetVolume(this));
        }
        else if (Input.GetMouseButtonDown(1))
        {
            GameObject.Destroy(this.gameObject);
        }

    }

    public Rhino.Geometry.Brep m_internalRepresentation;
    public Rhino.Geometry.Mesh[] m_meshList;
}
