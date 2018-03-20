using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterEntityMorpher : MonsterEntityAbstractBase
{

    public override void pasteScript(EntityCopiableAbstract baseScript)
    {

    }


    public override void prepareDestroyScript()
    {
        Constants.getMainCge().GetComponent<CgeMonsterManager>().deregisterEnemy(this);
        Destroy(this);
    }
    public override void assignScripts()
    {

    }
}
