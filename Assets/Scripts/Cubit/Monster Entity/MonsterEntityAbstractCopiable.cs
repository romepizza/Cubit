using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonsterEntityAbstractCopiable : MonoBehaviour
{
    //[Header("------- Abstract Copiable -------")]

    public abstract void pasteScript(MonsterEntityAbstractCopiable baseScript);
}
