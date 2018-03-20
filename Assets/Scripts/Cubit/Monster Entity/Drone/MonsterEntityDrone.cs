using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterEntityDrone : MonsterEntityAbstractBase
{
    public MonsterEntitySkillDrone m_droneSkillScript;

	// Update is called once per frame
	void Update ()
    {
		if(GetComponent<MonsterEntityBase>().m_target == null || GetComponent<MonsterEntityBase>().m_target.GetComponent<MonsterEntityBase>() == null)
        {
            GetComponent<MonsterEntityBase>().updateTarget();
        }
	}

    public override void pasteScript(EntityCopiableAbstract baseScript)
    {

    }

    public override void prepareDestroyScript()
    {
        if(m_droneSkillScript != null)
            m_droneSkillScript.destroyDrone(gameObject);
        Constants.getMainCge().GetComponent<CgeMonsterManager>().deregisterEnemy(this);
        Destroy(this);
    }
    public override void assignScripts()
    {

    }
}
