using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeEntityMovementAcceleration : MonoBehaviour
{
    [Header("----- SETTINGS -----")]
    public float m_duration;
    public float m_power;
    public float m_maxSpeed;
    public Vector3 m_targetDirection;
    public Vector3 m_targetPoint;

    [Header("----- DEBUG -----")]
    public float m_durationEndTime;
    //public CubeEntitySystem m_entitySystemScript;

    public CubeEntityMovement m_movementScript;
    private Rigidbody m_rb;

	// Use this for initialization
	void Start ()
    {
        m_rb = GetComponent<Rigidbody>();
        if (m_rb == null)
            Debug.Log("RigidBody of " + gameObject.name + " not found");	
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        updateDuration();
        updateAcceleration();
    }


    void updateDuration()
    {
        if(m_duration >= 0 && m_durationEndTime < Time.time)
        {
            m_movementScript.removeAccelerationComponent(this);
        }
    }

    void updateAcceleration()
    {
        if(m_rb.velocity.magnitude < m_maxSpeed)
        {
            m_rb.AddForce(m_targetDirection.normalized * m_power, ForceMode.Acceleration);
        }
    }
}
