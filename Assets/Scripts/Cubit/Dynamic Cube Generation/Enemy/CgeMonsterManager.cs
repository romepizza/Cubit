using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CgeMonsterManager : MonoBehaviour
{
    [Header("----- Settings -----")]

    [Header("----- DEBUG -----")]
    public bool m_toggle;

    public List<GameObject> m_ejectorsAlive;
    public List<GameObject> m_wormsAlive;

    private CGE m_cge;

    void Start()
    {
        //m_areaScript = gameObject.GetComponent<LevelEntityArea>();
        m_cge = GetComponent<CGE>();
    }

    void Update()
    {
        if (Input.GetButton("ButtonX"))
        {
            if (!m_toggle)
            {
                createEjector();
                m_toggle = true;
            }
        }
        else
        {
            m_toggle = false;
        }

        if (Input.GetKeyUp(KeyCode.T))
        {
            createEjector();
        }
    }

    void createEjector()
    {
        List<GameObject> potentialCubes = new List<GameObject>();

        foreach(GameObject cube in m_cge.m_activeCubes)
        {
            if(cube.GetComponent<CubeEntityState>().canBeCoreToEnemyEjector() && cube.GetComponent<MonsterEntityBase>() == null)
            {
                potentialCubes.Add(cube);
            }
        }

        GameObject core = null;
        if (potentialCubes.Count > 0)
        {
            int randomIndex = Random.Range(0, potentialCubes.Count);
            core = potentialCubes[randomIndex];
        }
        else
            Debug.Log("Warning: Tried to create Ejector, but no fitting cube was found!");

        if (core != null)
        {
            core.GetComponent<CubeEntitySystem>().setToCoreEjector();
            m_ejectorsAlive.Add(core);
            //core.GetComponent<MonsterEntityBase>().m_registeredInManager.Add(this);
        }
    }

    public void deregisterEnemy(GameObject enemyScript)
    {
        if (enemyScript.GetType() == typeof(MonsterEntityEjector))
        {
            m_ejectorsAlive.Remove(enemyScript);
        }
    }
}
