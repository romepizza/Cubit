using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterEntityAttachSystemNew : AttachSystemBase
{
    [Header("----- SETTINGS -----")]
    //public float m_duration = -1;
    
    [Header("----- DEBUG -----")]
    //public List<GameObject> m_cubeList;
    private MonsterEntityBase m_monsterBaseScript;
    public AttachEntityBase m_attachEntity;
    public bool m_isInitialized;
    //public int m_freePositions;

    /*
    [Header("--- (Smooth Arrival) ---")]
    public Vector3 m_lastCatchPointsDirection;
    public List<Vector3> m_lastCatchPointsDirections;
    public Vector3 m_lastCatchPosition;
    public int m_lastCatchPositionsIndex;
    */

    // Use this for initialization
    void Start ()
    {
        if(!m_isInitialized)
            initializeStuff();	
	}
    void initializeStuff()
    {
        AttachEntityBase[] attachEntities = GetComponents<AttachEntityBase>();
        if (attachEntities.Length != 1)
        {
            //Debug.Log("Aborted: More or less than one attachEntity found on player!");
            return;
        }

        m_attachEntity = attachEntities[0];
        m_cubeList = new List<GameObject>();
        m_isInitialized = true;
    }
	

    public bool registerToGrab(GameObject cubeAdd)
    {
        if (m_cubeList.Count < m_maxCubesGrabbed)
        {
            if(m_cubeList.Contains(cubeAdd))
            {
                Debug.Log("Aborted: Tried to add cube from attach system that was already in the list!");
                return false;
            }
            cubeAdd.GetComponent<CubeEntitySystem>().setAttachedDynamicly(GetComponent<CubeEntityState>());


            CubeEntityAttached attachedScript = cubeAdd.GetComponent<CubeEntitySystem>().getStateComponent().addAttachedScript();
            attachedScript.setValuesByObject(gameObject, this);


            m_cubeList.Add(cubeAdd);
            m_attachEntity.addAgent(cubeAdd);

            return true;
        }

        return false;
    }

    public override void deregisterCube(GameObject cubeRemove)
    {
        // if cubeRemove is the core
        if (cubeRemove == this.gameObject)
        {
            //cubeRemove.GetComponent<CubeEntityState>().removeAttachedScript();
            return;
        }
        if (m_cubeList == null)
            return;

        if(!m_cubeList.Contains(cubeRemove))
        {
            //Debug.Log("Aborted: Tried to remove cube from attach system that was not in the list!");
            return;
        }

        cubeRemove.GetComponent<CubeEntityState>().removeAttachedScript();
        m_attachEntity.removeAgent(cubeRemove);
        m_cubeList.Remove(cubeRemove);
    }
    public void deregisterAllCubes()
    {
        List<GameObject> list = new List<GameObject>(m_cubeList);
        foreach (GameObject cubeRemove in list)
            deregisterCube(cubeRemove);
    }
    

    public void setValues(MonsterEntityAttachSystemNew script)
    {
        m_maxCubesGrabbed = script.m_maxCubesGrabbed;
        m_movementAffectsCubesFactor = script.m_movementAffectsCubesFactor;
    }
    public override void pasteScript(EntityCopiableAbstract baseScript)
    {
        if (!m_isInitialized)
            initializeStuff();
        setValues((MonsterEntityAttachSystemNew)baseScript);
    }
    
    public override void prepareDestroyScript()
    {
        m_attachEntity.prepareDestroyScript();
        foreach (GameObject cube in m_cubeList)
        {
            cube.GetComponent<CubeEntityPrefapSystem>().setToPrefab(CubeEntityPrefabs.getInstance().s_inactivePrefab);
        }
        deregisterAllCubes();
        Destroy(this);
    }

    public override void assignScripts()
    {
        AttachEntityBase[] attachEntities = GetComponents<AttachEntityBase>();
        
        if (attachEntities.Length != 1)
        {
            //Debug.Log("Aborted: More or less than one attachEntity found on player!");
            return;
        }


        m_attachEntity = attachEntities[0];

        m_monsterBaseScript = GetComponent<MonsterEntityBase>();
    }
}
