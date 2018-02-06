using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEntityAttachSystem : AttachEntity
{
    [Header("----- SETTINGS -----")]
    public float m_duration = -1;
    public int m_maxCubesGrabbed;

    [Header("--- (Catch System) ---")]
    public Vector3 m_catchOffset;

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
    public List<GameObject> m_grabbedCubes;
    public List<CubeEntityMovementFollowPoint> m_grabbedCubesFollowPointScripts;

    [Header("--- (Catch System) ---")]
    public Vector3 m_catchPoint;
    
    /*
    [Header("--- (Smooth Arrival) ---")]
    public Vector3 m_lastCatchPointsDirection;
    public List<Vector3> m_lastCatchPointsDirections;
    public Vector3 m_lastCatchPosition;
    public int m_lastCatchPositionsIndex;
    */

	// Update is called once per frame
	void Update ()
    {
        manageCatchSystem();
    }
    
    // Grab System
    public bool addToGrab(GameObject cubeAdd, bool defaultMovement, bool boidMovement)
    {
        if (m_grabbedCubes.Count < m_maxCubesGrabbed)
        {

            CubeEntityAttached attachedScript = cubeAdd.GetComponent<CubeEntitySystem>().getStateComponent().addAttachedScript();
            attachedScript.setValuesByObject(this.gameObject, this);

            if (defaultMovement)
            {
                cubeAdd.GetComponent<CubeEntitySystem>().setToAttachedPlayer(Vector3.zero, m_duration, m_cubeMovementPower, m_maxSpeed);
                CubeEntityMovementFollowPoint script = cubeAdd.GetComponent<CubeEntitySystem>().getMovementComponent().getSingleFollowPointScript();
                m_grabbedCubes.Add(cubeAdd);
                m_grabbedCubesFollowPointScripts.Add(script);
            }
            if(boidMovement)
            {
                if(GetComponent<CEMBoidSystem>() != null)
                {
                    cubeAdd.GetComponent<CubeEntitySystem>().getPrefapSystem().setToAttachedPlayer();
                    cubeAdd.GetComponent<CubeEntitySystem>().getMovementComponent().removeAllAccelerationComponents();
                    GetComponent<CEMBoidSystem>().addAgent(cubeAdd);
                }
            }
            
            return true;
        }
        else
            return false;
    }
    public override void deregisterCube(GameObject cubeRemove)
    {
        if (m_grabbedCubes.Contains(cubeRemove))
        {
            cubeRemove.GetComponent<CubeEntityState>().removeAttachedScript();
            int removeIndex = m_grabbedCubes.IndexOf(cubeRemove);
            m_grabbedCubes.RemoveAt(removeIndex);
            m_grabbedCubesFollowPointScripts.RemoveAt(removeIndex);
        }
        else
            Debug.Log("Warning: Tried to remove cube from grab, that wasn't grabbed!");
    }

    // Catch System
    public void manageCatchSystem()
    {
        if (m_grabbedCubes.Count > 0)
        {
            m_catchPoint = transform.position + /*Camera.main.transform.rotation **/ m_catchOffset;
            moveGrabbedCubesSinglePoint();
        }
    }
    public void moveGrabbedCubesSinglePoint()
    {
        foreach (CubeEntityMovementFollowPoint followPointScript in m_grabbedCubesFollowPointScripts)
        {
            followPointScript.m_targetPoint = m_catchPoint;
        }
    }

    /*
    public void manageSmoothArrival()
    {
        if (m_useSmoothArrival && m_lastCatchPositionsCount > 0)
        {
            Vector3 newDirection = m_catchPoint - m_lastCatchPosition;
            if (m_lastCatchPointsDirections.Count < m_lastCatchPositionsCount)
                m_lastCatchPointsDirections.Add(newDirection);
            else
            {
                m_lastCatchPointsDirections[m_lastCatchPositionsIndex] = newDirection;
            }

            Vector3 total = Vector3.zero;
            foreach (Vector3 lastDirection in m_lastCatchPointsDirections)
            {
                total += lastDirection;
            }
            m_lastCatchPointsDirection = total / m_lastCatchPointsDirections.Count;

            m_lastCatchPosition = m_catchPoint;
            m_lastCatchPositionsIndex = (m_lastCatchPositionsIndex + 1) % m_lastCatchPositionsCount;

            foreach (CubeEntityMovementFollowPoint script in m_grabbedCubesFollowPointScripts)
            {
                script.m_targetPointMoveDirection = m_lastCatchPointsDirection;
            }
            //Debug.DrawRay(m_catchPoint, m_lastCatchPointsDirection * 10, Color.cyan);
        }
    }
    */
}
