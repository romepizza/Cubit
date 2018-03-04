using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AttachEntityBase : MonoBehaviour
{
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public abstract bool addAgent(GameObject agent);
    public abstract void removeAgent(GameObject agent);
    public abstract void setValuesByPrefab(GameObject prefab);
}
