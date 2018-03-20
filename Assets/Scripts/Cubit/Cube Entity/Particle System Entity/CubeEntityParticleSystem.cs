using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeEntityParticleSystem : MonoBehaviour
{
    [Header("------- Settings -------")]
    public List<GameObject> m_initialParticleEffects;
    public List<GameObject> m_onChangeStateParticleEffects;

    [Header("------- Debug -------")]
    public List<GameObject> m_activeParticleEffects;
    public List<GameObject> m_loopingParticleEffects;
    public List<GameObject> m_tempraryParticleEffects;

    public void createParticleEffect(GameObject particleEffect)
    {
        if(particleEffect == null)
        {
            Debug.Log("Aborted: particle effect was null!");
            return;
        }

        EntityIsParticleEffect particleScript = particleEffect.GetComponent<EntityIsParticleEffect>();
        if (particleScript == null)
        {
            Debug.Log("Aborted: particle script was null!");
            return;
        }

        GameObject effect = null;
        if (particleScript.m_stayOnGameObject)
        {
            effect = Instantiate(particleEffect, this.gameObject.transform);
            effect.transform.position = transform.position;
        }
        else
        {
            effect = Instantiate(particleEffect);
            effect.transform.position = transform.position;
            Rigidbody rbEffect = effect.GetComponent<Rigidbody>();
            if (rbEffect != null)
            {
                Rigidbody rbThis = GetComponent<Rigidbody>();
                if (rbThis != null)
                    rbEffect.velocity = rbThis.velocity * particleScript.m_startSpeed;
            }
        }

        EntityIsParticleEffect script = effect.GetComponent<EntityIsParticleEffect>();
        script.setValues(this, particleScript);
        addEffectToLists(effect);
    }

    
    // manage particle effects
    public void destroyParticleEffect(GameObject particleEffect)
    {
        if (particleEffect == null)
        {
            Debug.Log("Aborted: particle effect was null!");
            return;
        }

        if (!m_activeParticleEffects.Contains(particleEffect))
        {
            Debug.Log("Aborted: particle effect was not in the list!");
            return;
        }

        EntityIsParticleEffect particleScript = particleEffect.GetComponent<EntityIsParticleEffect>();
        if (particleScript == null)
        {
            Debug.Log("Aborted: particle script system was null!");
            return;
        }

        removeEffectFromList(particleEffect);
        particleScript.destroyObject();
    }
    public void destroyAllParticleEffects()
    {
        if (m_activeParticleEffects == null || m_activeParticleEffects.Count <= 0)
            return;

        for (int i = m_activeParticleEffects.Count - 1; i >= 0; i--)
        {
            GameObject particleEffect = m_activeParticleEffects[i];

            destroyParticleEffect(particleEffect);
        }

        if(m_activeParticleEffects.Count > 0)
        {
            Debug.Log("Oops!");
        }
    }
    public void destroyAllLoopingFromList()
    {
        if (m_loopingParticleEffects == null || m_loopingParticleEffects.Count <= 0)
            return;

        for (int i = m_loopingParticleEffects.Count - 1; i >= 0; i--)
        {
            if(m_loopingParticleEffects[i] != null)
                m_loopingParticleEffects[i].GetComponent<EntityIsParticleEffect>().destroyObject();
        }
    }

    // manage lists
    void addEffectToLists(GameObject particleEffect)
    {
        if(particleEffect == null)
        {
            Debug.Log("Aborted: particle effect was null!");
            return;
        }


        EntityIsParticleEffect particleScript = particleEffect.GetComponent<EntityIsParticleEffect>();
        if (particleScript == null)
        {
            Debug.Log("Aborted: particle script was null!");
            return;
        }

        if (!m_activeParticleEffects.Contains(particleEffect))
            m_activeParticleEffects.Add(particleEffect);

        if (particleScript.m_isLoop && !m_loopingParticleEffects.Contains(particleEffect))
            m_loopingParticleEffects.Add(particleEffect);
        else if(!particleScript.m_isLoop && !m_tempraryParticleEffects.Contains(particleEffect))
            m_tempraryParticleEffects.Add(particleEffect);
    }
    public void removeEffectFromList(GameObject particleEffect)
    {
        if (particleEffect == null)
        {
            Debug.Log("Aborted: particle effect was null!");
            return;
        }

        ParticleSystem particleSystem = particleEffect.GetComponent<ParticleSystem>();
        if (particleSystem)
        {
            Debug.Log("Aborted: particle system was null!");
            return;
        }

        EntityIsParticleEffect particleScript = particleEffect.GetComponent<EntityIsParticleEffect>();
        if(particleScript == null)
        {
            Debug.Log("Aborted: particle script was null!");
            return;
        }

        if (particleScript.m_isLoop && m_loopingParticleEffects.Contains(particleEffect))
            m_loopingParticleEffects.Remove(particleEffect);
        else if (!particleScript.m_isLoop && m_tempraryParticleEffects.Contains(particleEffect))
            m_tempraryParticleEffects.Remove(particleEffect);
    }

    // initial stuff
    public void createInitialParticleEffects()
    {
        foreach(GameObject particleEffect in m_initialParticleEffects)
        {
            if (particleEffect == null)
            {
                Debug.Log("Aborted: particle effect was null!");
                continue;
            }

            EntityIsParticleEffect particleScript = particleEffect.GetComponent<EntityIsParticleEffect>();
            if (particleScript == null)
            {
                Debug.Log("Aborted: particle script was null!");
                continue;
            }

            createParticleEffect(particleEffect);
        }
    }

    // deathrattle
    public void createStateChangeParticleEffects()
    {
        foreach (GameObject particleEffect in m_onChangeStateParticleEffects)
        {
            if(particleEffect == null)
            {
                Debug.Log("Aborted: particle effect was null!");
                continue;
            }

            EntityIsParticleEffect particleScript = particleEffect.GetComponent<EntityIsParticleEffect>();
            if (particleScript == null)
            {
                Debug.Log("Aborted: particle script was null!");
                continue;
            }
            createParticleEffect(particleEffect);
        }
    }


    public void setValues(GameObject prefab)
    {
        destroyAllLoopingFromList();
        createStateChangeParticleEffects();

        //m_activeParticleEffects = new List<GameObject>();
        //m_loopingParticleEffects = new List<GameObject>();
        //m_tempraryParticleEffects = new List<GameObject>();

        CubeEntityParticleSystem script = prefab.GetComponent<CubeEntityParticleSystem>();
        m_initialParticleEffects = script.m_initialParticleEffects;
        m_onChangeStateParticleEffects = script.m_onChangeStateParticleEffects;

        createInitialParticleEffects();
    }
}
