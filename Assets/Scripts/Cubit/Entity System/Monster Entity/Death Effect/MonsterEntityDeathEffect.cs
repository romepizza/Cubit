using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonsterEntityDeathEffect : MonoBehaviour {

    public abstract void setValues(GameObject prefab, MonsterEntityBase baseScript);
    public abstract void activateDeathEffect();
}
