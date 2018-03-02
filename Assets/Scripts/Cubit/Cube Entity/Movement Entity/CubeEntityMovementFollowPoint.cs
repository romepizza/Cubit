using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeEntityMovementFollowPoint : MonoBehaviour
{
    [Header("----- SETTINGS -----")]
    public float m_duration;
    public float m_power;
    public float m_maxSpeed;
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

    // Use this for initialization
    void Start ()
    {
        m_rb = GetComponent<Rigidbody>();
    }
	
	// Updates
	void FixedUpdate()
    {
        updateDuration();
        //updateDeviation();
        //updateSmoothArrivalInfos();
        updateAcceleration();
    }

    void updateDuration()
    {
        if (m_duration >= 0 && m_durationEndTime < Time.time)
        {
            m_movementScript.removeFollowPointComponent(this);
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
}
