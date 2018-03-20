using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeEntityCharge : MonoBehaviour
{
    [Header("------- Settings -------")]
    public float m_maxCharge;
    public float m_minCharge;

    [Header("--- Charge Power ---")]
    public float m_chargePower;

    [Header("--- Out Of Charge Effect ---")]
    public bool m_outOfChargeSetToActiveNeutral;
    public int m_outOfChargeLoseLife;


    [Header("------- Debug -------")]
    public float m_currentCharge;

    

    public bool checkCharge()
    {
        if (m_currentCharge < m_minCharge)
        {
            return true;
            //performOutOfCharge();
        }
        return false;
    }

    public bool evaluateChargeDissolve(GameObject otherObject)
    {
        if(otherObject.GetComponent<CubeEntityCharge>() != null)
        {
            return evaluateChargeDissolve(otherObject.GetComponent<CubeEntityCharge>());
        }
        return false;
    }
    public bool evaluateChargeDissolve(CubeEntityCharge otherChargeScript)
    {
        if (otherChargeScript != null)
        {
            addCharge(-otherChargeScript.m_chargePower);
            if (checkCharge())
                return true;
        }
        return false;
    }

    public void performOutOfCharge()
    {
        if (m_outOfChargeSetToActiveNeutral)
            setToActiveNeutral();

        if(m_outOfChargeLoseLife != 0)
        {
            loseLife(m_outOfChargeLoseLife);
        }
        //GetComponent<CubeEntityPrefapSystem>().setToPrefab(CubeEntityPrefabs.getInstance().s_inactivePrefab);
    }
    public void addCharge(float change)
    {
        setCharge(m_currentCharge + change);
    }
    public void setCharge(float value)
    {
        m_currentCharge = value;
        m_currentCharge = Mathf.Min(m_maxCharge, m_currentCharge);
        checkCharge();
    }

    public void initializeScript()
    {
        m_currentCharge = m_maxCharge;
    }

    // collision
    public void evaluateCollision(Collider collider/*, CubeEntityState m_stateScript*/)
    {
        CubeEntityState m_stateScript = GetComponent<CubeEntityState>();
        int m_ownState = m_stateScript.m_state;
        int m_ownAffiliation = m_stateScript.m_affiliation;

        int m_colliderState = collider.gameObject.GetComponent<CubeEntityState>().m_state;
        int m_colliderAffiliation = collider.gameObject.GetComponent<CubeEntityState>().m_affiliation;

        // Affiliation Neutral
        if (m_ownAffiliation == CubeEntityState.s_AFFILIATION_NEUTRAL)
        {

        }
        // Affiliation Player
        else if (m_ownAffiliation == CubeEntityState.s_AFFILIATION_PLAYER)
        {
            // Player Active
            if (m_ownState == CubeEntityState.s_STATE_ACTIVE)
            {
                if (m_colliderAffiliation == CubeEntityState.s_AFFILIATION_ENEMY_1)
                {
                    if (m_colliderState == CubeEntityState.s_STATE_ACTIVE)
                    {
                        if (evaluateChargeDissolve(collider.GetComponent<CubeEntityCharge>()))
                            setToActiveNeutral(gameObject);
                        if (collider.GetComponent<CubeEntityCharge>().evaluateChargeDissolve(this))
                            setToActiveNeutral(collider.gameObject);

                        // explosion
                        GameObject explosion = EffectManager.createSingleEffect(EffectManager.s_smallRedFireExplosion);
                        explosion.transform.position = collider.gameObject.transform.position;
                    }
                    else if (m_colliderState == CubeEntityState.s_STATE_ATTACHED)
                    {
                        if (evaluateChargeDissolve(collider.GetComponent<CubeEntityCharge>()))
                            setToActiveNeutral(gameObject);
                        if (collider.GetComponent<CubeEntityCharge>().evaluateChargeDissolve(this))
                            loseLife(collider.gameObject, 1);

                        // explosion
                        GameObject explosion = EffectManager.createSingleEffect(EffectManager.s_smallBluePlasmaExplosion);
                        explosion.transform.position = collider.gameObject.transform.position;
                    }
                    else if (m_colliderState == CubeEntityState.s_STATE_CORE)
                    {
                        if (collider.GetComponent<CubeEntityCharge>().evaluateChargeDissolve(this))
                            loseLife(collider.gameObject, 3);
                        if (evaluateChargeDissolve(collider.GetComponent<CubeEntityCharge>()))
                            setToActiveNeutral(gameObject);

                        // explosion
                            GameObject explosion = EffectManager.createSingleEffect(EffectManager.s_smallRedFireExplosion); ;
                            explosion.transform.position = collider.gameObject.transform.position;
                    }
                }
            }

            // Player Core
            if (m_ownState == CubeEntityState.s_STATE_CORE)
            {
                if (m_colliderAffiliation == CubeEntityState.s_AFFILIATION_ENEMY_1)
                {
                    collider.GetComponent<CubeEntityCharge>().evaluateChargeDissolve(this);
                    loseLife(gameObject, 1);

                    if (collider.GetComponent<CubeEntityState>().m_state == CubeEntityState.s_STATE_ACTIVE)
                        setToActiveNeutral(collider.gameObject);

                    // explosion
                    GameObject explosion = EffectManager.createSingleEffect(EffectManager.s_smallRedFireExplosion); ;
                    explosion.transform.position = collider.gameObject.transform.position;
                }
            }
        }
        // Affiliation Player Ally
        else if (m_ownAffiliation == CubeEntityState.s_AFFILIATION_PLAYER_ALLY)
        {
            // Player Ally Active
            if (m_ownState == CubeEntityState.s_STATE_ACTIVE)
            {
                if (m_colliderAffiliation == CubeEntityState.s_AFFILIATION_ENEMY_1)
                {
                    if (m_colliderState == CubeEntityState.s_STATE_ACTIVE)
                    {
                        if (evaluateChargeDissolve(collider.GetComponent<CubeEntityCharge>()))
                            setToActiveNeutral(gameObject);
                        if (collider.GetComponent<CubeEntityCharge>().evaluateChargeDissolve(this))
                            setToActiveNeutral(collider.gameObject);
                    }
                    else if (m_colliderState == CubeEntityState.s_STATE_ATTACHED)
                    {
                        if (collider.GetComponent<CubeEntityCharge>().evaluateChargeDissolve(this))
                            loseLife(collider.gameObject, 1);
                        if (evaluateChargeDissolve(collider.GetComponent<CubeEntityCharge>()))
                            setToActiveNeutral(gameObject);

                        // explosion
                        GameObject explosion = EffectManager.createSingleEffect(EffectManager.s_smallRedFireExplosion);
                        explosion.transform.position = collider.gameObject.transform.position;
                    }
                    else if (m_colliderState == CubeEntityState.s_STATE_CORE)
                    {
                        loseLife(collider.gameObject, 3);
                        if (evaluateChargeDissolve(collider.GetComponent<CubeEntityCharge>()))
                            setToActiveNeutral(gameObject);

                        // explosion
                        GameObject explosion = EffectManager.createSingleEffect(EffectManager.s_smallRedFireExplosion);
                        explosion.transform.position = collider.gameObject.transform.position;
                    }
                }
            }

            // Player Ally Core
            if (m_ownState == CubeEntityState.s_STATE_CORE)
            {
                if (m_colliderAffiliation == CubeEntityState.s_AFFILIATION_ENEMY_1)
                {
                    loseLife(gameObject, 1);

                    if (collider.GetComponent<CubeEntityCharge>().evaluateChargeDissolve(this) && collider.GetComponent<CubeEntityState>().m_state == CubeEntityState.s_STATE_ACTIVE)
                        setToActiveNeutral(collider.gameObject);

                    // explosion
                    GameObject explosion = EffectManager.createSingleEffect(EffectManager.s_smallRedFireExplosion);
                    explosion.transform.position = collider.gameObject.transform.position;
                }
            }
        }
        // Affiliation Enemy_1
        else if (m_ownAffiliation == CubeEntityState.s_AFFILIATION_ENEMY_1)
        {
            // Enemy_1 Active
            if (m_ownState == CubeEntityState.s_STATE_ACTIVE)
            {
                if (m_colliderAffiliation == CubeEntityState.s_AFFILIATION_PLAYER || m_colliderAffiliation == CubeEntityState.s_AFFILIATION_PLAYER_ALLY)
                {
                    if (m_colliderState == CubeEntityState.s_STATE_ACTIVE)
                    {
                        if (evaluateChargeDissolve(collider.GetComponent<CubeEntityCharge>()))
                            setToActiveNeutral(gameObject);
                        if (collider.GetComponent<CubeEntityCharge>().evaluateChargeDissolve(this))
                            setToActiveNeutral(collider.gameObject);
                    }

                    else if (m_colliderState == CubeEntityState.s_STATE_CORE)
                    {
                        loseLife(gameObject, 3);
                        if (collider.GetComponent<CubeEntityCharge>().evaluateChargeDissolve(this))
                            setToActiveNeutral(collider.gameObject);
                    }
                }
            }

            // Enemy_1 Attached
            if (m_ownState == CubeEntityState.s_STATE_ATTACHED)
            {
                if ((m_colliderAffiliation == CubeEntityState.s_AFFILIATION_PLAYER || m_colliderAffiliation == CubeEntityState.s_AFFILIATION_PLAYER_ALLY) && collider.GetComponent<CubeEntityState>().m_state == CubeEntityState.s_STATE_ACTIVE)
                {
                    if (m_colliderState == CubeEntityState.s_STATE_ACTIVE)
                    {
                        if (evaluateChargeDissolve(collider.GetComponent<CubeEntityCharge>()))
                            loseLife(gameObject, 1);
                        if (collider.GetComponent<CubeEntityCharge>().evaluateChargeDissolve(this))
                            setToActiveNeutral(collider.gameObject);

                        // explosion
                        GameObject explosion = EffectManager.createSingleEffect(EffectManager.s_smallBluePlasmaExplosion); ;
                        explosion.transform.position = collider.gameObject.transform.position;
                    }
                }
            }

            // Enemy_1 Core
            if (m_ownState == CubeEntityState.s_STATE_CORE)
            {
                if ((m_colliderAffiliation == CubeEntityState.s_AFFILIATION_PLAYER || m_colliderAffiliation == CubeEntityState.s_AFFILIATION_PLAYER_ALLY) && collider.GetComponent<CubeEntityState>().m_state == CubeEntityState.s_STATE_ACTIVE)
                {
                    loseLife(gameObject, 1);
                    if (collider.GetComponent<CubeEntityCharge>().evaluateChargeDissolve(this))
                        setToActiveNeutral(collider.gameObject);

                    // explosion
                    GameObject explosion = EffectManager.createSingleEffect(EffectManager.s_smallRedFireExplosion);
                    explosion.transform.position = collider.gameObject.transform.position;
                }
            }
        }
        else
        {
            Debug.Log("Warning: Something probably went wrong!");
        }
    }

    public void evaluateDischarge(float chargeAdd, int otherAffiliation, int otherState)
    {
        CubeEntityState m_stateScript = GetComponent<CubeEntityState>();
        int ownState = m_stateScript.m_state;
        int ownAffiliation = m_stateScript.m_affiliation;

        if(ownAffiliation != otherAffiliation)
        {
            if(otherState == CubeEntityState.s_STATE_ACTIVE)
            {

                addCharge(chargeAdd);
                if (checkCharge())
                    performOutOfCharge();
            }
        }
    }


    // effects
    void loseLife(GameObject victim, int damage)
    {
        CubeEntitySoundInstance soundInstance = SoundManager.addSoundPlayerCubeHit(victim.transform.position);
        soundInstance.m_audioSource.volume *= 0.2f;

        if (victim.GetComponent<PlayerEntityLifeSystem>() != null)
        {
            victim.GetComponent<PlayerEntityLifeSystem>().loseHp(damage);
        }
        else if (victim.GetComponent<CubeEntityAttached>() != null)
        {
            if (victim.GetComponent<CubeEntityAttached>().m_attachedToGameObject != null)
            {
                if (victim.GetComponent<CubeEntityAttached>().m_attachedToGameObject.GetComponent<MonsterEntityBase>() != null)
                {
                    victim.GetComponent<CubeEntityAttached>().m_attachedToGameObject.GetComponent<MonsterEntityBase>().loseHp(1, this.gameObject);
                }
            }
        }
    }
    void loseLife(int damage)
    {
        CubeEntitySoundInstance soundInstance = SoundManager.addSoundPlayerCubeHit(transform.position);
        soundInstance.m_audioSource.volume *= 0.2f;

        if (GetComponent<PlayerEntityLifeSystem>() != null)
        {
            GetComponent<PlayerEntityLifeSystem>().loseHp(damage);
        }
        else if (GetComponent<CubeEntityAttached>() != null)
        {
            if (GetComponent<CubeEntityAttached>().m_attachedToGameObject != null)
            {
                if (GetComponent<CubeEntityAttached>().m_attachedToGameObject.GetComponent<MonsterEntityBase>() != null)
                {
                    GetComponent<CubeEntityAttached>().m_attachedToGameObject.GetComponent<MonsterEntityBase>().loseHp(1, this.gameObject);
                }
            }
        }
    }
    void setToActiveNeutral(GameObject o)
    {
        o.GetComponent<CubeEntityPrefapSystem>().setToPrefab(CubeEntityPrefabs.getInstance().s_activeNeutralPrefab);
    }
    void setToActiveNeutral()
    {
        GetComponent<CubeEntityPrefapSystem>().setToPrefab(CubeEntityPrefabs.getInstance().s_activeNeutralPrefab);
    }


    // copy
    public void setValues(GameObject prefab)
    {
        if (prefab == null || prefab.GetComponent<CubeEntityCharge>() == null)
        {
            //Debug.Log("Oops!");
            return;
        }
        CubeEntityCharge prefabScript = prefab.GetComponent<CubeEntityCharge>();

        m_maxCharge = prefabScript.m_maxCharge;
        m_minCharge = prefabScript.m_minCharge;
        m_chargePower = prefabScript.m_chargePower;
        m_maxCharge = prefabScript.m_maxCharge;

        m_outOfChargeSetToActiveNeutral = prefabScript.m_outOfChargeSetToActiveNeutral;
        m_outOfChargeLoseLife = prefabScript.m_outOfChargeLoseLife;

        initializeScript();
    }
}
