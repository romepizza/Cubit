using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    public GameObject[] explosionEffects;
    public float radius;
    public float goals;
    public float goalsPerMin;
    public float currentTime;

    void Start ()
    {
        radius = transform.localScale.x / 2f;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (goals != 0)
            currentTime += Time.deltaTime;

        goalsPerMin = (int)((goals * 60 / ((currentTime < 1) ? 1 : currentTime)) * 1f) / 1f;
	}

    void score()
    {
        goals++;
    }


    void OnTriggerStay(Collider col)
    {
        radius = transform.localScale.x / 2f;
        Vector3 hitPoint = transform.position - col.ClosestPoint(transform.position);
        if (hitPoint.magnitude < radius)
        {
            if(col.gameObject.tag == "Ball")
            {
                for(int i = 0; i < explosionEffects.Length; i++)
                    Instantiate(explosionEffects[i], col.gameObject.transform.position, Quaternion.Euler(0, 0, 0));
                score();
                col.gameObject.GetComponent<ColorCube>().destroySelf();
            }
        }
        
    }
}
