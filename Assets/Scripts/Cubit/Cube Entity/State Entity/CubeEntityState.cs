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
    // 2 = enemy1
    public int m_affiliation;
    public static int s_AFFILIATION_NEUTRAL = 0;
    public static int s_AFFILIATION_PLAYER = 1;
    public static int s_AFFILIATION_ENEMY1 = 2;

    // 0 = no monster
    // 1 = ejector
    // 2 = worm
    public int m_monster;
    public static int s_MONSTER_NONE = 0;
    public static int s_MONSTER_EJECTOR = 1;
    public static int s_MONSTER_WORM = 2;

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
            m_monster = stateScript.m_monster;

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
        return m_state == s_STATE_INACTIVE && m_affiliation == s_AFFILIATION_NEUTRAL && m_monster == s_MONSTER_NONE;
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
        bool valid_0 = m_state == s_STATE_ATTACHED && m_affiliation == s_AFFILIATION_ENEMY1 && m_monster == s_MONSTER_EJECTOR;

        return valid_0;
    }
    public bool canBeAttachedToEnemy()
    {
        checkStateValidity();
        bool valid_0 = m_state == s_STATE_INACTIVE && m_affiliation == s_AFFILIATION_NEUTRAL;

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

    // Check Validity
    void checkStateValidity()
    {
        if (m_state == s_STATE_INACTIVE && m_affiliation != s_AFFILIATION_NEUTRAL)
            Debug.Log("(" + gameObject.name + ") Incorrect state: inactive & !neutral");
        if (m_state == s_STATE_ATTACHED && m_affiliation == s_AFFILIATION_NEUTRAL)
            Debug.Log("(" + gameObject.name + ") Incorrect state: attached & neutral");
        if (m_state == s_STATE_CORE && (m_affiliation == s_AFFILIATION_NEUTRAL))
            Debug.Log("(" + gameObject.name + ") Incorrect state: core & neutral");
        if (m_state == s_STATE_CORE && (m_affiliation == s_AFFILIATION_PLAYER))
            Debug.Log("(" + gameObject.name + ") Incorrect state: core & player");
    }
}
