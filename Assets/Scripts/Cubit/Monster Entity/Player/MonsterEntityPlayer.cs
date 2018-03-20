using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterEntityPlayer : MonsterEntityAbstractBase
{

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public override void pasteScript(EntityCopiableAbstract baseScript)
    {

    }

    public override void prepareDestroyScript()
    {
        Debug.Log("Oops: This should not have happened!");
    }
    public override void assignScripts()
    {

    }
}
