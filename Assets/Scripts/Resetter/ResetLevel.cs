using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetLevel : MonoBehaviour
{
    public GameObject resetLevelObject;
    public bool simulate;

    void Update()
    {
        if(simulate)
        {
            resetLevel(resetLevelObject);
            simulate = false;
        }
    }

	public void resetLevel(GameObject level)
    {
        foreach (Transform child in level.transform)
        {
            if (child.gameObject.name == "Obstacles")
            {
                foreach (Transform obstacle in child.transform)
                {
                    if (obstacle.GetComponent<PenaltyTrigger>() != null)
                        obstacle.GetComponent<PenaltyTrigger>().resetPenalty();
                }
            }
            if (child.gameObject.name == "Boosts")
            {
                foreach (Transform boost in child.transform)
                {
                    if (boost.GetComponent<BoostTrigger>() != null)
                        boost.GetComponent<BoostTrigger>().resetBoost();
                }
            }
            if (child.gameObject.name == "Penalties")
            {
                foreach (Transform penalty in child.transform)
                {
                    if (penalty.GetComponent<PenaltyTrigger>() != null)
                        penalty.GetComponent<PenaltyTrigger>().resetPenalty();
                }
            }
        }
    }
}
