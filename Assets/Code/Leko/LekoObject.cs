using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rhino.Compute;

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
    }

    private void OnMouseDown()
    {
        //LekoCore.RefreshRhinoRepresentation(this);
    }

    public Rhino.Geometry.Brep GetWithTransfornmApplied()
    {
        Rhino.Geometry.Brep tmpBrep = m_internalRepresentation.DuplicateBrep(); 
        if (tmpBrep != null)
        {
            tmpBrep.Transform(Rhino.Geometry.Transform.Scale(Rhino.Geometry.Plane.Unset, transform.localScale.x, transform.localScale.z, transform.localScale.y));
            tmpBrep.Transform(Rhino.Geometry.Transform.Translation(transform.position.x, transform.position.z, transform.position.y));
        }

        return tmpBrep;
    }

    public Rhino.Geometry.Brep m_internalRepresentation;
    public Rhino.Geometry.Mesh[] m_meshList;
}
