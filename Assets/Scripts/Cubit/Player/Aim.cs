using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Aim : MonoBehaviour
{
    public GameObject m_aimSprite;
    [Header("----- SETTING -----")]
    public LayerMask m_layerMask;
    public bool m_useAim;
    [Header("----- DEBUG -----")]
    public GameObject m_aimingAt;
    public bool m_isAim;
    public bool m_isLocked;
    public bool m_lockedToggle;


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        m_aimingAt = aimAtEnemy();
        setAimSprite();
        evaluateAimLock();
    }

    public GameObject aimAtEnemy()
    {
        GameObject cubeCore = null;
        List<GameObject> cubes = new List<GameObject>();

        RaycastHit[] hits = Physics.RaycastAll(Camera.main.transform.position, Camera.main.transform.forward);
        foreach (RaycastHit hit in hits)
        {
            GameObject cubeHit = hit.collider.gameObject;
            if (cubeHit.GetComponent<ColorCube>() != null && cubeHit.GetComponent<CubeMonster>() != null && cubeHit.GetComponent<CubeMonster>().isAlive && transform.InverseTransformPoint(cubeHit.transform.position).z > 0)
            {
                cubes.Add(cubeHit);
            }
            if (cubeHit.GetComponent<ColorCube>() != null && cubeHit.GetComponent<MonsterChase>() != null && cubeHit.GetComponent<MonsterChase>().isAlive && transform.InverseTransformPoint(cubeHit.transform.position).z > 0)
            {
                cubes.Add(cubeHit);
            }
        }

        float minDist = float.MaxValue;
        foreach (GameObject cube in cubes)
        {
            //Debug.Log(Camera.main.WorldToScreenPoint(cube.transform.position));

            Vector3 cubePos = Camera.main.WorldToViewportPoint(cube.transform.position);
            cubePos.z = 0;

            Vector3 defaultPos = new Vector3(0.5f, 0.5f, 0);


            float dist = (cubePos - defaultPos).magnitude;
            if (dist < minDist)
            {
                minDist = dist;
                cubeCore = cube;
            }
        }

        return cubeCore;
    }

    void setAimSprite()
    {
        if (m_aimingAt != null)
        {
            m_aimSprite.SetActive(true);
            m_aimSprite.transform.position = Camera.main.WorldToScreenPoint(m_aimingAt.transform.position);
        }
        else
            m_aimSprite.SetActive(false);
    }

    void evaluateAimLock()
    {
        if (Input.GetButton("ButtonX") || Input.GetButton("RightStickPress"))
        {
            if (!m_lockedToggle && !m_isLocked && m_aimingAt != null)
            {

                m_isLocked = true;
                m_lockedToggle = true;

            }
            if (!m_lockedToggle && m_isLocked)
            {
                m_isLocked = false;
                m_lockedToggle = true;
                
            }
        }
        else
            m_lockedToggle = false;



        if (m_isLocked && m_aimingAt == null)
        {
            m_isLocked = false;
            //GetComponent<CameraRotation>().currentAngleX = transform.rotation.eulerAngles.y;
            //GetComponent<CameraRotation>().currentAngleY = transform.rotation.eulerAngles.x;

        }

        //Debug.Log("----------------------------------------------------------");
    }
}