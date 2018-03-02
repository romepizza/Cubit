using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillGattling : MonoBehaviour
{
    [Header("----- SETTINGS -----")]
    public bool canShootFromGrabbed;
    public bool canShootFromNotGrabbed;
    public float cooldownNormal;
    public float cooldownGrabbed;
    public float minTimePressed;
    public float duration;
    public float power;
    public float maxSpeed;
    public float radius;
    public Vector3 aimOffset;
    public float activateDuration;
    public float activateDurationRandomBonus;

    public bool inCircleInFront;
    public float inCircleInFrontRadius;
    public float inFrontZBonus;
    public bool inCircleAround;
    public float inCircleAroundRadius;
    public bool inHemisphere;
    public float inHemisphereRadius;

    [Header("--- (Aim) ---")]
    public LayerMask layerMask;
    public bool aimHelpActivatedGrabbed;
    public bool aimHelpActivatedFree;

    [Header("----- DEBUG -----")]
    public float cooldownActual;
    public float currentRadius;
    public float currentCooldown;
    public bool isOnCooldown;
    public float pressingFor;

    [Header("--- Controller Input ---")]
    public bool autoShoot;
    public bool inputToggle;

    [Header("--- (Log) ---")]
    public bool isAim;
    public int gattlingedFromGrabbedTotal;
    public int gattlingedNotFromGrabbedTotal;

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        getInput();

        if (currentCooldown < cooldownActual && isOnCooldown)
            currentCooldown += Time.deltaTime;
        if (currentCooldown > cooldownActual)
        {
            isOnCooldown = false;
            currentCooldown = 0;
        }
    }

    void shoot()
    {
        currentCooldown = 0;
        isOnCooldown = true;
        
        GameObject cube = null;
        List<Collider> col = new List<Collider>();

        bool fromGrab = false;
        if (canShootFromGrabbed && GetComponent<GrabSystem>().grabbedCubes.Count > 0 && cube == null)
        {
            float farest = -1;
            foreach (GameObject cubeGrabbed in GetComponent<GrabSystem>().grabbedCubes)
            {
                if (cubeGrabbed.GetComponent<ColorCube>() != null && (transform.position - cubeGrabbed.transform.position).magnitude > farest)
                {
                    farest = (transform.position - cubeGrabbed.transform.position).magnitude;
                    cube = cubeGrabbed;
                    fromGrab = true;
                }
            }
        }

        if (canShootFromNotGrabbed)
        {
            float frontRadiusActual = inCircleInFrontRadius == 0 ? radius : inCircleInFrontRadius;
            if (inCircleInFront && cube == null)
            {
                Collider[] colFront = Physics.OverlapSphere(transform.position + Camera.main.transform.forward * (frontRadiusActual + inFrontZBonus), frontRadiusActual);

                float nearest = 1000000;
                foreach (Collider collider in colFront)
                {
                    if (collider.gameObject.GetComponent<ColorCube>() != null && !collider.gameObject.GetComponent<ColorCube>().isGrabbedByPlayer && collider.gameObject.GetComponent<ColorCube>().activePlayerCanBeActive && (transform.position - collider.gameObject.transform.position).magnitude < nearest)
                    {
                        nearest = (transform.position - collider.gameObject.transform.position).magnitude;
                        cube = collider.gameObject;
                    }
                }
            }

            float hemisphereRadiusActual = inHemisphereRadius == 0 ? radius : inHemisphereRadius;
            if (inHemisphere && cube == null)
            {
                Collider[] colHemisphere = Physics.OverlapSphere(transform.position, hemisphereRadiusActual);
                float nearest = 1000000;
                foreach (Collider collider in colHemisphere)
                {
                    if (collider.gameObject.GetComponent<ColorCube>() != null && !collider.gameObject.GetComponent<ColorCube>().isGrabbedByPlayer && collider.gameObject.GetComponent<ColorCube>().activePlayerCanBeActive && (transform.position - collider.gameObject.transform.position).magnitude < nearest)
                    {
                        if (transform.InverseTransformPoint(collider.gameObject.transform.position).z > 0)
                        {
                            nearest = (transform.position - collider.gameObject.transform.position).magnitude;
                            cube = collider.gameObject;
                        }
                    }
                }
            }

            float aroundRadiusActual = inCircleAroundRadius == 0 ? radius : inCircleAroundRadius;
            if (inCircleAround && cube == null)
            {
                Collider[] colAround = Physics.OverlapSphere(transform.position, aroundRadiusActual);
                float nearest = 1000000;
                foreach (Collider collider in colAround)
                {
                    if (collider.gameObject.GetComponent<ColorCube>() != null && !collider.gameObject.GetComponent<ColorCube>().isGrabbedByPlayer && collider.gameObject.GetComponent<ColorCube>().activePlayerCanBeActive && (transform.position - collider.gameObject.transform.position).magnitude < nearest)
                    {
                        nearest = (transform.position - collider.gameObject.transform.position).magnitude;
                        cube = collider.gameObject;
                    }
                }
            }
        }

        if (cube != null)
        {
            cube.GetComponent<Rigidbody>().velocity = cube.GetComponent<Rigidbody>().velocity.normalized * Mathf.Min(cube.GetComponent<Rigidbody>().velocity.magnitude, Mathf.Sqrt(cube.GetComponent<Rigidbody>().velocity.magnitude));

            if (fromGrab)
            {
                Vector3 targetPosition = calculateAimTargetPoint(cube, aimHelpActivatedGrabbed);
                gattlingedFromGrabbedTotal++;

                GetComponent<GrabSystem>().launchCube(cube, duration, targetPosition, power, maxSpeed, activateDuration + Random.Range(0, activateDurationRandomBonus), isAim, 1);
            }
            else
            {
                Vector3 targetPosition = calculateAimTargetPoint(cube, aimHelpActivatedFree);
                gattlingedNotFromGrabbedTotal++;
                cube.GetComponent<ColorCube>().addStateAccelerate(this.gameObject, duration, targetPosition, power, maxSpeed, activateDuration + Random.Range(0, activateDurationRandomBonus), 1, false, isAim);

            }
            cooldownActual = (fromGrab) ? cooldownGrabbed : cooldownNormal;
        }
        //else
            //Debug.Log("no cube found");
    }

    Vector3 calculateAimTargetPoint(GameObject cube, bool aimHelpActivated)
    {
        if (aimHelpActivated)
        {
            GameObject cubeCore = GetComponent<Aim>().aimAtEnemy();
            if (cubeCore != null)
            {
                bool isThrow = cubeCore.GetComponent<CubeMonster>().isAlive;
                bool isChase = cubeCore.GetComponent<MonsterChase>().isAlive;
                isAim = true;
                
                if (isThrow)
                {
                    List<GameObject> cubesAttached = new List<GameObject>();
                    cubesAttached.Add(cubeCore);
                    foreach (GameObject cubeAttached in cubeCore.GetComponent<CubeMonster>().cubesAttached)
                    {
                        if (cubeAttached != null)
                            cubesAttached.Add(cubeAttached);
                    }

                    return cubeCore.transform.position; //cubesAttached[Random.Range((int)0, (int)cubesAttached.Count)].transform.position;
                }
                else if (isChase)
                {
                    GameObject aimAtCube = cubeCore;
                    float targetCubeSpeed = aimAtCube.GetComponent<Rigidbody>().velocity.magnitude;
                    Vector3 targetCubeDirection = aimAtCube.GetComponent<Rigidbody>().velocity;
                    float distance = (cube.transform.position - aimAtCube.transform.position).magnitude;

                    float aimTimeInFuture = targetCubeSpeed / maxSpeed;
                    return aimAtCube.transform.position + targetCubeDirection.normalized * aimTimeInFuture * distance * Random.Range(0.7f, 1f);
                }
                else
                {
                    isAim = false;
                    Debug.Log("Assertion error: Cube found was not a monster!");
                    return transform.position + Camera.main.transform.rotation * aimOffset;
                }
            }
            else
            {
                isAim = false;
                return transform.position + Camera.main.transform.rotation * aimOffset;
            }
        }
        else
        {
            isAim = false;
            return transform.position + Camera.main.transform.rotation * aimOffset;
        }
    }

    void getInput()
    {
        if (Input.GetButton("ButtonA"))
        {
            if (!inputToggle)
            {
                autoShoot = !autoShoot;
                inputToggle = true;
            }
        }
        else
            inputToggle = false;

        if ((Input.GetMouseButton(0)) && !GameObject.Find("GeneralScriptObject").GetComponent<Options>().isFreeze)
            pressingFor += Time.deltaTime;
        else
            pressingFor = 0;

        if (!isOnCooldown && (Input.GetButton("RBumper") || Input.GetMouseButton(0) || autoShoot) && /*pressingFor >= minTimePressed &&*/ !GameObject.Find("GeneralScriptObject").GetComponent<Options>().isFreeze)
        {
            shoot();
            if(Random.Range(0, 1) < 0.2f)
                shoot();
        }
    }
}
