using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoidPanelCohesion : MonoBehaviour
{
    public GameObject m_useRule;

    public GameObject m_cohesionPerFrame;
    public GameObject m_cohesionPower;
    public GameObject m_cohesionRadius;
    public GameObject m_cohesionMaxPartners;
    public GameObject m_cohesionMaxPartnerChecks;

    public GameObject m_useAdjustRadius;
    public GameObject m_cohesionMinAdjustmentDifference;
    public GameObject m_cohesionMinRadius;
    public GameObject m_cohesionAdjustStep;

    public GameObject m_requireLineOfSight;

    public GameObject m_requireAngle;
    public GameObject m_maxAngle;

    bool boidFound = false;
    CemBoidRuleCohesion m_script;


    // Use this for initialization
    void Awake()
    {
        if (!boidFound)
        {
            m_script = Constants.getBoidSystem().GetComponent<CemBoidRuleCohesion>();
            if (m_script == false)
                Debug.Log("Warning: Rule Cohesion could not be found!");
            boidFound = true;
            updateInfo();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        if(!boidFound)
        {
            m_script = Constants.getBoidSystem().GetComponent<CemBoidRuleCohesion>();
            if (m_script == false)
                Debug.Log("Warning: Rule Cohesion could not be found!");
            boidFound = true;
            updateInfo();
        }
        
    }

    public void updateInfo()
    {
        m_useRule.GetComponent<Toggle>().isOn = m_script.m_useRule;

        m_cohesionPerFrame.GetComponent<InputField>().text = m_script.m_cohesionPerFrame.ToString();
        m_cohesionPower.GetComponent<InputField>().text = m_script.m_cohesionPower.ToString();
        m_cohesionRadius.GetComponent<InputField>().text = m_script.m_cohesionRadius.ToString();
        m_cohesionMaxPartners.GetComponent<InputField>().text = m_script.m_cohesionMaxPartners.ToString();
        m_cohesionMaxPartnerChecks.GetComponent<InputField>().text = m_script.m_cohesionMaxPartnerChecks.ToString();

        m_useAdjustRadius.GetComponent<Toggle>().isOn = m_script.m_useAdjustRadius;
        m_cohesionMinAdjustmentDifference.GetComponent<InputField>().text = m_script.m_cohesionMinAdjustmentDifference.ToString();
        m_cohesionMinRadius.GetComponent<InputField>().text = m_script.m_cohesionMinRadius.ToString();
        m_cohesionAdjustStep.GetComponent<InputField>().text = m_script.m_cohesionAdjustStep.ToString();

        m_requireLineOfSight.GetComponent<Toggle>().isOn = m_script.m_requireLineOfSight;

        m_requireAngle.GetComponent<Toggle>().isOn = m_script.m_requireAngle;
        m_maxAngle.GetComponent<InputField>().text = m_script.m_maxAngle.ToString();
    }

    public void updateUseRule()
    {
        bool active = m_useRule.GetComponent<Toggle>().isOn;
        m_script.m_useRule = active;
    }

    public void updateCohesionsPerFrame()
    {
        string input = m_cohesionPerFrame.GetComponent<InputField>().text;
        int output;
        if (int.TryParse(input, out output))
            m_script.m_cohesionPerFrame = output;
        else
            Debug.Log("Aborted: Parsing error!");
    }
    public void updateCohesionPower()
    {
        string input = m_cohesionPower.GetComponent<InputField>().text;
        float output;
        if (float.TryParse(input, out output))
            m_script.m_cohesionPower = output;
        else
            Debug.Log("Aborted: Parsing error!");
    }
    public void updateCohesionRadius()
    {
        string input = m_cohesionRadius.GetComponent<InputField>().text;
        float output;
        if (float.TryParse(input, out output))
            m_script.m_cohesionRadius = output;
        else
            Debug.Log("Aborted: Parsing error!");
    }
    public void updateCohesionMaxPartners()
    {
        string input = m_cohesionMaxPartners.GetComponent<InputField>().text;
        int output;
        if (int.TryParse(input, out output))
            m_script.m_cohesionMaxPartners = output;
        else
            Debug.Log("Aborted: Parsing error!");
    }
    public void updateCohesionMaxPartnerChecks()
    {
        string input = m_cohesionMaxPartnerChecks.GetComponent<InputField>().text;
        int output;
        if (int.TryParse(input, out output))
            m_script.m_cohesionMaxPartnerChecks = output;
        else
            Debug.Log("Aborted: Parsing error!");
    }

    public void updateCohesionUseAdjustmentRadius()
    {
        bool active = m_useAdjustRadius.GetComponent<Toggle>().isOn;
        m_script.m_useAdjustRadius = active;
        m_script.resetRadii();
    }
    public void updateCohesionMinAdjustmentDifference()
    {
        string input = m_cohesionMinAdjustmentDifference.GetComponent<InputField>().text;
        int output;
        if (int.TryParse(input, out output))
            m_script.m_cohesionMinAdjustmentDifference = output;
        else
            Debug.Log("Aborted: Parsing error!");
    }
    public void updateCohesionMinRadius()
    {
        string input = m_cohesionMinRadius.GetComponent<InputField>().text;
        float output;
        if (float.TryParse(input, out output))
            m_script.m_cohesionMinRadius = output;
        else
            Debug.Log("Aborted: Parsing error!");
    }
    public void updateCohesionAdjustStep()
    {
        string input = m_cohesionAdjustStep.GetComponent<InputField>().text;
        float output;
        if (float.TryParse(input, out output))
            m_script.m_cohesionAdjustStep = output;
        else
            Debug.Log("Aborted: Parsing error!");
    }

    public void updateCohesionRequireLineOfSight()
    {
        bool active = m_requireLineOfSight.GetComponent<Toggle>().isOn;
        m_script.m_requireLineOfSight = active;
    }

    public void updateCohesionRequireAngle()
    {
        bool active = m_requireAngle.GetComponent<Toggle>().isOn;
        m_script.m_requireAngle = active;
    }
    public void updateCohesionMaxAngle()
    {
        string input = m_maxAngle.GetComponent<InputField>().text;
        float output;
        if (float.TryParse(input, out output))
            m_script.m_maxAngle = output;
        else
            Debug.Log("Aborted: Parsing error!");
    }
}
