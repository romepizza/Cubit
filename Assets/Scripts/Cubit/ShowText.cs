using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowText : MonoBehaviour {

    public float goals;
    public GameObject goalTextObject;
    public GameObject goalPerMinTextObject;

	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		if(GetComponent<Goal>() != null)
        {
            //Debug.Log(goalTextObject.GetComponent<TextMesh>() != null);
            if (goalTextObject != null && goalTextObject.GetComponent<TextMesh>() != null)
            {
                goals = GetComponent<Goal>().goals;
                goalTextObject.GetComponent<TextMesh>().text = "Goals\n" + goals;
            }

            if (goalPerMinTextObject != null && goalPerMinTextObject.GetComponent<TextMesh>() != null)
            {
                int goals = (int)GetComponent<Goal>().goalsPerMin;
                goalPerMinTextObject.GetComponent<TextMesh>().text = "Goals/Min\n" + goals;

            }
        }
	}
}
