using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constants : MonoBehaviour
{
    public GameObject m_player;
    public GameObject m_mainCge;
    public CEMBoidSystem m_boidSystem;

    private static GameObject s_player;
    private static CGE s_mainCge;
    private static CEMBoidSystem s_boidSystem;

	// Use this for initialization
	void Start ()
    {
        s_player = m_player;

        s_boidSystem = m_boidSystem;
        
        if (m_mainCge != null)
            s_mainCge = m_mainCge.GetComponent<CGE>();
        else
            Debug.Log("Warning: No main CGE set!");
	}
    
    public static GameObject getPlayer()
    {
        return s_player;
    }

    public static CGE getMainCge()
    {
        return s_mainCge;
    }

    public static CEMBoidSystem getBoidSystem()
    {
        return s_boidSystem;
    }
}
