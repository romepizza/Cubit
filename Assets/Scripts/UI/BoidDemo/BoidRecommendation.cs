using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidRecommendation : MonoBehaviour
{
    public static bool s_performanceTested = false;
    public static int s_defaultPerformance = 63;
    public static float s_performanceFactor = 1;
    public static float s_performancePuffer = 0.2f;


    
    [Header("--- (Swarm Size) ---")]
    public int m_swarmSizeRecommendation;
    public int m_swarmSizeMinRecommendation;
    public int m_swarmSizeMaxRecommendation;

    [Header("--- (Number Predator) ---")]
    public int m_numberPredatorRecommendation;
    public int m_numberPredatorMinRecommendation;
    public int m_numberPredatorMaxRecommendation;

    public void Start()
    {
        //if(!s_performanceTested)
            //calculatePerformanceFactor();
    }

    public static void calculatePerformanceFactor()
    {
        int tickCountBefore = System.Environment.TickCount;
        for(int i = 0; i < 500000; i++)
        {
            float j = Mathf.Sqrt(Mathf.Cos(i * 0.025f));
        }
        int tickCountAfter = System.Environment.TickCount;
        int difference = tickCountAfter - tickCountBefore;
        if (difference > 0)
            s_performanceFactor = ((float)s_defaultPerformance / (float)difference);
        else
            s_performanceFactor = 1;

        if (s_performanceFactor < 1 + s_performancePuffer && s_performanceFactor > 1 - s_performancePuffer)
            s_performanceFactor = 1;

        s_performanceTested = true;
    }
}
