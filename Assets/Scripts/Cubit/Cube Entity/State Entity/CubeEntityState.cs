using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeEntityState : MonoBehaviour
{
    [Header("----- SETTINGS -----")]
    public float m_duration;

    // 0 = inactive
    // 1 = active
    // 2 = attached
    // 3 = core
    public int m_state;
    public static int s_STATE_INACTIVE = 0;
    public static int s_STATE_ACTIVE = 1;
    public static int s_STATE_ATTACHED = 2;
    public static int s_STATE_CORE = 3;

    // 0 = neutral
    // 1 = player

    // 5 = player ally

    // 20 = enemy1
    public int m_affiliation;
    public static int s_AFFILIATION_NEUTRAL = 0;
    public static int s_AFFILIATION_PLAYER = 1;
    public static int s_AFFILIATION_PLAYER_ALLY = 5;
    public static int s_AFFILIATION_ENEMY_1 = 20;

    // 0 = none
    // 1 = player

    // 5 = player ally:      drone

    // 20 = monster:         ejector
    // 21 = monster:         worm
    // 22 = monster:         morpher

    // 50 = dynamic:         plasma projectile

    public int m_type;
    public static int s_TYPE_NONE = 0;
    public static int s_TYPE_PLAYER = 1;
    public static int s_TYPE_DRONE = 5;
    public static int s_TYPE_EJECTOR = 20;
    public static int s_TYPE_WORM = 21;
    public static int s_TYPE_MORPHER = 22;
    public static int s_TYPE_PLASMA_PROJECTILE = 50;

    [Header("----- DEBUG -----")]
    public float m_durationEndTime;
    public CubeEntitySystem m_entitySystemScript;
    public CubeEntityAttached m_attachedScript;


    // Add Attached Script
    public CubeEntityAttached addAttachedScript()
    {
        m_attachedScript = gameObject.AddComponent<CubeEntityAttached>();
        return m_attachedScript;
    }

    public void removeAttachedScript()
    {
        Destroy(m_attachedScript);
        m_attachedScript = null;
    }

    // Setter
    public void setStateByPrefab(GameObject prefab)
    {
        CubeEntityState stateScript = prefab.GetComponent<CubeEntityState>();
        if (stateScript != null)
        {
            m_duration = stateScript.m_duration;
            m_state = stateScript.m_state;
            m_affiliation = stateScript.m_affiliation;
            m_type = stateScript.m_type;

            if (m_duration > 0)
            {
                CubeEntityEndState endStateScript = gameObject.GetComponent<CubeEntityEndState>();
                if (endStateScript == null)
                    endStateScript = gameObject.AddComponent<CubeEntityEndState>();
                endStateScript.setValuesByPrefab(prefab, m_entitySystemScript);
            }
            else
                Destroy(GetComponent<CubeEntityEndState>());
        }
        else
            Debug.Log("no state settings found!");
    }
    public void setDuration(float duration)
    {
        if (m_duration > 0)
        {
            CubeEntityEndState endStateScript = gameObject.GetComponent<CubeEntityEndState>();
            if (endStateScript == null)
                endStateScript = gameObject.AddComponent<CubeEntityEndState>();
            endStateScript.setDuration(duration, m_entitySystemScript);
        }
        else
        {
            //Destroy(gameObject.GetComponent<CubeEntityState>());
        }
    }
    public bool isInactive()
    {
        checkStateValidity();
        return m_state == s_STATE_INACTIVE && m_affiliation == s_AFFILIATION_NEUTRAL && m_type == s_TYPE_NONE;
    }

    // Getter
    public CubeEntityAttached getAttachedScript()
    {
        return m_attachedScript;
    }

    // State checks
    public bool canBeInactive()
    {
        return true;
    }
    public bool canBeActiveNeutral()
    {
        return true;
    }
    public bool canBeActivePlayer()
    {
        checkStateValidity();
        bool valid_0 = m_state == s_STATE_INACTIVE && m_affiliation == s_AFFILIATION_NEUTRAL;

        return valid_0;
    }
    public bool canBeAttachedToPlayer()
    {
        checkStateValidity();
        bool valid_0 = m_state == s_STATE_INACTIVE && m_affiliation == s_AFFILIATION_NEUTRAL;

        return valid_0;
    }
    public bool canBeActiveEnemyEjector()
    {
        checkStateValidity();
        bool valid_0 = m_state == s_STATE_ATTACHED && m_affiliation == s_AFFILIATION_ENEMY_1 && m_type == s_TYPE_EJECTOR;

        return valid_0;
    }
    public bool canBeCoreToEnemyEjector()
    {
        checkStateValidity();
        bool valid_0 = m_state == s_STATE_INACTIVE && m_affiliation == s_AFFILIATION_NEUTRAL;

        return valid_0;
    }
    public bool canBeCoreToEnemyWorm()
    {
        checkStateValidity();
        bool valid_0 = m_state == s_STATE_INACTIVE && m_affiliation == s_AFFILIATION_NEUTRAL;

        return valid_0;
    }

    // general state checks
    public bool canBeCoreGeneral()
    {
        checkStateValidity();
        bool valid_0 = m_state == s_STATE_INACTIVE && m_affiliation == s_AFFILIATION_NEUTRAL;

        return valid_0;
    }
    public bool canBeAttachedToEnemy()
    {
        checkStateValidity();
        bool valid_0 = m_state == s_STATE_INACTIVE && m_affiliation == s_AFFILIATION_NEUTRAL && m_type == s_TYPE_NONE;

        return valid_0;
    }

    // Check Validity
    void checkStateValidity()
    {
        if (m_state == s_STATE_INACTIVE && m_affiliation != s_AFFILIATION_NEUTRAL)
            Debug.Log("(" + gameObject.name + ") Incorrect state: inactive & !neutral");
        if (m_state == s_STATE_ATTACHED && m_affiliation == s_AFFILIATION_NEUTRAL)
            Debug.Log("(" + gameObject.name + ") Incorrect state: attached & neutral");
        if (m_state == s_STATE_CORE && (m_affiliation == s_AFFILIATION_NEUTRAL))
            Debug.Log("(" + gameObject.name + ") Incorrect state: core & neutral");
        //if (m_state == s_STATE_CORE && (m_affiliation == s_AFFILIATION_PLAYER))
            //Debug.Log("(" + gameObject.name + ") Incorrect state: core & player");
    }
}
