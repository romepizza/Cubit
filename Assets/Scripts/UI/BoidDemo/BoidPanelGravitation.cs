using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoidPanelGravitation : MonoBehaviour
{
    public GameObject m_useRule;

    public GameObject m_gravitationUseSwamCenter;
    public GameObject m_gravitationPerFrame;
    public GameObject m_gravitationPower;
    public GameObject m_gravitationMaxSpeed;

    bool boidFound = false;
    CemBoidRuleGraviation m_script;


    // Use this for initialization
    void Awake()
    {
        if (!boidFound)
        {
            m_script = Constants.getBoidSystem().GetComponent<CemBoidRuleGraviation>();
            if (m_script == false)
                Debug.Log("Warning: Rule Gravitation could not be found!");
            boidFound = true;
            updateInfo();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!boidFound)
        {
            m_script = Constants.getBoidSystem().GetComponent<CemBoidRuleGraviation>();
            if (m_script == false)
                Debug.Log("Warning: Rule Gravitation could not be found!");
            boidFound = true;
            updateInfo();
        }
    }

    public void updateInfo()
    {
        m_useRule.GetComponent<Toggle>().isOn = m_script.m_useRule;
        m_gravitationUseSwamCenter.GetComponent<Toggle>().isOn = m_script.m_gravitationUseSwamCenter;

        m_gravitationPerFrame.GetComponent<InputField>().text = m_script.m_gravitationPerFrame.ToString();
        m_gravitationPower.GetComponent<InputField>().text = m_script.m_gravitationPower.ToString();
        m_gravitationMaxSpeed.GetComponent<InputField>().text = m_script.m_gravitationMaxSpeed.ToString();
    }

    public void updateUseRule()
    {
        bool active = m_useRule.GetComponent<Toggle>().isOn;
        m_script.m_useRule = active;
    }
    public void updateUseSwamCenter()
    {
        bool active = m_gravitationUseSwamCenter.GetComponent<Toggle>().isOn;
        m_script.m_gravitationUseSwamCenter = active;
    }

    public void updateGravitationsPerFrame()
    {
        string input = m_gravitationPerFrame.GetComponent<InputField>().text;
        int output;
        if (int.TryParse(input, out output))
            m_script.m_gravitationPerFrame = output;
        else
            Debug.Log("Aborted: Parsing error!");
    }
    public void updateGravitationPower()
    {
        string input = m_gravitationPower.GetComponent<InputField>().text;
        float output;
        if (float.TryParse(input, out output))
            m_script.m_gravitationPower = output;
        else
            Debug.Log("Aborted: Parsing error!");
    }
    public void updateMaxSpeed()
    {
        string input = m_gravitationMaxSpeed.GetComponent<InputField>().text;
        float output;
        if (float.TryParse(input, out output))
            m_script.m_gravitationMaxSpeed = output;
        else
            Debug.Log("Aborted: Parsing error!");
    }
}
