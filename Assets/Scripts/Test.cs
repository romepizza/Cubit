using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public Material mat;


	// Use this for initialization
	void Start ()
    {
        GetComponent<MeshRenderer>().material = GetComponent<Renderer>().sharedMaterial;

        //MaterialPropertyBlock mpb = new MaterialPropertyBlock();
        //mpb.SetColor("_Color", Color.red);
        //GetComponent<Renderer>().SetPropertyBlock(mpb);
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
