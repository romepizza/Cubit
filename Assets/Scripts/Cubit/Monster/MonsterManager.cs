using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterManager : MonoBehaviour
{
    public GameObject player;
    public GameObject topCenterTop;
    public GameObject topCenterMiddle;
    public GameObject labelEjectorsToKill;
    public GameObject labelWormsToKill;
   // public GameObject gameFinishedPanel;
    public Text countdownText;
    public bool killAll;

    [Header("----- SETTINGS -----")]
    public int playerChances;
    public int[] stopAfterWaves;
    public int[] wavesThrowSpawnNumbers;
    public int[] wavesChaseSpawnNumbers;
    public int[] wavesThrowMaxNumbers;
    public int[] wavesChaseMaxNumbers;
    public float[] wavesThrowSpawnCooldown;
    public float[] wavesChaseSpawnCooldown;
    public int[] wavesThrowKillNumbers;
    public int[] wavesChaseKillNumbers;

    public float timeBetweenWaves;

    [Header("----- Debug -----")]
    public float currentTime;
    public float currentWaveTime;
    public int throwKilledTotal;
    public int throwAlive;
    public int chaseKilledTotal;
    public int chaseAlive;
    public int currentWave;
    public bool wavesIsActive;
    public int throwKilledCrurrentLevel;
    public int chaseKilledCrurrentLevel;
    public int playerDmgThisWave;
    public float timeBetweenWavesCurrent;
    public bool waitingForWave;
    public bool isFreeze;
    public int playerChancesCurrent;
    public float finishTimeSpawnThrowContinuous;
    public float finishTimeSpawnChaseContinuous;
    public int spawnedThrowByContinuous;
    public int spawnedChaseByContinuous;
    public List<float> finishTimesSpawnThrow;
    public List<float> finishTimesSpawnChase;

    [Header("--- (Player) ---")]
    public int p_defaultMaxCubes;
    public float p_defaultGrabInterval;
    public float p_defaultGattlingCooldown;
    public float p_defaultGattlingMaxSpeed;
    public float p_defaultGatherGrabCooldown;
    public float p_defaultGatherGrabRadius;
    public float p_defaultPushMaxSpeed;
    public int p_defaultHp;
    public float p_defaultAccelerationX;
    public float p_defaultAccelerationY;
    public float p_defaultAccelerationZ;

    // Use this for initialization
    void Start()
    {
        player = GameObject.Find("PlayerDrone");
        finishTimesSpawnThrow = new List<float>();
        finishTimesSpawnChase = new List<float>();
        finishTimeSpawnThrowContinuous = float.MaxValue;
        finishTimeSpawnChaseContinuous = float.MaxValue;

        p_defaultMaxCubes = player.GetComponent<GrabSystem>().maxCubes;
        p_defaultGrabInterval = player.GetComponent<GrabSystem>().grabbedInterval;
        p_defaultGattlingCooldown = player.GetComponent<SkillGattling>().cooldownNormal;
        p_defaultGattlingMaxSpeed = player.GetComponent<SkillGattling>().maxSpeed;
        p_defaultGatherGrabCooldown = player.GetComponent<SkillGather>().cooldownPassive;
        p_defaultGatherGrabRadius = player.GetComponent<SkillGather>().radiusPassive;
        p_defaultPushMaxSpeed = player.GetComponent<SkillPush>().maxSpeedGrabbed;
        p_defaultHp = player.GetComponent<PlayerLife>().maxHp;
        p_defaultAccelerationX = player.GetComponent<PlayerMovement>().accelerationPerGradX;
        p_defaultAccelerationY = player.GetComponent<PlayerMovement>().accelerationUpDown;
        p_defaultAccelerationZ = player.GetComponent<PlayerMovement>().accelerationPerGradZ;
    }

    // Update is called once per frame
    void Update()
    {
        manageCounter();
    }

    void manageLabels()
    {
        if(wavesIsActive)
        {
            if (wavesThrowKillNumbers[currentWave] != 0)
                labelEjectorsToKill.GetComponent<Text>().text = "Ejectors:  " + Mathf.Min(throwKilledCrurrentLevel, wavesThrowKillNumbers[currentWave]) + "/" + wavesThrowKillNumbers[currentWave];
            else
                labelEjectorsToKill.GetComponent<Text>().text = "";

            if (wavesChaseKillNumbers[currentWave] != 0)
                labelWormsToKill.GetComponent<Text>().text = "Worms:     " + Mathf.Min(chaseKilledCrurrentLevel, wavesChaseKillNumbers[currentWave]) + "/" + wavesChaseKillNumbers[currentWave];
            else
                labelWormsToKill.GetComponent<Text>().text = "";
        }
        else
        {
            labelEjectorsToKill.GetComponent<Text>().text = "";
            labelWormsToKill.GetComponent<Text>().text = "";
        }
    }

    public void manageCounter()
    {
        if (!isFreeze && wavesIsActive && !waitingForWave)
        {
            currentTime += Time.deltaTime;
            currentWaveTime += Time.deltaTime;
        }

        manageSpawnTimes();

        if (isFreeze && (Input.GetKeyDown(KeyCode.B) /*|| Input.GetButton("ButtonY")*/))
        {
            prepareNextWave();
            isFreeze = false;
        }

        if (waitingForWave)
        {
            if (!isFreeze && wavesIsActive && waitingForWave && timeBetweenWavesCurrent > Time.time)
            {
                countdownText.text = "" + (int)(timeBetweenWavesCurrent - Time.time + 1);
            }
            else
            {
                countdownText.text = "";
            }
        }

        if (!isFreeze && wavesIsActive && waitingForWave && timeBetweenWavesCurrent < Time.time)
        {
            spawnWave();
            waitingForWave = false;
        }

        if (killAll)
        {
            killAllEnemies(true);
            killAll = false;
        }
    }

    void manageSpawnTimes()
    {
        if (isFreeze)
        {
            finishTimeSpawnThrowContinuous += Time.deltaTime;
            finishTimeSpawnChaseContinuous += Time.deltaTime;

            for (int i = 0; i < finishTimesSpawnThrow.Count; i++)
                finishTimesSpawnThrow[i] += Time.deltaTime;
            for (int i = 0; i < finishTimesSpawnChase.Count; i++)
                finishTimesSpawnChase[i] += Time.deltaTime;
        }



        if (wavesIsActive && !isFreeze)
        {
            // Throw Continuous
            if (throwAlive < wavesThrowMaxNumbers[currentWave])
            {
                if (finishTimeSpawnThrowContinuous < Time.time && spawnedThrowByContinuous < (wavesThrowMaxNumbers[currentWave] - wavesThrowSpawnNumbers[currentWave]))
                {
                    createMonsterThrow(1);
                    spawnedThrowByContinuous++;
                    finishTimeSpawnThrowContinuous = wavesThrowSpawnCooldown[currentWave] + Time.time;
                }
            }

            // Throw Respawn
            if (throwAlive < wavesThrowMaxNumbers[currentWave])
            {
                for (int i = finishTimesSpawnThrow.Count - 1; i >= 0; i--)
                {
                    if (finishTimesSpawnThrow[i] < Time.time)
                    {
                        createMonsterThrow(1);
                        finishTimesSpawnThrow.RemoveAt(i);
                    }
                }
            }
            else
            {
                for(int i = finishTimesSpawnThrow.Count - 1; i >= 0; i--)
                {
                    finishTimesSpawnThrow[i] += Time.deltaTime;
                }
            }

            // Chase Continuous
            if (finishTimeSpawnChaseContinuous < Time.time && spawnedChaseByContinuous < wavesChaseMaxNumbers[currentWave] - wavesChaseSpawnNumbers[currentWave])
            {
                createMonsterChase(1);
                spawnedChaseByContinuous++;
                finishTimeSpawnChaseContinuous = wavesChaseSpawnCooldown[currentWave] + Time.time;
            }

            // Chase Respawn
            if (chaseAlive < wavesChaseMaxNumbers[currentWave])
            {
                for (int i = finishTimesSpawnChase.Count - 1; i >= 0; i--)
                {
                    if (finishTimesSpawnChase[i] < Time.time)
                    {
                        createMonsterChase(1);
                        finishTimesSpawnChase.RemoveAt(i);
                    }
                } 
            }
            else
            {
                for (int i = finishTimesSpawnChase.Count - 1; i >= 0; i--)
                {
                    finishTimesSpawnChase[i] += Time.deltaTime;
                }

                finishTimeSpawnChaseContinuous += Time.deltaTime;
            }
        }
    }

    public void startWaves()
    {


        currentWave = 0;
        currentTime = 0;
        wavesIsActive = true;
        timeBetweenWavesCurrent = timeBetweenWaves + Time.time;
        playerChancesCurrent = playerChances;
        topCenterMiddle.GetComponent<ShowTimedText>().showText("Wave 1 is about to start!", timeBetweenWaves);
        player.GetComponent<PlayerLife>().initializeLife();

        upgradeStuff(0);
        prepareNextWave();
    }

    public void endWaves(bool finished)
    {
        if (finished)
        {
            topCenterTop.GetComponent<ShowTimedText>().showText("Congratulations! You've made it!\n Your time was:\n" + (int)currentTime, 10);
            GameObject.Find("GeneralScriptObject").GetComponent<Options>().endGame();
        }
        else
            topCenterTop.GetComponent<ShowTimedText>().showText("You died at Wave " + currentWave + "! Maybe next time!\nYour time was:\n" + (int)currentTime, 10);

        wavesIsActive = false;
        killAllEnemies(false);
    }

    void prepareNextWave()
    {
        timeBetweenWavesCurrent = timeBetweenWaves + Time.time;
        waitingForWave = true;
        topCenterMiddle.GetComponent<ShowTimedText>().showText("Wave " + (currentWave + 1) + " is about to started!\nCurrent Time: " + (int)currentTime, timeBetweenWaves);
        //topCenterTop.GetComponent<ShowTimedText>().showText("Your drone's specks have improved!\nThe enemies grow stronger!", 1f);
        killAllEnemies(false);

        if(currentWave == 5)
        {
            topCenterTop.GetComponent<ShowTimedText>().showText("CHAOS MODE ACTIVATED!\nPrepare yourself!", timeBetweenWaves);
        }

        throwKilledCrurrentLevel = 0;
        chaseKilledCrurrentLevel = 0;
        manageLabels();


        finishTimeSpawnThrowContinuous = float.MaxValue;
        finishTimeSpawnChaseContinuous = float.MaxValue;

        if (GetComponent<EnemyLevel>() != null && GetComponent<EnemyLevel>().enabled)
            gameObject.GetComponent<EnemyLevel>().setEnemyLevel(currentWave);
    }

    void spawnWave()
    {
        playerDmgThisWave = 0;
        currentWaveTime = 0;

        throwKilledCrurrentLevel = 0;
        chaseKilledCrurrentLevel = 0;

        spawnedThrowByContinuous = 0;
        spawnedChaseByContinuous = 0;
        finishTimeSpawnThrowContinuous = wavesThrowSpawnCooldown[currentWave] + Time.time;
        finishTimeSpawnChaseContinuous = wavesChaseSpawnCooldown[currentWave] + Time.time;

        createMonsterThrow(wavesThrowSpawnNumbers[currentWave]);
        createMonsterChase(wavesChaseSpawnNumbers[currentWave]);
    }
    
    void endWave()
    {
        if (GameObject.Find("LogInformationObject") != null && GameObject.Find("LogInformationObject").GetComponent<Log>() != null)
            GameObject.Find("LogInformationObject").GetComponent<Log>().logRegisterWaveInformation();
        else if (GameObject.Find("GeneralScriptObject").GetComponent<Log>())
            GameObject.Find("GeneralScriptObject").GetComponent<Log>().logRegisterWaveInformation();


        killAllEnemies(false);

        finishTimesSpawnThrow = new List<float>();
        finishTimesSpawnChase = new List<float>();

        topCenterTop.GetComponent<ShowTimedText>().showText("Drone's specks have improved!\nThe enemies grow stronger!", timeBetweenWaves);
    }

    public void loseChance()
    {
        playerChancesCurrent--;
        
        if (playerChancesCurrent <= 0)
        {
            GameObject.Find("GeneralScriptObject").GetComponent<Options>().endGame();
            /*
            if (wavesIsActive)
                endWaves(false);
                */
        }
        else
        {
            reInitializeWave();
            freezeWaves("You died!\nChances left: " + playerChancesCurrent);
        }
    }

    void reInitializeWave()
    {
        throwKilledCrurrentLevel = 0;
        chaseKilledCrurrentLevel = 0;
        killAllEnemies(false);
    }

    void freezeWaves(string text)
    {
        topCenterTop.GetComponent<Text>().text = text;
        topCenterMiddle.GetComponent<Text>().text = "Press 'B' to continue!";
        isFreeze = true;
    }

    void manageWave()
    {
        manageLabels();


        if (wavesIsActive && throwKilledCrurrentLevel >= wavesThrowKillNumbers[currentWave] && chaseKilledCrurrentLevel >= wavesChaseKillNumbers[currentWave])
        {
            currentWave++;
            upgradeStuff(currentWave);
            endWave();

            if (currentWave >= wavesThrowSpawnNumbers.Length)
            {
                endWaves(true);
                return;
            }

            for (int i = 0; i < stopAfterWaves.Length; i++)
            {
                if (stopAfterWaves[i] == currentWave)
                {
                    freezeWaves("Time for a break.");
                }
            }
            if (!isFreeze)
                prepareNextWave();
        }
    }

    void upgradeStuff(int level)
    {
        player.GetComponent<GrabSystem>().maxCubes = p_defaultMaxCubes + level * 10;
        player.GetComponent<SkillGattling>().cooldownNormal = p_defaultGattlingCooldown - level * 0.02f;
        player.GetComponent<SkillGattling>().maxSpeed = p_defaultGattlingMaxSpeed + level * 10;
        player.GetComponent<SkillGather>().cooldownPassive = p_defaultGatherGrabCooldown - level * 0.012f;
        player.GetComponent<SkillGather>().radiusPassive = p_defaultGatherGrabRadius + level * 1;
        player.GetComponent<SkillPush>().maxSpeedGrabbed = p_defaultPushMaxSpeed + level * 10;

        player.GetComponent<PlayerLife>().maxHp = p_defaultHp + (int)level * 1;

        player.GetComponent<PlayerMovement>().accelerationPerGradX = p_defaultAccelerationX + level * 0.4f;
        player.GetComponent<PlayerMovement>().accelerationUpDown = p_defaultAccelerationY + level * 6f;
        player.GetComponent<PlayerMovement>().accelerationPerGradZ = p_defaultAccelerationZ + level * 0.4f;

        if(level == wavesThrowKillNumbers.Length - 1)
        {
            player.GetComponent<GrabSystem>().maxCubes = 150;
            player.GetComponent<SkillGattling>().cooldownNormal = 0.09f;
            player.GetComponent<SkillGattling>().maxSpeed = 300f;
            player.GetComponent<SkillGather>().cooldownPassive = 0.07f;
            player.GetComponent<SkillGather>().radiusPassive = 20;
            player.GetComponent<SkillPush>().maxSpeedGrabbed = 200f;

            player.GetComponent<PlayerLife>().maxHp = 20;

            player.GetComponent<PlayerMovement>().accelerationPerGradX = 5;
            player.GetComponent<PlayerMovement>().accelerationUpDown = 100;
            player.GetComponent<PlayerMovement>().accelerationPerGradZ = 5;
        }
    }

    public void registerMonsterThrowDeath()
    {
        throwKilledTotal++;
        throwKilledCrurrentLevel++;
        throwAlive--;
        if (wavesIsActive)
            finishTimesSpawnThrow.Add(wavesThrowSpawnCooldown[currentWave] + Time.time);
        player.GetComponent<PlayerLife>().gainLife(1);
        manageWave();
    }
    public void registerMonsterChaseDeath()
    {
        chaseKilledTotal++;
        chaseKilledCrurrentLevel++;
        chaseAlive--;
        if(wavesIsActive)
            finishTimesSpawnChase.Add(wavesChaseSpawnCooldown[currentWave] + Time.time);
        player.GetComponent<PlayerLife>().gainLife(1);
        manageWave();
    }

    public void killAllEnemies(bool registerDeath)
    {
        foreach (Transform child in transform)
        {
            if (child.gameObject.GetComponent<CubeMonster>() != null && child.gameObject.GetComponent<CubeMonster>().isAlive)
                child.gameObject.GetComponent<CubeMonster>().die(true, registerDeath);
            if (child.gameObject.GetComponent<MonsterChase>() != null && child.gameObject.GetComponent<MonsterChase>().isAlive)
                child.gameObject.GetComponent<MonsterChase>().die(true, registerDeath);
        }
        throwAlive = 0;
        chaseAlive = 0;
    }

    public void createMonsterThrow(int createNumber)
    {
        int monsterCreated = 0;
        for (int i = 0; i < transform.childCount; i++)// Transform cube in transform)
        {
            GameObject cube = transform.GetChild(Random.Range(0, transform.childCount)).gameObject;
            if (cube.GetComponent<ColorCube>() != null && cube.GetComponent<ColorCube>().coreCanBeCore && cube.GetComponent<CubeMonster>() != null)
            {
                if (monsterCreated >= createNumber)
                    break;
                cube.GetComponent<CubeMonster>().createMonster();
                monsterCreated++;
                throwAlive++;
            }
        }
    }
    public void createMonsterChase(int createNumber)
    {
        int monsterCreated = 0;
        for (int i = 0; i < transform.childCount; i++)// Transform cube in transform)
        {
            GameObject cube = transform.GetChild(Random.Range(0, transform.childCount)).gameObject;
            if (cube.GetComponent<ColorCube>() != null && cube.GetComponent<ColorCube>().coreCanBeCore && cube.GetComponent<MonsterChase>() != null)
            {
                if (monsterCreated >= createNumber)
                    break;
                cube.GetComponent<MonsterChase>().createMonster();
                monsterCreated++;
                chaseAlive++;
            }
        }
    }
}


    /*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterManager : MonoBehaviour
{
    public GameObject player;
    public GameObject showText;
    public Text countdownText;
    public bool killAll;

    [Header("----- SETTINGS -----")]
    public int playerChances;
    public int[] stopAfterWaves;
    public int[] wavesThrowSpawnNumbers;
    public int[] wavesThrowKillNumbers;
    public int[] wavesChaseSpawnNumbers;
    public int[] wavesChaseKillNumbers;

    public float timeBetweenWaves;

    [Header("----- Debug -----")]
    public float currentTime;
    public float currentWaveTime;
    public int throwKilledTotal;
    public int chaseKilledTotal;
    public int currentWave;
    public bool wavesIsActive;
    public int throwKilledCrurrentLevel;
    public int chaseKilledCrurrentLevel;
    public int playerDmgThisWave;
    public float timeBetweenWavesCurrent;
    public bool waitingForWave;
    public bool isFreeze;
    public int playerChancesCurrent;

    // Use this for initialization
    void Start ()
    {
        player = GameObject.Find("PlayerDrone");
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (!isFreeze && wavesIsActive && !waitingForWave)
        {
            currentTime += Time.deltaTime;
            currentWaveTime += Time.deltaTime;
        }

        if(isFreeze && Input.GetKeyDown(KeyCode.B))
        {
            timeBetweenWavesCurrent = timeBetweenWaves + Time.time;
            isFreeze = false;
        }

        if(!isFreeze && wavesIsActive && waitingForWave && timeBetweenWavesCurrent > Time.time)
        {
            countdownText.text = "" + (int)(timeBetweenWavesCurrent - Time.time);
        }
        else
        {
            countdownText.text = "";
        }

        if(!isFreeze && wavesIsActive && waitingForWave && timeBetweenWavesCurrent < Time.time)
        {
            spawnWave();
            waitingForWave = false;
        }

        if (killAll)
        {
            killAllEnemies();
            killAll = false;
        }
	}

    public void startWaves()
    {
        currentWave = 0;
        wavesIsActive = true;
        waitingForWave = true;
        timeBetweenWavesCurrent = timeBetweenWaves + Time.time;
        currentTime = 0;
        playerChancesCurrent = playerChances;
        showText.GetComponent<showStartText>().showText("Wave 1 is about to start!", timeBetweenWaves);
        player.GetComponent<PlayerLife>().initializeLife();
        endWave();
    }

    public void endWaves(bool finished)
    {
        if(GameObject.Find("LogInformationObject") != null)
            GameObject.Find("LogInformationObject").GetComponent<Log>().logRegisterWaveInformation();
        else if(GameObject.Find("GeneralScriptObject").GetComponent<Log>())
            GameObject.Find("GeneralScriptObject").GetComponent<Log>().logRegisterWaveInformation();


        if (finished)
            showText.GetComponent<showStartText>().showText("Congratulations! You've made it!\n Your time was:\n" + (int)currentTime, 10);
        else
            showText.GetComponent<showStartText>().showText("You died at Wave " + currentWave + "! Maybe next time!\nYour time was:\n" + (int)currentTime, 10);

        wavesIsActive = false;
        foreach (Transform child in transform)
        {
            if (child.gameObject.GetComponent<CubeMonster>() != null && child.gameObject.GetComponent<CubeMonster>().isAlive)
                child.gameObject.GetComponent<CubeMonster>().die(true);
            if (child.gameObject.GetComponent<MonsterChase>() != null && child.gameObject.GetComponent<MonsterChase>().isAlive)
                child.gameObject.GetComponent<MonsterChase>().die(true);
        }

    }
    

    void spawnWave()
    {
        playerDmgThisWave = 0;
        currentWaveTime = 0;
        throwKilledCrurrentLevel = 0;
        chaseKilledCrurrentLevel = 0;

        createMonsterThrow(wavesThrowSpawnNumbers[currentWave]);
        createMonsterChase(wavesChaseSpawnNumbers[currentWave]);
    }

    void endWave()
    {
        foreach (Transform child in transform)
        {
            if (child.gameObject.GetComponent<CubeMonster>() != null && child.gameObject.GetComponent<CubeMonster>().isAlive)
                child.gameObject.GetComponent<CubeMonster>().die(true);
            if (child.gameObject.GetComponent<MonsterChase>() != null && child.gameObject.GetComponent<MonsterChase>().isAlive)
                child.gameObject.GetComponent<MonsterChase>().die(true);
            throwKilledCrurrentLevel = 0;
            chaseKilledCrurrentLevel = 0;
        }
        if(currentWave > 0)
            showText.GetComponent<showStartText>().showText("Wave " + (currentWave) + " finished!\nWave " + (currentWave + 1) + " is about to started!\nCurrent Time: " + (int)currentTime, timeBetweenWaves);
    }

    public void loseChance()
    {
        playerChancesCurrent--;
        if (playerChancesCurrent <= 0)
        {
            if (wavesIsActive)
                endWaves(false);
        }
        else
        {
            reInitializeCurrentWave();
        }
    }

    void freezeWaves()
    {
        showText.GetComponent<Text>().text = "Please answer the question sheet :)\n Press B to continue!";
        isFreeze = true;
    }

    void manageWave()
    {
        if(wavesIsActive && throwKilledCrurrentLevel >= wavesThrowKillNumbers[currentWave] && chaseKilledCrurrentLevel >= wavesChaseKillNumbers[currentWave])
        {
            if (GameObject.Find("LogInformationObject") != null)
                GameObject.Find("LogInformationObject").GetComponent<Log>().logRegisterWaveInformation();
            else if (GameObject.Find("GeneralScriptObject").GetComponent<Log>())
                GameObject.Find("GeneralScriptObject").GetComponent<Log>().logRegisterWaveInformation();
            currentWave++;
            endWave();
            for(int i = 0; i < stopAfterWaves.Length; i++)
            {
                if (stopAfterWaves[i] == currentWave)
                {
                    freezeWaves();
                }
            }
            if (currentWave >= wavesThrowSpawnNumbers.Length)
            {
                endWaves(true);
                return;
            }
            waitingForWave = true;
            timeBetweenWavesCurrent = timeBetweenWaves + Time.time;
            
        }
    }

    void reInitializeCurrentWave()
    {
        foreach (Transform child in transform)
        {
            if (child.gameObject.GetComponent<CubeMonster>() != null && child.gameObject.GetComponent<CubeMonster>().isAlive)
                child.gameObject.GetComponent<CubeMonster>().die(true);
            if (child.gameObject.GetComponent<MonsterChase>() != null && child.gameObject.GetComponent<MonsterChase>().isAlive)
                child.gameObject.GetComponent<MonsterChase>().die(true);
            throwKilledCrurrentLevel = 0;
            chaseKilledCrurrentLevel = 0;
        }
    }

    public void registerMonsterThrowDeath()
    {
        throwKilledTotal++;
        throwKilledCrurrentLevel++;
        manageWave();
    }

    public void registerMonsterChaseDeath()
    {
        chaseKilledTotal++;
        chaseKilledCrurrentLevel++;
        manageWave();
    }

    public void killAllEnemies()
    {
        foreach (Transform child in transform)
        {
            if (child.gameObject.GetComponent<CubeMonster>() != null && child.gameObject.GetComponent<CubeMonster>().isAlive)
                child.gameObject.GetComponent<CubeMonster>().die(true);
            if (child.gameObject.GetComponent<MonsterChase>() != null && child.gameObject.GetComponent<MonsterChase>().isAlive)
                child.gameObject.GetComponent<MonsterChase>().die(true);
        }
    }

    public void createMonsterThrow(int createNumber)
    {
        int monsterCreated = 0;
        for (int i = 0; i < transform.childCount; i++)// Transform cube in transform)
        {
            GameObject cube = transform.GetChild(Random.Range(0, transform.childCount)).gameObject;
            if (cube.GetComponent<ColorCube>() != null && cube.GetComponent<ColorCube>().coreCanBeCore && cube.GetComponent<CubeMonster>() != null)
            {
                if (monsterCreated >= createNumber)
                    break;
                cube.GetComponent<CubeMonster>().createMonster();
                monsterCreated++;
            }
        }
    }
    public void createMonsterChase(int createNumber)
    {
        int monsterCreated = 0;
        for (int i = 0; i < transform.childCount; i++)// Transform cube in transform)
        {
            GameObject cube = transform.GetChild(Random.Range(0, transform.childCount)).gameObject;
            if (cube.GetComponent<ColorCube>() != null && cube.GetComponent<ColorCube>().coreCanBeCore && cube.GetComponent<MonsterChase>() != null)
            {
                if (monsterCreated >= createNumber)
                    break;
                cube.GetComponent<MonsterChase>().createMonster();
                monsterCreated++;
                
            }
        }
    }
}
*/
