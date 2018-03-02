using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEntitySkillGattling : MonoBehaviour
{
    [Header("----- SETTINGS -----")]
    public float m_cooldown;
    public float m_power;
    public float m_duration;
    public float m_maxSpeed;

    [Header("--- Aim ---")]
    public bool m_aimLock;
    public float m_randomRadius;

    [Header("--- (Potential Cubes) ---")]
    public bool m_fromGrabbedOnly;
    public bool m_fromGrabbedFirst;
    [Header("- (Box) -")]
    public bool m_useBox;
    public Vector3 m_boxSize;
    public Vector3 m_offsetBox;
    [Header("- (Sphere) -")]
    public bool m_useSphere;
    public float m_radiusSphere;
    public Vector3 m_offsetSphere;
    [Header("- (Hemisphere) -")]
    public bool m_useHemisphere;
    public float m_radiusHemisphere;
    public Vector3 m_offsetHemisphere;


    [Header("----- DEBUG -----")]
    public bool m_isAutoLock;
    public bool m_isToggle;
    public float m_cooldownFinishTime;
    public Vector3 m_targetPosition;
    public List<GameObject> m_potentialCubes;
    public GameObject m_cubeToShoot;
    public bool m_cubeSelectedFromGrab;

    public PlayerEntityAttachSystem m_attachScript;

	// Use this for initialization
 	void Start ()
    {
        /*
        SkillEntitAttach[] scripts = GetComponents<SkillEntitAttach>();
        SkillEntitAttach[] scriptsActual;
        for(int i = 0; i < scripts.Length; i++)
        {
            
        }
        if (scripts.Length == 1)
        {
            m_attachScript = scripts[0];
        }
        else if (scripts.Length == 0)
            Debug.Log("Warning: No attachScript attached!");
        else
            Debug.Log("Warning: More than one attachScript attached!");
            */
        m_attachScript = GetComponent<PlayerEntityAttachSystem>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        manageShot();
	}

    void manageShot()
    {
        if(Input.GetButton("ButtonB"))
        {
            if (!m_isToggle)
            {
                m_isAutoLock = !m_isAutoLock;
                m_isToggle = true;
            }
        }
        else
        {
            m_isToggle = false;
        }

        if(Input.GetKeyDown(KeyCode.Mouse1) || Input.GetKeyDown(KeyCode.E))
            m_isAutoLock = !m_isAutoLock;



        if ((Input.GetKey(KeyCode.Mouse0) || Input.GetButton("RBumper") || Input.GetButton("ButtonA") || m_isAutoLock) && m_cooldownFinishTime < Time.time)
        {
            getAimPosition();
            selectCube();
            shoot();
            m_cooldownFinishTime = m_cooldown + Time.time;
        }
    }

    void getAimPosition()
    {
        if (gameObject.GetComponent<PlayerEntityAim>() != null)
        {
            GameObject enemyCubeCore;
            enemyCubeCore = PlayerEntityAim.aim();
            if (enemyCubeCore != null)
            {
                if (enemyCubeCore.GetComponent<MonsterEntityBase>().m_isMovable)
                {

                }
                else
                {
                    if(m_aimLock)
                        m_targetPosition = enemyCubeCore.transform.position + Random.insideUnitSphere * m_randomRadius;
                    else
                        m_targetPosition = transform.position + Camera.main.transform.forward * Vector3.Distance(transform.position, enemyCubeCore.transform.position) + Random.insideUnitSphere * m_randomRadius;
                }
            }
            else
                m_targetPosition = transform.position + Camera.main.transform.forward * 500f;
        }
        else
        {
            m_targetPosition = transform.position + Camera.main.transform.forward * 500f;
        }
    }

    void selectCube()
    {
        GameObject cubeSelected = null;

        // From grabbed Only
        if (m_fromGrabbedOnly)
        {
            m_cubeSelectedFromGrab = false;

            float minDist = float.MaxValue;
            foreach (GameObject cube in m_attachScript.m_grabbedCubes)
            {
                float dist = Vector3.Distance(cube.transform.position, m_targetPosition);
                if (dist < minDist)
                {
                    m_cubeSelectedFromGrab = true;
                    cubeSelected = cube;
                    minDist = dist;
                }
            }
        }
        else
        {
            m_cubeSelectedFromGrab = true;
            // from grabbed first
            if (m_fromGrabbedFirst)
            {
                m_potentialCubes = m_attachScript.m_grabbedCubes;
            }

            // if no cubes are grabbed
            if (m_potentialCubes.Count == 0)
            {
                m_cubeSelectedFromGrab = false;

                // Box Colliders
                if (m_useBox)
                {
                    Collider[] collidersBox = Physics.OverlapBox(transform.position + Camera.main.transform.rotation * m_offsetBox, m_boxSize);

                    foreach (Collider cubeCollider in collidersBox)
                    {
                        if (cubeCollider.gameObject.GetComponent<CubeEntitySystem>() != null)
                        {
                            GameObject cube = cubeCollider.gameObject;
                            if (cube.GetComponent<CubeEntitySystem>().getStateComponent().isInactive())
                            {
                                m_potentialCubes.Add(cube);
                            }
                        }
                    }
                }
                // Hemisphere Colliders
                if (m_useHemisphere)
                {
                    Collider[] collidersHemisphere = Physics.OverlapSphere(transform.position + Camera.main.transform.rotation * m_offsetHemisphere, m_radiusHemisphere);

                    foreach (Collider cubeCollider in collidersHemisphere)
                    {
                        if (cubeCollider.gameObject.GetComponent<CubeEntitySystem>() != null)
                        {
                            GameObject cube = cubeCollider.gameObject;
                            if (cube.GetComponent<CubeEntitySystem>().getStateComponent().isInactive() && transform.InverseTransformPoint(cube.transform.position).z > 0)
                            {
                                m_potentialCubes.Add(cube);
                            }
                        }
                    }
                }
                // Sphere Colliders
                if (m_useSphere)
                {
                    Collider[] collidersSphere = Physics.OverlapSphere(transform.position + Camera.main.transform.rotation * m_offsetSphere, m_radiusSphere);

                    foreach (Collider cubeCollider in collidersSphere)
                    {
                        if (cubeCollider.gameObject.GetComponent<CubeEntitySystem>() != null)
                        {
                            GameObject cube = cubeCollider.gameObject;
                            if (cube.GetComponent<CubeEntitySystem>().getStateComponent().isInactive())
                            {
                                m_potentialCubes.Add(cube);
                            }
                        }
                    }
                }
            }

            // Choose Cube
            float minDist = float.MaxValue;
            foreach (GameObject cube in m_potentialCubes)
            {
                float dist = Vector3.Distance(cube.transform.position, transform.position);
                if(dist < minDist)
                {
                    cubeSelected = cube;
                    minDist = dist;
                }
            }
        }

        m_cubeToShoot = cubeSelected;
    }

    void shoot()
    {
        if(m_cubeToShoot != null)
        {
            if(m_cubeSelectedFromGrab)
                m_attachScript.deregisterCube(m_cubeToShoot);

            m_cubeToShoot.GetComponent<CubeEntitySystem>().setToActivePlayer(/*m_targetPosition, m_duration, m_power, m_maxSpeed*/);

            m_cubeToShoot.GetComponent<CubeEntitySystem>().getMovementComponent().removeAllMovementComponents();
            m_cubeToShoot.GetComponent<CubeEntitySystem>().getMovementComponent().addAccelerationComponent(m_targetPosition, m_duration, m_power, m_maxSpeed);
            m_cubeToShoot.AddComponent<LookInFlightDirection>();

            resetScript();
        }
    }

    void resetScript()
    {
        m_potentialCubes = new List<GameObject>();
    }
}
