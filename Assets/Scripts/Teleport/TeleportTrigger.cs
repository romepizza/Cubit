using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportTrigger : MonoBehaviour
{
    [Header("----- SETTINGS -----")]
    public GameObject teleportToVector;
    public GameObject resetCheckpointRoute;
    public GameObject resetLevel;


    [Header("----- DEBUG -----")]
    public GameObject gameObjectPlayer;
    public bool playerObjectFound;


    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.layer == 8)
        {
            if (!playerObjectFound)
            {
                getParentGameObject(col);
            }
            if (playerObjectFound)
            {

                if (resetCheckpointRoute != null)
                    resetCheckpointRoute.GetComponent<CheckpointRoute>().resetRoute();
                if (resetLevel != null)
                    GameObject.Find("GeneralScriptObject").GetComponent<ResetLevel>().resetLevel(resetLevel);

                gameObjectPlayer.GetComponent<Rigidbody>().velocity = Vector3.zero;
                gameObjectPlayer.transform.rotation = teleportToVector.transform.rotation;
                gameObjectPlayer.GetComponent<CameraRotation>().currentAngleX = teleportToVector.transform.rotation.eulerAngles.y;
                gameObjectPlayer.GetComponent<CameraRotation>().currentAngleY = teleportToVector.transform.rotation.eulerAngles.x;
                gameObjectPlayer.transform.position = teleportToVector.transform.position;
            }
        }
    }

    public bool getParentGameObject(Collider col)
    {
        int tries = 0;
        GameObject currentGameObject = col.gameObject;
        while (currentGameObject != null && !playerObjectFound && tries < 100)
        {
            if (currentGameObject.tag == "Player")
            {
                gameObjectPlayer = currentGameObject;
                playerObjectFound = true;
                return true;
            }
            if (currentGameObject.transform.parent != null)
                currentGameObject = currentGameObject.transform.parent.gameObject;
            else
                break;
            tries++;
        }
        return false;
    }
}
