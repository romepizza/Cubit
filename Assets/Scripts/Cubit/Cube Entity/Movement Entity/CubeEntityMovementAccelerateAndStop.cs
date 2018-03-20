using UnityEngine;

public class CubeEntityMovementAccelerateAndStop : CubeEntityMovementAbstract
{
    [Header("------- SETTINGS -------")]
    public CubeEntityMovementAbstract[] m_followUpMovementScripts;
    public Vector3 m_overrideTargetDirection;

    [Header("----- (Accelleration) -----")]
    public bool m_useAcceleration = true;
    public float m_accelerationPower;
    //public float m_accelerationInitialDelay;

    [Header("--- Aceleration Destroy Trigger ---")]
    [Header("- (Time) -")]
    public float m_accelerationDuration;
    [Header("- (Speed) -")]
    public float m_accelerationSpeedToReach;
    [Header("- (Distance) -")]
    public float m_accelerationDistanceToTargetFactorMin;
    public float m_accelerationDistanceToTargetFactorMax;
    public float m_accelerationDistance;
    [Header("- (Collision) -")]
    public float m_accelerationCollisionSpeed;

    [Space]
    [Header("--- (Idle) ---")]
    public bool m_useIdle = false;

    [Header("--- Idle Destroy Trigger ---")]
    [Header("- (Time) -")]
    public float m_idleDuration;
    [Header("- (Speed) -")]
    public float m_idleSpeedToReach;
    [Header("- (Distance) -")]
    public float m_idleDistance;
    [Header("- (Collision) -")]
    public float m_idleCollisionSpeed;

    [Space]
    [Header("--- (Deceleration) ---")]
    public bool m_useDeceleration = true;
    public float m_decelerationPower;

    [Header("--- Idle Deceleration Trigger ---")]
    [Header("- (Time) -")]
    public float m_decelerationDuration;
    [Header("- (Speed) -")]
    public float m_decelerationSpeedToReach;
    [Header("- (Distance) -")]
    public float m_decelerationDistance;
    [Header("- (Collision) -")]
    public float m_decelerationCollisionSpeed;


    [Space]
    [Header("------- DEBUG -------")]
    public int m_currentMovementState; // 0 = Acceleration, 1 = Idle, 2 = Deceleration
    public Vector3 m_targetPosition;
    public Vector3 m_targetDirection;
    public Vector3 m_forceVector;
    public float m_delayRdy;
    public float m_durationEnd;
    public float m_distanceTraveled;
    public float m_collisionSpeedCurrent;


    public CubeEntityMovement m_movementScript;
    public Rigidbody m_rb;
    public bool m_isInitialized;


    // Use this for initialization
    void Start()
    {
        if (!m_isInitialized)
            initializeStuff();
    }

    public void initializeStuff()
    {
        m_rb = GetComponent<Rigidbody>();
        if (m_rb == null)
            Debug.Log("RigidBody of " + gameObject.name + " not found");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (m_useThis)
            updateState();

        if (m_useThis)
            updateCalculation();

        if (m_useThis && updateDestroy())
        {
            activateFollowUpScripts();
            prepareDestroyScript();
        }

        if (m_useThis)
            applyForceVector();
    }

    // counter and stuff
    bool updateDestroy()
    {
        if (m_currentMovementState > 2)
            return true;
        
        return false;
    }
    bool updateState()
    {
        if (m_currentMovementState == 0)
        {
            m_distanceTraveled += m_rb.velocity.magnitude * Time.deltaTime;

            bool[] changeConditionsOr = {
                !m_useAcceleration,
                m_accelerationDuration > 0 && m_durationEnd < Time.time,
                (m_accelerationSpeedToReach > 0 && m_rb.velocity.magnitude > m_accelerationSpeedToReach),
                m_accelerationDistance > 0 && m_distanceTraveled > m_accelerationDistance,
                m_accelerationCollisionSpeed > 0 && m_collisionSpeedCurrent > m_accelerationCollisionSpeed
            };

            foreach (bool condition in changeConditionsOr)
            {
                if (condition)
                {
                    changeState();
                    break;
                }
            }
        }

        if (m_currentMovementState == 1)
        {
            m_distanceTraveled += m_rb.velocity.magnitude * Time.deltaTime;

            bool[] changeConditionsOr = {
                !m_useIdle,
                m_idleDuration > 0 && m_durationEnd < Time.time,
                (m_idleSpeedToReach > 0 && m_rb.velocity.magnitude > m_idleSpeedToReach),
                m_idleDistance > 0 && m_distanceTraveled > m_idleDistance,
                m_idleCollisionSpeed > 0 && m_collisionSpeedCurrent > m_idleCollisionSpeed
            };

            foreach (bool condition in changeConditionsOr)
            {
                if (condition)
                {
                    changeState();
                    break;
                }
            }
        }

        if (m_currentMovementState == 2)
        {
            m_distanceTraveled += m_rb.velocity.magnitude * Time.deltaTime;

            bool[] changeConditionsOr = {
                !m_useDeceleration,
                m_decelerationDuration > 0 && m_durationEnd < Time.time,
                (m_decelerationSpeedToReach > 0 && m_rb.velocity.magnitude < m_decelerationSpeedToReach),
                m_decelerationDistance > 0 && m_distanceTraveled > m_decelerationDistance,
                m_decelerationCollisionSpeed > 0 && m_collisionSpeedCurrent > m_decelerationCollisionSpeed
            };

            foreach (bool condition in changeConditionsOr)
            {
                if (condition)
                {
                    changeState();
                    break;
                }
            }
        }

        return false;
    }
    void changeState()
    {
        m_currentMovementState++;
        resetCounter();
        updateDestroy();
    }

