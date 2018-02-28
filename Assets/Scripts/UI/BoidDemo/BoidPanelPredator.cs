using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoidPanelPredator : MonoBehaviour
{

    public GameObject m_useRule;

    public GameObject m_predatorPower;
    public GameObject m_predatorRadius;
    public GameObject m_predatorMaxPartners;
    public GameObject m_predatorMaxPartnerChecks;

    //public GameObject m_predatorFillPredatorsOnAdd; //
    public GameObject m_predatorHighlightPredators; //
    public GameObject m_predatorSetNumber; //

    public GameObject m_useAdjustRadius;
    public GameObject m_predatorMinAdjustmentDifference;
    public GameObject m_predatorMinRadius;
    public GameObject m_predatorAdjustStep;

    public GameObject m_requireLineOfSight;

    public GameObject m_requireAngle;
    public GameObject m_maxAngle;

    public GameObject m_predatorPlayerIsPredator;

    bool boidFound = false;
    CemBoidRulePredator m_script;


    // Use this for initialization
    void Awake()
    {
        if (!boidFound)
        {
            m_script = Constants.getBoidSystem().GetComponent<CemBoidRulePredator>();
            if (m_script == false)
                Debug.Log("Warning: Rule Predator could not be found!");
            boidFound = true;
            updateInfo();
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (!boidFound)
        {
            m_script = Constants.getBoidSystem().GetComponent<CemBoidRulePredator>();
            if (m_script == false)
                Debug.Log("Warning: Rule Predator could not be found!");
            boidFound = true;
            updateInfo();
        }

    }

    public void updateInfo()
    {
        m_useRule.GetComponent<Toggle>().isOn = m_script.m_useRule;

        m_predatorPower.GetComponent<InputField>().text = m_script.m_predatorPower.ToString();
        m_predatorRadius.GetComponent<InputField>().text = m_script.m_predatorRadius.ToString();
        m_predatorMaxPartners.GetComponent<InputField>().text = m_script.m_predatorMaxPartners.ToString();
        m_predatorMaxPartnerChecks.GetComponent<InputField>().text = m_script.m_predatorMaxPartnerChecks.ToString();

        m_predatorHighlightPredators.GetComponent<Toggle>().isOn = m_script.m_predatorsHighlightPredators;
        m_predatorSetNumber.GetComponent<InputField>().text = m_script.getNumberPredators().ToString();

        m_useAdjustRadius.GetComponent<Toggle>().isOn = m_script.m_useAdjustRadius;
        m_predatorMinAdjustmentDifference.GetComponent<InputField>().text = m_script.m_predatorMinAdjustmentDifference.ToString();
        m_predatorMinRadius.GetComponent<InputField>().text = m_script.m_predatorMinRadius.ToString();
        m_predatorAdjustStep.GetComponent<InputField>().text = m_script.m_predatorAdjustStep.ToString();

        m_requireLineOfSight.GetComponent<Toggle>().isOn = m_script.m_requireLineOfSight;

        m_requireAngle.GetComponent<Toggle>().isOn = m_script.m_requireAngle;
        m_maxAngle.GetComponent<InputField>().text = m_script.m_maxAngle.ToString();

        m_predatorPlayerIsPredator.GetComponent<Toggle>().isOn = m_script.m_predatorPlayerIsPredator;
    }

    public void updateUseRule()
    {
        bool active = m_useRule.GetComponent<Toggle>().isOn;
        m_script.m_useRule = active;
    }

    public void updatePredatorPower()
    {
        string input = m_predatorPower.GetComponent<InputField>().text;
        float output;
        if (float.TryParse(input, out output))
            m_script.m_predatorPower = output;
        else
            Debug.Log("Aborted: Parsing error!");
    }
    public void updatePredatorRadius()
    {
        string input = m_predatorRadius.GetComponent<InputField>().text;
        float output;
        if (float.TryParse(input, out output))
        {
            m_script.m_predatorRadius = output;
            m_script.resetRadii();
        }
        else
            Debug.Log("Aborted: Parsing error!");
    }
    public void updatePredatorMaxPartners()
    {
        string input = m_predatorMaxPartners.GetComponent<InputField>().text;
        int output;
        if (int.TryParse(input, out output))
            m_script.m_predatorMaxPartners = output;
        else
            Debug.Log("Aborted: Parsing error!");
    }
    public void updatePredatorMaxPartnerChecks()
    {
        string input = m_predatorMaxPartnerChecks.GetComponent<InputField>().text;
        int output;
        if (int.TryParse(input, out output))
            m_script.m_predatorMaxPartnerChecks = output;
        else
            Debug.Log("Aborted: Parsing error!");
    }

    public void updatePredatorHighlighted()
    {
        bool active = m_predatorHighlightPredators.GetComponent<Toggle>().isOn;
        //m_script.m_predatorsHighlightPredators = active;
        m_script.setPredatorHighlight(active);
    }
    public void updatePredatorSetNumbers()
    {
        string input = m_predatorSetNumber.GetComponent<InputField>().text;
        int output;
        if (int.TryParse(input, out output))
        {
            m_script.setNumberOfPredators(output);
            m_predatorSetNumber.GetComponent<InputField>().text = m_script.m_predatorPredators.Count.ToString();
        }
        else
            Debug.Log("Aborted: Parsing error!");
    }

    public void updatePredatorUseAdjustmentRadius()
    {
        bool active = m_useAdjustRadius.GetComponent<Toggle>().isOn;
        m_script.m_useAdjustRadius = active;
        m_script.resetRadii();
    }
    public void updatePredatorMinAdjustmentDifference()
    {
        string input = m_predatorMinAdjustmentDifference.GetComponent<InputField>().text;
        int output;
        if (int.TryParse(input, out output))
            m_script.m_predatorMinAdjustmentDifference = output;
        else
            Debug.Log("Aborted: Parsing error!");
    }
    public void updatePredatorMinRadius()
    {
        string input = m_predatorMinRadius.GetComponent<InputField>().text;
        float output;
        if (float.TryParse(input, out output))
            m_script.m_predatorMinRadius = output;
        else
            Debug.Log("Aborted: Parsing error!");
    }
    public void updatePredatorAdjustStep()
    {
        string input = m_predatorAdjustStep.GetComponent<InputField>().text;
        float output;
        if (float.TryParse(input, out output))
            m_script.m_predatorAdjustStep = output;
        else
            Debug.Log("Aborted: Parsing error!");
    }

    public void updatePredatorRequireLineOfSight()
    {
        bool active = m_requireLineOfSight.GetComponent<Toggle>().isOn;
        m_script.m_requireLineOfSight = active;
    }

    public void updatePredatorRequireAngle()
    {
        bool active = m_requireAngle.GetComponent<Toggle>().isOn;
        m_script.m_requireAngle = active;
    }
    public void updatePredatorMaxAngle()
    {
        string input = m_maxAngle.GetComponent<InputField>().text;
        float output;
        if (float.TryParse(input, out output))
            m_script.m_maxAngle = output;
        else
            Debug.Log("Aborted: Parsing error!");
    }

    public void updatePredatorPlayerIsPredator()
    {
        bool active = m_predatorPlayerIsPredator.GetComponent<Toggle>().isOn;
        m_script.setPlayerAsPredator(active);
    }
}
