using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorCube : MonoBehaviour
{
    public GameObject player;
    [Header("----- SETTINGS -----")]
    public bool dontTouchAtStart;
    public bool dontTouchMaterial;
    public Material materialInactive;
    public Material materialActiveNeutral;
    public Material materialGrabbedByPlayer;
    public Material materialActiveByPlayer;
    public Material materialAttachedToEnemyThrow;
    public Material materialAttachedToEnemyChase;
    public Material materialActiveByEnemy;
    public Material materialEnemyCore;
    public GameObject cubeMesh;
    public GameObject sphereMeshCoreThrow;
    public GameObject sphereMeshCoreChase;
    public float trailTime;

    [Header("--- (Aim) ---")]
    public float localAimRadius = 0.5f;


    [Header("----- DEBUG -----")]
    public bool simulateLight;
    public Mesh defaultMesh;
    public Transform defaultTransform;
    public BoxCollider defaultCollider;
    public Material currentMaterial;
    public float rangeFactor;

    [Header("--- (States) ---")]
    public bool isInactive;
    public bool isActiveNeutral;
    public bool isGrabbedByPlayer;
    public bool isActiveByPlayer;
    public bool isAttachedToEnemyThrow;
    public bool isAttachedToEnemyChase;
    public bool isActiveByEnemy;
    public bool isCore;
    public bool isAccelerating;

    [Header("--- (Collision) ---")]
    public GameObject explosionEffectEnemyDmg;
    public GameObject explosionEffectPlayerDmg;
    public GameObject explosionEffectCoreExplode;
    public int collisionEffectPostponeFrames;

    [Header("--- (Light) ---")]
    //public bool lightIsActive;
    public float defaultIntensity;
    public float defaultLightRadius;
    public float lightDurationDefault;
    public float lightDurationActual;
    public float durationFactor;
    public float minDurationFactor;
    public float grabFactor;
    public float minGrabFactor;
    public float t;
    public bool lightCountChanged;

    [Header("--- (States) ---")]
    public GameObject collidedGameObject;
    public ColorCube colliderColorCubeScript;
    public bool canCollide;
    [Header("--- (State: Inactive) ---")]
    public bool setToInactive;
    public float inactiveDurationDefault;
    public float inactiveDurationActual;
    public float inactiveDurationCurrent;
    public float inactiveCollisionFrameCounter;
    public bool inactiveIsColliderInState;
    [Header("--- (State: Active Neutral) ---")]
    public bool setToActiveNeutral;
    public float activeNeutralDurationDefault;
    public float activeNeutralDurationActual;
    public float activeNeutralDurationCurrent;
    public float activeNeutralCollisionFrameCounter;
    public bool activeNeutralColliderIsInState;
    [Header("--- (State: Grabbed by Player) ---")]
    public bool setGrabbedPlayer;
    public float grabbedPlayerDurationDefault;
    public float grabbedPlayerDurationActual;
    public float grabbedPlayerDurationCurrent;
    public Vector3 grabbedPlayerForceDirection;
    public Vector3 grabbedPlayerTargetPosition;
    public float grabbedPlayerGetPositionCooldown;
    public float grabbedPlayerPower;
    public float grabbedPlayerMinSpeed;
    public float grabbedPlayerMaxSpeed;
    public bool grabbedPlayerCanBeGrabbed;
    public float grabbedPlayerKeepRadius;
    public float grabbedPlayerKeepFactor;
    public bool isGrabbingTemp;
    public float grabbedPlayerCollisionFrameCounter;
    public bool grabbedPlayerColliderIsInState;
    [Header("--- (State: Active by Player) ---")]
    public bool setToActivePlayer;
    public float activePlayerDurationDefault;
    public float activePlayerDurationActual;
    public float activePlayerDurationCurrent;
    public bool activePlayerCanBeActive;
    public float activePlayerCollisionFrameCounter;
    public bool activePlayerColliderIsInState;
    [Header("--- (State: Attached to Enemy Throw) ---")]
    public GameObject isAttachedToThrow;
    //public bool setToAttachedToEnemyThrow;
    //public float attachedEnemyThrowDurationDefault;
    //public float attachedEnemyThrowDurationActual;
    //public float attachedEnemyThrowDurationCurrent;
    public Vector3 attachedToEnemyThrowForceDirection;
    public Vector3 attachedToEnemyThrowTargetPoition;
    public float attachedToEnemyThrowPower;
    public float attachedToEnemyThrowMinSpeed;
    public float attachedToEnemyThrowMaxSpeed;
    public int attachedToEnemyThrowIndex;
    public float attachedToEnemyThrowKeepRadius;
    public float attachedToEnemyThrowKeepFactor;
    public bool attachedToEnemyThrowCanBeAttached;
    public float attachedEnemyThrowCollisionFrameCounter;
    public bool attachedEnemyThrowColliderIsInState;
    [Header("--- (State: Attached to Enemy Throw) ---")]
    public GameObject isAttachedToChase;
    //public bool setToAttachedToEnemyChase;
    //public float attachedEnemyChaseDurationDefault;
    //public float attachedEnemyChaseDurationActual;
    //public float attachedEnemyChaseDurationCurrent;
    public Vector3 attachedToEnemyChaseForceDirection;
    public Vector3 attachedToEnemyChaseTargetPoition;
    public float attachedToEnemyChasePower;
    public float attachedToEnemyChaseMinSpeed;
    public float attachedToEnemyChaseMaxSpeed;
    public int attachedToEnemyChaseIndex;
    public float attachedToEnemyChaseKeepRadius;
    public float attachedToEnemyChaseKeepFactor;
    public bool attachedToEnemyChaseCanBeAttached;
    public float attachedEnemyChaseCollisionFrameCounter;
    public bool attachedEnemyChaseColliderIsInState;
    [Header("--- (State: Active by Enemy) ---")]
    public bool setToActiveEnemy;
    public float activeEnemyDurationDefault;
    public float activeEnemyDurationActual;
    public float activeEnemyDurationCurrent;
    public bool activeEnemyCanBeActive;
    public float activeEnemyCollisionFrameCounter;
    public bool activeEnemyColliderIsInState;
    [Header("--- (State: Core) ---")]
    public bool setToCore;
    public float coreDurationDefault;
    public float coreDurationActual;
    public float coreDurationCurrent;
    public bool coreCanBeCore;
    public float coreCollisionFrameCounter;
    public bool coreIsColliderInState;
    [Header("--- (State: Accelerate) ---")]
    public float accelerateDurationDefault;
    public float accelerateDurationActual;
    public float accelerateDurationCurrent;
    public Vector3 accelerateTargetForceDirection;
    public Vector3 accelerateTargetPoition;
    public float acceleratePower;
    public float accelerateMaxSpeed;

    [Header("--- (Player Collision) ---")]
    public float playerCollisionDuration;
    public float playerCollisionPower;
    public Vector3 playerCollisionOffset;
    public float playerCollisionMaxSpeed;

    [Header("--- (Line Renderer) ---")]
    public Vector3 lineRendererDirection;
    public bool lineRendererAttached;

    [Header("--- (Counter) ---")]
    public float currentLightDuration;
    public float grabbedPlayerGetPositionCooldownCurrent;

    [Header("--- (Log) ---")]
    public int launchedBySkillType; // 0: Player push, 1: player gattling, 2: player collision, 3: enemy ejector
    public bool comesFromGrabbed;
    public bool wasAimed;
    public GameObject launchedBy;


    private Rigidbody rb;
    //public Vector3 currentDirection;
    //public Vector3 lastDirection;
    //public Material material;
    
    
    void Start ()
    {
        defaultMesh = GetComponent<MeshFilter>().sharedMesh;
        defaultTransform = transform;
        defaultCollider = GetComponent<BoxCollider>();
        currentMaterial = GetComponent<Renderer>().sharedMaterial;
        //activateLight(materialEmissive_25, 5f);
        player = GameObject.Find("PlayerDrone");
        rb = GetComponent<Rigidbody>();
        addStateInactive();
    }

	void FixedUpdate()
    {
        manageCollision();
        manageStates();
        manageLineRenderer();
        manageLightIntensity();

        if (GetComponent<Light>() != null && GetComponent<Light>().enabled)
            currentLightDuration += Time.deltaTime;
        if (currentLightDuration > lightDurationActual)
            addStateInactive();
    }

    void manageCollision()
    {
        // Inactive
        if (inactiveCollisionFrameCounter >= 0)
            inactiveCollisionFrameCounter--;

        if (inactiveCollisionFrameCounter == 0)
            inActiveCollisionEffect();

        // Active Neutral
        if (activeNeutralCollisionFrameCounter >= 0)
            activeNeutralCollisionFrameCounter--;

        if (activeNeutralCollisionFrameCounter == 0)
            activeNeutralCollisionEffect();

        // Grabbed Player
        if (grabbedPlayerCollisionFrameCounter >= 0)
            grabbedPlayerCollisionFrameCounter--;

        if (grabbedPlayerCollisionFrameCounter == 0)
            grabbedPlayerCollisionEffect();

        // Active Player
        if (activePlayerCollisionFrameCounter >= 0)
            activePlayerCollisionFrameCounter--;

        if(activePlayerCollisionFrameCounter == 0)
            activePlayerCollisionEffect();

        // Attached Enemy Throw
        if (attachedEnemyThrowCollisionFrameCounter >= 0)
            attachedEnemyThrowCollisionFrameCounter--;

        if (attachedEnemyThrowCollisionFrameCounter == 0)
            attachedEnemyThrowCollisionEffect();

        // Attached Enemy Chase
        if (attachedEnemyChaseCollisionFrameCounter >= 0)
            attachedEnemyChaseCollisionFrameCounter--;

        if (attachedEnemyChaseCollisionFrameCounter == 0)
            attachedEnemyChaseCollisionEffect();

        // Active Enemy
        if (activeEnemyCollisionFrameCounter >= 0)
            activeEnemyCollisionFrameCounter--;

        if (activeEnemyCollisionFrameCounter == 0)
            activeEnemyCollisionEffect();

        // Core
        if (coreCollisionFrameCounter >= 0)
            coreCollisionFrameCounter--;

        if (coreCollisionFrameCounter == 0)
            coreCollisionEffect();
    }

    void manageStates()
    {
        if(isAccelerating)
        {
            stateEffectAccelerate();
        }


        if (isInactive)
        {
            return;
        }
        if (isActiveNeutral)
        {
            return;
        }
        if(isGrabbedByPlayer)
        {
            stateEffectGrabPlayer();
            return;
        }
        if (isActiveByPlayer)
        {
            return;
        }
        if (isAttachedToEnemyThrow)
        {
            stateEffectAttachedToEnemyThrow();
            return;
        }
        if(isAttachedToChase)
        {
            stateEffectAttachedToEnemyChase();
            return;
        }
        if (isActiveByEnemy)
        {
            return;
        }
        if (isCore)
        {
            return;
        }
        addStateInactive();
    }

    // -------- STATE EFFECTS ---------
    void stateEffectGrabPlayer()
    {
        if (rb.velocity.magnitude < grabbedPlayerMaxSpeed)
        {
            if (grabbedPlayerGetPositionCooldownCurrent < Time.time)
            {
                grabbedPlayerTargetPosition = player.GetComponent<SkillGather>().getTargetPositionHard();
                grabbedPlayerGetPositionCooldownCurrent = Time.time + grabbedPlayerGetPositionCooldown;
            }
            else
                ;// grabbedPlayerTargetPosition = player.GetComponent<SkillGather>().getTargetPositionSoft();

            Vector3 direction = grabbedPlayerTargetPosition - transform.position;
            rangeFactor = Mathf.Clamp(1 - (direction.magnitude / grabbedPlayerKeepRadius * grabbedPlayerKeepFactor), (1 - minGrabFactor), 1f);
            grabbedPlayerForceDirection = grabbedPlayerPower * direction.normalized * rangeFactor;
            rb.AddForce(grabbedPlayerForceDirection, ForceMode.Acceleration);
        }
    }

    void stateEffectAttachedToEnemyThrow()
    {
        
        attachedToEnemyThrowTargetPoition = isAttachedToThrow.GetComponent<CubeMonster>().getTargetPosition(attachedToEnemyThrowIndex);
        Vector3 direction = attachedToEnemyThrowTargetPoition - transform.position;
        float rangeFactor = Mathf.Clamp(1 - (direction.magnitude / attachedToEnemyThrowKeepRadius * attachedToEnemyThrowKeepFactor), attachedToEnemyThrowKeepFactor, 1f);
        attachedToEnemyThrowForceDirection = attachedToEnemyThrowPower * direction.normalized * rangeFactor;
        if (rb.velocity.magnitude < 2 && direction.magnitude < 1)
            rb.velocity = Vector3.zero;
        else if (direction.magnitude < 1)
            rb.velocity = rb.velocity * 0.9f;
        else
            rb.AddForce(attachedToEnemyThrowForceDirection, ForceMode.Acceleration);
        rb.velocity = rb.velocity.normalized * Mathf.Clamp(rb.velocity.magnitude, 0, attachedToEnemyThrowMaxSpeed);
        
        if (isAttachedToThrow == this.gameObject)
            addStateInactive();
    }

    void stateEffectAttachedToEnemyChase()
    {
        
        attachedToEnemyChaseTargetPoition = isAttachedToChase.GetComponent<MonsterChase>().getTargetPosition(attachedToEnemyChaseIndex);
        Vector3 direction = attachedToEnemyChaseTargetPoition - transform.position;
        float rangeFactor = Mathf.Clamp(1 - (direction.magnitude / attachedToEnemyChaseKeepRadius * attachedToEnemyChaseKeepFactor), attachedToEnemyChaseKeepFactor, 1f);
        attachedToEnemyChaseForceDirection = attachedToEnemyChasePower * direction.normalized * rangeFactor;
        rb.AddForce(attachedToEnemyChaseForceDirection, ForceMode.Acceleration);
        
        rb.velocity = rb.velocity.normalized * Mathf.Clamp(rb.velocity.magnitude , 0, attachedToEnemyChaseMaxSpeed + isAttachedToChase.GetComponent<Rigidbody>().velocity.magnitude);

        if (isAttachedToChase == this.gameObject)
            addStateInactive();
    }

    void stateEffectAccelerate()
    {
        if (GetComponent<Rigidbody>().velocity.magnitude < accelerateMaxSpeed)
        {
            GetComponent<Rigidbody>().AddForce(accelerateTargetForceDirection, ForceMode.Acceleration);
        }
        //Debug.DrawRay(transform.position, accelerateTargetForceDirection, Color.magenta);

        accelerateDurationCurrent += Time.deltaTime;
        if (accelerateDurationCurrent > accelerateDurationActual)
        {
            removeStateAccelerate();
        }
    }

    // ---------- STATES CHANGE -------------
    void removeAllStates()
    {
        removeStateInactive();
        removeStateActiveByNeutral();
        removeStateGrabbedByPlayer(false);
        removeStateActiveByPlayer();
        removeStateAttachedToEnemyThrow(true);
        removeStateAttachedToEnemyChase(true);
        removeStateActiveByEnemy();
        //removeStateCore(false);
    }

    // Inactive
    public void addStateInactive()
    {
        removeAllStates();

        gameObject.layer = 12;
        isInactive = true;
        activateLight(materialInactive, 0, 1.0f, 1.0f, 1.0f, 1.0f);
        deactivateLight();
    }
    public void removeStateInactive()
    {
        //gameObject.layer = 8;
        isInactive = false;
    }

    // Active Neutral
    public void addStateActiveByNeutral(float duration)
    {
        removeAllStates();

        grabbedPlayerCanBeGrabbed = false;
        attachedToEnemyThrowCanBeAttached = false;
        attachedToEnemyChaseCanBeAttached = false;
        coreCanBeCore = false;

        isActiveNeutral = true;
        activateLight(materialActiveNeutral, duration, 1.0f, 1.0f, 1.0f, 1.0f);
    }
    public void removeStateActiveByNeutral()
    {
        grabbedPlayerCanBeGrabbed = true;
        attachedToEnemyThrowCanBeAttached = true;
        attachedToEnemyChaseCanBeAttached = true;
        coreCanBeCore = true;

        isActiveNeutral = false;
    }

    // Grabbed Player
    public void addStateGrabbedByPlayer(float duration, float power, float minSpeed, float maxSpeed, float keepRadius, float keepFactor)
    {
        removeAllStates();
        /*
        removeStateInactive();
        removeStateActiveByNeutral();
        removeStateActiveByPlayer();
        removeStateAttachedToEnemyThrow(true);
        removeStateActiveByEnemy();
        */
        GetComponent<Rigidbody>().velocity = player.GetComponent<Rigidbody>().velocity;

        gameObject.layer = 11;

        grabbedPlayerPower = power;
        grabbedPlayerMinSpeed = minSpeed;
        grabbedPlayerMaxSpeed = maxSpeed;
        grabbedPlayerKeepRadius = keepRadius;
        grabbedPlayerKeepFactor = keepFactor;

        grabbedPlayerCanBeGrabbed = false;
        activeEnemyCanBeActive = false;
        attachedToEnemyThrowCanBeAttached = false;
        attachedToEnemyChaseCanBeAttached = false;
        coreCanBeCore = false;

        if (GameObject.Find("LogInformationObject") != null && GameObject.Find("LogInformationObject").GetComponent<Log>() != null)
            GameObject.Find("LogInformationObject").GetComponent<Log>().logRegisterCubeGrab(transform.position);
        else if (GameObject.Find("GeneralScriptObject").GetComponent<Log>())
            GameObject.Find("GeneralScriptObject").GetComponent<Log>().logRegisterCubeGrab(transform.position);

        isGrabbedByPlayer = true;
        activateLight(materialGrabbedByPlayer, duration, 1.0f, 0.5f, 0.5f, 0.5f);
    }
    public void removeStateGrabbedByPlayer(bool changeGrabbedCubes)
    {
        gameObject.layer = 8;

        grabbedPlayerCanBeGrabbed = true;
        activeEnemyCanBeActive = true;
        attachedToEnemyThrowCanBeAttached = true;
        attachedToEnemyChaseCanBeAttached = true;
        coreCanBeCore = true;

        if(changeGrabbedCubes)
            player.GetComponent<GrabSystem>().removeCubeFromGrab(this.gameObject, false);
        
        isGrabbedByPlayer = false;
    }

    // Active Player
    public void addStateActiveByPlayer(float duration)
    {
        removeAllStates();

        grabbedPlayerCanBeGrabbed = false;
        activePlayerCanBeActive = false;
        activeEnemyCanBeActive = false;
        attachedToEnemyThrowCanBeAttached = false;
        attachedToEnemyChaseCanBeAttached = false;
        coreCanBeCore = false;

        isActiveByPlayer = true;
        activateLight(materialActiveByPlayer, duration, 1.0f, 1.0f, 1.0f, 1.0f);
    }
    public void removeStateActiveByPlayer()
    {
        grabbedPlayerCanBeGrabbed = true;
        activePlayerCanBeActive = true;
        activeEnemyCanBeActive = true;
        attachedToEnemyThrowCanBeAttached = true;
        attachedToEnemyChaseCanBeAttached = true;
        coreCanBeCore = true;

        isActiveByPlayer = false;
    }

    // Attached Enemy Throw
    public void addStateAttachedToEnemyThrow(GameObject origin, float duration, float power, float maxSpeed, int index, float keepRadius, float keepFactor)
    {
        removeAllStates();

        attachedToEnemyThrowIndex = index;
        attachedToEnemyThrowPower = power;
        attachedToEnemyThrowMaxSpeed = maxSpeed;
        isAttachedToThrow = origin;
        attachedToEnemyThrowKeepRadius = keepRadius;
        attachedToEnemyThrowKeepFactor = keepFactor;

        grabbedPlayerCanBeGrabbed = false;
        activePlayerCanBeActive = false;
        activeEnemyCanBeActive = false;
        attachedToEnemyChaseCanBeAttached = false;
        coreCanBeCore = false;

        isAttachedToEnemyThrow = true;
        activateLight(materialAttachedToEnemyThrow, duration, 1.0f, 0.5f, 0.5f, 1.0f);
    }
    public void removeStateAttachedToEnemyThrow(bool removeAttachedState)
    {
        if(isAttachedToThrow != null && removeAttachedState)
            isAttachedToThrow.GetComponent<CubeMonster>().removeCubeFromAttached(this.gameObject, -1, false);

        grabbedPlayerCanBeGrabbed = true;
        activePlayerCanBeActive = true;
        activeEnemyCanBeActive = true;
        attachedToEnemyChaseCanBeAttached = true;
        coreCanBeCore = true;

        attachedToEnemyThrowIndex = -1;
        isAttachedToEnemyThrow = false;
        isAttachedToThrow = null;
    }

    // Attached Enemy Chase
    public void addStateAttachedToEnemyChase(GameObject origin, float duration, float grabPower, float grabMaxSpeed, int index, float keepRadius, float keepFactor)
    {
        removeAllStates();

        attachedToEnemyChaseIndex = index;
        attachedToEnemyChasePower = grabPower;
        attachedToEnemyChaseMaxSpeed = grabMaxSpeed;
        isAttachedToChase = origin;
        attachedToEnemyChaseKeepRadius = keepRadius;
        attachedToEnemyChaseKeepFactor = keepFactor;

        grabbedPlayerCanBeGrabbed = false;
        activePlayerCanBeActive = false;
        attachedToEnemyThrowCanBeAttached = false;
        activeEnemyCanBeActive = false;
        coreCanBeCore = false;

        isAttachedToEnemyChase = true;
        activateLight(materialAttachedToEnemyChase, duration, 1.0f, 0.5f, 0.5f, 1.0f);
    }
    public void removeStateAttachedToEnemyChase(bool removeAttachedState)
    {
        if (isAttachedToChase != null && removeAttachedState)
            isAttachedToChase.GetComponent<MonsterChase>().removeCubeFromAttached(this.gameObject, -1, false);

        grabbedPlayerCanBeGrabbed = true;
        activePlayerCanBeActive = true;
        attachedToEnemyThrowCanBeAttached = true;
        activeEnemyCanBeActive = true;
        coreCanBeCore = true;

        attachedToEnemyChaseIndex = -1;
        isAttachedToEnemyChase = false;
        isAttachedToChase = null;
    }

    // Active Enemy
    public void addStateActiveByEnemy(float duration, Vector3 direction)
    {
        removeAllStates();

        grabbedPlayerCanBeGrabbed = false;
        activePlayerCanBeActive = false;
        attachedToEnemyThrowCanBeAttached = false;
        attachedToEnemyChaseCanBeAttached = false;
        coreCanBeCore = false;

        addLineRenderer(direction);

        isActiveByEnemy = true;
        activateLight(materialActiveByEnemy, duration, 1.0f, 1.0f, 1.0f, 1.0f);
    }
    public void removeStateActiveByEnemy()
    {
        grabbedPlayerCanBeGrabbed = true;
        activePlayerCanBeActive = true;
        attachedToEnemyThrowCanBeAttached = true;
        attachedToEnemyChaseCanBeAttached = true;
        coreCanBeCore = true;

        removeLineRenderer();

        isActiveByEnemy = false;
    }

    // Core
    public void addStateCore(float duration)
    {
        removeAllStates();

        grabbedPlayerCanBeGrabbed = false;
        activePlayerCanBeActive = false;
        activeEnemyCanBeActive = false;
        attachedToEnemyThrowCanBeAttached = false;
        attachedToEnemyChaseCanBeAttached = false;
        coreCanBeCore = false;

        setMesh(sphereMeshCoreThrow);
        addAimCollider();
        

        rb.mass = 10000f;
        rb.velocity = Vector3.zero;
        isCore = true;
        activateLight(materialEnemyCore, duration, 1.0f, 2.0f, 2.0f, 1.0f); // if changed, change at CubeMonster::loseHp() and MonsterChase::loseHp() aswell
    }
    public void removeStateCore(bool removeAttachedCubes)
    {
        grabbedPlayerCanBeGrabbed = true;
        activePlayerCanBeActive = true;
        activeEnemyCanBeActive = true;
        attachedToEnemyThrowCanBeAttached = true;
        attachedToEnemyChaseCanBeAttached = true;
        coreCanBeCore = true;

        if (removeAttachedCubes && GetComponent<MonsterChase>().isAlive)
            GetComponent<MonsterChase>().die(false, true);
        if (removeAttachedCubes && GetComponent<CubeMonster>().isAlive)
            GetComponent<CubeMonster>().die(false, true);

        if (explosionEffectCoreExplode)
            Instantiate(explosionEffectCoreExplode, transform.position, transform.rotation);

        removeAimCollider();
        setMesh(cubeMesh);

        rb.mass = 1f;
        isCore = false;
    }

    // Accelerate
    public void addStateAccelerate(GameObject origin, float duration, Vector3 targetPosition, float power, float maxSpeed, float activeTime, int skillType, bool fromGrabbed, bool aimed)
    {
        launchedBySkillType = skillType;
        comesFromGrabbed = fromGrabbed;
        wasAimed = aimed;
        launchedBy = origin;

        if (GameObject.Find("LogInformationObject") != null && GameObject.Find("LogInformationObject").GetComponent<Log>() != null)
            GameObject.Find("LogInformationObject").GetComponent<Log>().logRegisterCubeLaunch(transform.position, targetPosition, skillType, fromGrabbed, wasAimed);
        else if (GameObject.Find("GeneralScriptObject").GetComponent<Log>())
            GameObject.Find("GeneralScriptObject").GetComponent<Log>().logRegisterCubeLaunch(transform.position, targetPosition, skillType, fromGrabbed, wasAimed);

        accelerateDurationCurrent = 0;
        isAccelerating = true;
        acceleratePower = power;
        accelerateMaxSpeed = maxSpeed;
        accelerateDurationActual = duration;
        accelerateTargetPoition = targetPosition;
        accelerateTargetForceDirection = acceleratePower * (targetPosition - transform.position).normalized;
        if (origin == player)
        {
            addStateActiveByPlayer(activeTime);
        }
        else
        {
            addStateActiveByEnemy(activeTime, accelerateTargetForceDirection);
        }
    }
    public void removeStateAccelerate()
    {
        isAccelerating = false;
    }


    // --------- COLOR -------------
    void manageLightIntensity()
    {
        if (Random.Range(0f, 1f) < 0.1f)
        {
            if (lightCountChanged && player != null && isGrabbedByPlayer)
            {
                grabFactor = Mathf.Clamp((130f - player.GetComponent<GrabSystem>().grabbedCubes.Count) / 130f, minGrabFactor, 1);
                if ((player.GetComponent<SkillGather>().getTargetPositionHard() - transform.position).magnitude < 0 && grabFactor <= 0.5f)
                {
                    GetComponent<Light>().range = 100;
                    Debug.Log("What?");
                }
                lightCountChanged = false;
            }
            if (!isGrabbedByPlayer)
                grabFactor = 1;

            if ((isActiveNeutral || isActiveByPlayer || isActiveByEnemy) && GetComponent<Light>() != null && GetComponent<Light>().enabled)
                durationFactor = Mathf.Clamp((lightDurationActual - currentLightDuration) / lightDurationActual, minDurationFactor, 1);
            else
                durationFactor = 1;

            t = grabFactor * durationFactor;

            if (isActiveNeutral)
                setColor(materialActiveNeutral, t, t, Mathf.Max(t, 0.3f), t);
            if (isActiveByPlayer)
                setColor(materialActiveByPlayer, t, t, Mathf.Max(t, 0.3f), t);
            if (isActiveByEnemy)
                setColor(materialActiveByEnemy, t, t, Mathf.Max(t, 0.3f), t);
            if(isGrabbedByPlayer)
                setColor(materialGrabbedByPlayer, t, t, Mathf.Max(t, 0.3f), t);
        }
    }

    public void setColor(Material mat, float intensityMaterial, float intensityLight, float factorRadius, float intensityTrailRenderer)
    {
        // set Trail Renderer settings
        if (GetComponent<TrailRenderer>() != null)
        {
            GetComponent<TrailRenderer>().startColor = mat.color * intensityTrailRenderer;
            GetComponent<TrailRenderer>().endColor = mat.color * intensityTrailRenderer;
        }
        // set Light settings
        if (GetComponent<Light>() != null)
        {
            if (!isAttachedToThrow && !isAttachedToChase && !isGrabbedByPlayer)
            {
                GetComponent<Light>().color = mat.color;
                if (GetComponent<MonsterChase>().isAlive)
                    GetComponent<Light>().color = materialAttachedToEnemyChase.color;
                GetComponent<Light>().intensity = defaultIntensity * intensityLight;
                GetComponent<Light>().range = defaultLightRadius * factorRadius;
            }
            else
            {
                GetComponent<Light>().intensity = 0;
                GetComponent<Light>().range = 0;
            }
        }
        // set Material settings
        MaterialPropertyBlock pb = new MaterialPropertyBlock();
        pb.SetColor("_Color", mat.color * intensityMaterial);
        pb.SetColor("_EmissionColor", mat.GetColor("_EmissionColor") * intensityMaterial);
        //pb.SetFloat("_EmissionScaleUI", 1f);
        GetComponent<Renderer>().SetPropertyBlock(pb);
        
        if(mat.IsKeywordEnabled("_EMISSION"))
            GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
        else
            GetComponent<Renderer>().material.DisableKeyword("_EMISSION");


        currentMaterial = GetComponent<Renderer>().sharedMaterial;
    }
    
    public void activateLight(Material original, float duration, float intensityMaterial, float intensityLight, float factorRadius, float intensityTrailRenderer)
    {
        
        lightDurationActual = duration;
        currentLightDuration = 0;
        setColor(original, intensityMaterial, intensityLight, factorRadius, intensityTrailRenderer);

        if (GetComponent<Light>() != null)
            GetComponent<Light>().enabled = true;
        if (GetComponent<TrailRenderer>() != null)
            GetComponent<TrailRenderer>().time = trailTime;
    }

    public void deactivateLight()
    {
        setColor(materialInactive, 1f, 1f, 1f, 1f);

        if(GetComponent<Light>() != null)
            GetComponent<Light>().enabled = false;

        if(GetComponent<TrailRenderer>() != null)
            GetComponent<TrailRenderer>().time = 0;

        currentLightDuration = 0;
    }

    public void destroySelf()
    {
        removeAllStates();
        Destroy(this.gameObject, 0.0f);
    }

    // -------- MESH --------------
    public void setMesh(GameObject meshObject)
    {
        Mesh mesh2 = Instantiate(meshObject.GetComponent<MeshFilter>().sharedMesh);
        GetComponent<MeshFilter>().sharedMesh = mesh2;

        transform.localScale = meshObject.transform.localScale;

        BoxCollider[] boxColliders = GetComponents<BoxCollider>();
        SphereCollider[] shpereColliders = GetComponents<SphereCollider>();
        CapsuleCollider[] capsuleColliders = GetComponents<CapsuleCollider>();

        for (int i = 0; i < boxColliders.Length; i++)
            Destroy(boxColliders[i]);
        for (int i = 0; i < shpereColliders.Length; i++)
            Destroy(shpereColliders[i]);
        for (int i = 0; i < capsuleColliders.Length; i++)
            Destroy(capsuleColliders[i]);

        if (meshObject.GetComponent<BoxCollider>() != null)
        {
            gameObject.AddComponent<BoxCollider>();
            GetComponent<BoxCollider>().size = meshObject.GetComponent< BoxCollider>().size;
        }
        if (meshObject.GetComponent<SphereCollider>() != null)
        {
            gameObject.AddComponent<SphereCollider>();
            GetComponent<SphereCollider>().radius = meshObject.GetComponent<SphereCollider>().radius;
        }
    }

    public void setMesh(Mesh mesh, Transform thisTransform, BoxCollider collider)
    {
        Mesh mesh2 = Instantiate(mesh);
        GetComponent<MeshFilter>().sharedMesh = mesh2;
        transform.localScale = thisTransform.localScale;
        
        SphereCollider[] shpereColliders = GetComponents<SphereCollider>();
        CapsuleCollider[] capsuleColliders = GetComponents<CapsuleCollider>();
        
        for (int i = 0; i < shpereColliders.Length; i++)
            Destroy(shpereColliders[i]);
        for (int i = 0; i < capsuleColliders.Length; i++)
            Destroy(capsuleColliders[i]);

        if(gameObject.GetComponent<BoxCollider>() == null)
            gameObject.AddComponent<BoxCollider>();
        //gameObject.GetComponent<BoxCollider>().size = collider.size;
    }
    public void setMesh(Mesh mesh, Transform thisTransform, SphereCollider collider)
    {
        Mesh mesh2 = Instantiate(mesh);
        GetComponent<MeshFilter>().sharedMesh = mesh2;
        transform.localScale = thisTransform.localScale;

        BoxCollider[] boxColliders = GetComponents<BoxCollider>();
        CapsuleCollider[] capsuleColliders = GetComponents<CapsuleCollider>();

        for (int i = 0; i < boxColliders.Length; i++)
            Destroy(boxColliders[i]);
        for (int i = 0; i < capsuleColliders.Length; i++)
            Destroy(capsuleColliders[i]);

        if(GetComponent<SphereCollider>() == null)
            gameObject.AddComponent<SphereCollider>();
        //GetComponent<SphereCollider>().radius = collider.radius;
    }

    // Line Renderer
    void addLineRenderer(Vector3 direction)
    {
        lineRendererDirection = direction;

        LineRenderer lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.startColor = Color.red;
        lineRenderer.endColor = Color.green;
        lineRenderer.startWidth = 0.5f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, transform.position);
        lineRenderer.material = GetComponent<TrailRenderer>().material;

        lineRendererAttached = true;
    }
    void manageLineRenderer()
    {
        if (lineRendererAttached)
        {
            Vector3 direction = player.transform.position - transform.position;
            float angle = Vector3.Angle(rb.velocity, direction);
            float distance = direction.magnitude;
            LineRenderer lineRenderer = gameObject.GetComponent<LineRenderer>();

            if (distance < 150 && angle < 30)
            {
                lineRenderer.SetPosition(0, transform.position);
                float length = 100;
                lineRenderer.SetPosition(1, transform.position + rb.velocity.normalized * length);
            }
            else
            {
                lineRenderer.SetPosition(0, transform.position);
                lineRenderer.SetPosition(1, transform.position);
            }
        }

    }
    void removeLineRenderer()
    {
        Destroy(gameObject.GetComponent<LineRenderer>());
        lineRendererAttached = false;
    }

    // ------- AIM --------
    public void addAimCollider()
    {
        gameObject.AddComponent<SphereCollider>();
        GetComponent<SphereCollider>().radius = localAimRadius;
        GetComponent<SphereCollider>().isTrigger = true;
    }
    public void removeAimCollider()
    {
        SphereCollider[] sphereColliders = GetComponents<SphereCollider>();
        for(int i = 0; i < sphereColliders.Length; i++)
        {
            if (sphereColliders[i].radius == localAimRadius)
                Destroy(sphereColliders[i]);
        }
    }

    // -------- COLLISION EFFECTS --------
    void inActiveCollisionEffect()
    {
        if (colliderColorCubeScript != null && (activeNeutralColliderIsInState || activePlayerColliderIsInState || activeEnemyColliderIsInState || grabbedPlayerColliderIsInState || attachedEnemyThrowColliderIsInState))
        {
            //if (activePlayerColliderIsInState || grabbedPlayerColliderIsInState)
               // addStateActiveByNeutral(activeNeutralDurationDefault); // colliderColorCubeScript.lightDurationActual - colliderColorCubeScript.currentLightDuration);
            //if (activeEnemyColliderIsInState)
               // addStateActiveByNeutral(activeNeutralDurationDefault); //colliderColorCubeScript.lightDurationActual - colliderColorCubeScript.currentLightDuration);
            //if (attachedEnemyThrowColliderIsInState || coreIsColliderInState)
               // addStateActiveByNeutral(activeNeutralDurationDefault); //activeEnemyDurationDefault);
            //if (activeNeutralColliderIsInState)
                //addStateActiveByNeutral(activeNeutralDurationDefault); //colliderColorCubeScript.lightDurationActual - colliderColorCubeScript.currentLightDuration);
        }
        if (collidedGameObject == player)
        {
            addStateAccelerate(player, 0.2f, player.transform.position + Camera.main.transform.rotation * playerCollisionOffset, playerCollisionPower, Mathf.Sqrt(rb.velocity.magnitude) * playerCollisionMaxSpeed, activePlayerDurationDefault, 2, false, false);
        }
        canCollide = true;
    }
    void activeNeutralCollisionEffect()
    {
        if (colliderColorCubeScript != null && (activePlayerColliderIsInState || activeEnemyColliderIsInState || grabbedPlayerColliderIsInState || attachedEnemyThrowColliderIsInState))
        {
            addStateActiveByNeutral(activeNeutralDurationDefault);
        }
        if (collidedGameObject == player)
        {
            addStateAccelerate(player, 0.2f, player.transform.position + Camera.main.transform.rotation * playerCollisionOffset, playerCollisionPower, Mathf.Sqrt(rb.velocity.magnitude) * playerCollisionMaxSpeed, activePlayerDurationDefault, 2, false, false);
        }
        canCollide = true;
    }
    void grabbedPlayerCollisionEffect()
    {
        if (colliderColorCubeScript != null && false &&  (activeEnemyColliderIsInState || coreIsColliderInState))
        {
            addStateActiveByNeutral(activeNeutralDurationDefault);
        }
        canCollide = true;
    }
    void activePlayerCollisionEffect()
    {
        if (colliderColorCubeScript != null && (activeEnemyColliderIsInState || coreIsColliderInState || attachedEnemyThrowColliderIsInState))
        {
            //if (explosionEffectEnemyDmg != null)
                //Instantiate(explosionEffectEnemyDmg, transform.position, transform.rotation);
            addStateActiveByNeutral(activeNeutralDurationDefault);
        }
        if (collidedGameObject == player)
        {
            addStateAccelerate(player, 0.2f, player.transform.position + Camera.main.transform.rotation * playerCollisionOffset, playerCollisionPower, Mathf.Sqrt(rb.velocity.magnitude) * playerCollisionMaxSpeed, activePlayerDurationDefault, 2, false, false);
        }
        canCollide = true;
    }
    void attachedEnemyThrowCollisionEffect()
    {
        if (colliderColorCubeScript != null && activePlayerColliderIsInState)
        {
            if (GameObject.Find("LogInformationObject") != null && GameObject.Find("LogInformationObject").GetComponent<Log>() != null)
                GameObject.Find("LogInformationObject").GetComponent<Log>().logRegisterCubePlayerHit(0, collidedGameObject.GetComponent<ColorCube>().launchedBySkillType, collidedGameObject.GetComponent<ColorCube>().comesFromGrabbed, collidedGameObject.GetComponent<ColorCube>().wasAimed);
            else if (GameObject.Find("GeneralScriptObject").GetComponent<Log>())
                GameObject.Find("GeneralScriptObject").GetComponent<Log>().logRegisterCubePlayerHit(0, collidedGameObject.GetComponent<ColorCube>().launchedBySkillType, collidedGameObject.GetComponent<ColorCube>().comesFromGrabbed, collidedGameObject.GetComponent<ColorCube>().wasAimed);

            if (explosionEffectEnemyDmg != null)
                Instantiate(explosionEffectEnemyDmg, transform.position, transform.rotation);
            if (isAttachedToThrow != null)
            {
                player.GetComponent<HitIndicator>().registerHit(1);
                isAttachedToThrow.GetComponent<CubeMonster>().loseHp(this.gameObject, 1);
            }
            if(!isActiveNeutral)
                addStateActiveByNeutral(activeNeutralDurationDefault);
        }
        if (collidedGameObject == player)
        {
            if (GameObject.Find("LogInformationObject") != null && GameObject.Find("LogInformationObject").GetComponent<Log>() != null)
                GameObject.Find("LogInformationObject").GetComponent<Log>().logRegisterCubeEnemyHit(this.gameObject, 0);
            else if (GameObject.Find("GeneralScriptObject").GetComponent<Log>())
                GameObject.Find("GeneralScriptObject").GetComponent<Log>().logRegisterCubeEnemyHit(this.gameObject, 0);

            if (explosionEffectPlayerDmg != null)
                Instantiate(explosionEffectPlayerDmg, transform.position, transform.rotation);
            player.GetComponent<PlayerLife>().loseHp(1, 1);
            addStateInactive();
        }
        canCollide = true;
    }
    void attachedEnemyChaseCollisionEffect()
    {
        if (colliderColorCubeScript != null && activePlayerColliderIsInState)
        {
            if (GameObject.Find("LogInformationObject") != null && GameObject.Find("LogInformationObject").GetComponent<Log>() != null)
                GameObject.Find("LogInformationObject").GetComponent<Log>().logRegisterCubePlayerHit(1, collidedGameObject.GetComponent<ColorCube>().launchedBySkillType, collidedGameObject.GetComponent<ColorCube>().comesFromGrabbed, collidedGameObject.GetComponent<ColorCube>().wasAimed);
            else if (GameObject.Find("GeneralScriptObject").GetComponent<Log>())
                GameObject.Find("GeneralScriptObject").GetComponent<Log>().logRegisterCubePlayerHit(1, collidedGameObject.GetComponent<ColorCube>().launchedBySkillType, collidedGameObject.GetComponent<ColorCube>().comesFromGrabbed, collidedGameObject.GetComponent<ColorCube>().wasAimed);

            if (explosionEffectEnemyDmg != null)
                Instantiate(explosionEffectEnemyDmg, transform.position, transform.rotation);
            if (isAttachedToChase != null)
            {
                player.GetComponent<HitIndicator>().registerHit(1);
                isAttachedToChase.GetComponent<MonsterChase>().loseHp(this.gameObject, 1);
            }
            if (!isActiveNeutral)
                addStateActiveByNeutral(activeNeutralDurationDefault);
        }
        if (collidedGameObject == player)
        {
            if (GameObject.Find("LogInformationObject") != null && GameObject.Find("LogInformationObject").GetComponent<Log>() != null)
                GameObject.Find("LogInformationObject").GetComponent<Log>().logRegisterCubeEnemyHit(this.gameObject, 1);
            else if (GameObject.Find("GeneralScriptObject").GetComponent<Log>())
                GameObject.Find("GeneralScriptObject").GetComponent<Log>().logRegisterCubeEnemyHit(this.gameObject, 1);

            if (explosionEffectPlayerDmg != null)
                Instantiate(explosionEffectPlayerDmg, transform.position, transform.rotation);
            player.GetComponent<PlayerLife>().loseHp(1, 3);
            addStateInactive();
        }
        canCollide = true;
    }
    void activeEnemyCollisionEffect()
    {
        if (colliderColorCubeScript != null && (activePlayerColliderIsInState))
        {
            if (explosionEffectEnemyDmg != null)
                Instantiate(explosionEffectEnemyDmg, transform.position, transform.rotation);
            addStateActiveByNeutral(activeNeutralDurationDefault);
        }
        if (collidedGameObject == player)
        {
            if (GameObject.Find("LogInformationObject") != null && GameObject.Find("LogInformationObject").GetComponent<Log>() != null)
                GameObject.Find("LogInformationObject").GetComponent<Log>().logRegisterCubeEnemyHit(launchedBy, 4);
            else if (GameObject.Find("GeneralScriptObject").GetComponent<Log>())
                GameObject.Find("GeneralScriptObject").GetComponent<Log>().logRegisterCubeEnemyHit(launchedBy, 4);

            if (explosionEffectPlayerDmg != null)
                Instantiate(explosionEffectPlayerDmg, transform.position, transform.rotation);
            player.GetComponent<PlayerLife>().loseHp(1, 2);
            addStateInactive();
        }
        canCollide = true;
    }
    void coreCollisionEffect()
    {
        if (colliderColorCubeScript != null && activePlayerColliderIsInState)
        {
            if (explosionEffectEnemyDmg != null)
                Instantiate(explosionEffectEnemyDmg, transform.position, transform.rotation);
            if (GetComponent<CubeMonster>().isAlive)
            {
                if (GameObject.Find("LogInformationObject") != null && GameObject.Find("LogInformationObject").GetComponent<Log>() != null)
                    GameObject.Find("LogInformationObject").GetComponent<Log>().logRegisterCubePlayerHit(2, collidedGameObject.GetComponent<ColorCube>().launchedBySkillType, collidedGameObject.GetComponent<ColorCube>().comesFromGrabbed, collidedGameObject.GetComponent<ColorCube>().wasAimed);
                else if (GameObject.Find("GeneralScriptObject").GetComponent<Log>())
                    GameObject.Find("GeneralScriptObject").GetComponent<Log>().logRegisterCubePlayerHit(2, collidedGameObject.GetComponent<ColorCube>().launchedBySkillType, collidedGameObject.GetComponent<ColorCube>().comesFromGrabbed, collidedGameObject.GetComponent<ColorCube>().wasAimed);
                player.GetComponent<HitIndicator>().registerHit(2);
                GetComponent<CubeMonster>().loseHp(null, 2);
            }
            if (GetComponent<MonsterChase>().isAlive)
            {
                if (GameObject.Find("LogInformationObject") != null && GameObject.Find("LogInformationObject").GetComponent<Log>() != null)
                    GameObject.Find("LogInformationObject").GetComponent<Log>().logRegisterCubePlayerHit(3, collidedGameObject.GetComponent<ColorCube>().launchedBySkillType, collidedGameObject.GetComponent<ColorCube>().comesFromGrabbed, collidedGameObject.GetComponent<ColorCube>().wasAimed);
                else if (GameObject.Find("GeneralScriptObject").GetComponent<Log>())
                    GameObject.Find("GeneralScriptObject").GetComponent<Log>().logRegisterCubePlayerHit(3, collidedGameObject.GetComponent<ColorCube>().launchedBySkillType, collidedGameObject.GetComponent<ColorCube>().comesFromGrabbed, collidedGameObject.GetComponent<ColorCube>().wasAimed);
                player.GetComponent<HitIndicator>().registerHit(2);
                GetComponent<MonsterChase>().loseHp(null, 2);
            }
        }
        if (collidedGameObject == player)
        {
            if (GetComponent<CubeMonster>().isAlive)
            {
                if (GameObject.Find("LogInformationObject") != null && GameObject.Find("LogInformationObject").GetComponent<Log>() != null)
                    GameObject.Find("LogInformationObject").GetComponent<Log>().logRegisterCubeEnemyHit(this.gameObject, 2);
                else if (GameObject.Find("GeneralScriptObject").GetComponent<Log>())
                    GameObject.Find("GeneralScriptObject").GetComponent<Log>().logRegisterCubeEnemyHit(this.gameObject, 2);
            }
            if (GetComponent<MonsterChase>().isAlive)
            {
                if (GameObject.Find("LogInformationObject") != null && GameObject.Find("LogInformationObject").GetComponent<Log>() != null)
                    GameObject.Find("LogInformationObject").GetComponent<Log>().logRegisterCubeEnemyHit(this.gameObject, 3);
                else if (GameObject.Find("GeneralScriptObject").GetComponent<Log>())
                    GameObject.Find("GeneralScriptObject").GetComponent<Log>().logRegisterCubeEnemyHit(this.gameObject, 3);
            }

            if (explosionEffectPlayerDmg != null)
                Instantiate(explosionEffectPlayerDmg, transform.position, transform.rotation);
            player.GetComponent<PlayerLife>().loseHp(1, 0);
        }
        canCollide = true;
    }

    void OnCollisionEnter(Collision col)
    {
        if (canCollide)
        {
            collidedGameObject = col.gameObject;
            colliderColorCubeScript = col.gameObject.GetComponent<ColorCube>();
            if (colliderColorCubeScript != null)
            {
                inactiveIsColliderInState = col.gameObject.GetComponent<ColorCube>().isInactive;
                activeNeutralColliderIsInState = col.gameObject.GetComponent<ColorCube>().isActiveNeutral;
                grabbedPlayerColliderIsInState = col.gameObject.GetComponent<ColorCube>().isGrabbedByPlayer;
                activePlayerColliderIsInState = col.gameObject.GetComponent<ColorCube>().isActiveByPlayer;
                attachedEnemyThrowColliderIsInState = col.gameObject.GetComponent<ColorCube>().isAttachedToEnemyThrow;
                activeEnemyColliderIsInState = col.gameObject.GetComponent<ColorCube>().isActiveByEnemy;
                coreIsColliderInState = col.gameObject.GetComponent<ColorCube>().isCore;
            }
            

            if (isAccelerating)
            {

            }

            if (isInactive)
            {
                inactiveCollisionFrameCounter = collisionEffectPostponeFrames;
            }
            else if (isActiveNeutral)
            {
                activeNeutralCollisionFrameCounter = collisionEffectPostponeFrames;
            }
            else if (isGrabbedByPlayer)
            {
                grabbedPlayerCollisionFrameCounter = collisionEffectPostponeFrames;
            }
            else if (isActiveByPlayer)
            {
                activePlayerCollisionFrameCounter = collisionEffectPostponeFrames;
            }
            else if (isAttachedToEnemyThrow)
            {
                attachedEnemyThrowCollisionFrameCounter = collisionEffectPostponeFrames;
            }
            else if (isAttachedToEnemyChase)
            {
                attachedEnemyChaseCollisionFrameCounter = collisionEffectPostponeFrames;
            }
            else if (isActiveByEnemy)
            {
                activeEnemyCollisionFrameCounter = collisionEffectPostponeFrames;
            }
            else if (isCore)
            {
                coreCollisionFrameCounter = collisionEffectPostponeFrames;
            }
            canCollide = false;
        }
    }
}
