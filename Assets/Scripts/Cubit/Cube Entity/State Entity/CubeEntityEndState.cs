using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeEntityEndState : MonoBehaviour
{
    [Header("----- SETTINGS -----")]
    public float m_duration;

    [Header("----- DEBUG -----")]
    public float m_durationEndTime;

    private CubeEntitySystem m_cubeSystemScript;
	
	// Update is called once per frame
	void Update ()
    {
		if(m_durationEndTime < Time.time)
        {
            m_cubeSystemScript.setToInactive();
        }
	}

    public void setValuesByPrefab(GameObject prefab, CubeEntitySystem systemScript)
    {
        CubeEntityState stateScript = prefab.GetComponent<CubeEntityState>();

        if (stateScript != null)
        {
            m_duration = stateScript.m_duration;
            m_durationEndTime = stateScript.m_duration + Time.time;
            m_cubeSystemScript = systemScript;
        }
    }

    public void setDuration(float duration, CubeEntitySystem systemScript)
    {
        m_duration = duration;
        m_durationEndTime = duration + Time.time;
        m_cubeSystemScript = systemScript;
    }

}
