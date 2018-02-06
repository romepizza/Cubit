using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterChase : MonoBehaviour
{
    public bool createEnemy;
    [Header("----- SETTINGS -----")]

    [Header("--- (Cube) ---")]

    [Header("--- (Hp) ---")]
    public int maxLife;
    public int minLife;
    public int maxCubes;
    public int startCubes;

    [Header("--- (Form) ---")]
    public float radiusCircle;
    public float minRadiusCircle;

    [Header("--- (Movement)---")]
    public float movementMaxSpeed;
    public float movementMovePower;
    public float minAngleMovement;
    public float deviationPower;
    public Vector3 sphereOffset;
    public float movementOffsetRadius;
    public float newPointTime;
    public bool movementAffectsAttachedCubes;
    public float movementAttachedFactor;


    [Header("--- (Grab) ---")]
    public float grabCooldown;
    public float grabRadius;
    public float grabKeepRadius;
    public float grabKeepFactor;
    public float grabDuration;
    public float grabPower;
    public float grabMaxSpeed;
    public float grabOverlapSphereRadiusStep;
    public float grabLessCooldownPerAttachedCube;
    public float minCooldownGrab;
    public bool pickAvailableNearestCube;
    public bool grabCubesToNearestPosition;


    [Header("----- DEBUG -----")]
    public bool isAlive;
    public int maxCubesActual;
    public int currentCubeLife;
    public int cubesAttachedCount;
    public Vector3 targetShootPosition;
    public Vector3[] cubeWantPositionsLocal;
    public Vector3[] cubeWantPositionsWorld;
    public GameObject[] cubesAttached;
    public List<int> freeCubePositionsIndices;
    public float lightFactor;
    [Header("--- (Shoot) ---")]
    public float shootCooldownActual;
    public float grabCooldownActual;


    [Header("--- (Cooldowns) ---")]
    public float finishTimeGrab;
    public float finishTimeShoot;
    public float finishNewOffset;
    public bool lineRendererAttached;

    private GameObject player;
    private Rigidbody rb;

    void Start()
    {
        player = GameObject.Find("PlayerDrone");
        rb = GetComponent<Rigidbody>();
        for (int i = 0; i < cubeWantPositionsLocal.Length; i++)
        {
            //Debug.DrawLine(transform.position, transform.position + cubeWantPositionsLocal[i], Color.green, 10f);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (createEnemy)
        {
            createMonster();
            createEnemy = false;
        }

        if (isAlive)
        {
            for (int i = 0; i < cubeWantPositionsLocal.Length; i++)
            {
                //Debug.Log(transform.position + Camera.main.transform.rotation * cubeWantPositionsLocal[i]);
                cubeWantPositionsWorld[i] = transform.position + cubeWantPositionsLocal[i];
            }
            manageHp();
            if (isAlive)
            {
                manageLineRenderer();
                manageGrab();
                manageMovement();
            }
        }
    }

    void manageMovement()
    {
        Vector3 playerPosition = GameObject.Find("PlayerDrone").transform.position;

        Vector3 moveToPoint = playerPosition + sphereOffset;
        if(finishNewOffset < Time.time)
        {
            sphereOffset = Random.onUnitSphere * movementOffsetRadius;
            finishNewOffset = Time.time + newPointTime;
        }
        float rangeFactor = 1;
        Vector3 forceDirection = movementMovePower * (moveToPoint - transform.position).normalized * rangeFactor;


        Vector3 force = Vector3.zero;
        float angle = Vector3.Angle(forceDirection, rb.velocity);
        if (angle > minAngleMovement && rb.velocity.magnitude > 5)
        {
            Vector3 deviationDirection = (forceDirection - rb.velocity).normalized;
            Vector3 deviation = deviationDirection * movementMovePower * deviationPower;
            force = deviation;
        }
        else
            force = forceDirection;
        Debug.DrawRay(transform.position, forceDirection, Color.yellow);

        
        rb.AddForce(force, ForceMode.Acceleration);

        if(movementAffectsAttachedCubes)
        {
            foreach(GameObject cube in cubesAttached)
            {
                if(cube != null && rb.velocity.magnitude < movementMaxSpeed)
                    cube.GetComponent<Rigidbody>().AddForce(force * movementAttachedFactor, ForceMode.Acceleration);
            }
        }


        rb.velocity = rb.velocity.normalized * Mathf.Clamp(rb.velocity.magnitude, 0, movementMaxSpeed);
    }

    void manageHp()
    {
        if (currentCubeLife < minLife || currentCubeLife == 0)
            die(true, true);
    }

    public void loseHp(GameObject cubeHit, int damage)
    {
        currentCubeLife -= damage;

        lightFactor = (float)currentCubeLife / (float)maxLife;
        float intensityMaterial = 1.0f * lightFactor;                       // if changed, change at CubeMonster::loseHp() and CubeColor::AddStateCore() aswell
        float lightIntensity = 2.0f - (2.0f * (1 - lightFactor) * 1f);      // if changed, change at CubeMonster::loseHp() and CubeColor::AddStateCore() aswell
        float range = 2.0f - (2.0f * (1 - lightFactor) * 1f);               // if changed, change at CubeMonster::loseHp() and CubeColor::AddStateCore() aswell
        float intensityTrail = 1.0f * lightFactor;                          // if changed, change at CubeMonster::loseHp() and CubeColor::AddStateCore() aswell
        GetComponent<ColorCube>().activateLight(GetComponent<ColorCube>().materialEnemyCore, 100000, intensityMaterial, lightIntensity, range, intensityTrail);
        

        maxCubesActual = Mathf.Min(currentCubeLife, maxCubes);
        if (cubeHit != null)
        {
            removeCubeFromAttached(cubeHit, -1, true);
            for (int i = 1; i < damage; i++)
            {
                removeCubeFromAttached(null, -1, true);
            }
        }
        else
        {
            for (int i = 0; i < damage; i++)
            {
                removeCubeFromAttached(null, -1, true);
            }
        }
    }



    void manageGrab()
    {
        if (finishTimeGrab < Time.time && cubesAttachedCount < maxCubesActual)
        {
            grabCube();
            grabCooldownActual = Mathf.Max(grabCooldown - (grabCooldown * (maxCubesActual - cubesAttachedCount) * grabLessCooldownPerAttachedCube), minCooldownGrab);
            finishTimeGrab = grabCooldownActual + Time.time + Random.Range(0f, 0.05f);
        }
    }

    void grabCube()
    {
        GameObject grabbedCube = null;
        {
            for (float sphereRadius = grabOverlapSphereRadiusStep; sphereRadius < grabRadius; sphereRadius += grabOverlapSphereRadiusStep)
            {
                Collider[] colliders = Physics.OverlapSphere(transform.position, sphereRadius);
                float nearestDistance = grabRadius;
                bool found = false;
                for (int i = 0; i < colliders.Length; i++)
                {
                    bool isValid = true;
                    for (int j = 0; j < cubesAttached.Length; j++)
                    {
                        if (cubesAttached[j] == colliders[i].gameObject || colliders[i].gameObject.GetComponent<ColorCube>() == null || !colliders[i].gameObject.GetComponent<ColorCube>().attachedToEnemyChaseCanBeAttached)
                            isValid = false;
                    }
                    if (isValid)
                    {
                        float distance = (colliders[i].gameObject.transform.position - transform.position).magnitude;
                        if (distance < nearestDistance && colliders[i].GetComponent<ColorCube>() != null)
                        {
                            nearestDistance = distance;
                            grabbedCube = colliders[i].gameObject;
                            found = true;
                        }
                    }
                }
                if (found)
                    break;
            }

        }

        if (grabbedCube != null)
        {
            if (grabCubesToNearestPosition)
            {
                int positionIndex = -1;
                float nearestDistance = 10000;
                for (int i = 0; i < cubeWantPositionsWorld.Length; i++)// Vector3 position in cubeWantPositions)
                {
                    if (freeCubePositionsIndices.Contains(i))
                    {
                        float distance = (cubeWantPositionsWorld[i] - grabbedCube.transform.position).magnitude;
                        if (distance < nearestDistance)
                        {
                            nearestDistance = distance;
                            positionIndex = i;
                        }
                    }
                }
                if (positionIndex >= 0 && cubesAttached[positionIndex] != null)
                    removeCubeFromAttached(null, positionIndex, true);

                addCubeToAttached(grabbedCube, positionIndex);
            }
            else
            {
                addCubeToAttached(grabbedCube, -1);
            }
        }
    }

    void addCubeToAttached(GameObject cube, int index)
    {
        if (cube != null)
        {
            int indexActual = -1;
            if (index >= 0)
            {
                if (cubesAttached[index] != null)
                    removeCubeFromAttached(null, index, true);

                indexActual = index;
                cubesAttachedCount++;
                cubesAttached[index] = cube;
                freeCubePositionsIndices.Remove(index);
            }
            else
            {
                int randomIndex = freeCubePositionsIndices[Random.Range(0, freeCubePositionsIndices.Count)];

                indexActual = randomIndex;
                freeCubePositionsIndices.Remove(randomIndex);
                cubesAttachedCount++;
                cubesAttached[randomIndex] = cube;
            }
            //Debug.Log(name + " is Attaching to " + cube.name);
            cube.GetComponent<ColorCube>().addStateAttachedToEnemyChase(this.gameObject, grabDuration, grabPower, grabMaxSpeed, indexActual, grabKeepRadius, grabKeepFactor);
            cube.GetComponent<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity;
        }
        else
            Debug.Log("addCubeToAttached(...) Error: GameObject cube is null!");
    }

    public void removeCubeFromAttached(GameObject cube, int index, bool setToInactive)
    {
        //  if (cube != null && cube.GetComponent<ColorCube>() != null && cube.GetComponent<ColorCube>().isCore)
        //Debug.Log("Trying to remove Core");
        if (cubesAttachedCount > 0)
        {
            if (cube == null && index >= 0 && cubesAttached[index] != null)
            {
                cubesAttachedCount--;
                cubesAttached[index].GetComponent<ColorCube>().removeStateAttachedToEnemyChase(false);
                if (cubesAttached[index].GetComponent<ColorCube>().isActiveByEnemy)
                    cubesAttached[index].GetComponent<ColorCube>().addStateInactive();
                cubesAttached[index] = null;
                freeCubePositionsIndices.Add(index);
            }
            else if (cube != null && index < 0)
            {
                for (int i = 0; i < cubesAttached.Length; i++)
                {
                    if (cubesAttached[i] == cube)
                    {
                        cubesAttachedCount--;
                        cubesAttached[i] = null;
                        freeCubePositionsIndices.Add(i);
                        cube.GetComponent<ColorCube>().removeStateAttachedToEnemyChase(false);
                        if (cube.GetComponent<ColorCube>().isActiveByEnemy)
                            cube.GetComponent<ColorCube>().addStateInactive();
                        break;
                    }
                }
                //Debug.Log("to remove cube not fund!");
            }
            else if (cube == null && index < 0)
            {
                if (cubesAttached.Length > 0)
                {
                    int tries = 10000;
                    while (tries > 0)
                    {
                        int randomIndex = Random.Range(0, cubesAttached.Length);
                        if (cubesAttached[randomIndex] != null)
                        {
                            cubesAttachedCount--;
                            cubesAttached[randomIndex].GetComponent<ColorCube>().removeStateAttachedToEnemyChase(false);
                            if (cubesAttached[randomIndex].GetComponent<ColorCube>().isActiveByEnemy)
                                cubesAttached[randomIndex].GetComponent<ColorCube>().addStateInactive();
                            cubesAttached[randomIndex] = null;
                            freeCubePositionsIndices.Add(randomIndex);
                            break;
                        }
                        tries--;
                    }
                }
            }
            else
            {

                //Debug.Log("removeCubeFromAttached(...) Error: Gameobject cube and int index are not set properly!");
            }
        }
        else
        {
            //Debug.Log("Note: tried to remove attached cube from an empty list!");
        }
    }


    public Vector3 getTargetPosition(int index)
    {
        if (freeCubePositionsIndices.Contains(index))
            Debug.Log("Unwanted assertion!");

        return cubeWantPositionsWorld[index];
    }


    public void createMonster()
    {
        currentCubeLife = maxLife;
        maxCubesActual = maxCubes;
        cubeWantPositionsLocal = new Vector3[maxCubesActual];
        cubeWantPositionsWorld = new Vector3[maxCubesActual];
        cubesAttached = new GameObject[maxCubesActual];
        freeCubePositionsIndices = new List<int>();
        for (int i = 0; i < cubeWantPositionsLocal.Length; i++)
            freeCubePositionsIndices.Add(i);

        for (int i = 0; i < cubeWantPositionsLocal.Length; i++)
        {
            cubeWantPositionsLocal[i] = Random.insideUnitSphere * radiusCircle;
            cubeWantPositionsLocal[i] = cubeWantPositionsLocal[i].normalized * Mathf.Max(cubeWantPositionsLocal[i].magnitude, minRadiusCircle);
        }
        isAlive = true;
        for (int i = 0; i < startCubes; i++)
            grabCube();

        if (GetComponent<ColorCube>() != null)
        {
            GetComponent<ColorCube>().addStateCore(6000);
        }
        addLineRenderer();
    }

    void addLineRenderer()
    {
        LineRenderer lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.startColor = Color.green;
        lineRenderer.endColor = Color.yellow;
        lineRenderer.startWidth = 0.5f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, transform.position);
        lineRenderer.material = GetComponent<TrailRenderer>().material;

        lineRendererAttached = true;
    }

    void manageLineRenderer()
    {
        if (lineRendererAttached)
        {
            Vector3 direction = player.transform.position - transform.position;
            float angle = Vector3.Angle(rb.velocity, direction);
            float distance = direction.magnitude;
            LineRenderer lineRenderer = gameObject.GetComponent<LineRenderer>();
            if (distance < 150 && angle < 180)
            {
                lineRenderer.SetPosition(0, transform.position);
                float length = 100;
                lineRenderer.SetPosition(1, transform.position + rb.velocity.normalized * length);
            }
            else
            {
                lineRenderer.SetPosition(0, transform.position);
                lineRenderer.SetPosition(1, transform.position);
            }
        }
    }

    void removeLineRenderer()
    {
        Destroy(gameObject.GetComponent<LineRenderer>());
        lineRendererAttached = false;
    }

    public void die(bool removeStateCore, bool registerDeath)
    {
        for (int i = 0; i < cubesAttached.Length; i++)
            removeCubeFromAttached(null, i, true);
        if (GetComponent<ColorCube>() != null)
        {
            if (removeStateCore)
                GetComponent<ColorCube>().removeStateCore(false);
            GetComponent<ColorCube>().addStateInactive();
        }
        else
            Destroy(this.gameObject, 0.1f);

        removeLineRenderer();

        if (registerDeath && transform.parent.gameObject.GetComponent<MonsterManager>() != null)
        {
            transform.parent.gameObject.GetComponent<MonsterManager>().registerMonsterChaseDeath();
        }
        isAlive = false;
    }

    void OnDrawGizmos()
    {
        if (createEnemy)
        {
            //createMonster();
            //createEnemy = false;
        }
    }
}
