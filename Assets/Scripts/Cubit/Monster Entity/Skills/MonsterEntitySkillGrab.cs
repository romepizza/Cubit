using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterEntitySkillGrab : MonoBehaviour
{
    [Header("----- SETTINGS -----")]
    [Header("--- (Skill) ---")]
    public bool m_grabNearestCube = true;
    public float m_grabCooldown;
    public float m_grabRadius;
    public float m_grabRadiusIncrease;

    [Header("----- DEBUG -----")]

    [Header("--- (Timer) ---")]
    public float m_grabFinishTime;

    public MonsterEntityAttachSystem attachSystemScript;
    public MonsterEntityBase m_baseScript;

    void Start()
    {
        attachSystemScript = gameObject.GetComponent<MonsterEntityAttachSystem>();
    }

    // Updates
    void Update()
    {
        manageSkill();
    }

    // Setter
    public void setValuesByScript(GameObject prefab, MonsterEntityBase baseScript)
    {
        MonsterEntitySkillGrab script = prefab.GetComponent<MonsterEntitySkillGrab>();
        if (script != null)
        {
            m_grabNearestCube = script.m_grabNearestCube;
            m_grabCooldown = script.m_grabCooldown;
            m_grabRadius = script.m_grabRadius;
            m_grabRadiusIncrease = script.m_grabRadiusIncrease;
            m_baseScript = baseScript;
        }
        else
            Debug.Log("Warning: Tried to copy values of MonsterEntitySkillGrab from prefab, that didn't have the script attached!");
    }

    // Skill specific
    void manageSkill()
    {
        if (m_grabFinishTime < Time.time && attachSystemScript.m_occupiedPositions.Count < m_baseScript.m_currentMaxCubes)
        {
            activateSkill();

        }
    }
    void activateSkill()
    {
        // chose cube to add to attached
        GameObject cubeAdd = null;

        if (attachSystemScript.m_cubeList.Length < 5000)
        {
            if (m_grabRadiusIncrease <= 0)
            {
                Debug.Log("Aborted: m_grabRadiusIncrease was less than zero!");
                return;
            }
            for (float sphereRadius = m_grabRadiusIncrease; sphereRadius <= m_grabRadius; sphereRadius += m_grabRadiusIncrease)
            {
                Collider[] colliders = Physics.OverlapSphere(transform.position, sphereRadius);
                for (int col = 0; col < colliders.Length; col++)
                {
                    GameObject potentialCube = colliders[col].gameObject;
                    if (potentialCube.layer == 8 && potentialCube.GetComponent<CubeEntitySystem>() != null && potentialCube.GetComponent<CubeEntitySystem>().getStateComponent() != null && potentialCube.GetComponent<CubeEntitySystem>().getStateComponent().canBeAttachedToEnemy())
                    {
                        cubeAdd = potentialCube;
                        break;
                    }
                }
                if (cubeAdd != null)
                    break;
            }
        }
        else
        {
            // chose cube to add to attached
            Collider[] colliders = Physics.OverlapSphere(transform.position, m_grabRadius);
            Debug.Log(colliders.Length);

            float nearestDist = float.MaxValue;

            for (int i = 0; i < colliders.Length; i++)
            {
                GameObject cubePotential = colliders[i].gameObject;
                if (cubePotential.layer == 8 && cubePotential.GetComponent<CubeEntitySystem>() != null)
                {
                    if (cubePotential.GetComponent<CubeEntitySystem>().getStateComponent() != null && cubePotential.GetComponent<CubeEntitySystem>().getStateComponent().canBeAttachedToEnemy())
                    {
                        if (m_grabNearestCube)
                        {
                            float dist = Vector3.Distance(transform.position, cubePotential.transform.position);
                            if (dist < nearestDist)
                            {
                                nearestDist = dist;
                                cubeAdd = cubePotential;
                            }
                        }
                        else
                        {
                            cubeAdd = cubePotential;
                            break;
                        }
                    }
                }
            }
        }

        // add cube to attached
        if (cubeAdd != null)
        {
            if (cubeAdd.GetComponent<CubeEntityState>().m_state == CubeEntityState.s_STATE_ACTIVE)
                Debug.Log("Caution");
            attachSystemScript.addToGrab(cubeAdd);
        }

        m_grabFinishTime = m_grabCooldown + Time.time;
    }
}
