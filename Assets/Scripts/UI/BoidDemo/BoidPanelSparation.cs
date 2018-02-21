using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoidPanelSparation : MonoBehaviour
{

    public GameObject m_useRule;

    public GameObject m_separationPerFrame;
    public GameObject m_separationPower;
    public GameObject m_separationRadius;
    public GameObject m_separationMaxPartners;
    public GameObject m_separationMaxPartnerChecks;

    public GameObject m_useAdjustRadius;
    public GameObject m_separationMinAdjustmentDifference;
    public GameObject m_separationMinRadius;
    public GameObject m_separationAdjustStep;

    public GameObject m_requireLineOfSight;

    public GameObject m_requireAngle;
    public GameObject m_maxAngle;

    bool boidFound = false;
    CemBoidRuleSeparation m_script;


    // Use this for initialization
    void Awake()
    {
        if (!boidFound)
        {
            m_script = Constants.getBoidSystem().GetComponent<CemBoidRuleSeparation>();
            if (m_script == false)
                Debug.Log("Warning: Rule Separation could not be found!");
            boidFound = true;
            updateInfo();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!boidFound)
        {
            m_script = Constants.getBoidSystem().GetComponent<CemBoidRuleSeparation>();
            if (m_script == false)
                Debug.Log("Warning: Rule Separation could not be found!");
            boidFound = true;
            updateInfo();
        }
    }

    public void updateInfo()
    {
        m_useRule.GetComponent<Toggle>().isOn = m_script.m_useRule;

        m_separationPerFrame.GetComponent<InputField>().text = m_script.m_separationPerFrame.ToString();
        m_separationPower.GetComponent<InputField>().text = m_script.m_separationPower.ToString();
        m_separationRadius.GetComponent<InputField>().text = m_script.m_separationRadius.ToString();
        m_separationMaxPartners.GetComponent<InputField>().text = m_script.m_separationMaxPartners.ToString();
        m_separationMaxPartnerChecks.GetComponent<InputField>().text = m_script.m_separationMaxPartnerChecks.ToString();

        m_useAdjustRadius.GetComponent<Toggle>().isOn = m_script.m_useAdjustRadius;
        m_separationMinAdjustmentDifference.GetComponent<InputField>().text = m_script.m_separationMinAdjustmentDifference.ToString();
        m_separationMinRadius.GetComponent<InputField>().text = m_script.m_separationMinRadius.ToString();
        m_separationAdjustStep.GetComponent<InputField>().text = m_script.m_separationAdjustStep.ToString();

        m_requireLineOfSight.GetComponent<Toggle>().isOn = m_script.m_requireLineOfSight;

        m_requireAngle.GetComponent<Toggle>().isOn = m_script.m_requireAngle;
        m_maxAngle.GetComponent<InputField>().text = m_script.m_maxAngle.ToString();
    }

    public void updateUseRule()
    {
        bool active = m_useRule.GetComponent<Toggle>().isOn;
        m_script.m_useRule = active;
    }

    public void updateSeparationsPerFrame()
    {
        string input = m_separationPerFrame.GetComponent<InputField>().text;
        int output;
        if (int.TryParse(input, out output))
            m_script.m_separationPerFrame = output;
        else
            Debug.Log("Aborted: Parsing error!");
    }
    public void updateSeparationPower()
    {
        string input = m_separationPower.GetComponent<InputField>().text;
        float output;
        if (float.TryParse(input, out output))
            m_script.m_separationPower = output;
        else
            Debug.Log("Aborted: Parsing error!");
    }
    public void updateSeparationRadius()
    {
        string input = m_separationRadius.GetComponent<InputField>().text;
        float output;
        if (float.TryParse(input, out output))
            m_script.m_separationRadius = output;
        else
            Debug.Log("Aborted: Parsing error!");
    }
    public void updateSeparationMaxPartners()
    {
        string input = m_separationMaxPartners.GetComponent<InputField>().text;
        int output;
        if (int.TryParse(input, out output))
            m_script.m_separationMaxPartners = output;
        else
            Debug.Log("Aborted: Parsing error!");
    }
    public void updateSeparationMaxPartnerChecks()
    {
        string input = m_separationMaxPartnerChecks.GetComponent<InputField>().text;
        int output;
        if (int.TryParse(input, out output))
            m_script.m_separationMaxPartnerChecks = output;
        else
            Debug.Log("Aborted: Parsing error!");
    }

    public void updateSeparationUseAdjustmentRadius()
    {
        bool active = m_useAdjustRadius.GetComponent<Toggle>().isOn;
        m_script.m_useAdjustRadius = active;
        m_script.resetRadii();
    }
    public void updateSeparationMinAdjustmentDifference()
    {
        string input = m_separationMinAdjustmentDifference.GetComponent<InputField>().text;
        int output;
        if (int.TryParse(input, out output))
            m_script.m_separationMinAdjustmentDifference = output;
        else
            Debug.Log("Aborted: Parsing error!");
    }
    public void updateSeparationMinRadius()
    {
        string input = m_separationMinRadius.GetComponent<InputField>().text;
        float output;
        if (float.TryParse(input, out output))
            m_script.m_separationMinRadius = output;
        else
            Debug.Log("Aborted: Parsing error!");
    }
    public void updateSeparationAdjustStep()
    {
        string input = m_separationAdjustStep.GetComponent<InputField>().text;
        float output;
        if (float.TryParse(input, out output))
            m_script.m_separationAdjustStep = output;
        else
            Debug.Log("Aborted: Parsing error!");
    }

    public void updateSeparationRequireLineOfSight()
    {
        bool active = m_requireLineOfSight.GetComponent<Toggle>().isOn;
        m_script.m_requireLineOfSight = active;
    }

    public void updateSeparationRequireAngle()
    {
        bool active = m_requireAngle.GetComponent<Toggle>().isOn;
        m_script.m_requireAngle = active;
    }
    public void updateSeparationMaxAngle()
    {
        string input = m_maxAngle.GetComponent<InputField>().text;
        float output;
        if (float.TryParse(input, out output))
            m_script.m_maxAngle = output;
        else
            Debug.Log("Aborted: Parsing error!");
    }
}
