using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LekoObject : MonoBehaviour
{
    public Rhino.Geometry.Brep GetRhinoBrep()
    {
        return m_internalRepresentation;
    }

    LekoObject()
    {
    }

    private void Update()
    {
        if (m_internalRepresentation == null)
        {
            return;
        }

        m_internalRepresentation.Transform(Rhino.Geometry.Transform.Translation(transform.position.x, transform.position.z, transform.position.y));
        m_internalRepresentation.Transform(Rhino.Geometry.Transform.Scale(Rhino.Geometry.Plane.Unset, transform.localScale.x, transform.localScale.z, transform.localScale.y));

        /*if(!m_internalRepresentation.Transform(LekoCore.UnityToRhinoTransform(transform.localToWorldMatrix)))
        {
            Debug.Log("Transform failed!");
        }*/

    }

    private void OnMouseDown()
    {
        //LekoCore.RefreshRhinoRepresentation(this);
    }

    public Rhino.Geometry.Brep m_internalRepresentation;
}
