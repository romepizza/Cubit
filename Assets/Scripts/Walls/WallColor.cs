using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallColor : MonoBehaviour
{
    public GameObject player;

    [Header("----- SETTINGS -----")]
    public Color minColorPerma;
    public Color maxColorPerma;
    public Color minEmissionColorPerma;
    public Color maxEmissionColorPerma;
    //public Color minColorCollision;
    public Color maxColorCollision;
    //public Color minEmissionColorCollision;
    public Color maxEmissionColorCollision;

    [Header("--- (permanent) ---")]
    public float minDistance;
    public float maxDistance;

    [Header("--- (collision) ---")]
    public float collisionTime;



    [Header("----- DEBUG -----")]
    public float factorPermanent;
    public float factorCollision;
    public float currentCollisionTime;
    public Vector3 relativePlayerPosition;
    public float distance;
     
	// Use this for initialization
	void Start ()
    {
        player = GameObject.Find("PlayerDrone");
        //maxColorPerma = GetComponent<Renderer>().sharedMaterial.color;
    }
	
	// Update is called once per frame
	void Update ()
    {
        getDistance();
        //changeColor();
        collisionEffect();
    }

    void getDistance()
    {
        relativePlayerPosition = transform.InverseTransformPoint(player.transform.position);
        distance = relativePlayerPosition.z;
    }

    /*
    void changeColor()
    {
        factorPermanent = Mathf.Clamp(distance, minDistance, maxDistance) / maxDistance;
        Color lerpColor = Color.Lerp(minColorPerma, maxColorPerma, factorPermanent);
        Color lerpEmissionColor = Color.Lerp(minEmissionColorPerma, maxEmissionColorPerma, factorPermanent);

        MaterialPropertyBlock pb = new MaterialPropertyBlock();
        pb.SetColor("_Color", lerpColor);
        pb.SetColor("_EmissionColor", lerpEmissionColor);
        GetComponent<Renderer>().SetPropertyBlock(pb);
    }
    */
    void collisionEffect()
    {

        factorPermanent = 1 - (Mathf.Clamp(distance, minDistance, maxDistance) - minDistance) / (maxDistance - minDistance);
        factorPermanent = Mathf.Clamp(factorPermanent, 0, 1);
        Color lerpColorPerma = Color.Lerp(minColorPerma, maxColorPerma, factorPermanent);
        Color lerpEmissionColorPerma = Color.Lerp(minEmissionColorPerma, maxEmissionColorPerma, factorPermanent);

        Color lerpColorCollision = lerpColorPerma;
        Color lerpEmissionColorCollision = lerpEmissionColorPerma;

        if (currentCollisionTime > 0)
        {
            factorCollision = currentCollisionTime / collisionTime;
            lerpColorCollision = Color.Lerp(lerpColorPerma, maxColorCollision, factorCollision);
            lerpEmissionColorCollision = Color.Lerp(lerpEmissionColorPerma, maxEmissionColorCollision, factorCollision);
            
            currentCollisionTime -= Time.deltaTime;
        }

        MaterialPropertyBlock pb = new MaterialPropertyBlock();
        pb.SetColor("_Color", lerpColorCollision);
        pb.SetColor("_EmissionColor", lerpEmissionColorCollision);
        GetComponent<Renderer>().SetPropertyBlock(pb);
    }

    void OnCollisionEnter(Collision col)
    {
        if(col.gameObject == player)
        {
            currentCollisionTime = collisionTime;
        }
    }
}
