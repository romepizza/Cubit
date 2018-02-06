using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointRoute : MonoBehaviour
{
    public GameObject checkpointSystem;

    public bool deactivate;
    [Header("----- SETTINGS -----")]
    public float maxTime;
    public bool showGizmos;
    //public int checkpointRouteIndex;

    [Header("----- DEBUG -----")]
    public bool isReady;
    public bool isActive;
    public bool isFinished;
    public bool isFailed;
    public float startTime;
    public float currentTime;
    public int lastDoneCheckpointIndex;
    public GameObject firstCheckpoint;
    public GameObject lastCheckpoint;
    public List<GameObject> checkpoints;
    public float[] checkpointTimes;

    void Start ()
    {
        checkpointTimes = new float[checkpoints.Count];
    }
	
	void Update ()
    {
        if (deactivate)
        {
            deactivateRoute();
            deactivate = false;
        }
        
        if (!isFailed && lastDoneCheckpointIndex == checkpoints.Count - 1)
            isFinished = true;

        if(isActive)
            manageTime();
    }

    public void manageTime()
    {
        currentTime -= Time.deltaTime;

        if (currentTime < 0 && isActive)
        {
            isFailed = true;
            deactivateRoute();
        }
    }

    public void checkpointIsHit(Checkpoint checkpointScript, float gainTime)
    {
        if (checkpointScript.isFirstCheckpoint)
            activateRoute();

        if(!isFailed)
            currentTime += gainTime;

        for(int i = 1 + lastDoneCheckpointIndex; i <= checkpointScript.checkpointIndex; i++)
        {
            checkpointTimes[i] = Time.time - startTime;
            if(i < checkpointScript.checkpointIndex)
            {
                checkpoints[i].GetComponent<Checkpoint>().isAvailable = false;
            }
        }

        if (checkpointScript.isLastCheckpoint)
        {
            deactivateRoute();
        }
        lastDoneCheckpointIndex = checkpointScript.checkpointIndex;
        checkpointSystem.GetComponent<CheckpointSystem>().playSoundCheckpointPassed();
    }

    public void activateRoute()
    {
        isActive = true;
        isReady = false;
        currentTime = maxTime;
        startTime = Time.time;
        checkpointSystem.GetComponent<CheckpointSystem>().activeWaypointRoute = this.gameObject;
    }

    public void deactivateRoute()
    {
        isActive = false;
        checkpointSystem.GetComponent<CheckpointSystem>().activeWaypointRoute = null;
    }

    public void resetRoute()
    {
        isReady = true;
        isFinished = false;
        isFailed = false;
        lastDoneCheckpointIndex = -1;
        foreach (GameObject checkpoint in checkpoints)
            checkpoint.GetComponent<Checkpoint>().resetCheckpoint();
        for (int i = 0; i < checkpointTimes.Length; i++)
            checkpointTimes[i] = 0.0f;

    }

    /*
    public void softResetRoute()
    {
        currentTime = maxTime;
        isFinished = false;
        isFailed = false;
        lastDoneCheckpointIndex = -1;
        foreach (GameObject checkpoint in checkpoints)
            checkpoint.GetComponent<Checkpoint>().resetCheckpoint();
        for (int i = 0; i < checkpointTimes.Length; i++)
            checkpointTimes[i] = 0.0f;
    }
    */
    void OnDrawGizmos()
    {
        if (showGizmos)
        {
            Gizmos.color = Color.green;
            for (int i = 0; i < checkpoints.Count; i++)
            {
                if (i != checkpoints.Count - 1)
                    Gizmos.DrawLine(checkpoints[i].transform.position, checkpoints[i + 1].transform.position);
            }
        }
    }
}
