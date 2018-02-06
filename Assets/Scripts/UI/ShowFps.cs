using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fps : MonoBehaviour {

    public const float FPS_UPDATE_INTERVAL = 0.5f;
    public float fpsAccum = 0;
    public int fpsFrames = 0;
    private float fpsTimeLeft = FPS_UPDATE_INTERVAL;
    private float fps = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        fpsTimeLeft -= Time.deltaTime;
        	
	}
}
