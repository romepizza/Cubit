using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitializeCubes : MonoBehaviour
{
    [Header("----- Settings -----")]
    public bool initializeCubesGo;
    public bool removeComponentsGo;

    [Header("Add")]
    public bool setStateInformation;
    public bool addLights;
    public bool addExplosion;
    public bool addTrailRenderer;
    public bool addMonster;
    public bool addSphereMesh;

    [Header("Remove")]
    public bool removeLights;
    public bool removeTrailRenderer;
    public bool removeExplosionDo;
    public bool removeMonster;
    public bool removeSphereMesh;

    [Header("----- (Initialize) -----")]
    [Header("--- (Size) ---")]
    public bool setSize;
    public Transform[] transforms;
    public float[] chances;

    [Header("--- (Mesh) ---")]
    public GameObject cubeMesh;
    public GameObject sphereMeshCoreThrow;
    public GameObject sphereMeshCoreChase;

    [Header("--- (Color) ---")]
    public Material defaultMaterial;
    public Material[] materialsInactive;
    public Material[] materialsActiveNeutral;
    public Material[] materialsGrabbedByPlayer;
    public Material[] materialsActiveByPlayer;
    public Material[] materialAttachedToEnemyThrow;
    public Material[] materialAttachedToEnemyChase;
    public Material[] materialActiveByEnemy;
    public Material[] materialEnemyCore;
    public bool colorToRandom;
    public bool colorToDefault;

    [Header("--- (Aim) ---")]
    public float localAimRadius;

    [Header("--- (FX) ---")]
    public GameObject explosionEffectEnemyDmg;
    public GameObject explosionEffectPlayerDmg;
    public GameObject explosionEffectCoreExplode;

    [Header("--- (Light) ---")]
    public GameObject lightObject;
    public float lightDuration;
    public float lightDurationRandomBonus;
    public float lightRadius;
    public float lightIntensity;
    public float lightIndirectMultiplier;
    public float lightMinGrabFactor;
    public float lightMinDurationFactor;
    //public float chanceOnLightCube;

    [Header("--- (Monster Throw) ---")]
    public int monsterThrowMaxLife;
    public int monsterThrowMinLife;
    public int monsterThrowMaxCubes;
    public int monsterThrowStartCubes;

    [Header("- (Form Throw) -")]
    public float monsterThrowRadiusCircle;
    public float monsterThrowMinRadiusCircle;

    [Header("- (Grab Throw) -")]
    public float monsterThrowGrabCooldown;
    public float monsterThrowGrabRadius;
    public float monsterThrowGrabKeepRadius;
    public float monsterThrowGrabKeepFactor;
    public float monsterThrowGrabDuration;
    public float monsterThrowGrabPower;
    public float monsterThrowGrabMaxSpeed;
    public float monsterThrowGrabOverlapSphereRadiusStep;
    public float monsterThrowGrabLessCooldownPerAttachedCube;
    public float monsterThrowMinCooldownGrab;
    public bool monsterThrowPickAvailableNearestCube;
    public bool monsterThrowGrabCubesToNearestPosition;

    [Header("- (Shoot Throw) -")]
    public float monsterThrowShootCooldown;
    public float monsterThrowShootDuration;
    public float monsterThrowShootPower;
    public float monsterThrowShootMaxSpeed;
    public float monsterThrowShootActivateEnemyDuration;
    public float monsterThrowShootActivateEnemyDurationRandomBonus;
    public float monsterThrowShootLessCooldownPerAttachedCube;
    public float monsterThrowMinCooldownShoot;
    public float monsterThrowMinCubesAttachedForShoot;
    public bool monsterThrowShotNearestCube;
    
    [Header("--- (Shoot Help Throw) ---")]
    public bool shootLowerCubeSpeedBeforeLaunch;
    public bool shootInPlayerMoveDirection;
    public float shootInPlayerMoveDirectionMinRandom;
    public float shootInPlayerMoveDirectionMaxRandom;

    [Header("--- (Monster Chase) ---")]
    [Header("- (Hp) -")]
    public int monsterChaseMaxLife;
    public int monsterChaseMinLife;
    public int monsterChaseMaxCubes;
    public int monsterChaseStartCubes;
    [Header("- (Movement Chase) -")]
    public float movementMaxSpeed;
    public float movementMovePower;
    public float minAngleMovement;
    public float deviationPower;
    public float movementOffsetRadius;
    public float newPointTime;
    public bool movementAffectsAttachedCubes;
    public float movementAttachedFactor;
    [Header("- (Form Chase) -")]
    public float monsterChaseRadiusCircle;
    public float monsterChaseMinRadiusCircle;

    [Header("- (Grab Chase) -")]
    public float monsterChaseGrabCooldown;
    public float monsterChaseGrabRadius;
    public float monsterChaseGrabKeepRadius;
    public float monsterChaseGrabKeepFactor;
    public float monsterChaseGrabDuration;
    public float monsterChaseGrabPower;
    public float monsterChaseGrabMaxSpeed;
    public float monsterChaseGrabOverlapSphereRadiusStep;
    public float monsterChaseGrabLessCooldownPerAttachedCube;
    public float monsterChaseMinCooldownGrab;
    public bool monsterChasePickAvailableNearestCube;
    public bool monsterChaseGrabCubesToNearestPosition;

    [Header("--- (States) ---")]
    public int collisionEffectPostponeFrames;
    [Header("- (State: Inactive) -")]
    public float inactiveDurationDefault;
    public float inactiveDurationDefaultRandomBonus;
    [Header("- (State: Active Neutral) -")]
    public float activeNeutralDurationDefault;
    public float activeNeutralDurationDefaultRandomBonus;
    [Header("-(State: Grabbed by Player) -")]
    public float grabbedPlayerGetPositionCooldown;
    public float grabbedPlayerDurationDefault;
    public float grabbedPlayerDurationDefaultRandomBonus;
    [Header("--- (State: Active by Player) ---")]
    public float activePlayerDurationDefault;
    public float activePlayerDurationDefaultRandomBonus;
    [Header("--- (State: Attached to Enemy) ---")]
    public float attachedEnemyDurationDefault;
    public float attachedEnemyDurationDefaultRandomBonus;
    [Header("--- (State: Active by Enemy) ---")]
    public float activeEnemyDurationDefault;
    public float activeEnemyDurationDefaultRandomBonus;
    [Header("--- (State: Core) ---")]
    public float coreDurationDefault;
    public float coreDurationDefaultRandomBonus;
    [Header("--- (State: Accelerate) ---")]
    public float accelerateDurationDefault;
    public float accelerateDurationDefaultRandomBonus;

    [Header("--- (Player Collsion) ---")]
    public float playerCollisionDuration;
    public float playerCollisionPower;
    public Vector3 playerCollisionOffset;
    public float playerCollisionMaxSpeed;

    [Header("--- (Trail Renderer) ---")]
    public Material trailMaterial;
    public float trailTime;
    public float trailWidth;
    public bool useDefaultMaterial;


    /*
    [Header("--- (Halo)---")]
    public GameObject haloPrefab;
    public bool addHalo;
    public bool removeHalo;
    public float haloSize;
    */
    void Start()
    {
        foreach (Transform cube in transform)
        {
            if (cube.gameObject.GetComponent<Rigidbody>() != null && !cube.gameObject.GetComponent<ColorCube>().dontTouchAtStart)
            {
                cube.gameObject.GetComponent<Rigidbody>().velocity = new Vector3((Random.Range(0, 200) - 100) * 0.01f, (Random.Range(0, 200) - 100) * 0.01f, (Random.Range(0, 200) - 100) * 0.01f);
                cube.gameObject.GetComponent<Rigidbody>().AddRelativeTorque(new Vector3((Random.Range(0, 200) - 100) * 0.1f, (Random.Range(0, 200) - 100) * 0.1f, (Random.Range(0, 200) - 100) * 0.1f), ForceMode.Acceleration);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    void initializeCubes()
    {
        /*
        foreach (Transform cube in transform)
        {
            if(cube.gameObject.GetComponent<ColorCube>() != null)
                cube.gameObject.GetComponent<ColorCube>().player = null;
        }
        */

        generalUpdate();

        if (setStateInformation)
            setStates();

        if (colorToDefault)
            setColorToDefault();
        if (colorToRandom)
            setColorToRandom();

        if (addLights)
            addLightToCube();

        if (addTrailRenderer)
            addTrailRendererToCube();

        if (addExplosion)
            setExplosion();

        if (setSize)
            setSizeOfCube();

        if (addMonster)
            addMosnterToCubes();

        if (addSphereMesh)
            addSphereMeshToCube();
    }
    void removeComponents()
    {
        if (removeLights)
            removeLightFromCube();
        if (removeTrailRenderer)
            removeTrailRendererFromCube();
        if (removeExplosionDo)
            removeExplosion();
        if (removeMonster)
            removeMonsterFromCube();
        if (removeSphereMesh)
            removeSphereMeshFromCube();
    }

    void generalUpdate()
    {
        foreach (Transform cube in transform)
        {
            if (cube.GetComponent<ColorCube>() != null)
            {
                cube.gameObject.GetComponent<ColorCube>().localAimRadius = localAimRadius;
            }
        }
    }

    void setStates()
    {
        foreach (Transform cube in transform)
        {
            if(cube.GetComponent<ColorCube>() != null)
            {
                cube.gameObject.GetComponent<ColorCube>().canCollide = true;
                cube.gameObject.GetComponent<ColorCube>().collisionEffectPostponeFrames = collisionEffectPostponeFrames;
                cube.gameObject.GetComponent<ColorCube>().inactiveDurationDefault = inactiveDurationDefault + Random.Range(0, inactiveDurationDefaultRandomBonus);
                cube.gameObject.GetComponent<ColorCube>().activeNeutralDurationDefault = activeNeutralDurationDefault + Random.Range(0, activeNeutralDurationDefaultRandomBonus);
                cube.gameObject.GetComponent<ColorCube>().grabbedPlayerDurationDefault = grabbedPlayerDurationDefault + Random.Range(0, grabbedPlayerDurationDefaultRandomBonus);
                cube.gameObject.GetComponent<ColorCube>().grabbedPlayerGetPositionCooldown = grabbedPlayerGetPositionCooldown;
                cube.gameObject.GetComponent<ColorCube>().activePlayerDurationDefault = activePlayerDurationDefault + Random.Range(0, activePlayerDurationDefaultRandomBonus);
                //cube.gameObject.GetComponent<ColorCube>().attachedEnemyThrowDurationDefault = attachedEnemyDurationDefault + Random.Range(0, attachedEnemyDurationDefaultRandomBonus);
                cube.gameObject.GetComponent<ColorCube>().activeEnemyDurationDefault = activeEnemyDurationDefault + Random.Range(0, activeEnemyDurationDefaultRandomBonus);
                cube.gameObject.GetComponent<ColorCube>().coreDurationDefault = coreDurationDefault + Random.Range(0, coreDurationDefaultRandomBonus);
                cube.gameObject.GetComponent<ColorCube>().accelerateDurationDefault = accelerateDurationDefault + Random.Range(0, accelerateDurationDefaultRandomBonus);

                cube.gameObject.GetComponent<ColorCube>().playerCollisionDuration = playerCollisionDuration;
                cube.gameObject.GetComponent<ColorCube>().playerCollisionPower = playerCollisionPower;
                cube.gameObject.GetComponent<ColorCube>().playerCollisionOffset = playerCollisionOffset;
                cube.gameObject.GetComponent<ColorCube>().playerCollisionMaxSpeed = playerCollisionMaxSpeed;
            }
        }
    }

    void setSizeOfCube()
    {
        float totalChanceValue = 0;
        for (int i = 0; i < transforms.Length; i++)
        {
            totalChanceValue += chances[i];
        }

        for (int j = 0; j < transform.childCount; j++)
        {
            float currentChance = 0;
            for (int i = 0; i < transforms.Length; i++)
            {
                float random = Random.Range(0f, totalChanceValue);
                currentChance += chances[i];
                if (i == transforms.Length && transform.GetChild(j).gameObject.GetComponent<ColorCube>() != null)
                {
                    transform.GetChild(j).transform.localScale = transforms[i].localScale;
                    break;
                }
                else if (currentChance > random && transform.GetChild(j).gameObject.GetComponent<ColorCube>() != null)
                {
                    transform.GetChild(j).transform.localScale = transforms[i].localScale;
                    break;
                }
            }
        }
    }

    void addMosnterToCubes()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            GameObject cube = transform.GetChild(i).gameObject;
            if (cube.GetComponent<ColorCube>() != null)
            {
                if (cube.GetComponent<CubeMonster>() == null)
                    cube.AddComponent<CubeMonster>();

                // Throw
                cube.GetComponent<CubeMonster>().maxLife = monsterThrowMaxLife;
                cube.GetComponent<CubeMonster>().minLife = monsterThrowMinLife;
                cube.GetComponent<CubeMonster>().maxCubes = monsterThrowMaxCubes;
                cube.GetComponent<CubeMonster>().startCubes = monsterThrowStartCubes;

                cube.GetComponent<CubeMonster>().radiusCircle = monsterThrowRadiusCircle;
                cube.GetComponent<CubeMonster>().minRadiusCircle = monsterThrowMinRadiusCircle;

                cube.GetComponent<CubeMonster>().grabCooldown = monsterThrowGrabCooldown;
                cube.GetComponent<CubeMonster>().grabRadius = monsterThrowGrabRadius;
                cube.GetComponent<CubeMonster>().grabKeepRadius = monsterThrowGrabKeepRadius;
                cube.GetComponent<CubeMonster>().grabKeepFactor = monsterThrowGrabKeepFactor;
                cube.GetComponent<CubeMonster>().grabDuration = monsterThrowGrabDuration;
                cube.GetComponent<CubeMonster>().grabPower = monsterThrowGrabPower;
                cube.GetComponent<CubeMonster>().grabMaxSpeed = monsterThrowGrabMaxSpeed; ;
                cube.GetComponent<CubeMonster>().grabOverlapSphereRadiusStep = monsterThrowGrabOverlapSphereRadiusStep;
                cube.GetComponent<CubeMonster>().grabLessCooldownPerAttachedCube = monsterThrowGrabLessCooldownPerAttachedCube;
                cube.GetComponent<CubeMonster>().minCooldownGrab = monsterThrowMinCooldownGrab;
                cube.GetComponent<CubeMonster>().pickAvailableNearestCube = monsterThrowPickAvailableNearestCube;
                cube.GetComponent<CubeMonster>().grabCubesToNearestPosition = monsterThrowGrabCubesToNearestPosition;

                cube.GetComponent<CubeMonster>().shootCooldown = monsterThrowShootCooldown;
                cube.GetComponent<CubeMonster>().shootDuration = monsterThrowShootDuration;
                cube.GetComponent<CubeMonster>().shootPower = monsterThrowShootPower;
                cube.GetComponent<CubeMonster>().shootMaxSpeed = monsterThrowShootMaxSpeed;
                cube.GetComponent<CubeMonster>().shootActivateEnemyDuration = monsterThrowShootActivateEnemyDuration;
                cube.GetComponent<CubeMonster>().shootActivateEnemyDurationRandomBonus = monsterThrowShootActivateEnemyDurationRandomBonus;
                cube.GetComponent<CubeMonster>().shootLessCooldownPerAttachedCube = monsterThrowShootLessCooldownPerAttachedCube;
                cube.GetComponent<CubeMonster>().shootLowerCubeSpeedBeforeLaunch = shootLowerCubeSpeedBeforeLaunch;
                cube.GetComponent<CubeMonster>().shootInPlayerMoveDirection = shootInPlayerMoveDirection;
                cube.GetComponent<CubeMonster>().shootInPlayerMoveDirectionMinRandom = shootInPlayerMoveDirectionMinRandom;
                cube.GetComponent<CubeMonster>().shootInPlayerMoveDirectionMaxRandom = shootInPlayerMoveDirectionMaxRandom;
                
                cube.GetComponent<CubeMonster>().minCooldownShoot = monsterThrowMinCooldownShoot;
                cube.GetComponent<CubeMonster>().minCubesAttachedForShoot = monsterThrowMinCubesAttachedForShoot;
                cube.GetComponent<CubeMonster>().shotNearestCube = monsterThrowShotNearestCube;

                // Chase
                if (cube.GetComponent<MonsterChase>() == null)
                    cube.AddComponent<MonsterChase>();

                cube.GetComponent<MonsterChase>().maxLife = monsterChaseMaxLife;
                cube.GetComponent<MonsterChase>().minLife = monsterChaseMinLife;
                cube.GetComponent<MonsterChase>().maxCubes = monsterChaseMaxCubes;
                cube.GetComponent<MonsterChase>().startCubes = monsterChaseStartCubes;

                cube.GetComponent<MonsterChase>().movementMaxSpeed = movementMaxSpeed;
                cube.GetComponent<MonsterChase>().movementMovePower = movementMovePower;

                cube.GetComponent<MonsterChase>().radiusCircle = monsterChaseRadiusCircle;
                cube.GetComponent<MonsterChase>().minRadiusCircle = monsterChaseMinRadiusCircle;
                cube.GetComponent<MonsterChase>().minAngleMovement = minAngleMovement;
                cube.GetComponent<MonsterChase>().deviationPower = deviationPower;
                cube.GetComponent<MonsterChase>().movementOffsetRadius = movementOffsetRadius;
                cube.GetComponent<MonsterChase>().newPointTime = newPointTime;
                cube.GetComponent<MonsterChase>().movementAffectsAttachedCubes = movementAffectsAttachedCubes;
                cube.GetComponent<MonsterChase>().movementAttachedFactor = movementAttachedFactor;


                cube.GetComponent<MonsterChase>().grabCooldown = monsterChaseGrabCooldown;
                cube.GetComponent<MonsterChase>().grabRadius = monsterChaseGrabRadius;
                cube.GetComponent<MonsterChase>().grabKeepRadius = monsterChaseGrabKeepRadius;
                cube.GetComponent<MonsterChase>().grabKeepFactor = monsterChaseGrabKeepFactor;
                cube.GetComponent<MonsterChase>().grabDuration = monsterChaseGrabDuration;
                cube.GetComponent<MonsterChase>().grabPower = monsterChaseGrabPower; ;
                cube.GetComponent<MonsterChase>().grabMaxSpeed = monsterChaseGrabMaxSpeed; ;
                cube.GetComponent<MonsterChase>().grabOverlapSphereRadiusStep = monsterChaseGrabOverlapSphereRadiusStep;
                cube.GetComponent<MonsterChase>().grabLessCooldownPerAttachedCube = monsterChaseGrabLessCooldownPerAttachedCube;
                cube.GetComponent<MonsterChase>().minCooldownGrab = monsterChaseMinCooldownGrab;
                cube.GetComponent<MonsterChase>().pickAvailableNearestCube = monsterChasePickAvailableNearestCube;
                cube.GetComponent<MonsterChase>().grabCubesToNearestPosition = monsterChaseGrabCubesToNearestPosition;
            }
        }
    }

    void removeMonsterFromCube()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject cube = transform.GetChild(i).gameObject;
            if (cube.GetComponent<ColorCube>() != null)
            {
                DestroyImmediate(cube.GetComponent<CubeMonster>());
            }
        }
    }


    void createComponents(GameObject cube)
    {
        if (cube.GetComponent<Rigidbody>() == null)
            cube.AddComponent<Rigidbody>();
    }

    void setColorToDefault()
    {
        foreach(Transform cube in transform)
        {
            if (cube.gameObject.GetComponent<MeshRenderer>() != null && cube.gameObject.GetComponent<ColorCube>() != null && !cube.gameObject.GetComponent<ColorCube>().dontTouchMaterial)
            {
                int random = Random.Range(0, materialsInactive.Length);
                cube.gameObject.GetComponent<ColorCube>().lightDurationDefault = lightDuration + Random.Range(0, lightDurationRandomBonus);
                cube.gameObject.GetComponent<ColorCube>().lightDurationActual = lightDuration + Random.Range(0, lightDurationRandomBonus);
                random = Random.Range(0, materialsInactive.Length);
                cube.gameObject.GetComponent<MeshRenderer>().material = materialsInactive[random];
                random = Random.Range(0, materialsInactive.Length);
                cube.gameObject.GetComponent<ColorCube>().materialInactive = materialsInactive[random];
                random = Random.Range(0, materialsActiveNeutral.Length);
                cube.gameObject.GetComponent<ColorCube>().materialActiveNeutral = materialsActiveNeutral[random];
                random = Random.Range(0, materialsActiveByPlayer.Length);
                cube.gameObject.GetComponent<ColorCube>().materialActiveByPlayer = materialsActiveByPlayer[random];
                random = Random.Range(0, materialsGrabbedByPlayer.Length);
                cube.gameObject.GetComponent<ColorCube>().materialGrabbedByPlayer = materialsGrabbedByPlayer[random];
                random = Random.Range(0, materialAttachedToEnemyThrow.Length);
                cube.gameObject.GetComponent<ColorCube>().materialAttachedToEnemyThrow = materialAttachedToEnemyThrow[random];
                random = Random.Range(0, materialAttachedToEnemyChase.Length);
                cube.gameObject.GetComponent<ColorCube>().materialAttachedToEnemyChase = materialAttachedToEnemyChase[random];
                random = Random.Range(0, materialActiveByEnemy.Length);
                cube.gameObject.GetComponent<ColorCube>().materialActiveByEnemy = materialActiveByEnemy[random];
                random = Random.Range(0, materialEnemyCore.Length);
                cube.gameObject.GetComponent<ColorCube>().materialEnemyCore = materialEnemyCore[random];
            }
        }
    }

    void setColorToRandom()
    {
        foreach (Transform cube in transform)
        {
            if (cube.gameObject.GetComponent<MeshRenderer>() != null && cube.gameObject.GetComponent<ColorCube>() != null && !cube.gameObject.GetComponent<ColorCube>().dontTouchMaterial)
            {
                int random = Random.Range(0, materialsInactive.Length);
                cube.gameObject.GetComponent<MeshRenderer>().material = materialsInactive[random];
                cube.gameObject.GetComponent<ColorCube>().materialInactive = materialsInactive[random];
                cube.gameObject.GetComponent<ColorCube>().lightDurationDefault = lightDuration + Random.Range(0, lightDurationRandomBonus);
                cube.gameObject.GetComponent<ColorCube>().lightDurationActual = lightDuration + Random.Range(0, lightDurationRandomBonus);
                cube.gameObject.GetComponent<ColorCube>().materialActiveByPlayer = materialsActiveByPlayer[random];
                cube.gameObject.GetComponent<ColorCube>().materialGrabbedByPlayer = materialsGrabbedByPlayer[random];
            }
        }
    }

    void setExplosion()
    {
        foreach (Transform cube in transform)
        {
            if(cube.GetComponent<ColorCube>() != null)
            {
                cube.GetComponent<ColorCube>().explosionEffectEnemyDmg = explosionEffectEnemyDmg;
                cube.GetComponent<ColorCube>().explosionEffectPlayerDmg = explosionEffectPlayerDmg;
                cube.GetComponent<ColorCube>().explosionEffectCoreExplode = explosionEffectCoreExplode;
            }
        }
    }

    void removeExplosion()
    {
        foreach (Transform cube in transform)
        {
            if (cube.GetComponent<ColorCube>() != null)
            {
                cube.GetComponent<ColorCube>().explosionEffectEnemyDmg = null;
                cube.GetComponent<ColorCube>().explosionEffectPlayerDmg = null;
                cube.GetComponent<ColorCube>().explosionEffectCoreExplode = null;
            }
        }
    }

    void addLightToCube()
    {
        foreach (Transform cube in transform)
        {
            if (cube.gameObject.GetComponent<ColorCube>() != null)
            {
                if(cube.GetComponent<Light>()== null)
                {
                    cube.gameObject.AddComponent<Light>();
                    cube.gameObject.GetComponent<Light>().type = LightType.Point;
                    cube.gameObject.GetComponent<Light>().shadows = LightShadows.None;
                }
                cube.gameObject.GetComponent<ColorCube>().lightDurationDefault = lightDuration + Random.Range(0, lightDurationRandomBonus);

                Color color1 = cube.GetComponent<Renderer>().sharedMaterial.color;
                if (cube.gameObject.GetComponent<Light>() != null)
                {
                    cube.gameObject.GetComponent<Light>().color = color1;
                    cube.gameObject.GetComponent<Light>().intensity = lightIntensity;
                    cube.gameObject.GetComponent<ColorCube>().defaultIntensity = lightIntensity;
                    cube.gameObject.GetComponent<Light>().range = lightRadius;
                    cube.gameObject.GetComponent<ColorCube>().defaultLightRadius = lightRadius;
                    cube.gameObject.GetComponent<Light>().bounceIntensity = lightIndirectMultiplier;
                    cube.gameObject.GetComponent<ColorCube>().minGrabFactor = lightMinGrabFactor;
                    cube.gameObject.GetComponent<ColorCube>().minDurationFactor = lightMinDurationFactor;
                    cube.gameObject.GetComponent<Light>().enabled = false;
                }
                /*
                //
                cube.gameObject.GetComponent<ColorCube>().lightDurationDefault = lightDuration + Random.Range(0, lightDurationRandomBonus);
                bool effectFound = false;
                bool lightFound = false;
                foreach (Transform child in cube)
                {
                    if (child.gameObject.GetComponent<Light>() != null)
                        lightFound = true;
                    if (child.gameObject.GetComponent<ParticleSystem>() != null)
                        effectFound = true;
                }

                //if (Random.Range((float)0, (float)1) < chanceOnLightCube)
                {
                    if (!lightFound && lightObject != null)
                        Instantiate(lightObject, cube);
                    if (!effectFound && effectObject != null)
                        Instantiate(effectObject, cube);
                }

                foreach (Transform child in cube)
                {
                    Color color = cube.GetComponent<Renderer>().sharedMaterial.color;
                    if (child.gameObject.GetComponent<Light>() != null)
                    {
                        child.gameObject.GetComponent<Light>().color = color;
                        child.gameObject.GetComponent<Light>().intensity = lightIntensity;
                        cube.gameObject.GetComponent<ColorCube>().defaultIntensity = lightIntensity;
                        child.gameObject.GetComponent<Light>().range = lightRadius;
                        cube.gameObject.GetComponent<ColorCube>().defaultLightRadius = lightRadius;
                        child.gameObject.GetComponent<Light>().bounceIntensity = lightIndirectMultiplier;
                        cube.gameObject.GetComponent<ColorCube>().minGrabFactor = lightMinGrabFactor;
                        cube.gameObject.GetComponent<ColorCube>().minDurationFactor = lightMinDurationFactor;
                        child.gameObject.SetActive(false);
                    }
                    if (child.gameObject.GetComponent<ParticleSystem>() != null)
                        child.localPosition = Vector3.zero;

                }*/
            }
        }
    }

    void removeLightFromCube()
    {
        foreach (Transform cube in transform)
        {
            if (cube.gameObject.GetComponent<ColorCube>() != null)
            {
                if (cube.gameObject.GetComponent<Light>() != null)
                    DestroyImmediate(cube.gameObject.GetComponent<Light>());

                //if (child != null && child.gameObject != null && child.gameObject.GetComponent<ParticleSystem>() != null)
                //    DestroyImmediate(child.gameObject);
            }
        }
    }

    void addTrailRendererToCube()
    {
        foreach (Transform cube in transform)
        {
            if (cube.gameObject.GetComponent<ColorCube>() != null)
            {
                if (cube.gameObject.GetComponent<TrailRenderer>() == null)
                    cube.gameObject.AddComponent<TrailRenderer>();
                if (useDefaultMaterial)
                    cube.gameObject.GetComponent<TrailRenderer>().material = cube.GetComponent<ColorCube>().materialActiveByPlayer;
                else
                    cube.gameObject.GetComponent<TrailRenderer>().material = trailMaterial;
                cube.gameObject.GetComponent<ColorCube>().trailTime = trailTime;
                cube.gameObject.GetComponent<TrailRenderer>().time = 0;//cube.gameObject.GetComponent<ColorCube>().lightDurotation;
                cube.gameObject.GetComponent<TrailRenderer>().widthMultiplier = trailWidth;
                cube.gameObject.GetComponent<TrailRenderer>().startColor = cube.gameObject.GetComponent<ColorCube>().materialActiveByPlayer.color;
                cube.gameObject.GetComponent<TrailRenderer>().endColor = cube.gameObject.GetComponent<ColorCube>().materialActiveByPlayer.color;
            }
        }
    }
    
    void removeTrailRendererFromCube()
    {
        foreach (Transform cube in transform)
        {
            if (cube.gameObject.GetComponent<ColorCube>() != null)
            {
                DestroyImmediate(cube.gameObject.GetComponent<TrailRenderer>());
            }
        }
    }

    void addSphereMeshToCube()
    {
        foreach (Transform cube in transform)
        {
            if (cube.gameObject.GetComponent<ColorCube>() != null)
            {
                cube.gameObject.GetComponent<ColorCube>().cubeMesh = cubeMesh;
                cube.gameObject.GetComponent<ColorCube>().sphereMeshCoreThrow = sphereMeshCoreThrow;
                cube.gameObject.GetComponent<ColorCube>().sphereMeshCoreChase = sphereMeshCoreChase;
            }
        }
    }

    void removeSphereMeshFromCube()
    {

    }

    void OnDrawGizmos()
    {
        if (initializeCubesGo)  
        {
            initializeCubes();
            initializeCubesGo = false;
        }

        if(removeComponentsGo)
        {
            removeComponents();
            removeComponentsGo = false;
        }

        
        /*
        if (colorToDefault)
        {
            setColorToDefault();
            colorToDefault = false;
        }

        if (colorToRandom)
        {
            setColorToRandom();
            colorToRandom = false;
        }

        if (addLights)
        {

            addLightToCube();
            addLights = false;
        }

        if (removeLights)
        {
            removeLightFromCube();
            removeLights = false;
        }


        if (addTrailRenderer)
        {
            addTrailRendererToCube();
            addTrailRenderer = false;
        }

        if (removeTrailRenderer)
        {
            removeTrailRendererToCube();
            removeTrailRenderer = false;
        }
        */
        /*
        if(addHalo)
        {
            foreach (Transform cubes in transform)
            {
                if (cubes.gameObject.name == "LightCubes")
                {
                    foreach (Transform cube in cubes)
                    {
                        //if ((Behaviour)cube.gameObject.GetComponent("Halo") == null)
                        if (cube.gameObject.GetComponent("Halo") == null && haloPrefab != null)
                        {
                            GameObject halo = Instantiate(haloPrefab) as GameObject;
                            halo.transform.SetParent(cube.transform, false);
                        }
                    }
                }
            }


            addHalo = false;
        }

        if(removeHalo)
        {
            foreach (Transform cubes in transform)
            {
                if (cubes.gameObject.name == "LightCubes")
                {
                    foreach (Transform cube in cubes)
                    {
                        if (cube.gameObject.GetComponent("Halo") == null)
                        {
                            foreach (Transform child in cube)
                            {
                                if (child != null && child.gameObject != null && child.gameObject.GetComponent("Halo") != null)
                                    DestroyImmediate(child.gameObject);
                            }
                        }
                    }
                }
            }
            removeHalo = false;
        }
        */
    }
}
