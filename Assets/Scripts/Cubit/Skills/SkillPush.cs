using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillPush : MonoBehaviour
{

    [Header("----- SETTINGS -----")]
    public float cooldown;
    public float pressForMaxSeconds;
    public bool launchCubesSuccessively;

    [Header("--- (Aim) ---")]
    public LayerMask layerMask;
    public bool aimHelpActivatedGrabbed;
    public bool aimHelpActivatedFree;

    [Header("--- (Grabbed) ---")]
    public bool canShootFromGrabbed;
    public float chanceOnThrowGrabbed;
    public float durationGrabbed;
    //public float radiusGrabbed;
    public float powerGrabbed;
    public float maxSpeedGrabbed;
    public Vector3 aimOffsetGrabbed;
    public float activateDurationGrabbed;
    public float activateDurationRandomBonusGrabbed;

    [Header("--- (Free) ---")]
    public bool canShootFromNotGrabbed;
    public float chanceOnThrowFree;
    public float durationFree;
    public float radiusFree;
    public float powerFree;
    public float maxSpeedFree;
    public Vector3 aimOffsetFree;
    public float activateDurationFree;
    public float activateDurationRandomBonusFree;
    public bool inCircleInFront;
    public float inCircleInFrontRadius;
    public float inFrontZBonus;
    public bool inCircleAround;
    public float inCircleAroundRadius;
    public bool inHemisphere;
    public float inHemisphereRadius;

    [Header("----- DEBUG -----")]
    public float currentRadius;
    public float currentCooldown;
    //public float currentDurotation;
    public bool isOnCooldown;
    public float pressingFor;
    //public List<GameObject> waves; 

    [Header("--- (Log) ---")]
    public bool isAim;
    public int pushedNotFromGrabbedTotal;

    // Use this for initialization
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        getInput();

        if (currentCooldown < cooldown && isOnCooldown)
            currentCooldown += Time.deltaTime;
        if (currentCooldown > cooldown)
        {
            isOnCooldown = false;
            currentCooldown = 0;
        }
        //if (isOnDurotation)
            //activateSkill();
    }

    void activateSkill()
    {
        currentCooldown = 0;
        isOnCooldown = true;
        if (canShootFromGrabbed && GetComponent<GrabSystem>().grabbedCubes.Count > 0)
        {
            //for (int i = 0; i < GetComponent<GrabSystem>().grabbedCubes.Count; i++)
            {
                //if (Random.Range(0f, 1f) < chanceOnThrowGrabbed)
                {
                    //Vector3 forceDirection = powerGrabbed * direction.normalized; // * (1 - (transform.position - GetComponent<WeaponSystem>().grabbedCubes[i].transform.position).magnitude / (radius == 0 ? 1 : radius) * 1f);
                    if(launchCubesSuccessively)
                        GetComponent<GrabSystem>().launchCubesSuccessively(durationGrabbed, powerGrabbed, maxSpeedGrabbed, activateDurationGrabbed + Random.Range(0, activateDurationRandomBonusGrabbed), layerMask, aimOffsetGrabbed, aimHelpActivatedGrabbed);
                    else
                        GetComponent<GrabSystem>().launchCubes(durationGrabbed, powerGrabbed, maxSpeedGrabbed, activateDurationGrabbed + Random.Range(0, activateDurationRandomBonusGrabbed), layerMask, aimOffsetGrabbed, aimHelpActivatedGrabbed);
                }
            }
        }
        else if(canShootFromNotGrabbed)
        {
            List<Collider> col = new List<Collider>();
            float frontRadiusActual = inCircleInFrontRadius == 0 ? radiusFree : inCircleInFrontRadius;
            if (inCircleInFront)
            {
                Collider[] colFront = Physics.OverlapSphere(transform.position + Camera.main.transform.forward * (frontRadiusActual + inFrontZBonus), frontRadiusActual);
                for (int i = 0; i < colFront.Length; i++)
                    col.Add(colFront[i]);
            }

            float aroundRadiusActual = inCircleAroundRadius == 0 ? radiusFree : inCircleAroundRadius;
            if (inCircleAround)
            {
                Collider[] colAround = Physics.OverlapSphere(transform.position, aroundRadiusActual);
                for (int i = 0; i < colAround.Length; i++)
                    col.Add(colAround[i]);
            }

            float hemisphereRadiusActual = inHemisphereRadius == 0 ? radiusFree : inHemisphereRadius;
            if (inHemisphere)
            {
                Collider[] colHemisphere = Physics.OverlapSphere(transform.position, hemisphereRadiusActual);
                for (int i = 0; i < colHemisphere.Length; i++)
                {
                    if (transform.InverseTransformPoint(colHemisphere[i].gameObject.transform.position).z > 0)
                        col.Add(colHemisphere[i]);

                }
            }


            currentRadius = Mathf.Max(radiusFree, frontRadiusActual + inFrontZBonus, aroundRadiusActual, hemisphereRadiusActual);

            List<GameObject> cubes = new List<GameObject>();
            for (int i = 0; i < col.Count; i++)
            {
                if (col[i].gameObject.GetComponent<ColorCube>() != null && !col[i].gameObject.GetComponent<ColorCube>().isGrabbedByPlayer)
                {
                    cubes.Add(col[i].gameObject);
                }
            }
            
            int pushed = 0;
            for (int i = 0; i < cubes.Count; i++)
            {
                if (cubes[i].GetComponent<ColorCube>().activePlayerCanBeActive)
                {
                    pushedNotFromGrabbedTotal++;
                    pushed++;
                    //cubes[i].GetComponent<Rigidbody>().velocity = cubes[i].GetComponent<Rigidbody>().velocity.normalized * Mathf.Min(cubes[i].GetComponent<Rigidbody>().velocity.magnitude, Mathf.Sqrt(cubes[i].GetComponent<Rigidbody>().velocity.magnitude));
                    cubes[i].GetComponent<Rigidbody>().velocity = Vector3.zero;
                    Vector3 targetPosition = calculateAimTargetPoint(cubes[i]);
                    cubes[i].GetComponent<ColorCube>().addStateAccelerate(this.gameObject, durationFree, targetPosition, powerFree, maxSpeedFree, activateDurationFree + Random.Range(0, activateDurationRandomBonusFree), 0, false, isAim);
                }
            }
            if (GameObject.Find("LogInformationObject") != null && GameObject.Find("LogInformationObject").GetComponent<Log>() != null)
                GameObject.Find("LogInformationObject").GetComponent<Log>().logRegisterSkillPush(pushed, false, isAim);
            else if (GameObject.Find("GeneralScriptObject").GetComponent<Log>())
                GameObject.Find("GeneralScriptObject").GetComponent<Log>().logRegisterSkillPush(pushed, false, isAim);
        }
    }

    Vector3 calculateAimTargetPoint(GameObject cube)
    {
        //RaycastHit hit;
        if (aimHelpActivatedFree/* && Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 500f, layerMask)*/)
        {
            GameObject cubeCore = GetComponent<Aim>().m_aimingAt;
            if(cubeCore != null)
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

                if (cubeCore.GetComponent<CubeMonster>().isAlive)
                {
                    isThrow = true;
                    return cubeCore.transform.position;
                    List<GameObject> cubesAttached = new List<GameObject>();
                    cubesAttached.Add(cubeCore);
                    foreach (GameObject cubeAttached in cubeCore.GetComponent<CubeMonster>().cubesAttached)
                    {
                        if (cubeAttached != null)
                            cubesAttached.Add(cubeAttached);
                    }

                    return cubesAttached[Random.Range((int)0, (int)cubesAttached.Count)].transform.position;
                }
                else if (cubeCore.GetComponent<MonsterChase>().isAlive)
                {
                    isChase = true;
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

                    float targetCubeSpeed = aimAtCube.GetComponent<Rigidbody>().velocity.magnitude;
                    Vector3 targetCubeDirection = aimAtCube.GetComponent<Rigidbody>().velocity;
                    float distance = (cube.transform.position - aimAtCube.transform.position).magnitude;

                    float aimTimeInFuture = targetCubeSpeed / maxSpeedFree;
                    return aimAtCube.transform.position + targetCubeDirection.normalized * distance * aimTimeInFuture/* * Random.Range(0.8f, 1.4f)*/;
                }
                else
                    return transform.position + Camera.main.transform.rotation * aimOffsetFree;
            }
            else
                return transform.position + Camera.main.transform.rotation * aimOffsetFree;
        }
        else
        {
            isAim = false;
            return transform.position + Camera.main.transform.rotation * aimOffsetFree;
        }
        
    }


    void getInput()
    {
        if (Input.GetMouseButton(0) ||Input.GetMouseButtonUp(0) && !GameObject.Find("GeneralScriptObject").GetComponent<Options>().isFreeze)
            pressingFor += Time.deltaTime;
        else
            pressingFor = 0;


        if (!isOnCooldown && ((Input.GetMouseButtonUp(1) && pressingFor < pressForMaxSeconds) || Input.GetButton("ButtonB") || Input.GetButton("RBumper")) && !GameObject.Find("GeneralScriptObject").GetComponent<Options>().isFreeze)
        {
            activateSkill();
        }
    }
}
