using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeEnemyAtRuntime : MonoBehaviour
{
    public bool changeAll;

    public bool changeHpEjectorGo;
    public bool changeFormEjectorGo;
    public bool changeGrabEjectorGo;
    public bool changeShootEjectorGo;
    public bool changeShootHelpEjectorGo;

    public bool changeHpWormGo;
    public bool changeMovementWormGo;
    public bool changeFormWormGo;
    public bool changeGrabWormGo;

    [Header("----- SETTINGS -----")]
    [Header("--- (Ejector) ---")]
    [Header("- (Hp Ejector) -")]
    public int monsterThrowMaxLife = -1;
    public int monsterThrowMinLife = -1;
    public int monsterThrowMaxCubes = -1;
    public int monsterThrowStartCubes = -1;

    [Header("- (Form Ejector) -")]
    public float monsterThrowRadiusCircle = -1;
    public float monsterThrowMinRadiusCircle = -1;

    [Header("- (Grab Ejector) -")]
    public float monsterThrowGrabCooldown = -1;
    public float monsterThrowGrabRadius = -1;
    public float monsterThrowGrabKeepRadius = -1;
    public float monsterThrowGrabKeepFactor = -1;
    public float monsterThrowGrabPower = -1;
    public float monsterThrowGrabMaxSpeed = -1;
    public float monsterThrowGrabLessCooldownPerAttachedCube = -1;
    public float monsterThrowMinCooldownGrab = -1;
    public bool monsterThrowPickAvailableNearestCube;
    public bool monsterThrowGrabCubesToNearestPosition;

    [Header("- (Shoot Ejector) -")]
    public float monsterThrowShootCooldown = -1;
    public float monsterThrowShootDuration = -1;
    public float monsterThrowShootPower = -1;
    public float monsterThrowShootMaxSpeed = -1;
    public float monsterThrowShootActivateEnemyDuration = -1;
    public float monsterThrowShootActivateEnemyDurationRandomBonus = -1;
    public float monsterThrowShootLessCooldownPerAttachedCube = -1;
    public float monsterThrowMinCooldownShoot = -1;
    public float monsterThrowMinCubesAttachedForShoot = -1;
    
    [Header("- (Shoot Help Ejector) -")]
    public bool shootLowerCubeSpeedBeforeLaunch;
    public bool shootInPlayerMoveDirection;
    public float shootInPlayerMoveDirectionMinRandom = -1;
    public float shootInPlayerMoveDirectionMaxRandom = -1;


    [Header("--- (Change Worm) ---")]
    [Header("- (Hp) -")]
    public int monsterChaseMaxLife = -1;
    public int monsterChaseMinLife = -1;
    public int monsterChaseMaxCubes = -1;
    public int monsterChaseStartCubes = -1;
    [Header("- (Movement Worm) -")]
    public float movementMaxSpeed = -1;
    public float movementMovePower = -1;
    public float minAngleMovement = -1;
    public float deviationPower = -1;
    public float movementOffsetRadius = -1;
    public float newPointTime;
    [Header("- (Form Worm) -")]
    public float monsterChaseRadiusCircle = -1;
    public float monsterChaseMinRadiusCircle = -1;
    [Header("- (Grab Worm) -")]
    public float monsterChaseGrabCooldown = -1;
    public float monsterChaseGrabRadius = -1;
    public float monsterChaseGrabKeepRadius = -1;
    public float monsterChaseGrabKeepFactor = -1;
    public float monsterChaseGrabDuration = -1;
    public float monsterChaseGrabPower = -1;
    public float monsterChaseGrabMaxSpeed = -1;
    public float monsterChaseGrabOverlapSphereRadiusStep = -1;
    public float monsterChaseGrabLessCooldownPerAttachedCube = -1;
    public float monsterChaseMinCooldownGrab = -1;

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(changeAll)
        {
            changeHpEjectorGo = true;
            changeFormEjectorGo = true;
            changeGrabEjectorGo = true;
            changeShootEjectorGo = true;
            changeShootHelpEjectorGo = true;

            changeHpWormGo = true;
            changeMovementWormGo = true;
            changeFormWormGo = true;
            changeGrabWormGo = true;

            changeAll = false;
        }

        if (changeHpEjectorGo)
        {
            changeHpEjector();
            changeHpEjectorGo = false;
        }
        if (changeFormEjectorGo)
        {
            changeFormEjector();
            changeFormEjectorGo = false;
        }
        if (changeGrabEjectorGo)
        {
            changeGrabEjector();
            changeGrabEjectorGo = false;
        }
        if (changeShootEjectorGo)
        {
            changeShootEjector();
            changeShootEjectorGo = false;
        }
        if (changeShootHelpEjectorGo)
        {
            changeShootHelpEjector();
            changeShootHelpEjectorGo = false;
        }

        if (changeHpWormGo)
        {
            changeHpWorm();
            changeHpWormGo = false;
        }
        if (changeMovementWormGo)
        {
            changeMovementWorm();
            changeMovementWormGo = false;
        }
        if (changeFormWormGo)
        {
            changeFormWorm();
            changeFormWormGo = false;
        }
        if (changeGrabWormGo)
        {
            changeGrabWorm();
            changeGrabWormGo = false;
        }
    }
    
    void changeHpEjector()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject cube = transform.GetChild(i).gameObject;
            if (cube.GetComponent<ColorCube>() != null)
            {
                if (cube.GetComponent<CubeMonster>() == null)
                    cube.AddComponent<CubeMonster>();

                if(monsterThrowMaxLife >= 0)
                    cube.GetComponent<CubeMonster>().maxLife = monsterThrowMaxLife;
                if (monsterThrowMinLife >= 0)
                    cube.GetComponent<CubeMonster>().minLife = monsterThrowMinLife;
                if (monsterThrowMaxCubes >= 0)
                    cube.GetComponent<CubeMonster>().maxCubes = monsterThrowMaxCubes;
                if (monsterThrowStartCubes >= 0)
                    cube.GetComponent<CubeMonster>().startCubes = monsterThrowStartCubes;
            }
        }
    }

    void changeFormEjector()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject cube = transform.GetChild(i).gameObject;
            if (cube.GetComponent<ColorCube>() != null)
            {
                if (cube.GetComponent<CubeMonster>() == null)
                    cube.AddComponent<CubeMonster>();

                if(monsterThrowRadiusCircle >= 0)
                    cube.GetComponent<CubeMonster>().radiusCircle = monsterThrowRadiusCircle;
                if(monsterThrowMinRadiusCircle >= 0)
                    cube.GetComponent<CubeMonster>().minRadiusCircle = monsterThrowMinRadiusCircle;
            }
        }
    }

    void changeGrabEjector()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject cube = transform.GetChild(i).gameObject;
            if (cube.GetComponent<ColorCube>() != null)
            {
                if (cube.GetComponent<CubeMonster>() == null)
                    cube.AddComponent<CubeMonster>();

                if(monsterThrowGrabCooldown >= 0)
                cube.GetComponent<CubeMonster>().grabCooldown = monsterThrowGrabCooldown;
                if (monsterThrowGrabRadius >= 0)
                    cube.GetComponent<CubeMonster>().grabRadius = monsterThrowGrabRadius;
                if (monsterThrowGrabKeepRadius >= 0)
                    cube.GetComponent<CubeMonster>().grabKeepRadius = monsterThrowGrabKeepRadius;
                if (monsterThrowGrabKeepFactor >= 0)
                    cube.GetComponent<CubeMonster>().grabKeepFactor = monsterThrowGrabKeepFactor;
                if (monsterThrowGrabPower >= 0)
                    cube.GetComponent<CubeMonster>().grabPower = monsterThrowGrabPower;
                if (monsterThrowGrabMaxSpeed >= 0)
                    cube.GetComponent<CubeMonster>().grabMaxSpeed = monsterThrowGrabMaxSpeed; ;
                if (monsterThrowGrabLessCooldownPerAttachedCube >= 0)
                    cube.GetComponent<CubeMonster>().grabLessCooldownPerAttachedCube = monsterThrowGrabLessCooldownPerAttachedCube;
                if (monsterThrowMinCooldownGrab >= 0)
                    cube.GetComponent<CubeMonster>().minCooldownGrab = monsterThrowMinCooldownGrab;
                cube.GetComponent<CubeMonster>().pickAvailableNearestCube = monsterThrowPickAvailableNearestCube;
                cube.GetComponent<CubeMonster>().grabCubesToNearestPosition = monsterThrowGrabCubesToNearestPosition;
            }
        }
    }

    void changeShootEjector()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject cube = transform.GetChild(i).gameObject;
            if (cube.GetComponent<ColorCube>() != null)
            {
                if(monsterThrowShootCooldown >= 0)
                    cube.GetComponent<CubeMonster>().shootCooldown = monsterThrowShootCooldown;
                if (monsterThrowShootDuration >= 0)
                    cube.GetComponent<CubeMonster>().shootDuration = monsterThrowShootDuration;
                if (monsterThrowShootPower >= 0)
                    cube.GetComponent<CubeMonster>().shootPower = monsterThrowShootPower;
                if (monsterThrowShootMaxSpeed >= 0)
                    cube.GetComponent<CubeMonster>().shootMaxSpeed = monsterThrowShootMaxSpeed;
                if (monsterThrowShootActivateEnemyDuration >= 0)
                    cube.GetComponent<CubeMonster>().shootActivateEnemyDuration = monsterThrowShootActivateEnemyDuration;
                if (monsterThrowShootActivateEnemyDurationRandomBonus >= 0)
                    cube.GetComponent<CubeMonster>().shootActivateEnemyDurationRandomBonus = monsterThrowShootActivateEnemyDurationRandomBonus;
                cube.GetComponent<CubeMonster>().shootLessCooldownPerAttachedCube = monsterThrowShootLessCooldownPerAttachedCube;
            }
        }
    }

    void changeShootHelpEjector()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject cube = transform.GetChild(i).gameObject;
            if (cube.GetComponent<ColorCube>() != null)
            {
                cube.GetComponent<CubeMonster>().shootLowerCubeSpeedBeforeLaunch = shootLowerCubeSpeedBeforeLaunch;
                cube.GetComponent<CubeMonster>().shootInPlayerMoveDirection = shootInPlayerMoveDirection;
                if (shootInPlayerMoveDirectionMinRandom >= 0)
                    cube.GetComponent<CubeMonster>().shootInPlayerMoveDirectionMinRandom = shootInPlayerMoveDirectionMinRandom;
                if (shootInPlayerMoveDirectionMaxRandom >= 0)
                    cube.GetComponent<CubeMonster>().shootInPlayerMoveDirectionMaxRandom = shootInPlayerMoveDirectionMaxRandom;
            }
        }
    }

    void changeHpWorm()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject cube = transform.GetChild(i).gameObject;
            if (cube.GetComponent<ColorCube>() != null)
            {
                if (cube.GetComponent<MonsterChase>() == null)
                    cube.AddComponent<MonsterChase>();

                if(monsterChaseMaxLife >= 0)
                    cube.GetComponent<MonsterChase>().maxLife = monsterChaseMaxLife;
                if (monsterChaseMinLife >= 0)
                    cube.GetComponent<MonsterChase>().minLife = monsterChaseMinLife;
                if (monsterChaseMaxCubes >= 0)
                    cube.GetComponent<MonsterChase>().maxCubes = monsterChaseMaxCubes;
                if (monsterChaseStartCubes >= 0)
                    cube.GetComponent<MonsterChase>().startCubes = monsterChaseStartCubes;
            }
        }
    }

    public void changeFormWorm()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject cube = transform.GetChild(i).gameObject;
            if (cube.GetComponent<ColorCube>() != null)
            {
                if (cube.GetComponent<MonsterChase>() == null)
                    cube.AddComponent<MonsterChase>();


                if (movementMaxSpeed >= 0)
                    cube.GetComponent<MonsterChase>().movementMaxSpeed = movementMaxSpeed;
                if (movementMovePower >= 0)
                    cube.GetComponent<MonsterChase>().movementMovePower = movementMovePower;
            }
        }
    }

    public void changeMovementWorm()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject cube = transform.GetChild(i).gameObject;
            if (cube.GetComponent<ColorCube>() != null)
            {
                if (cube.GetComponent<MonsterChase>() == null)
                    cube.AddComponent<MonsterChase>();

                if (monsterChaseRadiusCircle >= 0)
                    cube.GetComponent<MonsterChase>().radiusCircle = monsterChaseRadiusCircle;
                if (monsterChaseMinRadiusCircle >= 0)
                    cube.GetComponent<MonsterChase>().minRadiusCircle = monsterChaseMinRadiusCircle;
                if (minAngleMovement >= 0)
                    cube.GetComponent<MonsterChase>().minAngleMovement = minAngleMovement;
                if (deviationPower >= 0)
                    cube.GetComponent<MonsterChase>().deviationPower = deviationPower;
                if (movementOffsetRadius >= 0)
                    cube.GetComponent<MonsterChase>().movementOffsetRadius = movementOffsetRadius;
                if (newPointTime >= 0)
                    cube.GetComponent<MonsterChase>().newPointTime = newPointTime;
            }
        }
    }

    public void changeGrabWorm()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject cube = transform.GetChild(i).gameObject;
            if (cube.GetComponent<ColorCube>() != null)
            {
                if (cube.GetComponent<MonsterChase>() == null)
                    cube.AddComponent<MonsterChase>();

                if (monsterChaseGrabCooldown >= 0)
                    cube.GetComponent<MonsterChase>().grabCooldown = monsterChaseGrabCooldown;
                if (monsterChaseGrabRadius >= 0)
                    cube.GetComponent<MonsterChase>().grabRadius = monsterChaseGrabRadius;
                if (monsterChaseGrabKeepRadius >= 0)
                    cube.GetComponent<MonsterChase>().grabKeepRadius = monsterChaseGrabKeepRadius;
                if (monsterChaseGrabKeepFactor >= 0)
                    cube.GetComponent<MonsterChase>().grabKeepFactor = monsterChaseGrabKeepFactor;
                if (monsterChaseGrabDuration >= 0)
                    cube.GetComponent<MonsterChase>().grabDuration = monsterChaseGrabDuration;
                if (monsterChaseGrabPower >= 0)
                    cube.GetComponent<MonsterChase>().grabPower = monsterChaseGrabPower;
                if (monsterChaseGrabMaxSpeed >= 0)
                    cube.GetComponent<MonsterChase>().grabMaxSpeed = monsterChaseGrabMaxSpeed;
                if (monsterChaseGrabOverlapSphereRadiusStep >= 0)
                    cube.GetComponent<MonsterChase>().grabOverlapSphereRadiusStep = monsterChaseGrabOverlapSphereRadiusStep;
                if (monsterChaseGrabLessCooldownPerAttachedCube >= 0)
                    cube.GetComponent<MonsterChase>().grabLessCooldownPerAttachedCube = monsterChaseGrabLessCooldownPerAttachedCube;
                if (monsterChaseMinCooldownGrab >= 0)
                    cube.GetComponent<MonsterChase>().minCooldownGrab = monsterChaseMinCooldownGrab;
            }
        }
    }
}
