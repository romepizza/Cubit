using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeEntityMovement : MonoBehaviour
{
    public bool doStuff;
    [Header("----- SETTINGS -----")]
    [Header("----- DEBUG -----")]
    public CubeEntitySystem m_entitySystemScript;
    public List<CubeEntityMovementAcceleration> m_accelerationComponents;
    public List<CubeEntityMovementFollowPoint> m_followPointsComponents;


    // Getter
    public CubeEntityMovementAcceleration getSingleAccelerationScript()
    {
        if (m_accelerationComponents.Count != 1)
        {
            Debug.Log("Assertion Error: There were more than one CubeEntityMovementAcceleration script attached to the cube.");
            return null;
        }
        else
            return m_accelerationComponents[0];
    }
    public CubeEntityMovementFollowPoint getSingleFollowPointScript()
    {
        if (m_followPointsComponents.Count != 1)
        {
            Debug.Log("Assertion Error: There were more than one CubeEntityMovementFollowPoint script attached to the cube.");
            return null;
        }
        else
            return m_followPointsComponents[0];
    }

    // Remove Components
    public void removeAllMovementComponents()
    {
        removeAllAccelerationComponents();
        removeAllFollowPointComponents();
    }
    public void removeAllAccelerationComponents()
    {
        for (int i = m_accelerationComponents.Count - 1; i >= 0; i--)
            removeAccelerationComponent(m_accelerationComponents[i]);
        m_accelerationComponents = new List<CubeEntityMovementAcceleration>();
    }
    public void removeAccelerationComponent(CubeEntityMovementAcceleration removeScript)
    {
        if(m_accelerationComponents.Contains(removeScript))
        {
            m_accelerationComponents.Remove(removeScript);
            Destroy(removeScript);
        }
        else
        {
            Debug.Log("Warning: Tried to remove CubeEntityMovementAcceleration that wasn't registered!");
        }
    }

    public void removeAllFollowPointComponents()
    {
        for (int i = m_followPointsComponents.Count - 1; i >= 0; i--)
            removeFollowPointComponent(m_followPointsComponents[i]);
        m_followPointsComponents = new List<CubeEntityMovementFollowPoint>();
    }
    public void removeFollowPointComponent(CubeEntityMovementFollowPoint removeScript)
    {
        if (m_followPointsComponents.Contains(removeScript))
        {
            m_followPointsComponents.Remove(removeScript);
            Destroy(removeScript);
        }
        else
        {
            Debug.Log("Warning: Tried to remove CubeEntityMovementFollowPoint that wasn't registered!");
        }
    }

    // Add Movement Components
    public CubeEntityMovementAcceleration addAccelerationComponent(Vector3 targetPoint, float duration, float power, float maxSpeed)
    {
        CubeEntityMovementAcceleration tmp = gameObject.AddComponent<CubeEntityMovementAcceleration>();
        //tmp.m_entitySystemScript = m_entitySystemScript;

        tmp.m_targetDirection = targetPoint-transform.position;
        tmp.m_targetPoint = targetPoint;
        tmp.m_duration = duration;
        tmp.m_durationEndTime = duration + Time.time;
        tmp.m_power = power;
        tmp.m_maxSpeed = maxSpeed;
        tmp.m_movementScript = this;

        m_accelerationComponents.Add(tmp);

        return tmp;
    }
    public CubeEntityMovementFollowPoint addFollowPointComponent(Vector3 targetPoint, float duration, float power, float maxSpeed)
    {
        CubeEntityMovementFollowPoint tmp = gameObject.AddComponent<CubeEntityMovementFollowPoint>();

        tmp.m_targetPoint = targetPoint;
        tmp.m_power = power;
        tmp.m_maxSpeed = maxSpeed;
        tmp.m_duration = duration;
        tmp.m_durationEndTime = duration + Time.time;
        //tmp.m_useSmoothArrival = smoothArrival;
        tmp.m_movementScript = this;

        m_followPointsComponents.Add(tmp);

        return tmp;
    }

    void OnDrawGizmos()
    {
        if(doStuff)
        {
            doTestStuff();
            doStuff = false;
        }
    }

    void doTestStuff()
    {
        
    }
}
