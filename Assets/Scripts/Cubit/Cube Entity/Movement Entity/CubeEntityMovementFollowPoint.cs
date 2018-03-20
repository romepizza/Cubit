using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeEntityMovementFollowPoint : CubeEntityMovementAbstract
{
    [Header("----- SETTINGS -----")]
    public float m_duration;
    public float m_power;
    public Vector3 m_targetPoint;

    [Header("--- (Smooth Arrival) ---")]
    public bool m_useSmoothArrival;

    [Header("----- DEBUG -----")]
    public Vector3 m_targetDirection;
    public float m_durationEndTime;

    [Header("--- (Smooth Arrival) ---")]
    public Vector3 m_targetPointMoveDirection;

    [Header("--- (Deviation) ---")]
    public float m_angle;

    public CubeEntityMovement m_movementScript;
    private Rigidbody m_rb;
    public bool m_isInitialized;

    // Use this for initialization
    void Start ()
    {
        if (!m_isInitialized)
            initializeStuff();
    }

    void initializeStuff()
    {
        m_rb = GetComponent<Rigidbody>();
        m_isInitialized = true;
    }
	
	// Updates
	void FixedUpdate()
    {

        updateDuration();
        //updateDeviation();
        //updateSmoothArrivalInfos();
        if(m_useThis)
            updateAcceleration();
    }

    void updateDuration()
    {
        if (m_duration >= 0 && m_durationEndTime < Time.time)
        {
            m_movementScript.removeComponent(this);
        }
    }

    void updateDeviation()
    {
        if(transform.InverseTransformPoint(m_targetPoint).z < 0)
        {
            m_rb.AddForce(-m_rb.velocity, ForceMode.Acceleration);
        }
    }

    void updateSmoothArrivalInfos()
    {
        if(m_useSmoothArrival)
        {
            Vector3 targetFuturePoint = m_targetPoint + m_targetPointMoveDirection.normalized;
            m_targetDirection = targetFuturePoint - transform.position;
            m_rb.AddForce(m_targetDirection.normalized * m_power, ForceMode.Acceleration);
            Debug.DrawLine(m_targetPoint, targetFuturePoint, Color.yellow);
        }
    }

    void updateAcceleration()
    {
        if (m_rb.velocity.magnitude < m_maxSpeed)
        {
            m_targetDirection = m_targetPoint - transform.position;
            m_rb.AddForce(m_targetDirection.normalized * m_power, ForceMode.Acceleration);
        }
    }

    // copy
    void setValues(CubeEntityMovementFollowPoint copyScript)
    {
        m_duration = copyScript.m_duration;
        m_power = copyScript.m_power;
        m_maxSpeed = copyScript.m_maxSpeed;
        m_targetPoint = copyScript.m_targetPoint;
        m_useThis = copyScript.m_useThis;
    }

    // abstract
    public override void prepareDestroyScript()
    {
        if (GetComponent<CubeEntityMovement>().m_movementComponents.Contains(this))
            GetComponent<CubeEntityMovement>().m_movementComponents.Remove(this);
        Destroy(this);
    }

    public override void pasteScript(EntityCopiableAbstract baseScript)
    {
        if (!m_isInitialized)
            initializeStuff();
        setValues((CubeEntityMovementFollowPoint)baseScript);
    }
    public override void assignScripts()
    {
        m_movementScript = GetComponent<CubeEntityMovement>();
    }
    public override void pasteScript(EntityCopiableAbstract baseScript, GameObject target, Vector3 targetPosition)
    {
        if (!m_isInitialized)
            initializeStuff();
        setValues((CubeEntityMovementFollowPoint)baseScript);
    }
}
