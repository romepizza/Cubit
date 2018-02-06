using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteSelfScript : MonoBehaviour {
	public float timeFrame = 0.02f;
	// Use this for initialization
	void Start () {
		Destroy (this.gameObject, timeFrame);
	}
}
