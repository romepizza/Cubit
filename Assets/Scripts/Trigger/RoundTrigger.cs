using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundTrigger : MonoBehaviour
{
    public bool isHit;
    public float radius;
    public bool playerObjectFound;
    public GameObject gameObjectPlayer;
       
	void Start ()
    {
        radius = transform.localScale.x / 2f;
    }
	
	void Update ()
    {

    }

    void OnTriggerStay(Collider col)
    {
        if (!isHit)
        {
            Vector3 hitPoint = transform.position - col.ClosestPoint(transform.position);
            if(hitPoint.magnitude < radius)
            {
                if (!playerObjectFound)
                    getParentGameObject(col);
                isHit = true;
            }
        }
    }

    public void getParentGameObject(Collider col)
    {
        int tries = 0;
        GameObject currentGameObject = col.gameObject;
        while(currentGameObject != null && !playerObjectFound && tries < 100)
        {
            if(currentGameObject.tag == "Player")
            {
                gameObjectPlayer = currentGameObject;
                playerObjectFound = true;
                break;
            }
            currentGameObject = currentGameObject.transform.parent.gameObject;
            tries++;
        }
        playerObjectFound = false;
    }
}
