using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterEntityAttachSystem : AttachSystemBase
{
    public class GrabbedCube
    {
        public GameObject cube;
        public CubeEntityMovementFollowPoint script;
        public Vector3 positionOffset;
        public Vector3 positionWorld;

        public GrabbedCube(GameObject cube, CubeEntityMovementFollowPoint script, Vector3 positionOffset, Vector3 positionWorld)
        {
            this.cube = cube;
            this.script = script;
            this.positionOffset = positionOffset;
            this.positionWorld = positionWorld;
        }
    }

    [Header("----- SETTINGS -----")]
    public float m_duration = -1;
    public int m_maxCubesGrabbed;

    [Header("--- (Catch System) ---")]
    public bool m_putNewCubeToNearestPosition;
    public float m_minCatchRadius;
    public float m_maxCatchRadius;
    
    /*
    [Header("--- (Smooth Arrival) ---")]
    public bool m_useSmoothArrival;
    public int m_lastCatchPositionsCount;
    */

    [Header("--- (Cube Movement) ---")]
    public float m_movementAffectsCubesFactor;
    public float m_cubeMovementPower;
    public float m_maxSpeed;

    [Header("----- DEBUG -----")]
    

    //[Header("--- (Catch System) ---")]
    public GrabbedCube[] m_cubeList;
    public List<int> m_freePositions;
    public List<int> m_occupiedPositions;

    private MonsterEntityBase m_monsterBaseScript;

    /*
    [Header("--- (Smooth Arrival) ---")]
    public Vector3 m_lastCatchPointsDirection;
    public List<Vector3> m_lastCatchPointsDirections;
    public Vector3 m_lastCatchPosition;
    public int m_lastCatchPositionsIndex;
    */

    // Use this for initialization
    void Start()
    {
        m_monsterBaseScript = GetComponent<MonsterEntityBase>();
        initializeCubeList();
    }

    // Update is called once per frame
    void Update()
    {
        manageCatchSystem();
    }

    // Getter
    public List<GameObject> getAttachedCubes()
    {
        List<GameObject> cubeList = new List<GameObject>();
        foreach(GrabbedCube grabbedCube in m_cubeList)
        {
            if(grabbedCube.cube != null)
            {
                cubeList.Add(grabbedCube.cube);
            }
        }
        return cubeList;
    }

    // Setter
    public void setValuesByScript(GameObject prefab)
    {
        MonsterEntityAttachSystem script = prefab.GetComponent<MonsterEntityAttachSystem>();
        if (script != null)
        {
            m_duration = script.m_duration;
            m_putNewCubeToNearestPosition = script.m_putNewCubeToNearestPosition;
            m_minCatchRadius = script.m_minCatchRadius;
            m_maxCatchRadius = script.m_maxCatchRadius;
            m_movementAffectsCubesFactor = script.m_movementAffectsCubesFactor;
            m_cubeMovementPower = script.m_cubeMovementPower;
            m_maxSpeed = script.m_maxSpeed;
        }
        else
            Debug.Log("Warning: Tried to copy values of MonsterEntityAttachSystem from prefab, that didn't have the script attached!");

        MonsterEntityBase scriptBase = prefab.GetComponent<MonsterEntityBase>();
        if(scriptBase)
        {
            m_maxCubesGrabbed = scriptBase.m_maxCubes;
        }

        initializeCubeList();
    }

    // Grab System
    public bool addToGrab(GameObject cubeAdd)
    {
        if (m_freePositions.Count > 0)
        {
            cubeAdd.GetComponent<CubeEntitySystem>().setToAttachedEnemyEjector(Vector3.zero, m_duration, m_cubeMovementPower, m_maxSpeed);
            CubeEntityMovementFollowPoint script = cubeAdd.GetComponent<CubeEntitySystem>().getMovementComponent().getSingleFollowPointScript();

            CubeEntityAttached attachedScript = cubeAdd.GetComponent<CubeEntitySystem>().getStateComponent().addAttachedScript();
            attachedScript.setValuesByObject(this.gameObject, this);

            int index = -1;
            if (m_putNewCubeToNearestPosition)
            {
                float minDist = float.MaxValue;
                for(int i = 0; i < m_freePositions.Count; i++)
                {
                    float dist = (m_cubeList[m_freePositions[i]].positionWorld - cubeAdd.transform.position).magnitude;
                    if(dist < minDist)
                    {
                        index = m_freePositions[i];
                        minDist = dist;
                    }
                }
            }
            else
            {
                index = m_freePositions[Random.Range(0, m_freePositions.Count)];
            }

            if (index >= 0)
            {
                m_cubeList[index].cube = cubeAdd;
                m_cubeList[index].script = script;
                m_freePositions.Remove(index);
                m_occupiedPositions.Add(index);
            }
            else
                Debug.Log("Warning: No matching position for cube found!");
            return true;
        }
        else
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

        for(int i = 0; i < m_cubeList.Length; i++)
        {
            if(m_cubeList[i].cube == cubeRemove)
            {
                cubeRemove.GetComponent<CubeEntityState>().removeAttachedScript();
                Destroy(m_cubeList[i].script);
                m_cubeList[i].script = null;
                m_cubeList[i].cube = null;
                m_freePositions.Add(i);
                m_occupiedPositions.Remove(i);
                //cubeRemove.GetComponent<CubeEntitySystem>().setToInactive();
                return;
            }
        }
        //Debug.Log("Warning: Tried to remove cube from grab, that wasn't grabbed!");
    }

    public void deregisterAllCubes(bool setToInactive)
    {
        for(int i = m_cubeList.Length - 1; i >= 0; i--)
        {

            GameObject cubeRemove = m_cubeList[i].cube;
            if (cubeRemove == this.gameObject || cubeRemove == null)
                continue;

            //cubeRemove.GetComponent<CubeEntityState>().removeAttachedScript();
            Destroy(m_cubeList[i].script);
            m_cubeList[i].script = null;
            m_cubeList[i].cube = null;
            m_freePositions.Add(i);
            m_occupiedPositions.Remove(i);

            if (setToInactive)
                cubeRemove.GetComponent<CubeEntitySystem>().setToInactive();
        }
    }

    // Catch System
    public void manageCatchSystem()
    {
        foreach(GrabbedCube grabbedCube in m_cubeList)
        {
            if(grabbedCube.cube != null)
            {
                grabbedCube.positionWorld = transform.position + grabbedCube.positionOffset;
                grabbedCube.script.m_targetPoint = grabbedCube.positionWorld;
            }
        }
    }



    // Intern
    public void initializeCubeList()
    {

        m_freePositions = new List<int>();
        m_occupiedPositions = new List<int>();
        m_cubeList = new GrabbedCube[m_maxCubesGrabbed];

        for (int i = 0; i < m_cubeList.Length; i++)
        {
            Vector3 pos = Random.insideUnitSphere;
            pos = pos.normalized * Random.Range(m_minCatchRadius, m_maxCatchRadius);

            m_cubeList[i] = new GrabbedCube(null, null, pos, transform.position + pos);
            m_freePositions.Add(i);
        }
    }
    public void destroyScript()
    {
        /*
        foreach(grabbedCube grabbedCube in m_cubeList)
        {
            if(grabbedCube.cube != null)
            {
                //grabbedCube.cube.GetComponent<CubeEntitySystem>().setToInactive();
                deregisterCube(grabbedCube.cube);
            }
        }
        */

        for (int i = m_cubeList.Length - 1; i >= 0; i--)
        {

            GameObject cubeRemove = m_cubeList[i].cube;
            if (cubeRemove == this.gameObject || cubeRemove == null)
                continue;

            //cubeRemove.GetComponent<CubeEntityState>().removeAttachedScript();
            //Destroy(m_cubeList[i].script);
        }

        Destroy(this);
    }
}
