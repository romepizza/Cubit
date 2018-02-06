using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckpointSystem : MonoBehaviour
{
    public GameObject player;
    public GameObject timeText;
    public Material materialFirstCheckpointAvailable;
    public Material materialFirstCheckpointNotAvailable;
    public Material materialMiddleCheckpointAvailable;
    public Material materialMiddleCheckpointNotAvailable;
    public Material materialLastCheckpointAvailable;
    public Material materialLastCheckpointNotAvailable;

    [Header("----- SETTINGS -----")]
    public bool initializeCheckpointRoutes;

    [Header("----- DEBUG -----")]
    public GameObject activeWaypointRoute;
    public List<GameObject> checkpointRoutes;

    [Header("----- SIMULATE -----")]
    public bool simulate;
    public int activationIndex;

	void Start ()
    {
        
    }
	
	void Update ()
    {
		if(simulate)
        {
            activateCheckpointRoute(checkpointRoutes[activationIndex]);
            simulate = false;
        }
	}
    
    public void activateCheckpointRoute(GameObject waypointRoute)
    {
        activeWaypointRoute = waypointRoute;
        activeWaypointRoute.GetComponent<CheckpointRoute>().activateRoute();
    }


    public void playSoundCheckpointPassed()
    {
        player.GetComponent<SoundSystem>().playSoundCheckpointPassed();
    }


    void OnDrawGizmos()
    {
        if (initializeCheckpointRoutes)
        {
            checkpointRoutes = new List<GameObject>();
            foreach (Transform checkpointRoute in transform)
            {
                if (checkpointRoute.gameObject.GetComponent<CheckpointRoute>() == null)
                    checkpointRoute.gameObject.AddComponent<CheckpointRoute>();
                checkpointRoute.gameObject.GetComponent<CheckpointRoute>().checkpointSystem = this.gameObject;
                checkpointRoute.gameObject.GetComponent<CheckpointRoute>().checkpoints = new List<GameObject>();
                int checkpointIndex = 0;
                foreach(Transform checkpoint in checkpointRoute.transform)
                {
                    if (checkpoint.gameObject.GetComponent<Checkpoint>() == null)
                        checkpoint.gameObject.AddComponent<Checkpoint>();

                    checkpoint.gameObject.GetComponent<Checkpoint>().isFirstCheckpoint = false;
                    checkpoint.gameObject.GetComponent<Checkpoint>().isMiddleCheckpoint = false;
                    checkpoint.gameObject.GetComponent<Checkpoint>().isLastCheckpoint = false;
                    if (checkpointIndex == 0)
                    {
                        checkpoint.gameObject.GetComponent<Checkpoint>().isFirstCheckpoint = true;
                        checkpoint.gameObject.GetComponent<MeshRenderer>().material = materialFirstCheckpointAvailable;
                        checkpoint.gameObject.GetComponent<Checkpoint>().materialAvailable = materialFirstCheckpointAvailable;
                        checkpoint.gameObject.GetComponent<Checkpoint>().materialNotAvailable = materialFirstCheckpointNotAvailable;
                        checkpointRoute.gameObject.GetComponent<CheckpointRoute>().firstCheckpoint = checkpoint.gameObject;
                    }
                    if(checkpointIndex == checkpointRoute.transform.childCount - 1)
                    {
                        checkpoint.gameObject.GetComponent<Checkpoint>().isLastCheckpoint = true;
                        checkpoint.gameObject.GetComponent<MeshRenderer>().material = materialLastCheckpointAvailable;
                        checkpoint.gameObject.GetComponent<Checkpoint>().materialAvailable = materialLastCheckpointAvailable;
                        checkpoint.gameObject.GetComponent<Checkpoint>().materialNotAvailable = materialLastCheckpointNotAvailable;
                        checkpointRoute.gameObject.GetComponent<CheckpointRoute>().lastCheckpoint = checkpoint.gameObject;
                    }
                    if(!(checkpointIndex == 0) && !(checkpointIndex == checkpointRoute.transform.childCount - 1))
                    {
                        checkpoint.gameObject.GetComponent<Checkpoint>().isMiddleCheckpoint = true;
                        checkpoint.gameObject.GetComponent<MeshRenderer>().material = materialMiddleCheckpointAvailable;
                        checkpoint.gameObject.GetComponent<Checkpoint>().materialAvailable = materialMiddleCheckpointAvailable;
                        checkpoint.gameObject.GetComponent<Checkpoint>().materialNotAvailable = materialMiddleCheckpointNotAvailable;
                    }
                    if (checkpoint.gameObject.GetComponent<RoundTrigger>() != null)
                        checkpoint.gameObject.GetComponent<RoundTrigger>().isHit = false;

                    checkpoint.gameObject.GetComponent<Checkpoint>().isAvailable = true;
                    checkpoint.gameObject.GetComponent<Checkpoint>().checkpointIndex = checkpointIndex;
                    checkpoint.gameObject.GetComponent<Checkpoint>().checkpointRoute = checkpointRoute.gameObject;
                    checkpoint.gameObject.tag = "Checkpoint";
                    checkpointRoute.gameObject.tag = "CheckpointRoute";
                    checkpointRoute.gameObject.GetComponent<CheckpointRoute>().checkpoints.Add(checkpoint.gameObject);
                    checkpointIndex++;
                }


                checkpointRoutes.Add(checkpointRoute.gameObject);
            }
            initializeCheckpointRoutes = false;
        }
    }
}
