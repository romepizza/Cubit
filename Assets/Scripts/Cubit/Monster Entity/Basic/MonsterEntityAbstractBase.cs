using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterEntityAbstractBase : EntityCopiableAbstract
{

    // abstract
    public override void pasteScript(EntityCopiableAbstract baseScript)
    {

    }
    public override void prepareDestroyScript()
    {
        Destroy(this);
    }
    public override void assignScripts()
    {

    }
}
