using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowTime : MonoBehaviour
{
    public CheckpointSystem checkpointSystemScript;
    public GameObject player;

	// Use this for initialization
	void Start ()
    {
        checkpointSystemScript = GameObject.Find("CheckpointSystem").GetComponent<CheckpointSystem>();

    }

    // Update is called once per frame
    void Update()
    {
        showTimeOnCanvas();
    }

    
    public void showTimeOnCanvas()
    {
        if (checkpointSystemScript.activeWaypointRoute != null)
        {
            float remainingTime = checkpointSystemScript.activeWaypointRoute.GetComponent<CheckpointRoute>().currentTime;
            if (remainingTime > 0)
            {
                GetComponent<Text>().GetComponent<Text>().text = ((int)remainingTime).ToString();
            }
            else
                GetComponent<Text>().GetComponent<Text>().text = "";
        }
        else
            GetComponent<Text>().GetComponent<Text>().text = "";
    }
    
}
