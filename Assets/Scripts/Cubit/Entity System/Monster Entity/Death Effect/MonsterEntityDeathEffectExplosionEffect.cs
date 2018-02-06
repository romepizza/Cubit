using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterEntityDeathEffectExplosionEffect : MonoBehaviour
{
    [Header("------- Settings -------")]
    public int m_tossesPerFrame;
    public float m_explosionRadius;
    public float m_activeDuration;
    public float m_explosionPower;
    public float m_maxSpeed;
    //public float m_distance;

    [Header("------- Debug -------")]
    public Queue<GameObject> m_cubes;

	// Use this for initialization
	void Start ()
    {
        //m_cubes = new Queue<GameObject>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        tossCubes();
	}

    void tossCubes()
    {
        for (int i = 0; i < m_tossesPerFrame; i++)
        {
            if (m_cubes.Count <= 0)
            {
                Destroy(this);
                Destroy(this.gameObject);
                return;
            }

            GameObject cube = m_cubes.Dequeue();

            CubeEntitySystem systemScript = cube.GetComponent<CubeEntitySystem>();
            //if (systemScript != null && systemScript.getStateComponent().isInactive())
            {
                // save state
                //systemScript.getStateComponent().removeAttachedScript();
                systemScript.setToActiveEnemyEjector();
                systemScript.getMovementComponent().removeAllMovementComponents();
                float distanceFactor = Mathf.Max(0.2f,  (Vector3.Distance(cube.transform.position, transform.position) / m_explosionRadius));
                systemScript.getMovementComponent().addAccelerationComponent(cube.transform.position + (cube.transform.position - transform.position), m_activeDuration, m_explosionPower, m_maxSpeed * distanceFactor);
            }
        }
    }
}
