using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Penalty : MonoBehaviour
{
    public bool isPenalty;
    public List<float> penalties;

    void Start()
    {
        penalties = new List<float>();
    }


    void Update()
    {
        managePenalties();
    }

    void managePenalties()
    {
        for (int i = 0; i < penalties.Count; i++)
        {
            if (penalties[i] < Time.time)
            {
                penalties.RemoveAt(i);
            }
        }

        isPenalty = penalties.Count > 0;
    }

    public void addPenalty(float penaltyTime)
    {
        penalties.Add(Time.time + penaltyTime);
    }
}
