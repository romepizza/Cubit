using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterEntityEjector : MonsterEntityBase
{
    [Header("----- SETTINGS -----")]
    [Header("----- DEBUG -----")]
    [Header("--- (Scripts) ---")]
    public bool placeHolder;
    // Use this for initialization
    void Start ()
    {
        m_isMovable = false;
        m_rb = GetComponent<Rigidbody>();
        if (m_rb == null)
            Debug.Log("Warning: No rigidbody detected!");
        m_rb.mass = 100000f;
    }
	
	// Update is called once per frame
	/*void Update ()
    {
		if(m_rb.velocity.magnitude > 0.001f)
        {
            m_rb.velocity = Vector3.zero;
        }
	}*/

    public void setValuesByScriptEjector(GameObject prefab)
    {

    }
    
    public void destroyScript()
    {
        Destroy(this);
    }
}
