using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableAtStart : MonoBehaviour
{
    public bool isDeactivated;
	// Use this for initialization
	void Start ()
    {
	    	
	}
	
	// Update is called once per frame
	void Update ()
    {
		if(!isDeactivated)
        {
            this.gameObject.SetActive(false);
            isDeactivated = true;
        }
	}
}
