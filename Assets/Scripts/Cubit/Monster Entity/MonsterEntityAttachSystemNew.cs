using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterEntityAttachSystemNew : AttachSystemBase
{
    [Header("----- SETTINGS -----")]
    //public float m_duration = -1;
    public int m_maxCubesGrabbed;
    
    [Header("----- DEBUG -----")]
    public List<GameObject> m_cubeList;
    private MonsterEntityBase m_monsterBaseScript;
    public AttachEntityBase m_attachEntity;

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
        initializeStuff();	
	}
    void initializeStuff()
    {


        AttachEntityBase[] attachEntities = GetComponents<AttachEntityBase>();
        if (attachEntities.Length != 1)
        {
            Debug.Log("Aborted: More or less than one attachEntity found on player!");
            return;
        }

        m_attachEntity = attachEntities[0];
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void addToGrab(GameObject agent)
    {

    }

    public override void deregisterCube(GameObject cube)
    {

    }
}
