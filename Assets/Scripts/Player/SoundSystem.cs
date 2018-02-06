using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundSystem : MonoBehaviour {

    public AudioSource checkpointPassed;

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void playSoundCheckpointPassed()
    {
        checkpointPassed.Play();
    }
}
