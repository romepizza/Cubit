using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CemBoidAttached : MonoBehaviour
{
    public CemBoidBase m_isAttachedToBase;
    public List<CemBoidRuleBase> m_predatorBaseScripts;


    void Awake()
    {
        m_predatorBaseScripts = new List<CemBoidRuleBase>();
    }

    public void removeFromSwarm()
    {
        if(m_isAttachedToBase == null)
        {
            Debug.Log("Aborted: swarmScript was null!");
            return;
        }

        m_isAttachedToBase.removeAgent(gameObject);

        if (m_predatorBaseScripts.Count == 0)
            Destroy(this);
    }
}
