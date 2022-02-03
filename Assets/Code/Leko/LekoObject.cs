using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LekoObject : MonoBehaviour
{
    public Rhino.Geometry.Brep GetRhinoBrep()
    {
        return m_internalRepresentation;
    }

    private void Update()
    {
        if (m_internalRepresentation == null)
        {
            return;
        }

        if(!m_internalRepresentation.Transform(LekoCore.UnityToRhinoTransform(transform.localToWorldMatrix)))
        {
            Debug.Log("Transform failed!");
        }

    }

    public Rhino.Geometry.Brep m_internalRepresentation;
}
