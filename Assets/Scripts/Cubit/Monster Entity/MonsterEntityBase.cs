using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterEntityBase : MonoBehaviour
{
    public bool doStuf;
    [Header("----- SETTINGS -----")]
    public bool m_isMovable;
    public float m_aimColliderRadius;

    [Header("--- (Hp) ---")]
    public int m_maxHp;
    public int m_minHp;
    public GameObject m_deathExplosion;


    [Header("--- (Cubes) ---")]
    public int m_maxCubes;

    [Header("----- DEBUG -----")]
    public int m_currentHp;
    public int m_currentMaxCubes;
    public List<LevelEntityMonsterManager> m_registeredInManager;
    public SphereCollider m_collider;

    public float m_size;
    
    [Header("--- (Scripts) ---")]
    public MonsterEntityAttachSystem m_attachSystemScript;
    public AttachSystemBase m_attachSystemScriptNew;
    public CubeEntityAttached m_attachedToScript;
    [Header("- (Monster Type) -")]
    public MonsterEntityEjector m_ejectorScript;
    public MonsterEntityWorm m_wormScript;
    [Header("- (Skills) -")]
    public MonsterEntitySkillGrab m_grabScript;
    public MonsterEntitySkillEject m_ejectScript;
    // Use this for initialization

    public Rigidbody m_rb;
	void Start ()
    {
        m_rb = GetComponent<Rigidbody>();
        if (m_rb == null)
            Debug.Log("Warning: No rigidbody detected!");

        initializeValues();
    }

    // Update is called once per frame
    /*
    void Update ()
    {
		if(doStuf)
        {
            doStuff();
            doStuf = false;
        }
	}
    */

    // Setter
    public void createMonster(GameObject prefab)
    {
        setValues(prefab);
        applyCollider();
        m_size = Mathf.Max(Mathf.Max(5f, m_maxHp / 10f));
        //setSize();
    }

    public void setValues(GameObject prefab)
    {
        // Base Script
        MonsterEntityBase scriptBase = prefab.GetComponent<MonsterEntityBase>();
        if (scriptBase != null)
        {
            m_isMovable = scriptBase.m_isMovable;
            m_minHp = scriptBase.m_minHp;
            m_maxHp = scriptBase.m_maxHp;
            m_maxCubes = scriptBase.m_maxCubes;
            m_aimColliderRadius = scriptBase.m_aimColliderRadius;

            m_deathExplosion = scriptBase.m_deathExplosion;
        }
        else
            Debug.Log("Warning: Tried to Copy MonsterEntityBase script from prefab, that didn't have it attached!");
        initializeValues();

        // other scripts
        MonsterEntityAbstractCopiable[] copyScripts = prefab.GetComponents<MonsterEntityAbstractCopiable>();
        foreach(MonsterEntityAbstractCopiable copyScript in copyScripts)
        {
            MonsterEntityAbstractCopiable pasteScript = gameObject.AddComponent<MonsterEntityAbstractCopiable>();
            pasteScript.pasteScript(copyScript);
        }

        // Monster Ejector
        MonsterEntityEjector scriptEjector = prefab.GetComponent<MonsterEntityEjector>();
        if (scriptEjector != null)
        {
            if (gameObject.GetComponent<MonsterEntityEjector>() == null)
            {
                MonsterEntityEjector thisEjectorScript = gameObject.AddComponent<MonsterEntityEjector>();
                m_ejectorScript = thisEjectorScript;
                thisEjectorScript.setValuesByScriptEjector(prefab);
            }
            else
                Debug.Log("Warning: Tried to add MonsterEntityEjector, but there was already one attached!");
        }
        // Monster Worm
        // ...

        // Attach System New
        AttachSystemBase[] scriptAttachSystemNew = prefab.GetComponents<AttachSystemBase>();
        if(scriptAttachSystemNew.Length == 1)
        {
            if (GetComponent<AttachSystemBase>() == null)
            {

            }
            else
                Debug.Log("Aborted: AttachSystemBase was already attached!");
        }

        // Attach System
        MonsterEntityAttachSystem scriptAttachSystem = prefab.GetComponent<MonsterEntityAttachSystem>();
        if (scriptAttachSystem != null)
        {
            if (gameObject.GetComponent<MonsterEntityAttachSystem>() == null)
            {
                MonsterEntityAttachSystem thisAttachSystemScript = gameObject.AddComponent<MonsterEntityAttachSystem>();
                m_attachSystemScript = thisAttachSystemScript;
                thisAttachSystemScript.setValuesByScript(prefab);
            }
            else
                Debug.Log("Warning: Tried to add MonsterEntityAttachSystem, but there was already one attached!");
        }

        // Attach
        // linked to scriptAttachSystem
        CubeEntityAttached attachedScript = prefab.GetComponent<CubeEntityAttached>();
        if (attachedScript != null)
        {
            if (gameObject.GetComponent<CubeEntityAttached>() == null)
            {
                CubeEntityAttached thisAttachedScript = GetComponent<CubeEntitySystem>().getStateComponent().addAttachedScript();
                m_attachedToScript = thisAttachedScript;
                thisAttachedScript.setValuesByScript(prefab, scriptAttachSystem);
                thisAttachedScript.m_attachedToGameObject = this.gameObject;
            }
            else
                Debug.Log("Warning: Tried to add MonsterEntityAttached, but there was already one attached!");
        }

        // Skill Grab
        MonsterEntitySkillGrab scriptSkillGrab = prefab.GetComponent<MonsterEntitySkillGrab>();
        if (scriptAttachSystem != null)
        {
            if (gameObject.GetComponent<MonsterEntitySkillGrab>() == null)
            {
                MonsterEntitySkillGrab thisSkillGrabScript = gameObject.AddComponent<MonsterEntitySkillGrab>();
                m_grabScript = thisSkillGrabScript;
                thisSkillGrabScript.setValuesByScript(prefab, this);
            }
            else
                Debug.Log("Warning: Tried to add MonsterEntitySkillGrab, but there was already one attached!");
        }

        // Skill Eject
        MonsterEntitySkillEject scriptSkillEject = prefab.GetComponent<MonsterEntitySkillEject>();
        if (scriptAttachSystem != null)
        {
            if (gameObject.GetComponent<MonsterEntitySkillEject>() == null)
            {
                MonsterEntitySkillEject thisSkillEjectScript = gameObject.AddComponent<MonsterEntitySkillEject>();
                m_ejectScript = thisSkillEjectScript;
                thisSkillEjectScript.setValuesByScript(prefab);
            }
            else
                Debug.Log("Warning: Tried to add MonsterEntitySkillGrab, but there was already one attached!");
        }

        

        // Death Effect
        MonsterEntityDeathEffect[] deathEffectScripts = prefab.GetComponents<MonsterEntityDeathEffect>();
        for(int i = 0; i < deathEffectScripts.Length; i++)
        {
            if (gameObject.GetComponent<MonsterEntityDeathEffect>() == null)
            {
                MonsterEntityDeathEffect thisDeathEffectScript = gameObject.AddComponent(deathEffectScripts[i].GetType()) as MonsterEntityDeathEffect;
                thisDeathEffectScript.setValues(prefab, this);
            }
            else
                Debug.Log("Warning: attached Death");
        }

        // Assign scripts to each other
        if (m_ejectScript != null)
        {
            m_ejectScript.m_attachSystem = m_attachSystemScript;
            m_ejectScript.m_base = this;
        }
    }
    // Hp
    public void loseHp(int lifeLoss, GameObject cube)
    {
        m_currentHp -= lifeLoss;
        //m_currentMaxCubes = Mathf.Min(m_currentHp, m_maxCubes);

        //if (cube != null)
           // m_attachSystemScript.deregisterCube(cube);

        manageHp();
    }

    public void changeMaxCubes(int cubeNumber)
    {
        m_currentMaxCubes += cubeNumber;
        m_currentMaxCubes = Mathf.Max(0, m_currentMaxCubes);
    }

    void manageHp()
    {
        //setSize();
        if(m_currentHp < m_minHp)
        {
            die();
        }
    }

    // Die
    public void die()
    {
        SoundManager.addSoundEjectorDeath(transform.position);

        foreach (LevelEntityMonsterManager script in m_registeredInManager)
        {
            script.deregisterEnemy(this.gameObject);
        }

        activateDeathEffect();

        GetComponent<CubeEntityState>().removeAttachedScript();

        if (m_attachSystemScript != null)
            m_attachSystemScript.destroyScript();
        m_attachSystemScript = null;

        Destroy(getMonsterScript());

        Destroy(m_ejectScript);
        m_ejectScript = null;

        Destroy(m_grabScript);
        m_grabScript = null;

        //Destroy(m_attachedToScript);


        gameObject.GetComponent<CubeEntitySystem>().setToInactive();

        
        Destroy(this);
    }
    public void deactivateEnemy(bool setToInactive)
    {
        foreach (LevelEntityMonsterManager script in m_registeredInManager)
        {
            script.deregisterEnemy(this.gameObject);
        }

        Destroy(getMonsterScript());
        if (m_attachSystemScript != null)
            m_attachSystemScript.destroyScript();
        Destroy(m_ejectScript);
        Destroy(m_grabScript);
        //Destroy(m_attachedToScript);
        removeCollider();
        Destroy(this);

        if(setToInactive)
            gameObject.GetComponent<CubeEntitySystem>().setToInactive();
    }
    public void activateDeathEffect()
    {
        MonsterEntityDeathEffect[] deathEffects = gameObject.GetComponents<MonsterEntityDeathEffect>();
        for (int i = 0; i < deathEffects.Length; i++)
            deathEffects[i].activateDeathEffect();
        //return;
        if (m_deathExplosion != null)
        {
            GameObject explosion = Instantiate(m_deathExplosion);
            explosion.transform.position = transform.position;
        }
    }

    // Collider
    void applyCollider()
    {
        m_collider = gameObject.AddComponent<SphereCollider>();
        m_collider.radius = m_aimColliderRadius;
        m_collider.isTrigger = true;
    }
    void removeCollider()
    {
        Destroy(m_collider);
        m_collider = null;
    }

    // Getter Type
    public MonsterEntityBase getMonsterScript()
    {
        if (m_ejectorScript != null)
            return m_ejectorScript;
        else if (m_wormScript != null)
            return m_wormScript;
        else
            return null;
    }

    // Intern
    public void initializeValues()
    {
        m_currentHp = m_maxHp;
        m_currentMaxCubes = Mathf.Min(m_maxCubes);
        m_registeredInManager = new List<LevelEntityMonsterManager>();
    }
    void setSize()
    {
        float size = Mathf.Max(5f, m_size * ((float)m_currentHp / (float)m_maxHp));
        transform.localScale = new Vector3(size, size, size);
        //m_attachSystemScript.
    }

    public void doStuff()
    {
        Destroy(null);
    }
}
