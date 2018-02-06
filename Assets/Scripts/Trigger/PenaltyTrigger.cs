using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PenaltyTrigger : MonoBehaviour
{

    public Material materialAvailable;
    public Material materialNotAvailable;

    [Header("----- SETTINGS -----")]
    public float penaltyDurotation;

    [Header("----- DEBUG -----")]
    public bool penaltyAvailable = true;
    public bool playerObjectFound;
    public GameObject gameObjectPlayer;


    private RoundTrigger roundTriggerScript;

    void Start()
    {
        roundTriggerScript = GetComponent<RoundTrigger>();
    }

    void Update()
    {
        if (roundTriggerScript != null && roundTriggerScript.isHit && penaltyAvailable)
        {
            roundTriggerScript.gameObjectPlayer.GetComponent<Boost>().addBoost(penaltyDurotation);
            penaltyAvailable = false;
        }
        
        if (penaltyAvailable)
            GetComponent<MeshRenderer>().material = materialAvailable;
        else
            GetComponent<MeshRenderer>().material = materialNotAvailable;
    }

    void OnCollisionEnter(Collision col)

    {
        if (!playerObjectFound)
            getParentGameObject(col);

        if (penaltyAvailable && playerObjectFound)
        {
            gameObjectPlayer.GetComponent<Penalty>().addPenalty(penaltyDurotation);
            penaltyAvailable = false;
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (!playerObjectFound)
            getParentGameObject(col);

        if (penaltyAvailable && gameObjectPlayer)
        {
            gameObjectPlayer.GetComponent<Penalty>().addPenalty(penaltyDurotation);
            penaltyAvailable = false;
        }
    }

    public void getParentGameObject(Collision col)
    {
        int tries = 0;
        GameObject currentGameObject = col.gameObject;
        while (currentGameObject != null && !playerObjectFound && tries < 100)
        {
            if (currentGameObject.tag == "Player")
            {
                gameObjectPlayer = currentGameObject;
                playerObjectFound = true;
                return;
            }
            currentGameObject = currentGameObject.transform.parent.gameObject;
            tries++;
        }
    }

    public void getParentGameObject(Collider col)
    {
        int tries = 0;
        GameObject currentGameObject = col.gameObject;
        while (currentGameObject != null && !playerObjectFound && tries < 100)
        {
            if (currentGameObject.tag == "Player")
            {
                gameObjectPlayer = currentGameObject;
                playerObjectFound = true;
                return;
            }
            currentGameObject = currentGameObject.transform.parent.gameObject;
            tries++;
        }
    }

    public void resetPenalty()
    {
        penaltyAvailable = true;
        if (GetComponent<RoundTrigger>() != null)
            GetComponent<RoundTrigger>().isHit = false;
    }
}
