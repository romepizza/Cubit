using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterEntityDeathEffectDamageExplosion : MonsterEntityDeathEffect
{
    [Header("-------- Settings -------")]
    public float m_explosionRadius;
    public float m_damagePerHit;
    public float m_damageMax;
    public float m_hitsMax;

    [Header("------- Debug -------")]
    bool b;
    public override void activateDeathEffect(MonsterEntityBase baseScript)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, m_explosionRadius);
        foreach(Collider collider in colliders)
        {
            if(collider.GetComponent<CubeEntityCharge>() != null)
            {
                collider.GetComponent<CubeEntityCharge>().evaluateDischarge(-m_damagePerHit, GetComponent<CubeEntityState>().m_affiliation, CubeEntityState.s_STATE_ACTIVE);
            }
        }

        Destroy(this);
    }

    // copy
    public void setValues(MonsterEntityDeathEffectDamageExplosion copyScript)
    {
        m_explosionRadius = copyScript.m_explosionRadius;
        m_damagePerHit = copyScript.m_damagePerHit;
        m_damageMax = copyScript.m_damageMax;
        m_hitsMax = copyScript.m_hitsMax;
    }

    // abstract
    public override void pasteScript(EntityCopiableAbstract baseScript)
    {
        setValues((MonsterEntityDeathEffectDamageExplosion)baseScript);
    }
    public override void prepareDestroyScript()
    {
        Destroy(this);
    }
    public override void assignScripts()
    {
        
    }
}
