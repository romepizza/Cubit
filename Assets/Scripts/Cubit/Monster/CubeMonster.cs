using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeMonster : MonoBehaviour
{
    public bool createEnemy;
    [Header("----- SETTINGS -----")]
    public int maxLife;
    public int minLife;
    public int maxCubes;
    public int startCubes;

    [Header("--- (Form) ---")]
    public float radiusCircle;
    public float minRadiusCircle;

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

    [Header("--- (Shoot) ---")]
    public float shootCooldown;
    public float shootDuration;
    public float shootPower;
    public float shootMaxSpeed;
    public float shootActivateEnemyDuration;
    public float shootActivateEnemyDurationRandomBonus;
    public float shootLessCooldownPerAttachedCube;
    public float minCooldownShoot;
    public float minCubesAttachedForShoot;
    public bool shotNearestCube;

    [Header("--- (Shoot Help) ---")]
    public bool shootLowerCubeSpeedBeforeLaunch;
    public bool shootInPlayerMoveDirection;
    public float shootInPlayerMoveDirectionMinRandom;
    public float shootInPlayerMoveDirectionMaxRandom;



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
    //public bool isOnCooldown;

	// Use this for initialization
	void Start ()
    {
        finishTimeShoot = Random.Range(0, 2 * shootCooldown);
        for (int i = 0; i < cubeWantPositionsLocal.Length; i++)
        {
           //Debug.DrawLine(transform.position, transform.position + cubeWantPositionsLocal[i], Color.green, 10f);

        }
    }
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        /*
        if (createEnemy)
        {
            createMonster();
            createEnemy = false;
        }
        */

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
                manageCounter();
                manageShoot();
                manageGrab();
                GetComponent<Rigidbody>().velocity = Vector3.zero;
            }
        }
    }

    void manageShoot()
    { 
        if(finishTimeShoot < Time.time && cubesAttachedCount > minCubesAttachedForShoot)
        {
            shoot();
            shootCooldownActual = Mathf.Max(shootCooldown - (shootCooldown * (cubesAttachedCount - minCubesAttachedForShoot) * shootLessCooldownPerAttachedCube), minCooldownShoot);
            finishTimeShoot = shootCooldownActual + Time.time + Random.Range(0f, 0.1f);
        }
    }

    void shoot()
    {
        int shootCubeIndex = -1;
        if (shotNearestCube)
        {
            float nearestDistance = 10000;
            for (var i = 0; i < cubesAttached.Length; i++)
            {
                if (cubesAttached[i] != null)
                {
                    float distance = (cubesAttached[i].transform.position - targetShootPosition).magnitude;
                    if (distance < nearestDistance)
                    {
                        nearestDistance = distance;
                        shootCubeIndex = i;
                    }
                }
            }
        }
        else
        {
            shootCubeIndex = freeCubePositionsIndices[Random.Range(0, freeCubePositionsIndices.Count)];
        }

        if(shootCubeIndex >= 0)
        {
            GameObject target = GameObject.Find("PlayerDrone");
            if (shootInPlayerMoveDirection)
            {
                float targetSpeed = target.GetComponent<Rigidbody>().velocity.magnitude;
                Vector3 targetDirection = target.GetComponent<Rigidbody>().velocity;
                //float distance = Vector3.Distance(cubesAttached[shootCubeIndex].transform.position, target.transform.position);

                float aimTimeInFuture = targetSpeed / shootMaxSpeed * 1.0f;
                targetShootPosition = target.transform.position + targetDirection * aimTimeInFuture * Random.Range(shootInPlayerMoveDirectionMinRandom, shootInPlayerMoveDirectionMaxRandom);
            }
            else
                targetShootPosition = target.transform.position;

            if (shootLowerCubeSpeedBeforeLaunch)
                cubesAttached[shootCubeIndex].GetComponent<Rigidbody>().velocity = cubesAttached[shootCubeIndex].GetComponent<Rigidbody>().velocity.normalized * Mathf.Sqrt(cubesAttached[shootCubeIndex].GetComponent<Rigidbody>().velocity.magnitude);

            cubesAttached[shootCubeIndex].GetComponent<ColorCube>().addStateAccelerate(this.gameObject, shootDuration, targetShootPosition, shootPower, shootMaxSpeed, shootActivateEnemyDuration + Random.Range(0f, shootActivateEnemyDurationRandomBonus), 3, false, false);
        }
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
        float intensityMaterial = 1.0f * lightFactor;                       // if changed, change at MonsterChase::loseHp() and CubeColor::AddStateCore() aswell
        float lightIntensity =  2.0f - (2.0f * (1 - lightFactor) * 1f);     // if changed, change at MonsterChase::loseHp() and CubeColor::AddStateCore() aswell
        float range =           2.0f - (2.0f * (1 - lightFactor) * 1f);     // if changed, change at MonsterChase::loseHp() and CubeColor::AddStateCore() aswell
        float intensityTrail = 1.0f * lightFactor;                          // if changed, change at MonsterChase::loseHp() and CubeColor::AddStateCore() aswell
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

    public void die(bool removeStateCore, bool registerDeath)
    {
        isAlive = false;
        for (int i = 0; i < cubesAttached.Length; i++)
            removeCubeFromAttached(null, i, true);
        if (GetComponent<ColorCube>() != null)
        {
            if(removeStateCore)
                GetComponent<ColorCube>().removeStateCore(false);
            GetComponent<ColorCube>().addStateInactive();
        }
        else
            Destroy(this.gameObject, 0.1f);

        if (registerDeath && transform.parent.gameObject.GetComponent<MonsterManager>() != null)
        {
            transform.parent.gameObject.GetComponent<MonsterManager>().registerMonsterThrowDeath();
        }
    }


    void manageGrab()
    {
        if(finishTimeGrab < Time.time && cubesAttachedCount < maxCubesActual)
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
                        if (cubesAttached[j] == colliders[i].gameObject || colliders[i].gameObject.GetComponent<ColorCube>() == null || !colliders[i].gameObject.GetComponent<ColorCube>().attachedToEnemyThrowCanBeAttached)
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
            cube.GetComponent<ColorCube>().addStateAttachedToEnemyThrow(this.gameObject, grabDuration, grabPower, grabMaxSpeed, indexActual, grabKeepRadius, grabKeepFactor);
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
                cubesAttached[index].GetComponent<ColorCube>().removeStateAttachedToEnemyThrow(false);
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
                        cube.GetComponent<ColorCube>().removeStateAttachedToEnemyThrow(false);
                        if(cube.GetComponent<ColorCube>().isActiveByEnemy)
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
                            cubesAttached[randomIndex].GetComponent<ColorCube>().removeStateAttachedToEnemyThrow(false);
                            if(cubesAttached[randomIndex].GetComponent<ColorCube>().isActiveByEnemy)
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

    void manageCounter()
    {
        
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

        if(GetComponent<ColorCube>() != null)
        {
            GetComponent<ColorCube>().addStateCore(6000);
        }
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
