using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoidPanelBubble : MonoBehaviour
{

    public GameObject m_useRule;

    public GameObject m_bubbleUseSwamCenter;
    public GameObject m_bubblePerFrame;
    public GameObject m_bubbleMinPercentPerFrame;
    public GameObject m_bubblePower;
    public GameObject m_bubbleMaxDistance;
    public GameObject m_bubbleMaxSpeed;

    bool boidFound = false;
    CemBoidRuleBubble m_script;


    // Use this for initialization
    void Awake()
    {
        if (!boidFound)
        {
            m_script = Constants.getBoidSystem().GetComponent<CemBoidRuleBubble>();
            if (m_script == false)
                Debug.Log("Warning: Rule Bubble could not be found!");
            boidFound = true;
            updateInfo();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!boidFound)
        {
            m_script = Constants.getBoidSystem().GetComponent<CemBoidRuleBubble>();
            if (m_script == false)
                Debug.Log("Warning: Rule Bubble could not be found!");
            boidFound = true;
            updateInfo();
        }
    }

    public void updateInfo()
    {
        m_useRule.GetComponent<Toggle>().isOn = m_script.m_useRule;
        m_bubbleUseSwamCenter.GetComponent<Toggle>().isOn = m_script.m_bubbleUseSwamCenter;

        m_bubblePerFrame.GetComponent<InputField>().text = m_script.m_bubblePerFrame.ToString();
        m_bubbleMinPercentPerFrame.GetComponent<InputField>().text = m_script.m_bubbleMinPercentPerFrame.ToString();
        m_bubblePower.GetComponent<InputField>().text = m_script.m_bubblePower.ToString();
        m_bubbleMaxDistance.GetComponent<InputField>().text = m_script.m_bubbleMaxDistance.ToString();
        m_bubbleMaxSpeed.GetComponent<InputField>().text = m_script.m_bubbleMaxSpeed.ToString();
    }

    public void updateUseRule()
    {
        bool active = m_useRule.GetComponent<Toggle>().isOn;
        m_script.m_useRule = active;
    }
    public void updateUseSwamCenter()
    {
        bool active = m_bubbleUseSwamCenter.GetComponent<Toggle>().isOn;
        m_script.m_bubbleUseSwamCenter = active;
    }

    public void updateBubblesPerFrame()
    {
        string input = m_bubblePerFrame.GetComponent<InputField>().text;
        int output;
        if (int.TryParse(input, out output))
            m_script.m_bubblePerFrame = output;
        else
            Debug.Log("Aborted: Parsing error!");
    }
    public void updateBubbleMinPercentPerFrame()
    {
        string input = m_bubbleMinPercentPerFrame.GetComponent<InputField>().text;
        float output;
        if (float.TryParse(input, out output))
            m_script.m_bubbleMinPercentPerFrame = output;
        else
            Debug.Log("Aborted: Parsing error!");
    }
    public void updateBubblePower()
    {
        string input = m_bubblePower.GetComponent<InputField>().text;
        float output;
        if (float.TryParse(input, out output))
            m_script.m_bubblePower = output;
        else
            Debug.Log("Aborted: Parsing error!");
    }
    public void updateMaxDistance()
    {
        string input = m_bubbleMaxDistance.GetComponent<InputField>().text;
        float output;
        if (float.TryParse(input, out output))
            m_script.m_bubbleMaxDistance = output;
        else
            Debug.Log("Aborted: Parsing error!");
    }
    public void updateMaxSpeed()
    {
        string input = m_bubbleMaxSpeed.GetComponent<InputField>().text;
        float output;
        if (float.TryParse(input, out output))
            m_script.m_bubbleMaxSpeed = output;
        else
            Debug.Log("Aborted: Parsing error!");
    }
}
