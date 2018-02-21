using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoidPanelPrefab : MonoBehaviour
{
    [Header("--- (Dropdown) ---")]
    public GameObject[] m_boidPrefabs;
    public GameObject m_dropDownObject;

    [Header("--- (Input Fields) ---")]
    public GameObject m_prefabSwarmSize;
    public GameObject m_prefabNumberPredator;

    [Header("--- (Toggles) ---")]
    public GameObject m_prefabConsiderToggle;
    public GameObject m_resetPositionToggle;

    [Header("--- (Labels) ---")]
    public GameObject m_prefabRecommendedSwarmSize;
    public GameObject m_prefabRecommendedNumberPredators;
    public GameObject m_prefabPerformanceFactorLabel;

    
    /*
    public GameObject m_useRule;

    public GameObject m_gravitationUseSwamCenter;
    public GameObject m_gravitationPerFrame;
    public GameObject m_gravitationPower;
    public GameObject m_gravitationMaxSpeed;
    */
    [Header("--- (Debug) ---")]
    bool boidFound = false;
    CemBoidBase m_scriptBase;
    CemBoidRulePredator m_scriptPredator;
    public Dropdown m_dropDown;

    
    // Use this for initialization
    void Awake()
    {
        if (!boidFound)
        {
            m_dropDown = m_dropDownObject.GetComponent<Dropdown>();
            m_scriptBase = Constants.getBoidSystem();
            m_scriptPredator = m_scriptBase.GetComponent<CemBoidRulePredator>();
            if (m_scriptBase == null)
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
            m_scriptBase = Constants.getBoidSystem();
            m_scriptPredator = m_scriptBase.GetComponent<CemBoidRulePredator>();
            if (m_scriptBase == null)
                Debug.Log("Warning: Rule Gravitation could not be found!");
            boidFound = true;
            updateInfo();
        }
    }

    public void updateInfo()
    {
        m_prefabSwarmSize.GetComponent<InputField>().text = m_scriptBase.m_agents.Count.ToString();
        m_prefabNumberPredator.GetComponent<InputField>().text = m_scriptPredator.m_predatorPredators.Count.ToString();
        onDropDownChange();
        /*
        m_useRule.GetComponent<Toggle>().isOn = m_script.m_useRule;
        m_gravitationUseSwamCenter.GetComponent<Toggle>().isOn = m_script.m_gravitationUseSwamCenter;

        m_gravitationPerFrame.GetComponent<InputField>().text = m_script.m_gravitationPerFrame.ToString();
        m_gravitationPower.GetComponent<InputField>().text = m_script.m_gravitationPower.ToString();
        m_gravitationMaxSpeed.GetComponent<InputField>().text = m_script.m_gravitationMaxSpeed.ToString();
        */
    }

    // on UI
    public void prefabApplyPrefab()
    {
        CemBoidBase copyScript = getCurrentPrefab();

        int swarmSize = getSwarmSize();
        int numberPredators = getPredatorNumber();

        if (numberPredators > swarmSize)
            numberPredators = swarmSize;

        if (copyScript != null)
        {
            if (m_resetPositionToggle.GetComponent<Toggle>().isOn)
                resetPosition();
            m_scriptBase.setValues(copyScript, swarmSize, numberPredators);
            //m_dropDown.GetComponent<Dropdown>().value = 0;
        }
    }
    public void onDropDownChange()
    {
        updateRecommendation();
    }
    public void resetPosition()
    {
        foreach(GameObject agent in m_scriptBase.m_agents)
        {
            if (agent == m_scriptBase.m_leader)
                continue;
            Vector3 randomVector = m_scriptBase.m_leader.transform.position + Random.insideUnitSphere * 100f;
            agent.transform.position = randomVector;
            agent.transform.rotation = m_scriptBase.m_leader.transform.rotation;
            agent.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }

    // recommendation
    public void fillRecommendation()
    {
        BoidRecommendation script = getCurrentPrefab().gameObject.GetComponent<BoidRecommendation>();
        if (script == null)
        {
            Debug.Log("Aborted: recommendation script was null!");
            return;
        }
        if (script.m_swarmSizeRecommendation >= 0)
        {
            int actualSwarmSizeRec = script.m_swarmSizeRecommendation;
            if (actualSwarmSizeRec >= 2 && m_prefabConsiderToggle.GetComponent<Toggle>().isOn)
            {
                actualSwarmSizeRec = (int)((float)script.m_swarmSizeRecommendation * BoidRecommendation.s_performanceFactor + 0.1);
                if (actualSwarmSizeRec == 0 && script.m_swarmSizeRecommendation > 0)
                    actualSwarmSizeRec = 1;
            }

            m_prefabSwarmSize.GetComponent<InputField>().text = actualSwarmSizeRec.ToString();
        }
        if (script.m_numberPredatorRecommendation >= 0 )
        {
            int actualNumberPredatorRec = script.m_numberPredatorRecommendation;
            if (actualNumberPredatorRec >= 2 && m_prefabConsiderToggle.GetComponent<Toggle>().isOn)
            {
                actualNumberPredatorRec = (int)((float)script.m_numberPredatorRecommendation * BoidRecommendation.s_performanceFactor + 0.1);
                if (actualNumberPredatorRec == 0 && script.m_numberPredatorRecommendation > 0)
                    actualNumberPredatorRec = 1;
            }

            m_prefabNumberPredator.GetComponent<InputField>().text = actualNumberPredatorRec.ToString();
        }
    }
    public void updateRecommendation()
    {
        CemBoidBase copyScript = getCurrentPrefab();
        if (copyScript == null)
        {
            Debug.Log("Aborted: copy script was null!");
            return;
        }

        BoidRecommendation script = copyScript.gameObject.GetComponent<BoidRecommendation>();
        if(script == null)
        {
            Debug.Log("Aborted: recommendation script was null!");
            return;
        }
        
        // --- Swarm Size ---
        string swarmSizeString = "";
        
        // get actual swarm sizes
        int actualSwarmSizeMinRec = script.m_swarmSizeMinRecommendation;
        if (actualSwarmSizeMinRec >= 2 && m_prefabConsiderToggle.GetComponent<Toggle>().isOn)
        {
            actualSwarmSizeMinRec = (int)((float)script.m_swarmSizeMinRecommendation * BoidRecommendation.s_performanceFactor);
            if (actualSwarmSizeMinRec == 0 && script.m_swarmSizeMinRecommendation > 0)
                actualSwarmSizeMinRec = 1;
        }

        int actualSwarmSizeMaxRec = script.m_swarmSizeMaxRecommendation;
        if (actualSwarmSizeMaxRec >= 2 && m_prefabConsiderToggle.GetComponent<Toggle>().isOn)
        {
            actualSwarmSizeMaxRec = (int)((float)script.m_swarmSizeMaxRecommendation * BoidRecommendation.s_performanceFactor);
            if (actualSwarmSizeMaxRec == 0 && script.m_swarmSizeMaxRecommendation > 0)
                actualSwarmSizeMaxRec = 1;
        }

        // apply swarm sizes
        if (actualSwarmSizeMinRec < 0)
        {
            if (actualSwarmSizeMaxRec < 0)
                swarmSizeString = "(-)";
            else
                swarmSizeString = "(" + actualSwarmSizeMaxRec + ")";
        }
        else if(actualSwarmSizeMaxRec < 0)
        {
            swarmSizeString = "(" + actualSwarmSizeMinRec + ")";
        }
        else
        {
            swarmSizeString = "(" + actualSwarmSizeMinRec + "-" + actualSwarmSizeMaxRec + ")";
        }
        m_prefabRecommendedSwarmSize.GetComponent<Text>().text = swarmSizeString;


        // --- Number Predator ---
        string numberPredatorString = "";

        // get actual number predators
        int actualNumberPredatorMinRec = script.m_numberPredatorMinRecommendation;
        if (actualNumberPredatorMinRec >= 2 && m_prefabConsiderToggle.GetComponent<Toggle>().isOn)
        {
            actualNumberPredatorMinRec = (int)((float)script.m_numberPredatorMinRecommendation * BoidRecommendation.s_performanceFactor + 0.1);
            if (actualNumberPredatorMinRec == 0 && script.m_numberPredatorMinRecommendation > 0)
                actualNumberPredatorMinRec = 1;
        }

        int actualNumberPredatorMaxRec = script.m_numberPredatorRecommendation;
        if (actualNumberPredatorMaxRec >= 2 && m_prefabConsiderToggle.GetComponent<Toggle>().isOn)
        {
            actualNumberPredatorMaxRec = (int)((float)script.m_numberPredatorRecommendation * BoidRecommendation.s_performanceFactor + 0.1);
            if (actualNumberPredatorMaxRec == 0 && script.m_numberPredatorRecommendation > 0)
                actualNumberPredatorMaxRec = 1;
        }

        // apply number predators
        if (actualNumberPredatorMinRec < 0)
        {
            if (actualNumberPredatorMaxRec < 0)
                numberPredatorString = "(-)";
            else
                numberPredatorString = "(" + actualNumberPredatorMaxRec + ")";
        }
        else if (actualNumberPredatorMaxRec < 0)
        {
            numberPredatorString = "(" + actualNumberPredatorMinRec + ")";
        }
        else
        {
            numberPredatorString = "(" + actualNumberPredatorMinRec + "-" + actualNumberPredatorMaxRec + ")";
        }
        m_prefabRecommendedNumberPredators.GetComponent<Text>().text = numberPredatorString;
    }
    public void onChangeConsiderToggle()
    {
        if (m_prefabConsiderToggle.GetComponent<Toggle>().isOn)
        {
            BoidRecommendation.calculatePerformanceFactor();
            m_prefabPerformanceFactorLabel.GetComponent<Text>().text = "(" + BoidRecommendation.s_performanceFactor.ToString() + ")";
        }
        else
        {
            m_prefabPerformanceFactorLabel.GetComponent<Text>().text = "";
        }
        updateRecommendation();
    }
    // parse input fields
    int getSwarmSize()
    {
        string input = m_prefabSwarmSize.GetComponent<InputField>().text;
        int output;
        if (int.TryParse(input, out output))
            return output;
        else
            Debug.Log("Aborted: Parsing error!");
        return m_scriptBase.m_agents.Count;
    }
    int getPredatorNumber()
    {
        string input = m_prefabNumberPredator.GetComponent<InputField>().text;
        int output;
        if (int.TryParse(input, out output))
            return output;
        else
            Debug.Log("Aborted: Parsing error!");
        return m_scriptPredator.m_predatorPredators.Count;
    }

    // get current prefab
    CemBoidBase getCurrentPrefab()
    {
        int index = m_dropDown.value;
        if (m_dropDown.options.Count != m_boidPrefabs.Length)
        {
            Debug.Log("Aborted: index might be corrupted!");
            return null;
        }

        GameObject copyObject = m_boidPrefabs[index];
        if (copyObject == null)
        {
            Debug.Log("Aborted: copy object was null!");
            return null;
        }

        CemBoidBase copyScript = copyObject.GetComponent<CemBoidBase>();

        if (copyScript != null)
        {
            return copyScript;
        }
        else
            Debug.Log("Aborted: copy script was null!");

        return null;
    }
    /*
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
    */
}
