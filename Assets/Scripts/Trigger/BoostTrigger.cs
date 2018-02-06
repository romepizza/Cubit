using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostTrigger : MonoBehaviour
{
    [Header("----- SETTINGS -----")]
    public float boostDurotation;
    public Material materialAvailable;
    public Material materialNotAvailable;

    [Header("----- DEBUG -----")]
    public bool boostAvailable = true;
    private RoundTrigger roundTriggerScript;

	void Start ()
    {
        roundTriggerScript = GetComponent<RoundTrigger>();
    }
	
	void Update ()
    {
		if(roundTriggerScript.isHit && boostAvailable)
        {
            roundTriggerScript.gameObjectPlayer.GetComponent<Boost>().addBoost(boostDurotation);
            boostAvailable = false;
        }

        if (boostAvailable)
            GetComponent<MeshRenderer>().material = materialAvailable;
        else
            GetComponent<MeshRenderer>().material = materialNotAvailable;
    }

    public void resetBoost()
    {
        boostAvailable = true;
        if (GetComponent<RoundTrigger>() != null)
            GetComponent<RoundTrigger>().isHit = false;
    }
}
