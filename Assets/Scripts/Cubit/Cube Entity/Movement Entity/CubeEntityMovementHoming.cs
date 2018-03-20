using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeEntityMovementHoming : CubeEntityMovementAbstract
{
    public CubeEntityMovementAbstract[] m_followUpMovementScripts;
    [Header("------- SETTINGS -------")]
    public float m_accelerationPower;

    [Header("--- (Deviation) ---")]
    public float m_deviationPower;
    public float m_maxDegree;
    public float m_deviationDecreaseRange;

    [Space]
    [Header("--- (Calculation) ---")]
    public float m_calculationCooldown;
    public float m_initialCooldown;
    public bool m_reducePower = false;
    public float m_calculationCooldownMinRandom;
    public float m_calculationCooldownMaxRandom;

    [Space]
    [Header("--- (Destroy Trigger) ---")]
    [Header("- Min Homing Duration -")]
    public float m_minHomingDuration = 1f;
    [Header("- Duration -")]
    public float m_duration;
    [Header("- Angle -")]
    public float m_maxAngle;
    public float m_maxAngleMinDistance;
    [Header("- Collision -")]
    public float m_collisionSpeed;

    [Space]
    [Header("------- DEBUG -------")]
    public float m_durationEndTime;
    public Vector3 m_targetDirection;
    public Vector3 m_targetPoint;
    //public CubeEntitySystem m_entitySystemScript;
    public float m_currentCollisionSpeed;
    public CubeEntityMovement m_movementScript;
    private Rigidbody m_rb;
    public Vector3 m_forceVectorAcceleration;
    public Vector3 m_forceVectorDeviation;
    public float m_calculationRdy;
    public bool m_isInitialized;
    public float m_isActiveTime;
    public float m_initialCooldownRdy;
    public bool m_isInitialCooldown;
    public Vector3 m_targetPosition;

    // Use this for initialization
    void Start()
    {
        if(!m_isInitialized)
            initializeStuff();
    }

    public void initializeStuff()
    {
        m_rb = GetComponent<Rigidbody>();
        if (m_rb == null)
            Debug.Log("RigidBody of " + gameObject.name + " not found");

        m_durationEndTime = m_duration + Time.time;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (m_useThis)
            updateCalculation();

        if (m_useThis && updateDestroy())
        {
            activateFollowUpScripts();
            prepareDestroyScript();// m_movementScript.removeComponent(this);
        }

        if(m_useThis)
            applyForceVector();
    }
    
    // Movement
    void updateCalculation()
    {
        if(m_isInitialCooldown)
        {
            if(m_initialCooldownRdy <= Time.time)
            {
                m_calculationRdy = Time.time;
                m_isInitialCooldown = false;
            }
        }

        if (m_calculationRdy <= Time.time && !m_isInitialCooldown)
        {
            getForceVector();
            m_calculationRdy = Time.time + (m_calculationCooldown * Random.Range(m_calculationCooldownMinRandom, m_calculationCooldownMaxRandom));
        }
        //applyForceVector();
    }
    void getForceVector()
    {
        if (m_target == null)
        {
            m_forceVectorDeviation = Vector3.zero;
            m_forceVectorAcceleration = Vector3.zero;
            return;
        }
        m_targetPoint = m_target.transform.position;


        Vector3 direction = m_targetPoint - transform.position;
        float angle = Statics.getAngle(direction, m_rb.velocity);
        m_forceVectorAcceleration = direction.normalized * m_accelerationPower;

        float angleFactor = 1;
        if(m_maxDegree > 0)
            angleFactor = Mathf.Clamp(angle / m_maxDegree, 0.2f, 1f);

        m_forceVectorDeviation = Vector3.zero;
        if (m_rb.velocity.magnitude > 0.01f)
        {
            Vector3 starePosition = transform.position + m_rb.velocity.normalized * direction.magnitude;
            Vector3 deviationDirection = m_targetPoint - starePosition;
            if (deviationDirection.magnitude > 0.01f)
            {
                float distanceFactor = Mathf.Clamp(Vector3.Distance(transform.position, m_targetPoint) / m_deviationDecreaseRange, 0.4f, 1f);
                m_forceVectorDeviation = deviationDirection.normalized * angleFactor * m_deviationPower * distanceFactor;
            }
        }
    }
    void applyForceVector()
    {
        Vector3 reducedDeviationFactor = m_forceVectorDeviation;
        if (m_reducePower)
        {
            reducedDeviationFactor *= Mathf.Clamp01((m_calculationRdy - Time.time) / m_calculationCooldown);
        }

        Vector3 finalForceVector = reducedDeviationFactor;
        if (m_rb.velocity.magnitude < m_maxSpeed)
            finalForceVector += m_forceVectorAcceleration;

        //Debug.DrawRay(transform.position, m_forceVectorAcceleration, Color.blue);
        //Debug.DrawRay(transform.position, m_forceVectorDeviation, Color.green);
        //Debug.DrawRay(transform.position + new Vector3(2f, 2f, 2f), finalForceVector, Color.magenta);

        m_rb.AddForce(finalForceVector, ForceMode.Acceleration);
    }

    // destroy
    bool updateDestroy()
    {
        if(m_target != null && m_target.GetComponent<MonsterEntityBase>() == null && m_target != Constants.getPlayer())
        {
            return true;
        }

        if (m_duration > 0 && m_durationEndTime < Time.time)
            return true;

        m_isActiveTime += Time.deltaTime;
        if (m_isActiveTime > m_minHomingDuration && !m_isInitialCooldown)
        {
            if (m_maxAngle > 0 && Vector3.Distance(transform.position, m_targetPoint) < m_maxAngleMinDistance && Statics.getAngle(m_targetPoint - transform.position, m_rb.velocity) > m_maxAngle)
                return true;
        }

        return false;
    }
    void activateFollowUpScripts()
    {
        foreach (CubeEntityMovementAbstract script in m_followUpMovementScripts)
        {
            if (script == null)
                continue;

            if(m_target != null)
                m_targetPosition = m_target.transform.position;
            GetComponent<CubeEntitySystem>().getMovementComponent().addMovementComponent(script, m_target, m_targetPosition);

            script.m_useThis = true;
        }
    }
    // collision
    public void OnCollisionEnter(Collision collision)
    {
        if (m_duration - (Time.time - m_durationEndTime) < m_minHomingDuration)
        {
            if (m_collisionSpeed > 0)
            {
                m_currentCollisionSpeed += collision.impulse.magnitude;
                if (m_currentCollisionSpeed >= m_collisionSpeed)
                    prepareDestroyScript();
            }
        }
    }

    // copy
    void setValues(CubeEntityMovementHoming copyScript)
    {
        m_target = copyScript.m_target;
        m_useThis = copyScript.m_useThis;

        m_followUpMovementScripts = copyScript.m_followUpMovementScripts;

        m_accelerationPower = copyScript.m_accelerationPower;
        m_maxSpeed = copyScript.m_maxSpeed;

        m_deviationPower = copyScript.m_deviationPower;
        m_maxDegree = copyScript.m_maxDegree;
        m_deviationDecreaseRange = copyScript.m_deviationDecreaseRange;


        m_minHomingDuration = copyScript.m_minHomingDuration;
        m_initialCooldown = copyScript.m_initialCooldown;
        m_calculationCooldown = copyScript.m_calculationCooldown;
        m_reducePower = copyScript.m_reducePower;
        m_calculationCooldownMinRandom = copyScript.m_calculationCooldownMinRandom;
        m_calculationCooldownMaxRandom = copyScript.m_calculationCooldownMaxRandom;



        m_duration = copyScript.m_duration;
        m_maxAngle = copyScript.m_maxAngle;
        m_maxAngleMinDistance = copyScript.m_maxAngleMinDistance;
        m_collisionSpeed = copyScript.m_collisionSpeed;

        m_isInitialCooldown = true;
        m_initialCooldownRdy = Time.time + m_initialCooldown;

    }

    // abstract
    public override void prepareDestroyScript()
    {
        if (m_movementScript.m_movementComponents.Contains(this))
            m_movementScript.m_movementComponents.Remove(this);
        Destroy(this);
    }
    public override void pasteScript(EntityCopiableAbstract baseScript)
    {
        if (!m_isInitialized)
            initializeStuff();
        setValues((CubeEntityMovementHoming)baseScript);
    }
    public override void assignScripts()
    {
        m_movementScript = GetComponent<CubeEntityMovement>();
    }
    public override void pasteScript(EntityCopiableAbstract baseScript, GameObject target, Vector3 targetPosition)
    {
        if (!m_isInitialized)
            initializeStuff();
        setValues((CubeEntityMovementHoming)baseScript);
        m_target = target;
        m_targetPoint = targetPosition;
    }
}
