using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookInFlightDirection : ParentEntityDeleteOnStateChange
{
    public static float lookPower = 0.1f;

	void Update ()
    {
        Rigidbody rb = GetComponent<Rigidbody>();

        if (rb == null)
            return;


        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(rb.velocity), lookPower);
	}
}
