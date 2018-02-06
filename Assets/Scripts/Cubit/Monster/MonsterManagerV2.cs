using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterManagerV2 : MonoBehaviour
{
    public GameObject player;
    public GameObject topCenterTop;
    public GameObject topCenterMiddle;
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
    void Start()
    {
        player = GameObject.Find("PlayerDrone");
    }

    // Update is called once per frame
    void Update()
    {
        manageCounter();
    }

    public void manageCounter()
    {
        if (!isFreeze && wavesIsActive && !waitingForWave)
        {
            currentTime += Time.deltaTime;
            currentWaveTime += Time.deltaTime;
        }

        if (isFreeze && Input.GetKeyDown(KeyCode.B))
        {
            prepareNextWave();
            isFreeze = false;
        }

        if (waitingForWave)
        {
            if (!isFreeze && wavesIsActive && waitingForWave && timeBetweenWavesCurrent > Time.time)
            {
                countdownText.text = "" + (int)(timeBetweenWavesCurrent - Time.time);
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

    public void startWaves()
    {
        currentWave = 0;
        currentTime = 0;
        wavesIsActive = true;
        timeBetweenWavesCurrent = timeBetweenWaves + Time.time;
        playerChancesCurrent = playerChances;
        topCenterMiddle.GetComponent<showStartText>().showText("Wave 1 is about to start!", timeBetweenWaves);
        player.GetComponent<PlayerLife>().initializeLife();
        prepareNextWave();
    }

    public void endWaves(bool finished)
    {
        if (GameObject.Find("LogInformationObject") != null && GameObject.Find("LogInformationObject").GetComponent<Log>() != null)
            GameObject.Find("LogInformationObject").GetComponent<Log>().logRegisterWaveInformation();
        else if (GameObject.Find("GeneralScriptObject").GetComponent<Log>())
            GameObject.Find("GeneralScriptObject").GetComponent<Log>().logRegisterWaveInformation();


        if (finished)
            topCenterTop.GetComponent<showStartText>().showText("Congratulations! You've made it!\n Your time was:\n" + (int)currentTime, 10);
        else
            topCenterTop.GetComponent<showStartText>().showText("You died at Wave " + currentWave + "! Maybe next time!\nYour time was:\n" + (int)currentTime, 10);

        wavesIsActive = false;
        killAllEnemies(false);
    }

    void prepareNextWave()
    {
        timeBetweenWavesCurrent = timeBetweenWaves + Time.time;
        waitingForWave = true;
        topCenterMiddle.GetComponent<showStartText>().showText("Wave " + (currentWave + 1) + " is about to started!\nCurrent Time: " + (int)currentTime, timeBetweenWaves);
        killAllEnemies(false);
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
        if (GameObject.Find("LogInformationObject") != null && GameObject.Find("LogInformationObject").GetComponent<Log>() != null)
            GameObject.Find("LogInformationObject").GetComponent<Log>().logRegisterWaveInformation();
        else if (GameObject.Find("GeneralScriptObject").GetComponent<Log>())
            GameObject.Find("GeneralScriptObject").GetComponent<Log>().logRegisterWaveInformation();

        killAllEnemies(false);
        topCenterTop.GetComponent<showStartText>().showText("Wave " + (currentWave) + " finished!", timeBetweenWaves);
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
            prepareNextWave();
        }
    }

    void freezeWaves()
    {
        topCenterTop.GetComponent<Text>().text = "Please answer the question sheet :)\n Press B to continue!";
        isFreeze = true;
    }

    void manageWave()
    {
        if (wavesIsActive && throwKilledCrurrentLevel >= wavesThrowKillNumbers[currentWave] && chaseKilledCrurrentLevel >= wavesChaseKillNumbers[currentWave])
        {
            
            currentWave++;
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
                    freezeWaves();
                }
            }
            if(!isFreeze)
                prepareNextWave();
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

    public void killAllEnemies(bool registerDeath)
    {
        foreach (Transform child in transform)
        {
            if (child.gameObject.GetComponent<CubeMonster>() != null && child.gameObject.GetComponent<CubeMonster>().isAlive)
                child.gameObject.GetComponent<CubeMonster>().die(true, registerDeath);
            if (child.gameObject.GetComponent<MonsterChase>() != null && child.gameObject.GetComponent<MonsterChase>().isAlive)
                child.gameObject.GetComponent<MonsterChase>().die(true, registerDeath);
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
