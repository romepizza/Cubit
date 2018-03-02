using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeEntityPrefapSystem : MonoBehaviour
{
    
    [Header("----- SETTINGS -----")]
    

    [Header("----- DEBUG -----")]
    public CubeEntitySystem m_entitySystemScript;

    // Set properties to prefab
    public void setToInactive()
    {
        clearScripts();

        m_entitySystemScript.getMovementComponent().removeAllMovementComponents();
        m_entitySystemScript.getStateComponent().setStateByPrefab(CubeEntityPrefabs.getInstance().s_inactivePrefab);
        m_entitySystemScript.getTransformComponent().setTransform(CubeEntityPrefabs.getInstance().s_inactivePrefab);
        m_entitySystemScript.getAppearanceComponent().setAppearanceByScript(CubeEntityPrefabs.getInstance().s_inactivePrefab);
    }
    public void setToActiveNeutral()
    {
        clearScripts();

        m_entitySystemScript.getStateComponent().setStateByPrefab(CubeEntityPrefabs.getInstance().s_activeNeutralPrefab);
        m_entitySystemScript.getTransformComponent().setTransform(CubeEntityPrefabs.getInstance().s_activeNeutralPrefab);
        m_entitySystemScript.getAppearanceComponent().setAppearanceByScript(CubeEntityPrefabs.getInstance().s_activeNeutralPrefab);

        registerInCge();
    }
    public void setToActivePlayer()
    {
        clearScripts();

        m_entitySystemScript.getStateComponent().setStateByPrefab(CubeEntityPrefabs.getInstance().s_activePlayerPrefab);
        m_entitySystemScript.getTransformComponent().setTransform(CubeEntityPrefabs.getInstance().s_activePlayerPrefab);
        m_entitySystemScript.getAppearanceComponent().setAppearanceByScript(CubeEntityPrefabs.getInstance().s_activePlayerPrefab);

        registerInCge();
        
        CubeEntitySoundInstance soundInstance = SoundManager.addSoundActivePlayerSound(this.gameObject);
    }
    public void setToAttachedPlayer()
    {
        clearScripts();

        m_entitySystemScript.getStateComponent().setStateByPrefab(CubeEntityPrefabs.getInstance().s_attachedPlayerPrefab);
        m_entitySystemScript.getTransformComponent().setTransform(CubeEntityPrefabs.getInstance().s_attachedPlayerPrefab);
        m_entitySystemScript.getAppearanceComponent().setAppearanceByScript(CubeEntityPrefabs.getInstance().s_attachedPlayerPrefab);
    }
    public void setToActiveEnemyEjector()
    {
        clearScripts();

        m_entitySystemScript.getStateComponent().setStateByPrefab(CubeEntityPrefabs.getInstance().s_activeEnemyEjector);
        m_entitySystemScript.getTransformComponent().setTransform(CubeEntityPrefabs.getInstance().s_activeEnemyEjector);
        m_entitySystemScript.getAppearanceComponent().setAppearanceByScript(CubeEntityPrefabs.getInstance().s_activeEnemyEjector);

        registerInCge();
        SoundManager.addSoundActiveEnemyEjectorSound(this.gameObject);
    }
    public void setToAttachedEnemyEjector()
    {
        clearScripts();

        m_entitySystemScript.getStateComponent().setStateByPrefab(CubeEntityPrefabs.getInstance().s_attachedEnemyEjector);
        m_entitySystemScript.getTransformComponent().setTransform(CubeEntityPrefabs.getInstance().s_attachedEnemyEjector);
        m_entitySystemScript.getAppearanceComponent().setAppearanceByScript(CubeEntityPrefabs.getInstance().s_attachedEnemyEjector);
    }
    public void setToAttachedEnemyWorm()
    {
        clearScripts();

        m_entitySystemScript.getStateComponent().setStateByPrefab(CubeEntityPrefabs.getInstance().s_attachedEnemyWorm);
        m_entitySystemScript.getTransformComponent().setTransform(CubeEntityPrefabs.getInstance().s_attachedEnemyWorm);
        m_entitySystemScript.getAppearanceComponent().setAppearanceByScript(CubeEntityPrefabs.getInstance().s_attachedEnemyWorm);
    }
    public void setToCoreEjector()
    {
        clearScripts();

        m_entitySystemScript.getStateComponent().setStateByPrefab(CubeEntityPrefabs.getInstance().s_coreEnemyEjector);
        m_entitySystemScript.getTransformComponent().setTransform(CubeEntityPrefabs.getInstance().s_coreEnemyEjector);
        m_entitySystemScript.getAppearanceComponent().setAppearanceByScript(CubeEntityPrefabs.getInstance().s_coreEnemyEjector);

        MonsterEntityBase monsterBase = gameObject.AddComponent<MonsterEntityBase>();
        monsterBase.createMonster(CubeEntityPrefabs.getInstance().s_coreEnemyEjector);

        SoundManager.addSoundCoreEjectorSound(this.gameObject);
    }
    public void setToCoreWorm()
    {
        clearScripts();

        m_entitySystemScript.getStateComponent().setStateByPrefab(CubeEntityPrefabs.getInstance().s_coreEnemyWorm);
        m_entitySystemScript.getTransformComponent().setTransform(CubeEntityPrefabs.getInstance().s_coreEnemyWorm);
        m_entitySystemScript.getAppearanceComponent().setAppearanceByScript(CubeEntityPrefabs.getInstance().s_coreEnemyWorm);
    }

    void clearScripts()
    {
        SoundManager.clearSounds(this.gameObject);
        
        ParentEntityDeleteOnStateChange[] fadeScripts = gameObject.GetComponents<ParentEntityDeleteOnStateChange>();
        for (int i = 0; i < fadeScripts.Length; i++)
        {
            Destroy(fadeScripts[i]);
        }

        GetComponent<CubeEntityCollision>().removeCollisionEffectScripts();

        CemBoidAttached[] attachedBoidScript = GetComponents<CemBoidAttached>();
        foreach (CemBoidAttached script in attachedBoidScript)
            script.removeFromSwarm();
    }

    void registerInCge()
    {
        Constants.getMainCge().registerActiveCube(this.gameObject);
    }
}
