using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoidPanelSwarmMovement : MonoBehaviour
{
    public GameObject m_useRule;
    public GameObject m_swarmMovementPower;

    bool boidFound = false;
    CemBoidRuleSwarmMovement m_script;


    // Use this for initialization
    void Awake()
    {
        if (!boidFound)
        {
            m_script = Constants.getBoidSystem().GetComponent<CemBoidRuleSwarmMovement>();
            if (m_script == false)
                Debug.Log("Warning: Rule SwarmMovement could not be found!");
            boidFound = true;
            updateInfo();
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (!boidFound)
        {
            m_script = Constants.getBoidSystem().GetComponent<CemBoidRuleSwarmMovement>();
            if (m_script == false)
                Debug.Log("Warning: Rule SwarmMovement could not be found!");
            boidFound = true;
            updateInfo();
        }

    }

    public void updateInfo()
    {
        m_useRule.GetComponent<Toggle>().isOn = m_script.m_useRule;
        
        m_swarmMovementPower.GetComponent<InputField>().text = m_script.m_swarmMovementPower.ToString();
    }

    public void updateUseRule()
    {
        bool active = m_useRule.GetComponent<Toggle>().isOn;
        m_script.m_useRule = active;
    }

    public void updateSwarmMovementPower()
    {
        string input = m_swarmMovementPower.GetComponent<InputField>().text;
        float output;
        if (float.TryParse(input, out output))
            m_script.m_swarmMovementPower = output;
        else
            Debug.Log("Aborted: Parsing error!");
    }
}
