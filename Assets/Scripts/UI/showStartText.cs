using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class showStartText : MonoBehaviour
{
    public float maxDuration;
    public float currentDuration;
    public bool isShowingTextTimed;
    // Use this for initialization
    void Start()
    {
        showText("Press escape to view controls.\nPress 'B' to Start the Waves.\nHave fun!", 5f);
    }

    // Update is called once per frame
    void Update()
    {
        if (isShowingTextTimed)
        {
            if (currentDuration < maxDuration)
                currentDuration += Time.deltaTime;
            else
            {
                GetComponent<Text>().text = "";
                isShowingTextTimed = false;
            }
        }
    }

    public void showText(string str, float duration)
    {
        GetComponent<Text>().text = str;
        maxDuration = duration;
        currentDuration = 0;
        isShowingTextTimed = true;
    }
}
