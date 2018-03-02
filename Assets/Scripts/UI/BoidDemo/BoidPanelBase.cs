using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoidPanelBase : MonoBehaviour
{


    public GameObject m_setSawrmSize;

    public GameObject m_maxIndividualSpeed;
    public GameObject m_airResistancePower;
    public GameObject m_lookInFlightDirection;
    public GameObject m_lookInFlightDirectionPower;

    public GameObject m_affectedByPlayerMovementPower;

    public GameObject m_calculateInBaseScript;
    public GameObject m_calculateInFixedUpdate;
    public GameObject m_moveCamerWhileIdle;

    bool boidFound = false;
    CemBoidBase m_script;


    // Use this for initialization
    void Awake()
    {
        if (!boidFound)
        {
            m_script = Constants.getBoidSystem().GetComponent<CemBoidBase>();
            if (m_script == false)
                Debug.Log("Warning: Rule Base could not be found!");
            boidFound = true;
            updateInfo();
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (!boidFound)
        {
            m_script = Constants.getBoidSystem().GetComponent<CemBoidBase>();
            if (m_script == false)
                Debug.Log("Warning: Rule Base could not be found!");
            boidFound = true;
            updateInfo();
        }

    }

    public void updateInfo()
    {
        m_setSawrmSize.GetComponent<InputField>().text = m_script.m_agents.Count.ToString();
        m_maxIndividualSpeed.GetComponent<InputField>().text = m_script.m_maxIndividualSpeed.ToString();
        m_airResistancePower.GetComponent<InputField>().text = m_script.m_airResistancePower.ToString();
        m_lookInFlightDirection.GetComponent<Toggle>().isOn = m_script.m_lookInFlightDirection;
        m_lookInFlightDirectionPower.GetComponent<InputField>().text = m_script.m_lookInFlightDirectionPower.ToString();

        m_affectedByPlayerMovementPower.GetComponent<InputField>().text = m_script.m_affectedByplayerMovementPower.ToString();


        m_calculateInBaseScript.GetComponent<Toggle>().isOn = CemBoidBase.s_calculateInBase;
        m_calculateInFixedUpdate.GetComponent<Toggle>().isOn = CemBoidBase.s_calculateInFixedUpdate;
        //m_moveCamerWhileIdle.GetComponent<Toggle>().isOn = CemBoidBase.s_moveCameraWhileIdle;
    }
    public void updateBaseSwarmSize()
    {
        string input = m_setSawrmSize.GetComponent<InputField>().text;
        int output;
        if (int.TryParse(input, out output))
        {
            //m_script.setSwarmSize(output);
            m_setSawrmSize.GetComponent<InputField>().text = m_script.m_agents.Count.ToString();
        }
        else
            Debug.Log("Aborted: Parsing error!");
    }
    public void updateBaseIndividualMaxSpeed()
    {
        string input = m_maxIndividualSpeed.GetComponent<InputField>().text;
        float output;
        if (float.TryParse(input, out output))
            m_script.m_maxIndividualSpeed = output;
        else
            Debug.Log("Aborted: Parsing error!");
    }
    public void updateBaseAirResistancePower()
    {
        string input = m_airResistancePower.GetComponent<InputField>().text;
        float output;
        if (float.TryParse(input, out output))
            m_script.m_airResistancePower = output;
        else
            Debug.Log("Aborted: Parsing error!");
    }
    public void updateUseLookInFlightDirection()
    {
        bool active = m_lookInFlightDirection.GetComponent<Toggle>().isOn;
        m_script.m_lookInFlightDirection = active;
    }
    public void updateLookInFlightDirectionPower()
    {
        string input = m_lookInFlightDirectionPower.GetComponent<InputField>().text;
        float output;
        if (float.TryParse(input, out output))
            m_script.m_lookInFlightDirectionPower = output;
        else
            Debug.Log("Aborted: Parsing error!");
    }
    public void updateAffectedByPlayerMovementPower()
    {
        string input = m_affectedByPlayerMovementPower.GetComponent<InputField>().text;
        float output;
        if (float.TryParse(input, out output))
            m_script.m_affectedByplayerMovementPower = output;
        else
            Debug.Log("Aborted: Parsing error!");
    }
    public void updateCalculateInBaseScript()
    {
        bool active = m_calculateInBaseScript.GetComponent<Toggle>().isOn;
        CemBoidBase.s_calculateInBase = active;
    }
    public void updateCalculateInFixedUpdate()
    {
        bool active = m_calculateInFixedUpdate.GetComponent<Toggle>().isOn;
        CemBoidBase.s_calculateInFixedUpdate = active;
    }
    public void updateMoveCameraWhileIdle()
    {
        bool active = m_moveCamerWhileIdle.GetComponent<Toggle>().isOn;
        //CemBoidBase.s_moveCameraWhileIdle = active;
    }
}
