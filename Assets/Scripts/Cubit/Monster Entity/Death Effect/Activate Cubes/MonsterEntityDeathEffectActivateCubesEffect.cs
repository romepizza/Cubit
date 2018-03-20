using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterEntityDeathEffectActivateCubesEffect : MonoBehaviour
{
    [Header("------- Settings -------")]
    public int m_tossesPerFrame;
    public int m_cubeTossesNumber;
    //public float m_explosionRadius;
    //public float m_activeDuration;
    //public float m_explosionPower;
    //public float m_maxSpeed;
    public List<CubeEntityMovementAbstract> m_movementScript;
    //public float m_distance;

    [Header("------- Debug -------")]
    public Queue<GameObject> m_cubes;
    public int m_monsterOrigin;
    public int m_cubesTossed;

	// Use this for initialization
	void Start ()
    {
        //m_cubes = new Queue<GameObject>();
        //m_movementScript = new CubeEntityMovementAbstract[0];

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
            if (m_cubes.Count <= 0 || m_cubesTossed >= m_cubeTossesNumber)
            {
                Destroy(this);
                Destroy(this.gameObject);
                return;
            }

            GameObject cube = m_cubes.Dequeue();

            CubeEntitySystem systemScript = cube.GetComponent<CubeEntitySystem>();
            systemScript.setActiveDynamicly(m_monsterOrigin);
            systemScript.getMovementComponent().removeComponents(typeof(CubeEntityMovementAbstract));
            //float distanceFactor = Mathf.Max(0.2f,  (Vector3.Distance(cube.transform.position, transform.position) / m_explosionRadius));

            Vector3 targetPosition = transform.position + (cube.transform.position - transform.position).normalized * 1000f;
            //Debug.Log(m_movementScript.Count);
            if (m_movementScript != null)
            {
                foreach (CubeEntityMovementAbstract script in m_movementScript)
                {
                    if (script == null)
                        continue;
                    cube.GetComponent<CubeEntitySystem>().getMovementComponent().addMovementComponent(script, Constants.getPlayer(), targetPosition);
                }
            }
            m_cubesTossed++;
        }
    }
}
