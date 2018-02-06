using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boost : MonoBehaviour
{
    public bool isBoosting;
    public List<float> boosts;

	void Start ()
    {
        boosts = new List<float>();
    }
	

	void Update ()
    {
        manageBoost();
	}

    void manageBoost()
    {
        for(int i = 0; i < boosts.Count; i++)
        {
            if(boosts[i] < Time.time)
            {
                boosts.RemoveAt(i);
            }
        }

        isBoosting = boosts.Count > 0;
    }

    public void addBoost(float boostTime)
    {
        boosts.Add(Time.time + boostTime);
    }
}
