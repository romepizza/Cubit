using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeEntityAttached : MonoBehaviour
{
    public GameObject m_attachedToGameObject;
    public int m_affiliation;
    public int m_monster;
    public AttachSystemBase m_attachSystemScript;

    public void setValuesByScript(GameObject prefab, AttachSystemBase attachSystemScript)
    {
        CubeEntityAttached script = prefab.GetComponent<CubeEntityAttached>();
        m_affiliation = script.m_affiliation;
        m_monster = script.m_monster;
        m_attachSystemScript = attachSystemScript;
    }

    public void setValuesByObject(GameObject gameObject, AttachSystemBase attachSystemScript)
    {
        m_attachSystemScript = attachSystemScript;
        if (gameObject == Constants.getPlayer())
        {
            m_attachedToGameObject = gameObject;
            m_affiliation = CubeEntityState.s_AFFILIATION_PLAYER;
            m_monster = CubeEntityState.s_MONSTER_NONE;
        }
        else if (gameObject.GetComponent<CubeEntitySystem>() != null)
        {
            m_attachedToGameObject = gameObject;
            m_affiliation = gameObject.GetComponent<CubeEntitySystem>().getStateComponent().m_affiliation;
            m_monster = gameObject.GetComponent<CubeEntitySystem>().getStateComponent().m_monster;
        }
        else
            Debug.Log("Warning: Something might have gone wrong here!");
    }

    public void deregisterAttach()
    {
        if(m_attachSystemScript != null)
        {
            m_attachSystemScript.deregisterCube(this.gameObject);
        }
        else
            Debug.Log("Warning: m_attachSystemScript was null");
    }
}
