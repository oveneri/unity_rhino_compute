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

    public Rhino.Geometry.Brep m_internalRepresentation;
    public Rhino.Geometry.Mesh[] m_meshList;
}
