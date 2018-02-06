using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterEntityWorm : MonsterEntityBase
{
    [Header("----- SETTINGS -----")]
    [Header("----- DEBUG -----")]
    [Header("--- (Scripts) ---")]
    public bool placeHolder;

    // Use this for initialization
    void Start()
    {
        m_isMovable = true;

        m_rb.mass = 1f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (m_rb.velocity.magnitude > 0.001f)
        {
            m_rb.velocity = Vector3.zero;
        }
    }

    public void setValuesByScriptEjector(GameObject prefab)
    {

    }

    public void destroyScript()
    {
        Destroy(this);
    }
}
