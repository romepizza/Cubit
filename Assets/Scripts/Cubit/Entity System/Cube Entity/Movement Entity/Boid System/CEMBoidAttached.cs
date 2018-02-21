using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CEMBoidAttached : MonoBehaviour
{
    public CEMBoidSystem m_isAttachedToNormal;
    public CemBoidBase m_isAttachedToBase;
    public CEMBoidSystem m_isPredatorToNormal;
    //public CemBoidRulePredator m_isPredatorToBase;
    public List<CemBoidRuleBase> m_predatorBaseScripts;

    [Header("- (Cohesion) -")]
    [Header("- (Alignment) -")]
    [Header("- (Separation) -")]
    bool b;

    void Awake()
    {
        m_predatorBaseScripts = new List<CemBoidRuleBase>();
    }
}
