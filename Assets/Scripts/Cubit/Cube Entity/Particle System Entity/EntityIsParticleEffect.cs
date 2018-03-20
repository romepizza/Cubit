using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityIsParticleEffect : MonoBehaviour
{
    [Header("------- Debug ------")]
    public float m_lifeTime;
    public bool m_isLoop;
    public bool m_stayOnGameObject;
    public float m_startSpeed;

    [Header("------- Debug ------")]
    public List<CubeEntityParticleSystem> m_registeredInParticleSystems;
    public float m_destroyTime;

	// Use this for initialization
	void Start ()
    {
        m_registeredInParticleSystems = new List<CubeEntityParticleSystem>();

    }
	
	// Update is called once per frame
	void Update ()
    {
        manageDestroy();
	}

    // manage lifetime
    void manageDestroy()
    {
        if(!m_isLoop && m_destroyTime < Time.time)
        {
            destroyObject();
        }
    }

    // manage particle systems
    public void registerParticleSystem(CubeEntityParticleSystem system)
    {
        if (system == null)
            return;

        if(m_registeredInParticleSystems.Contains(system))
        {
            Debug.Log("Aborted: system script was already in the list!");
            return;
        }

        m_registeredInParticleSystems.Add(system);
    }

    public void removeParticleSystem(CubeEntityParticleSystem system)
    {
        if (!m_registeredInParticleSystems.Contains(system))
        {
            Debug.Log("Aborted: system script not in the list!");
            return;
        }

        m_registeredInParticleSystems.Remove(system);
    }

    // destroy
    public void destroyObject()
    {
        foreach(CubeEntityParticleSystem system in m_registeredInParticleSystems)
        {
            system.removeEffectFromList(gameObject);
        }
        Destroy(this.gameObject);
    }
    
    // copy
    public void setValues(CubeEntityParticleSystem system, EntityIsParticleEffect effectScript)
    {
        m_registeredInParticleSystems = new List<CubeEntityParticleSystem>();
        registerParticleSystem(system);
        m_lifeTime = effectScript.m_lifeTime;
        m_isLoop = effectScript.m_isLoop;
        m_stayOnGameObject = effectScript.m_stayOnGameObject;
        m_startSpeed = effectScript.m_startSpeed;

        m_destroyTime = m_lifeTime + Time.time;
    }
}
