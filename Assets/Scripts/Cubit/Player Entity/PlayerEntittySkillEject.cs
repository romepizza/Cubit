using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEntittySkillEject : MonoBehaviour {

    public bool m_useSkill = true;
    public KeyCode keyCode0;
    public KeyCode keyCode1;
    public KeyCode keyCode2;

    [Header("----- SETTINGS -----")]
    public GameObject m_spawnPrefab;
    public float m_cooldown;
    public float m_bonusRandomCooldown;
    public float m_shootDurationFrames;
    public int m_shotsPerFrame;

    [Header("--- (Movement) ---")]
    public CubeEntityMovementAbstract[] m_movementScript;
    public float m_startSpeed;


    [Header("--- Conditions ---")]
    public int m_minCubes;
    public float m_minDistanceToCore;

    [Header("--- (Aim) ---")]
    public bool m_aimHelperOn;
    public float m_randomRadius;
    public float m_shootInFlightDirectionMin;
    public float m_shootInFlightDirectionMax;
    public float m_rangeIfNoTarget;

    [Header("--- (Cube Selection) ---")]
    public bool m_selectNearestFirst;
    public bool m_fromGrabbedOnly;
    public bool m_fromGrabbedFirst;
    public bool m_createIfNoneFound;
    public bool m_createOnly;

    [Header("------- Debug -------")]
    public float m_cooldownReady;
    public float m_shootingReady;
    public bool m_isInitialized;
    public bool m_isBursting;
    public List<GameObject> m_cubesToShoot;
    public Dictionary<GameObject, Vector3> m_targetPositions;
    public Vector3 m_targetPosition;
    public int m_framesShot;
    public GameObject m_target;
    public AttachSystemBase m_attachSystem;

    // Use this for initialization
    void Start()
    {
        if (!m_isInitialized)
            initializeStuff();
    }

    void initializeStuff()
    {
        m_cooldownReady = m_cooldown + Time.time + Random.Range(0, m_bonusRandomCooldown);
        m_attachSystem = GetComponent<AttachSystemBase>();

        m_cubesToShoot = new List<GameObject>();
        m_targetPositions = new Dictionary<GameObject, Vector3>();

        m_isInitialized = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!m_useSkill || m_attachSystem == null)
            return;

        manageCounter();
        manageShot();
    }

    void manageCounter()
    {
        if (!m_isBursting && m_cooldownReady <= Time.time && isPressingKey())
        {
            m_framesShot = 0;
            m_isBursting = true;
        }
    }

    bool isPressingKey()
    {
        return Input.GetKey(keyCode0) || Input.GetKey(keyCode1) || Input.GetKey(keyCode2);
    }

    void manageShot()
    {
        if (!m_isBursting)
            return;


        if (m_framesShot >= m_shootDurationFrames)
        {
            m_cooldownReady = m_cooldown + Random.Range(0, m_bonusRandomCooldown) + Time.time;
            m_isBursting = false;
            return;
        }
        
        selectCubes();
        if (m_cubesToShoot.Count > 0)
        {
            getTargetPositions();
            shootCubes();
        }
        

        m_framesShot++;
    }


    void selectCubes()
    {
        m_target = PlayerEntityAim.aim();
        if (m_target == null)
            m_targetPosition = transform.position + Camera.main.transform.forward * m_rangeIfNoTarget;
        else
            m_targetPosition = m_target.transform.position;

        m_cubesToShoot = new List<GameObject>();
        List<GameObject> potentialCubes = new List<GameObject>();

        if (m_fromGrabbedOnly)
        {
            potentialCubes = m_attachSystem.m_cubeList;

            if (m_selectNearestFirst)
                potentialCubes = sortList(potentialCubes, m_targetPosition);


            foreach (GameObject cube in potentialCubes)
            {
                if (m_cubesToShoot.Count >= m_shotsPerFrame)
                    break;

                float dist = Vector3.Distance(cube.transform.position, transform.position);
                if (dist > 0 & dist <= m_minDistanceToCore)
                {
                    m_cubesToShoot.Add(cube);
                }
            }
        }

        if (m_createOnly)
        {
            m_cubesToShoot = new List<GameObject>();
            while (m_cubesToShoot.Count < m_shotsPerFrame && Constants.getMainCge().m_inactiveCubes.Count > 0)
            {
                Vector3 spawnPosition = transform.position + Random.insideUnitSphere.normalized * 15f;
                GameObject cube = Constants.getMainCge().activateCubeSafe(spawnPosition);
                if (cube.GetComponent<Rigidbody>().velocity.magnitude < 0.1f)
                {
                    cube.GetComponent<Rigidbody>().velocity = (spawnPosition - transform.position).normalized * m_startSpeed;
                }
                m_cubesToShoot.Add(cube);
            }
        }

        if (m_fromGrabbedFirst)
        {
            potentialCubes = m_attachSystem.m_cubeList;

            if (m_selectNearestFirst)
                potentialCubes = sortList(potentialCubes, m_targetPosition);


            foreach (GameObject cube in potentialCubes)
            {
                if (m_cubesToShoot.Count >= m_shotsPerFrame)
                    break;

                float dist = Vector3.Distance(cube.transform.position, transform.position);
                if (dist > 0 & dist <= m_minDistanceToCore)
                {
                    m_cubesToShoot.Add(cube);
                }
            }

            if (m_createIfNoneFound)
            {
                while (m_cubesToShoot.Count < m_shotsPerFrame)
                {
                    Vector3 spawnPosition = transform.position + (m_targetPosition - transform.position).normalized * 15f + Random.insideUnitSphere * 10f;
                    GameObject cube = Constants.getMainCge().activateCubeSafe(spawnPosition);
                    if (cube.GetComponent<Rigidbody>().velocity.magnitude < 0.1f)
                    {
                        cube.GetComponent<Rigidbody>().velocity = Random.insideUnitSphere * 3f;
                    }
                    m_cubesToShoot.Add(cube);
                }
            }
        }
    }

    List<GameObject> sortList(List<GameObject> list, Vector3 targetPosition)
    {
        return list;
    }


    void getTargetPositions()
    {
        foreach (GameObject cube in m_cubesToShoot)
        {
            if (m_aimHelperOn && m_target != null && (m_shootInFlightDirectionMin > 0 || m_shootInFlightDirectionMax > 0) )
            {
                Vector3 targetDirection = m_target.GetComponent<Rigidbody>().velocity;
                float dist = Vector3.Distance(m_target.transform.position, cube.transform.position);
                m_targetPositions[cube] = m_target.transform.position + targetDirection * (dist / m_movementScript[0].m_maxSpeed) * Random.Range(m_shootInFlightDirectionMin, m_shootInFlightDirectionMax) + Random.insideUnitSphere * m_randomRadius;
            }
            else if(m_aimHelperOn && m_target != null)
            {
                m_targetPositions[cube] = m_target.transform.position;
            }
            else
            {
                m_targetPositions[cube] = m_targetPosition;
            }
        }
    }


    void shootCubes()
    {
        foreach (GameObject cube in m_cubesToShoot)
        {
            if(m_attachSystem.m_cubeList.Contains(cube))
                m_attachSystem.deregisterCube(cube);

            if (m_spawnPrefab != null)
                cube.GetComponent<CubeEntityPrefapSystem>().setToPrefab(m_spawnPrefab);
            else
                cube.GetComponent<CubeEntitySystem>().setActiveDynamicly(GetComponent<CubeEntityState>());
            // set affiliation manually
            cube.GetComponent<CubeEntityState>().m_affiliation = GetComponent<CubeEntityState>().m_affiliation;

            cube.GetComponent<CubeEntitySystem>().getMovementComponent().removeComponents(typeof(CubeEntityMovementAbstract));

            foreach (CubeEntityMovementAbstract script in m_movementScript)
            {
                if (script == null)
                    continue;
                cube.GetComponent<CubeEntitySystem>().getMovementComponent().addMovementComponent(script, m_target, m_targetPositions[cube]);
            }
        }
    }
}
