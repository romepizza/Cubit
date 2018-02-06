using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTime : MonoBehaviour
{
    public float timePlayed;
    
	void Start ()
    {
		
	}
	

	void Update ()
    {
        if (!GetComponent<Options>().isFreeze)
            timePlayed += Time.deltaTime;
	}
}
