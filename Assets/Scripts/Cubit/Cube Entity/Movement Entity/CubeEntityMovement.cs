using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeEntityMovement : MonoBehaviour
{
    public bool doStuff;
    [Header("----- SETTINGS -----")]
    [Header("----- DEBUG -----")]
    public CubeEntitySystem m_entitySystemScript;
    //public List<CubeEntityMovementAcceleration> m_accelerationComponents;
    //public List<CubeEntityMovementFollowPoint> m_followPointsComponents;
    //public List<CubeEntityMovementHoming> m_homingComponents;
    public List<CubeEntityMovementAbstract> m_movementComponents;


    // Getter
    /*
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
    */


    // --- Add Components ---
    public CubeEntityMovementAbstract addMovementComponent(CubeEntityMovementAbstract copyScript, GameObject target, Vector3 targetPosition)
    {
        if (copyScript == null)
        {
            Debug.Log("Aborted: copyScript was null!");
        }
        System.Type type = copyScript.GetType();
        Component copy = gameObject.AddComponent(type);

        ((CubeEntityMovementAbstract)copy).enabled = true;
        ((CubeEntityMovementAbstract)copy).pasteScript(copyScript, target, targetPosition);
        ((CubeEntityMovementAbstract)copy).assignScripts();
        ((CubeEntityMovementAbstract)copy).m_useThis = true;

        return (CubeEntityMovementAbstract)copy;
    }

    // --- Remove Components ---
    public void removeComponents(System.Type type)
    {
        int i = 0;
        Component component;
        do
        {
            component = GetComponent(type) as Component;
            if (component == null)
            {
                break;
            }
            
            CubeEntityMovementAbstract movementComponent = (CubeEntityMovementAbstract)component;
            if (m_movementComponents.Contains(movementComponent))
                m_movementComponents.Remove(movementComponent);
            movementComponent.prepareDestroyScript();
            i++;
        } while (component == null && i < 20);
        if(i >= 15)
        {
            Debug.Log("Warning: >15 movementscripts were attached");
        }
    }
    public void removeComponent(CubeEntityMovementAbstract removeScript)
    {
        if (m_movementComponents.Contains(removeScript))
            m_movementComponents.Remove(removeScript);
        removeScript.prepareDestroyScript();
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

        m_movementComponents.Add(tmp);

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

        m_movementComponents.Add(tmp);

        return tmp;
    }
    public CubeEntityMovementHoming addHomingComponent(GameObject target)
    {
        CubeEntityMovementHoming tmp = gameObject.AddComponent<CubeEntityMovementHoming>();

        /*
        tmp.m_accelerationPower = ;
        tmp.m_maxSpeed = ;
        tmp.m_deviationPower = ;
        tmp.m_duration = ;
        tmp.m_maxAngle = ;
        tmp.m_collisionSpeed = ;
        */
        
        tmp.m_target = target;
        tmp.m_movementScript = this;
        tmp.initializeStuff();

        m_movementComponents.Add(tmp);

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
