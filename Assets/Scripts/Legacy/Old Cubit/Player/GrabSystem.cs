using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabSystem : MonoBehaviour
{
    public GameObject waveSystem;

    [Header("----- SETTINGS -----")]
    public int maxCubes;

    [Header("--- (Log) ---")]
    public float grabbedInterval;

    [Header("----- DEBUG -----")]
    public List<GameObject> grabbedCubes;
    public List<float> grabbedTimes;

    [Header("--- (Successively) ---")]
    public bool isLaunching;
    float m_duration;
    float m_power;
    float m_maxSpeed;
    float m_activeTime;
    LayerMask m_layerMask;
    Vector3 m_offset;
    bool m_isAim;
    public int shootCubesTotal;
    public int shootCubesSoFar;

    [Header("--- (Log) ---")]
    public bool isAim;
    public int logGrabbedCount;
    public float currentLogGrabbedCounter;
    public int totalGrabbed;
    public int pushedFromGrabTotal;

    

	// Use this for initialization
	void Start ()
    {
        //manageGrabTimes();
        
	}

    void FixedUpdate()
    {
        if(isLaunching)
        {
            launchCubeGattlingPush(m_duration, m_power, m_maxSpeed, m_activeTime, m_layerMask, m_offset, m_isAim);
        }
    }

    void manageGrabTimes()
    {
        List<int> deleteIndices = new List<int>();
        for (int i = 0; i < grabbedCubes.Count; i++)
        {
            if (grabbedCubes[i] == null)
            {
                deleteIndices.Add(i);
            }
            if (grabbedTimes[i] < Time.time)
            {
                removeCubeFromGrab(grabbedCubes[i], true);
            }
        }

        for(var i = deleteIndices.Count - 1; i >= 0; i--)
        {
            if (grabbedCubes[deleteIndices[i]] == null)
            {
                grabbedTimes.RemoveAt(i);
                grabbedCubes.Remove(grabbedCubes[i]);
                continue;
            }
            removeCubeFromGrab(grabbedCubes[deleteIndices[i]], true);
        }
    }

    public void launchCubes(float duration, float power, float maxSpeed, float activeTime, LayerMask layerMask, Vector3 offset, bool aimHelpActivated)
    {
        bool logged = false;
        for (int i = 0; i < grabbedCubes.Count; i++)
        {
            grabbedCubes[i].GetComponent<Rigidbody>().velocity = grabbedCubes[i].GetComponent<Rigidbody>().velocity.normalized * Mathf.Min(grabbedCubes[i].GetComponent<Rigidbody>().velocity.magnitude, Mathf.Sqrt(grabbedCubes[i].GetComponent<Rigidbody>().velocity.magnitude));
            Vector3 targetPoint = calculateAimTargetPoint(grabbedCubes[i], layerMask, offset, maxSpeed, aimHelpActivated);

            pushedFromGrabTotal++;
            //Debug.DrawLine(grabbedCubes[i].transform.position, targetPoint, Color.magenta, 10f);
            grabbedCubes[i].GetComponent<ColorCube>().addStateAccelerate(this.gameObject, duration, targetPoint, power, maxSpeed, activeTime, 0, true, isAim);
        }
        int tries = 0;
        while (grabbedCubes.Count > 0 && tries < 10000)
        {
            tries++;
            removeCubeFromGrab(grabbedCubes[0], true);
        }
        foreach (GameObject cubeGrabbed in grabbedCubes)
        {
            cubeGrabbed.GetComponent<ColorCube>().lightCountChanged = true;
        }
    }

    
    public void launchCubesSuccessively(float duration, float power, float maxSpeed, float activeTime, LayerMask layerMask, Vector3 offset, bool aimHelpActivated)
    {
        if (!isLaunching)
        {
            m_duration = duration;
            m_power = power;
            m_maxSpeed = maxSpeed;
            m_activeTime = activeTime;
            m_layerMask = layerMask;
            m_offset = offset;
            m_isAim = aimHelpActivated;

            shootCubesSoFar = 0;
            shootCubesTotal = Mathf.Min(Mathf.Max(10, (int)(grabbedCubes.Count * 1f)), grabbedCubes.Count);

            isLaunching = true;
        }
    }

    public void launchCubeGattlingPush(float duration, float power, float maxSpeed, float activeTime, LayerMask layerMask, Vector3 offset, bool aimHelpActivated)
    {
        if(grabbedCubes.Count <= 0 || shootCubesSoFar >= shootCubesTotal)
        {
            isLaunching = false;
            shootCubesSoFar = 0;
            return;
        }

        GameObject cubeShot = null;

        // get nearest cube
        float minDist = float.MaxValue;
        for(int i = 0; i < grabbedCubes.Count; i++)
        {
            float dist = Vector3.Distance(calculateAimTargetPoint(grabbedCubes[i], layerMask, offset, maxSpeed, aimHelpActivated), grabbedCubes[i].transform.position);
            if(dist < minDist)
            {
                cubeShot = grabbedCubes[i];
                minDist = dist;
            }
        }
        if (cubeShot != null)
        {
            shootCubesSoFar++;
            cubeShot.GetComponent<Rigidbody>().velocity = cubeShot.GetComponent<Rigidbody>().velocity.normalized * cubeShot.GetComponent<Rigidbody>().velocity.sqrMagnitude;
            launchCube(cubeShot, duration, calculateAimTargetPoint(cubeShot, layerMask, offset, maxSpeed, aimHelpActivated), power, maxSpeed, activeTime, isAim, 0);
            //launchCube(cubeShot, duration, calculateAimTargetPoint(cubeShot, layerMask, offset, maxSpeed, aimHelpActivated), power, maxSpeed, activeTime, isAim);
        }
        else
        {
            isLaunching = false;
            shootCubesSoFar = 0;
            return;
        }
    }

    Vector3 calculateAimTargetPoint(GameObject cube, LayerMask layerMask, Vector3 aimOffsetGrabbed, float maxSpeed, bool aimHelpActivated)
    {
        //RaycastHit hit;
        if (aimHelpActivated/* && Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 500f, layerMask)*/)
        {
            GameObject cubeCore = GetComponent<Aim>().m_aimingAt;
            if (cubeCore != null)
            {
                bool isThrow = cubeCore.GetComponent<CubeMonster>().isAlive;
                bool isChase = cubeCore.GetComponent<MonsterChase>().isAlive;
                /*
                if (hit.rigidbody.gameObject.GetComponent<CubeMonster>() != null && hit.rigidbody.gameObject.GetComponent<CubeMonster>().isAlive && transform.InverseTransformPoint(hit.collider.gameObject.transform.position).z > 0)
                {
                    isThrow = true;
                    cubeCore = hit.rigidbody.gameObject;
                }
                else if (hit.rigidbody.gameObject.GetComponent<MonsterChase>() != null && hit.rigidbody.gameObject.GetComponent<MonsterChase>().isAlive && transform.InverseTransformPoint(hit.collider.gameObject.transform.position).z > 0)
                {
                    isChase = true;
                    cubeCore = hit.rigidbody.gameObject;
                }
                else
                    cubeCore = null;
                    */
                if (cubeCore != null)
                    isAim = true;
                else
                    isAim = false;

                if (isThrow)
                {
                    return cubeCore.transform.position + Random.insideUnitSphere;
                    List<GameObject> cubesAttached = new List<GameObject>();
                    cubesAttached.Add(cubeCore);
                    foreach (GameObject cubeAttached in cubeCore.GetComponent<CubeMonster>().cubesAttached)
                    {
                        if (cubeAttached != null)
                            cubesAttached.Add(cubeAttached);
                    }
                    
                    return cubesAttached[Random.Range((int)0, (int)cubesAttached.Count)].transform.position;
                }
                else if (isChase)
                {
                    /*List<GameObject> cubesAttached = new List<GameObject>();
                    cubesAttached.Add(cubeCore);
                    foreach (GameObject cubeAttached in cubeCore.GetComponent<MonsterChase>().cubesAttached)
                    {
                        if (cubeAttached != null)
                            cubesAttached.Add(cubeAttached);
                    }

                    GameObject aimAtCube = cubesAttached[Random.Range((int)0, (int)cubesAttached.Count)];
                    */
                    GameObject aimAtCube = cubeCore;

                    float targetCubeSpeed = cubeCore.GetComponent<Rigidbody>().velocity.magnitude;
                    Vector3 targetCubeDirection = aimAtCube.GetComponent<Rigidbody>().velocity;
                    float distance = Vector3.Distance(aimAtCube.transform.position, cube.transform.position);

                    float aimTimeInFuture = targetCubeSpeed / maxSpeed;
                    return aimAtCube.transform.position + targetCubeDirection.normalized * distance * aimTimeInFuture * Random.Range(0.7f, 1.0f);
                }
                else
                    return transform.position + Camera.main.transform.rotation * aimOffsetGrabbed;
            }
            else
                return transform.position + Camera.main.transform.rotation * aimOffsetGrabbed;
        }
        else
        {
            isAim = false;
            return transform.position + Camera.main.transform.rotation * aimOffsetGrabbed;
        }
    }

    public void launchCube(GameObject cube, float duration, Vector3 targetPoint, float power, float maxSpeed, float activeTime, bool isAim1, int skillType)
    {
        //cube.GetComponent<Rigidbody>().velocity = cube.GetComponent<Rigidbody>().velocity.normalized * Mathf.Min(cube.GetComponent<Rigidbody>().velocity.magnitude, Mathf.Sqrt(cube.GetComponent<Rigidbody>().velocity.magnitude));
        cube.GetComponent<Rigidbody>().velocity = Vector3.zero;
        cube.GetComponent<ColorCube>().addStateAccelerate(this.gameObject, duration, targetPoint, power, maxSpeed, activeTime, skillType, true, isAim1);
        removeCubeFromGrab(cube, true);
        
        foreach (GameObject cubeGrabbed in grabbedCubes)
        {
            cubeGrabbed.GetComponent<ColorCube>().lightCountChanged = true;
        }
    }

    public bool addCubeToGrab(GameObject cube, float duration, float power, float minSpeed, float maxSpeed, float keepRadius, float keepFactor)
    {
        if (cube.GetComponent<ColorCube>().grabbedPlayerCanBeGrabbed && grabbedCubes.Count < maxCubes)
        {
            if (!grabbedCubes.Contains(cube))
            {
                // Debug.Log("trying to add " + cube.name);
                totalGrabbed++;
                grabbedCubes.Add(cube);
                grabbedTimes.Add(duration + Time.time);
            }
            else
            {
                grabbedTimes[grabbedCubes.IndexOf(cube)] = Time.time + duration;
            }
            cube.GetComponent<ColorCube>().addStateGrabbedByPlayer(duration, power, minSpeed, maxSpeed, keepRadius, keepFactor);
            return true;
        }
        foreach(GameObject cubeGrabbed in grabbedCubes)
        {
            cubeGrabbed.GetComponent<ColorCube>().lightCountChanged = true;
        }
        return false;
    }

    public void removeCubeFromGrab(GameObject cube, bool changeCubeState)
    {
        //Debug.Log("removed " + cube);
        if (grabbedCubes.Contains(cube))
        {
            if (changeCubeState)
                cube.GetComponent<ColorCube>().removeStateGrabbedByPlayer(false);
            grabbedTimes.RemoveAt(grabbedCubes.IndexOf(cube));
            grabbedCubes.RemoveAt(grabbedCubes.IndexOf(cube));
        }
        else
            ;// Debug.Log("Error: tried to remove cube from grabbed, that was not in the list!");
    }

    public void removeAllCubesFromGrabbed()
    {
        for (int i = grabbedCubes.Count - 1; i >= 0; i--)
            removeCubeFromGrab(grabbedCubes[i], true);
    }

    public void logGrabbed()
    {
        if(currentLogGrabbedCounter < Time.time)
        {
            logGrabbedCount += grabbedCubes.Count;
            currentLogGrabbedCounter = grabbedInterval + Time.time;
        }
    }

    // Update is called once per frame
    void Update ()
    {
        manageGrabTimes();
    }
}
