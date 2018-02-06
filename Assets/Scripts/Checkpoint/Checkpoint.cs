using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public GameObject checkpointRoute;
    public Material materialAvailable;
    public Material materialNotAvailable;

    [Header("----- SETTINGS -----")]
    public float gainTime;
    public int checkpointIndex;
    public bool isFirstCheckpoint;
    public bool isMiddleCheckpoint;
    public bool isLastCheckpoint;

    [Header("----- DEBUG -----")]
    public bool isAvailable;

    private RoundTrigger roundTriggerScript;

	void Start ()
    {
        if (GetComponent<RoundTrigger>())
            roundTriggerScript = GetComponent<RoundTrigger>();
    }
	
	void Update ()
    {
		if(roundTriggerScript != null && roundTriggerScript.isHit && isAvailable)
        {
            checkpointIsHit();
        }

        if (isAvailable)
            GetComponent<MeshRenderer>().material = materialAvailable;
        else
            GetComponent<MeshRenderer>().material = materialNotAvailable;
    }

    public void checkpointIsHit()
    {
        checkpointRoute.GetComponent<CheckpointRoute>().checkpointIsHit(this, gainTime);
        isAvailable = false;
    }

    public void resetCheckpoint()
    {
        isAvailable = true;
        if (GetComponent<RoundTrigger>() != null)
            GetComponent<RoundTrigger>().isHit = false;
    }

    public void OnTriggerEnter(Collider col)
    {
        if(roundTriggerScript == null && isAvailable)
        {
            checkpointIsHit();
        }
    }

    
}
