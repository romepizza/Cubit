using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineRendererExample : MonoBehaviour
{
    public Transform m_start;
    public Transform m_end;

    public LineRenderer m_lineRenderer;
	// Use this for initialization
	void Start ()
    {
        m_lineRenderer = GetComponent<LineRenderer>();
        m_lineRenderer.SetPosition(0, m_start.position);
        m_lineRenderer.SetPosition(1, m_end.position);
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