    void resetCounter()
    {
        m_distanceTraveled = 0;
        m_collisionSpeedCurrent = 0;

        if (m_currentMovementState == 1)
        {
            m_durationEnd = m_idleDuration + Time.time;
        }
        if (m_currentMovementState == 2)
        {
            m_durationEnd = m_decelerationDuration + Time.time;
        }
    }

    // follow up
    void activateFollowUpScripts()
    {
        foreach(CubeEntityMovementAbstract script in m_followUpMovementScripts)
        {
            if (script == null)
                continue;

            GetComponent<CubeEntitySystem>().getMovementComponent().addMovementComponent(script, m_target, m_targetPosition);

            script.m_useThis = true;
        }
    }

    // Movement
    void updateCalculation()
    {
        getForceVector();
    }
    void getForceVector()
    {
        if(m_currentMovementState == 0)
        {
            m_forceVector = m_targetDirection.normalized * m_accelerationPower;
        }
        if(m_currentMovementState == 1)
        {
            m_forceVector = Vector3.zero;
        }
        if(m_currentMovementState == 2)
        {
            if (m_rb.velocity.magnitude > 0.01f)
                m_forceVector = -m_rb.velocity.normalized * m_decelerationPower;
            else
                m_forceVector = Vector3.zero;
        }
    }
    void applyForceVector()
    {
        m_rb.AddForce(m_forceVector, ForceMode.Acceleration);
    }

    // collision
    public void OnCollisionEnter(Collision collision)
    {
        //if (m_duration - (Time.time - m_durationEndTime) < m_minHomingDuration)
        {
            if (m_accelerationCollisionSpeed > 0)
            {
                m_collisionSpeedCurrent += collision.impulse.magnitude;
            }
        }
        
    }

    // copy
    void setValues(CubeEntityMovementAccelerateAndStop copyScript)
    {
        m_target = copyScript.m_target;
        m_useThis = copyScript.m_useThis;
        m_maxSpeed = copyScript.m_maxSpeed;

        m_followUpMovementScripts = copyScript.m_followUpMovementScripts;
        m_overrideTargetDirection = copyScript.m_overrideTargetDirection;

        m_useAcceleration = copyScript.m_useAcceleration;
        m_accelerationPower = copyScript.m_accelerationPower;
        //m_accelerationInitialDelay = copyScript.m_accelerationInitialDelay;
        m_accelerationDuration = copyScript.m_accelerationDuration;
        m_accelerationSpeedToReach = copyScript.m_accelerationSpeedToReach;
        m_accelerationDistanceToTargetFactorMin = copyScript.m_accelerationDistanceToTargetFactorMin;
        m_accelerationDistanceToTargetFactorMax = copyScript.m_accelerationDistanceToTargetFactorMax;
        m_accelerationDistance = Mathf.Max(copyScript.m_accelerationDistance, m_targetDirection.magnitude * Random.Range(m_accelerationDistanceToTargetFactorMin, m_accelerationDistanceToTargetFactorMax));
        m_accelerationCollisionSpeed = copyScript.m_accelerationCollisionSpeed;

        m_useIdle = copyScript.m_useIdle;
        m_idleDuration = copyScript.m_idleDuration;
        m_idleSpeedToReach = copyScript.m_idleSpeedToReach;
        m_idleDistance = copyScript.m_idleDistance;
        m_idleCollisionSpeed = copyScript.m_idleCollisionSpeed;

        m_useDeceleration = copyScript.m_useDeceleration;
        m_decelerationPower = copyScript.m_decelerationPower;
        m_decelerationDuration = copyScript.m_decelerationDuration;
        m_decelerationSpeedToReach = copyScript.m_decelerationSpeedToReach;
        m_decelerationDistance = copyScript.m_decelerationDistance;
        m_decelerationCollisionSpeed = copyScript.m_decelerationCollisionSpeed;

        // Debug
        m_durationEnd = m_accelerationDuration + Time.time;
        
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
        setValues((CubeEntityMovementAccelerateAndStop)baseScript);
    }
    public override void assignScripts()
    {
        m_movementScript = GetComponent<CubeEntityMovement>();
    }
    public override void pasteScript(EntityCopiableAbstract baseScript, GameObject target, Vector3 targetPosition)
    {
        if (!m_isInitialized)
            initializeStuff();
        m_target = target;
        m_targetPosition = targetPosition;
        m_targetDirection = targetPosition - transform.position;
        setValues((CubeEntityMovementAccelerateAndStop)baseScript);
    }
}
