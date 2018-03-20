using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterEntityDeathEffectActivateCubes : MonsterEntityDeathEffect
{
    public AudioSource m_deathSound;
    [Header("-------- Settings -------")]
    public int m_cubeTossNumber;
    //public float m_explosionRadius;
    //public float m_explosionPower;
    //public float m_activeDuration;
    //public float m_maxSpeed;

    [Header("--- (Effect) ---")]
    public int m_tossesPerFrame;
    public CubeEntityMovementAbstract[] m_movementScript;

    [Header("------- Debug -------")]
    public MonsterEntityBase m_baseScript;
    public CubeEntityState m_stateScript;
    public int m_monsterOrigin;

    /*
    //public override void setValues(GameObject prefab, MonsterEntityBase baseScript)
    //{
    MonsterEntityDeathEffectExplosion script = prefab.GetComponent<MonsterEntityDeathEffectExplosion>();
    m_cubeTossNumber = script.m_cubeTossNumber;
    m_explosionRadius = script.m_explosionRadius;
    m_explosionPower = script.m_explosionPower;
    m_activeDuration = script.m_activeDuration;
    m_maxSpeed = script.m_maxSpeed;
    m_tossesPerFrame = script.m_tossesPerFrame;
    m_movementScript = script.m_movementScript;
    m_baseScript = baseScript;
    //}
    */
    
    public override void activateDeathEffect(MonsterEntityBase baseScript)
    {
        GameObject child = new GameObject("MonsterEntityDeathEffectExplosionEffect");
        child.transform.position = transform.position;
        MonsterEntityDeathEffectActivateCubesEffect effect = child.AddComponent<MonsterEntityDeathEffectActivateCubesEffect>();
        effect.m_tossesPerFrame = m_tossesPerFrame;
        //effect.m_explosionRadius = m_explosionRadius;
        //effect.m_activeDuration = m_activeDuration;
        //effect.m_explosionPower = m_explosionPower;
        effect.m_cubeTossesNumber = m_cubeTossNumber;
        effect.m_monsterOrigin = m_monsterOrigin;
        //effect.m_maxSpeed = m_maxSpeed;
        effect.m_cubes = new Queue<GameObject>();
        //m_baseScript = baseScript;

        effect.m_movementScript = new List<CubeEntityMovementAbstract>();
        if (m_movementScript != null)
        {
            foreach (CubeEntityMovementAbstract e in m_movementScript)
            {
                if (e == null)
                    continue;
                effect.m_movementScript.Add(e);
            }
        }

        List<GameObject> cubesGrabbedPreviously = new List<GameObject>();
        AttachSystemBase attachSystem = m_baseScript.GetComponent<AttachSystemBase>();
        for (int i = 0; i < attachSystem.m_cubeList.Count; i++)
            cubesGrabbedPreviously.Add(attachSystem.m_cubeList[i]);

        foreach(GameObject cube in cubesGrabbedPreviously)
        {
            if(cube != null)
                effect.m_cubes.Enqueue(cube);
        }

        if(m_deathSound != null)
        {
            m_deathSound.Play();
        }

        Destroy(this);
    }

    // copy
    public void setValues(MonsterEntityDeathEffectActivateCubes prefab)
    {
        m_cubeTossNumber = prefab.m_cubeTossNumber;
        //m_explosionRadius = prefab.m_explosionRadius;
        //m_explosionPower = prefab.m_explosionPower;
        //m_activeDuration = prefab.m_activeDuration;
        //m_maxSpeed = prefab.m_maxSpeed;
        m_movementScript = prefab.m_movementScript;
        m_tossesPerFrame = prefab.m_tossesPerFrame;
    }

    // abstract
    public override void pasteScript(EntityCopiableAbstract baseScript)
    {
        setValues((MonsterEntityDeathEffectActivateCubes)baseScript);
    }
    public override void prepareDestroyScript()
    {
        //activateDeathEffect(null);
        Destroy(this);
    }
    public override void assignScripts()
    {
        m_baseScript = GetComponent<MonsterEntityBase>();
        m_stateScript = GetComponent<CubeEntityState>();
        m_monsterOrigin = m_stateScript.m_type;
    }
}
