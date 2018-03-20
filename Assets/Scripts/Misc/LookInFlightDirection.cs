using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookInFlightDirection : EntityCopiableAbstract
{
    public float lookPower = 0.1f;

	void Update ()
    {
        Rigidbody rb = GetComponent<Rigidbody>();

        if (rb == null || rb.velocity.magnitude <= 0.01)
            return;


        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(rb.velocity), lookPower);
	}

    // copy+
    void setValues(LookInFlightDirection baseScript)
    {
        lookPower = baseScript.lookPower;
    }

    public override void pasteScript(EntityCopiableAbstract baseScript)
    {
        setValues((LookInFlightDirection)baseScript);
    }
    public override void assignScripts()
    {

    }
    public override void prepareDestroyScript()
    {
        Destroy(this);
    }
}
