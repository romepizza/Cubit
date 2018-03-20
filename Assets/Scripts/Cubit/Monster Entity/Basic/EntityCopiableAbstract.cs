using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntityCopiableAbstract : MonoBehaviour
{
    public abstract void pasteScript(EntityCopiableAbstract baseScript);
    public abstract void prepareDestroyScript();
    public abstract void assignScripts();
    //public abstract void postInitialisation();

}
