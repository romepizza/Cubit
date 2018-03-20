using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterEntityWorm : MonsterEntityAbstractBase
{
    [Header("----- SETTINGS -----")]
    [Header("----- DEBUG -----")]
    [Header("--- (Scripts) ---")]
    public bool placeHolder;
    // Use this for initialization
   
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
