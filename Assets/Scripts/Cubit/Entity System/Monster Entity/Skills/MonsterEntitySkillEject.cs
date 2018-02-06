using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterEntitySkillEject : MonoBehaviour
{
    [Header("----- SETTINGS -----")]
    public float m_cooldown;
    public float m_duration;
    public float m_power;
    public float m_maxSpeed;
    public bool m_selectNearestCube;
    public int m_minCubes;
    public float m_minDistanceToCore;

    [Header("--- (Shoot Help) ---")]
    public float m_minShootInDirection;
    public float m_maxShootInDirection;

    [Header("----- DEBUG -----")]
    public GameObject m_cubeToShoot;
    public List<GameObject> m_potentialCubes;
    public bool m_isShooting;
    public float m_cooldownFinishTime;
    public GameObject m_player;
    public Vector3 m_targetPosition;

    public MonsterEntityAttachSystem m_attachSystem;
    public MonsterEntityBase m_base;

	// Use this for initialization
	void Start ()
    {
        m_potentialCubes = new List<GameObject>();
        m_player = Constants.getPlayer();
	}
	
	// Update is called once per frame
	void Update ()
    {
        manageShot();
	}

    void manageShot()
    {
        if (m_isShooting && m_cooldownFinishTime < Time.time && m_attachSystem.m_occupiedPositions.Count > m_minCubes)
        {
            selectCube(); if (m_cubeToShoot != null)
            {
                getTargetPosition();
                shoot();
            }
        }
    }

    void selectCube()
    {
        m_potentialCubes = m_attachSystem.getAttachedCubes();
        
        float minDistance = float.MaxValue;
        foreach(GameObject cube in m_potentialCubes)
        {
            float distToCore = Vector3.Distance(cube.transform.position, transform.position);
            float dist = Vector3.Distance(cube.transform.position, m_player.transform.position);
            if (dist < minDistance && distToCore < m_minDistanceToCore)
            {
                m_cubeToShoot = cube;
                minDistance = dist;
            }
        }
    }

    void getTargetPosition()
    {
        Vector3 targetDirection = m_player.GetComponent<Rigidbody>().velocity;
        float dist = Vector3.Distance(m_player.transform.position, m_cubeToShoot.transform.position);
        m_targetPosition = m_player.transform.position + targetDirection * (dist / m_maxSpeed) * Random.Range(m_minShootInDirection, m_maxShootInDirection);
    }

    void shoot()
    {
        m_cooldownFinishTime = m_cooldown + Time.time;

        m_attachSystem.deregisterCube(m_cubeToShoot);
        m_cubeToShoot.GetComponent<Rigidbody>().velocity = Vector3.zero;// m_cubeToShoot.GetComponent<Rigidbody>().velocity.normalized * Mathf.Sqrt(m_cubeToShoot.GetComponent<Rigidbody>().velocity.magnitude);
        m_cubeToShoot.GetComponent<CubeEntitySystem>().setToActiveEnemyEjector(/*m_targetPosition, m_duration, m_power, m_maxSpeed*/);
        m_cubeToShoot.GetComponent<CubeEntitySystem>().getMovementComponent().removeAllMovementComponents();
        m_cubeToShoot.GetComponent<CubeEntitySystem>().getMovementComponent().addAccelerationComponent(m_targetPosition, m_duration, m_power, m_maxSpeed);
    }

    public void setValuesByScript(GameObject prefab)
    {
        MonsterEntitySkillEject script = prefab.GetComponent<MonsterEntitySkillEject>();
        m_cooldown = script.m_cooldown;
        m_cooldownFinishTime = m_cooldown + Time.time;
        m_duration = script.m_duration;
        m_power = script.m_power;
        m_maxSpeed = script.m_maxSpeed;
        m_minCubes = script.m_minCubes;
        m_minShootInDirection = script.m_minShootInDirection;
        m_maxShootInDirection = script.m_maxShootInDirection;
        m_minCubes = script.m_minCubes;
        m_minDistanceToCore = script.m_minDistanceToCore;

        // Debug
        m_isShooting = script.m_isShooting;
    }
}
