using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterEntityDeathEffectExplosion : MonsterEntityDeathEffect
{
    public AudioSource m_deathSound;
    [Header("-------- Settings -------")]
    public int m_cubeTossNumber;
    public float m_explosionRadius;
    public float m_explosionPower;
    public float m_activeDuration;
    public float m_maxSpeed;

    [Header("--- (Effect) ---")]
    public int m_tossesPerFrame;

    [Header("------- Debug -------")]
    public MonsterEntityBase m_baseScript;
    
    public override void setValues(GameObject prefab, MonsterEntityBase baseScript)
    {
        MonsterEntityDeathEffectExplosion script = prefab.GetComponent<MonsterEntityDeathEffectExplosion>();
        m_cubeTossNumber = script.m_cubeTossNumber;
        m_explosionRadius = script.m_explosionRadius;
        m_explosionPower = script.m_explosionPower;
        m_activeDuration = script.m_activeDuration;
        m_maxSpeed = script.m_maxSpeed;
        m_tossesPerFrame = script.m_tossesPerFrame;
        m_baseScript = baseScript;
    }
    public override void activateDeathEffect()
    {
        GameObject child = new GameObject("MonsterEntityDeathEffectExplosionEffect");
        child.transform.position = transform.position;
        MonsterEntityDeathEffectExplosionEffect effect = child.AddComponent<MonsterEntityDeathEffectExplosionEffect>();
        effect.m_tossesPerFrame = m_tossesPerFrame;
        effect.m_explosionRadius = m_explosionRadius;
        effect.m_activeDuration = m_activeDuration;
        effect.m_explosionPower = m_explosionPower;
        effect.m_maxSpeed = m_maxSpeed;
        effect.m_cubes = new Queue<GameObject>();


        List<GameObject> cubesGrabbedPreviously = new List<GameObject>();
        for (int i = 0; i < m_baseScript.m_attachSystemScript.m_cubeList.Length; i++)
            cubesGrabbedPreviously.Add(m_baseScript.m_attachSystemScript.m_cubeList[i].cube);

        /*
        Collider[] colliders = Physics.OverlapSphere(transform.position, m_explosionRadius);
        {
            for (int i = 0; i < colliders.Length; i++)
            {
                GameObject cube = colliders[i].gameObject;
                CubeEntitySystem systemScript = cube.GetComponent<CubeEntitySystem>();
                if (systemScript != null && systemScript.getStateComponent().isInactive())
                {
                    effect.m_cubes.Enqueue(cube);
                }
            }
        }*/

        foreach(GameObject cube in cubesGrabbedPreviously)
        {
            if(cube != null)
                effect.m_cubes.Enqueue(cube);
        }



        if(m_deathSound != null)
        {
            m_deathSound.Play();
        }
        /*
        Collider[] colliders = Physics.OverlapSphere(transform.position, m_explosionRadius);
        int tossCubes = 0;
        for(int i = 0; i < colliders.Length; i++)
        {
            GameObject cube = colliders[i].gameObject;
            CubeEntitySystem systemScript = cube.GetComponent<CubeEntitySystem>();
            if (systemScript != null && systemScript.getStateComponent().isInactive())
            {
                tossCubes++;
                if (tossCubes >= m_cubeTossNumber)
                    break;

                systemScript.setToActiveEnemyEjector();
                //Debug.DrawLine(cube.transform.position, cube.transform.position + (cube.transform.position - transform.position), Color.white, 10f);
                //systemScript.getMovementComponent().removeAllMovementComponents();
                float distanceFactor = Vector3.Distance(cube.transform.position, transform.position) / m_explosionRadius;
                systemScript.getMovementComponent().addAccelerationComponent(cube.transform.position + (cube.transform.position - transform.position), m_activeDuration, m_explosionPower * distanceFactor, m_maxSpeed * distanceFactor);
                //cube.GetComponent<Rigidbody>().AddExplosionForce(m_explosionPower, transform.position, m_explosionRadius);
            }
        }
        */
        Destroy(this);
    }
}
