using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeEntityPrefapSystem : MonoBehaviour
{
    
    [Header("----- SETTINGS -----")]
    

    [Header("----- DEBUG -----")]
    public CubeEntitySystem m_entitySystemScript;

    // Set properties to prefab
    public void setToPrefab(GameObject prefab)
    {
        clearScripts();

        GetComponent<CubeEntityMovement>().removeComponents(typeof(CubeEntityMovementAbstract));

        GetComponent<CubeEntityState>().setStateByPrefab(prefab);
        GetComponent<CubeEntityTransform>().setTransform(prefab);
        GetComponent<CubeEntityAppearance>().setAppearanceByScript(prefab);
        GetComponent<CubeEntityCharge>().setValues(prefab);
        GetComponent<CubeEntityParticleSystem>().setValues(prefab);

        if (GetComponent<CubeEntityState>().m_state != CubeEntityState.s_STATE_INACTIVE)
            registerInCge();


        EntitySystemBase entitySystemBase = gameObject.GetComponent<EntitySystemBase>();
        if (entitySystemBase == null)
        {
            Debug.Log("Aborted: entitySystemBase was null!");
            return;
        }
        entitySystemBase.copyPasteComponents(prefab);
    }
    /*
    // neutral
    public void setToInactive()
    {
        clearScripts();

        m_entitySystemScript.getMovementComponent().removeComponents(typeof(CubeEntityMovementAbstract));
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
    // player
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
    // ejector
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
    public void setToCoreEjector()
    {
        clearScripts();

        m_entitySystemScript.getStateComponent().setStateByPrefab(CubeEntityPrefabs.getInstance().s_coreEnemyEjector);
        m_entitySystemScript.getTransformComponent().setTransform(CubeEntityPrefabs.getInstance().s_coreEnemyEjector);
        m_entitySystemScript.getAppearanceComponent().setAppearanceByScript(CubeEntityPrefabs.getInstance().s_coreEnemyEjector);

        //MonsterEntityBase monsterBase = gameObject.AddComponent<MonsterEntityBase>();
        //monsterBase.createMonster(CubeEntityPrefabs.getInstance().s_coreEnemyEjector);

        EntitySystemBase entitySystemBase = gameObject.GetComponent<EntitySystemBase>();
        if(entitySystemBase == null)
        {
            Debug.Log("Aborted: entitySystemBase was null!");
            return;
        }
        entitySystemBase.copyPasteComponents(CubeEntityPrefabs.getInstance().s_coreEnemyEjector);

        SoundManager.addSoundCoreEjectorSound(this.gameObject);
    }
    // worm
    public void setToAttachedEnemyWorm()
    {
        clearScripts();

        m_entitySystemScript.getStateComponent().setStateByPrefab(CubeEntityPrefabs.getInstance().s_attachedEnemyWorm);
        m_entitySystemScript.getTransformComponent().setTransform(CubeEntityPrefabs.getInstance().s_attachedEnemyWorm);
        m_entitySystemScript.getAppearanceComponent().setAppearanceByScript(CubeEntityPrefabs.getInstance().s_attachedEnemyWorm);
    }
    public void setToCoreWorm()
    {
        clearScripts();

        m_entitySystemScript.getStateComponent().setStateByPrefab(CubeEntityPrefabs.getInstance().s_coreEnemyWorm);
        m_entitySystemScript.getTransformComponent().setTransform(CubeEntityPrefabs.getInstance().s_coreEnemyWorm);
        m_entitySystemScript.getAppearanceComponent().setAppearanceByScript(CubeEntityPrefabs.getInstance().s_coreEnemyWorm);

        //MonsterEntityBase monsterBase = gameObject.AddComponent<MonsterEntityBase>();
        //monsterBase.createMonster(CubeEntityPrefabs.getInstance().s_coreEnemyWorm);

        EntitySystemBase entitySystemBase = gameObject.GetComponent<EntitySystemBase>();
        if (entitySystemBase == null)
        {
            Debug.Log("Aborted: entitySystemBase was null!");
            return;
        }
        entitySystemBase.copyPasteComponents(CubeEntityPrefabs.getInstance().s_coreEnemyWorm);

        SoundManager.addSoundCoreEjectorSound(this.gameObject);
    }
    // morpher
    public void setToActiveEnemyMorpher()
    {
        clearScripts();

        m_entitySystemScript.getStateComponent().setStateByPrefab(CubeEntityPrefabs.getInstance().s_activeEnemyMorpher);
        m_entitySystemScript.getTransformComponent().setTransform(CubeEntityPrefabs.getInstance().s_activeEnemyMorpher);
        m_entitySystemScript.getAppearanceComponent().setAppearanceByScript(CubeEntityPrefabs.getInstance().s_activeEnemyMorpher);

        registerInCge();
        SoundManager.addSoundActiveEnemyEjectorSound(this.gameObject);
    }
    public void setToAttachedEnemyMorpher()
    {
        clearScripts();

        m_entitySystemScript.getStateComponent().setStateByPrefab(CubeEntityPrefabs.getInstance().s_attachedEnemyMorpher);
        m_entitySystemScript.getTransformComponent().setTransform(CubeEntityPrefabs.getInstance().s_attachedEnemyMorpher);
        m_entitySystemScript.getAppearanceComponent().setAppearanceByScript(CubeEntityPrefabs.getInstance().s_attachedEnemyMorpher);
    }
    public void setToCoreMorpher()
    {
        clearScripts();

        m_entitySystemScript.getStateComponent().setStateByPrefab(CubeEntityPrefabs.getInstance().s_coreEnemyMorpher);
        m_entitySystemScript.getTransformComponent().setTransform(CubeEntityPrefabs.getInstance().s_coreEnemyMorpher);
        m_entitySystemScript.getAppearanceComponent().setAppearanceByScript(CubeEntityPrefabs.getInstance().s_coreEnemyMorpher);

        //MonsterEntityBase monsterBase = gameObject.AddComponent<MonsterEntityBase>();
        //monsterBase.createMonster(CubeEntityPrefabs.getInstance().s_coreEnemyMorpher);

        EntitySystemBase entitySystemBase = gameObject.GetComponent<EntitySystemBase>();
        if (entitySystemBase == null)
        {
            Debug.Log("Aborted: entitySystemBase was null!");
            return;
        }
        entitySystemBase.copyPasteComponents(CubeEntityPrefabs.getInstance().s_coreEnemyMorpher);

        SoundManager.addSoundCoreEjectorSound(this.gameObject);
    }
    */
    void clearScripts()
    {
        SoundManager.clearSounds(this.gameObject);
        
        ParentEntityDeleteOnStateChange[] fadeScripts = gameObject.GetComponents<ParentEntityDeleteOnStateChange>();
        for (int i = 0; i < fadeScripts.Length; i++)
        {
            Destroy(fadeScripts[i]);
        }

        

        //GetComponent<CubeEntityCollision>().removeCollisionEffectScripts();

        //CemBoidAttached[] attachedBoidScript = GetComponents<CemBoidAttached>();
        //foreach(CemBoidAttached script in attachedBoidScript)
        //script.removeFromSwarm();
    }

    void registerInCge()
    {
        Constants.getMainCge().registerActiveCube(this.gameObject);
    }
}
