using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CubeEntityMovementAbstract : EntityCopiableAbstract
{
    public float m_maxSpeed;
    public GameObject m_target;
    public bool m_useThis = true;
    // abstract
    public abstract void pasteScript(EntityCopiableAbstract baseScript, GameObject target, Vector3 targetPosition);

}
