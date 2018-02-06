using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenralScript : MonoBehaviour
{
    public bool isInMenu;
    public bool isInGame;

    void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 999;
    }

	void Start ()
    {
        Cursor.visible = false;
    }
	
	void Update ()
    {
		
	}
}
