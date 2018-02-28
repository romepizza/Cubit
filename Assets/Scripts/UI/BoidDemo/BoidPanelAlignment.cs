using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoidPanelAlignment : MonoBehaviour
{
    public GameObject m_useRule;

    public GameObject m_alignmentPerFrame;
    public GameObject m_alignmentMinPercentPerFrame;
    public GameObject m_alignmentPower;
    public GameObject m_alignmentRadius;
    public GameObject m_alignmentMaxPartners;
    public GameObject m_alignmentMaxPartnerChecks;

    public GameObject m_useAdjustRadius;
    public GameObject m_alignmentMinAdjustmentDifference;
    public GameObject m_alignmentMinRadius;
    public GameObject m_alignmentAdjustStep;

    public GameObject m_requireLineOfSight;

    public GameObject m_requireAngle;
    public GameObject m_maxAngle;

    bool boidFound = false;
    CemBoidRuleAlignment m_script;


    // Use this for initialization
    void Awake()
    {
        if (!boidFound)
        {
            m_script = Constants.getBoidSystem().GetComponent<CemBoidRuleAlignment>();
            if (m_script == false)
                Debug.Log("Warning: Rule Alignment could not be found!");
            boidFound = true;
            updateInfo();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!boidFound)
        {
            m_script = Constants.getBoidSystem().GetComponent<CemBoidRuleAlignment>();
            if (m_script == false)
                Debug.Log("Warning: Rule Alignment could not be found!");
            boidFound = true;
            updateInfo();
        }
    }

    public void updateInfo()
    {
        m_useRule.GetComponent<Toggle>().isOn = m_script.m_useRule;

        m_alignmentPerFrame.GetComponent<InputField>().text = m_script.m_alignmentPerFrame.ToString();
        m_alignmentMinPercentPerFrame.GetComponent<InputField>().text = m_script.m_alignmentMinPercentPerFrame.ToString();
        m_alignmentPower.GetComponent<InputField>().text = m_script.m_alignmentPower.ToString();
        m_alignmentRadius.GetComponent<InputField>().text = m_script.m_alignmentRadius.ToString();
        m_alignmentMaxPartners.GetComponent<InputField>().text = m_script.m_alignmentMaxPartners.ToString();
        m_alignmentMaxPartnerChecks.GetComponent<InputField>().text = m_script.m_alignmentMaxPartnerChecks.ToString();

        m_useAdjustRadius.GetComponent<Toggle>().isOn = m_script.m_useAdjustRadius;
        m_alignmentMinAdjustmentDifference.GetComponent<InputField>().text = m_script.m_alignmentMinAdjustmentDifference.ToString();
        m_alignmentMinRadius.GetComponent<InputField>().text = m_script.m_alignmentMinRadius.ToString();
        m_alignmentAdjustStep.GetComponent<InputField>().text = m_script.m_alignmentAdjustStep.ToString();

        m_requireLineOfSight.GetComponent<Toggle>().isOn = m_script.m_requireLineOfSight;

        m_requireAngle.GetComponent<Toggle>().isOn = m_script.m_requireAngle;
        m_maxAngle.GetComponent<InputField>().text = m_script.m_maxAngle.ToString();
    }

    public void updateUseRule()
    {
        bool active = m_useRule.GetComponent<Toggle>().isOn;
        m_script.m_useRule = active;
    }

    public void updateAlignmentsPerFrame()
    {
        string input = m_alignmentPerFrame.GetComponent<InputField>().text;
        int output;
        if (int.TryParse(input, out output))
            m_script.m_alignmentPerFrame = output;
        else
            Debug.Log("Aborted: Parsing error!");
    }
    public void updateAlignmentMinPercentPerFrame()
    {
        string input = m_alignmentMinPercentPerFrame.GetComponent<InputField>().text;
        float output;
        if (float.TryParse(input, out output))
            m_script.m_alignmentMinPercentPerFrame = output;
        else
            Debug.Log("Aborted: Parsing error!");
    }
    public void updateAlignmentPower()
    {
        string input = m_alignmentPower.GetComponent<InputField>().text;
        float output;
        if (float.TryParse(input, out output))
            m_script.m_alignmentPower = output;
        else
            Debug.Log("Aborted: Parsing error!");
    }
    public void updateAlignmentRadius()
    {
        string input = m_alignmentRadius.GetComponent<InputField>().text;
        float output;
        if (float.TryParse(input, out output))
        {
            m_script.m_alignmentRadius = output;
            m_script.resetRadii();
        }
        else
            Debug.Log("Aborted: Parsing error!");
    }
    public void updateAlignmentMaxPartners()
    {
        string input = m_alignmentMaxPartners.GetComponent<InputField>().text;
        int output;
        if (int.TryParse(input, out output))
            m_script.m_alignmentMaxPartners = output;
        else
            Debug.Log("Aborted: Parsing error!");
    }
    public void updateAlignmentMaxPartnerChecks()
    {
        string input = m_alignmentMaxPartnerChecks.GetComponent<InputField>().text;
        int output;
        if (int.TryParse(input, out output))
            m_script.m_alignmentMaxPartnerChecks = output;
        else
            Debug.Log("Aborted: Parsing error!");
    }

    public void updateAlignmentUseAdjustmentRadius()
    {
        bool active = m_useAdjustRadius.GetComponent<Toggle>().isOn;
        m_script.m_useAdjustRadius = active;
        m_script.resetRadii();
    }
    public void updateAlignmentMinAdjustmentDifference()
    {
        string input = m_alignmentMinAdjustmentDifference.GetComponent<InputField>().text;
        int output;
        if (int.TryParse(input, out output))
            m_script.m_alignmentMinAdjustmentDifference = output;
        else
            Debug.Log("Aborted: Parsing error!");
    }
    public void updateAlignmentMinRadius()
    {
        string input = m_alignmentMinRadius.GetComponent<InputField>().text;
        float output;
        if (float.TryParse(input, out output))
            m_script.m_alignmentMinRadius = output;
        else
            Debug.Log("Aborted: Parsing error!");
    }
    public void updateAlignmentAdjustStep()
    {
        string input = m_alignmentAdjustStep.GetComponent<InputField>().text;
        float output;
        if (float.TryParse(input, out output))
            m_script.m_alignmentAdjustStep = output;
        else
            Debug.Log("Aborted: Parsing error!");
    }

    public void updateAlignmentRequireLineOfSight()
    {
        bool active = m_requireLineOfSight.GetComponent<Toggle>().isOn;
        m_script.m_requireLineOfSight = active;
    }

    public void updateAlignmentRequireAngle()
    {
        bool active = m_requireAngle.GetComponent<Toggle>().isOn;
        m_script.m_requireAngle = active;
    }
    public void updateAlignmentMaxAngle()
    {
        string input = m_maxAngle.GetComponent<InputField>().text;
        float output;
        if (float.TryParse(input, out output))
            m_script.m_maxAngle = output;
        else
            Debug.Log("Aborted: Parsing error!");
    }
}
