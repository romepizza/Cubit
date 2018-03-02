using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillGather : MonoBehaviour
{
    [Header("----- SETTINGS -----")]

    [Header("--- (Active) ---")]
    public bool useActive;
    public int maxGrabPerActivisionActive;
    public float cooldownActive;
    public float radiusActive;

    [Header("--- (Passive) ---")]
    public bool usePassive;
    public float cooldownPassive;
    public int maxGrabPerActivisionPassive;
    public float radiusPassive;
    public int grabbedPerGrabPassive;

    [Header("--- (Mvement) ---")]
    public float duration;
    public float keepRadius;
    public float keepFactor;
    public float power;
    public float maxSpeed;
    public float minSpeed;
    public Vector3 aimOffset;
    public bool offsetPlaneY;
    //public float lightDurotation;
    //public float lightDurotationRandomBonus;
    public float maxOffset;
    public float offsetBonusPerCube;
    //public float grabDurotation;

    [Header("----- DEBUG -----")]
    public float currentCooldownActive;
    public float currentDurotation;
    public bool isOnCooldownActive;
    public bool isOnDurotation;
    public Collider[] colliders;
    public bool activateAbilityOnce;
    public float cooldownFinishTimePassive;


    // Use this for initialization
    void Start ()
    {

	}

    // Update is called once per frame
    void FixedUpdate()
    {
        getInput();
        if (usePassive)
            passiveSkillEffect();


    }

    void manageCounter()
    {
        if (currentCooldownActive < cooldownActive && isOnCooldownActive)
            currentCooldownActive += Time.deltaTime;
        if (currentCooldownActive > cooldownActive)
        {
            isOnCooldownActive = false;
            currentCooldownActive = 0;
        }


        if (activateAbilityOnce)
        {
            activateSkill();
            activateAbilityOnce = false;
        }

        if (currentDurotation < duration && isOnDurotation)
            currentDurotation += Time.deltaTime;
        if (currentDurotation > duration)
        {
            isOnDurotation = false;
            currentDurotation = 0;
        }

        if (isOnDurotation)
            ;//skillEffect();
    }

    void passiveSkillEffect()
    {
        if (cooldownFinishTimePassive < Time.time)
        {
            int grabbed = 0;
            colliders = Physics.OverlapSphere(transform.position, radiusPassive);
            foreach (Collider col in colliders)
            {
                if (grabbed >= maxGrabPerActivisionPassive)
                    break;
                if (col.gameObject.GetComponent<ColorCube>() != null)
                {
                    if (col.gameObject.GetComponent<Rigidbody>() != null)
                    {
                        bool b = GetComponent<GrabSystem>().addCubeToGrab(col.gameObject, duration, power, minSpeed, maxSpeed, keepRadius, keepFactor);
                        if (b)
                            grabbed++;
                    }
                }
            }
            cooldownFinishTimePassive = cooldownPassive + Time.time;
        }
    }

    void activateSkill()
    {
        int grabbed = 0;
        colliders = Physics.OverlapSphere(transform.position, radiusActive);
        foreach (Collider col in colliders)
        {
            if (grabbed >= maxGrabPerActivisionActive)
                break;
            if (col.gameObject.GetComponent<ColorCube>() != null)
            {
                if (col.gameObject.GetComponent<Rigidbody>() != null)
                {
                    bool b = GetComponent<GrabSystem>().addCubeToGrab(col.gameObject, duration, power, minSpeed, maxSpeed, keepRadius, keepFactor);
                    if(b)
                        grabbed++;
                    //col.gameObject.GetComponent<ColorCube>().activateLight((lightDurotation + Random.Range(0, lightDurotationRandomBonus) * (1 - (transform.position - col.gameObject.transform.position).magnitude / (radius == 0 ? 1 : radius) * 0.5f)));
                }
            }
        }
    }

    public Vector3 getTargetPositionSoft()
    {
        return transform.position + /*Camera.main.transform.rotation **/ aimOffset;
    }

    public Vector3 getTargetPositionHard()
    {
        Vector3 offsetFinal = Camera.main.transform.rotation * new Vector3(aimOffset.x, 0, aimOffset.z);

        if (offsetPlaneY)
            return transform.position + new Vector3(offsetFinal.x, 0, offsetFinal.z) +  new Vector3(0, aimOffset.y, 0);
        else
            return transform.position + Camera.main.transform.rotation * aimOffset;
    }

    void skillEffect()
    {
        foreach(GameObject cube in GetComponent<GrabSystem>().grabbedCubes)
        {
            //if(Random.Range(0f, 1f) < 0.5f)
            {
                if (cube != null)
                {
                    Vector3 direction = (transform.position + Camera.main.transform.rotation * new Vector3(aimOffset.x, aimOffset.y, aimOffset.z)) - cube.transform.position;
                    if (cube.GetComponent<Rigidbody>() != null)
                    {
                        Vector3 forceDirection = power * direction.normalized * (1 - (transform.position + Camera.main.transform.rotation * new Vector3(aimOffset.x, aimOffset.y, aimOffset.z) -cube.transform.position).magnitude / (keepRadius == 0 ? 1 : keepRadius) * .4f);
                        forceDirection = forceDirection.normalized * Mathf.Clamp(forceDirection.magnitude, minSpeed, maxSpeed);
                        //Debug.DrawRay(cube.transform.position, direction, Color.white);
                        cube.gameObject.GetComponent<Rigidbody>().AddForce(forceDirection, ForceMode.Acceleration);
                    }
                }   
            }
        }
    }

    void getInput()
    {
        if (useActive && Input.GetMouseButton(1) && !GameObject.Find("GeneralScriptObject").GetComponent<Options>().isFreeze)
        {
            if (!isOnCooldownActive)
            {
                activateAbilityOnce = true;
                isOnDurotation = true;
                currentCooldownActive = 0;
                isOnCooldownActive = true;
            }
        }
    }
}
