using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Text;

public class ShowLog : MonoBehaviour
{
    public string m_inputFolder;
    [Header("--- Read Operations ---")]
    public bool getStuff;
    public bool visualize;
    public bool getInformation;

    [Header("--- Read Operators ---")]
    public bool getPlayerSummary;
    public bool getWaveInformation;
    public bool getPlayerDeath;
    public bool getSkillGather;
    public bool getSkillPush;
    public bool getSkillGattling;
    public bool getCubeHitPlayer;
    public bool getCubeHitEnemy;

    [Header("--- Write Operations ---")]
    public bool writeStuff;

    [Header("--- Write Operators ---")]
    public bool writePlayerSummary;

    [Header("----- Informational Stuff -----")]

    [Header("--- (Skills) ---")]
    [Header("- (Gather) -")]
    public float averageGathered;
    [Header("- (Push) -")]
    public float pushedRate;
    public float pushedFromGrabbedAverageCubesPushed;
    [Header("- (Gattling) -")]
    public float gattlingedRate;

    [Header("--- (Enemies) ---")]
    [Header("- (Enemy Specific) -")]
    public float dmgByEjectorRate;
    public float dmgByWormRate;
    [Header("- (Cube Type Specific) -")]
    public float dmgByEjectorActiveRate;
    public float dmgByEjectorAttachedRate;
    public float dmgByEjectorCoreRate;
    public float dmgByWormAttachedRate;
    public float dmgByWormCoreRate;

    [Header("--- (Hit Rates) ---")]
    [Header("- (Total: Aimed/not) -")]
    public float hitRateTotal;
    public float hitRateAimed;

    [Header("- (Push) -")]
    public float pushHitRateTotal;
    [Header("(Aimed/not)")]
    public float pushHitRateAimed;
    public float pushHitRateNotAimed;
    [Header("(Grabbed/not)")]
    public float pushHitRateGrabbed;
    public float pushHitRateNotGrabbed;

    [Header("- (Gattling) -")]
    public float gattlingHitRate;
    [Header("(Aimed/not)")]
    public float gattlingHitRateAimed;
    public float gattlingHitRateNotAimed;
    [Header("(Grabbed/not)")]
    public float gattlingHitRateGrabbed;
    public float gattlingHitRateNotGrabbed;


    [Header("----- DEBUG -----")]
    public List<Vector3> positionsPlayerDeath;
    public List<Vector3> positionsPlayerHpLoss;

    [Header("--- (Skills) ---")]
    [Header("- (Gather) -")]
    public float gatheredUsed;
    public float gatheredCubesTotal;
    [Header("- (Push) -")]
    public float pushUsed;
    [Header("- (Gattling) -")]
    public float gattlingUsed;

    [Header("--- (Enemies) ---")]
    public float dmgByEjectorTotal;
    public float dmgByWormTotal;
    public float dmgByEjectorActiveTotal;
    public float dmgByEjectorAttachedTotal;
    public float dmgByEjectorCoreTotal;
    public float dmgByWormAttachedTotal;
    public float dmgByWormCoreTotal;

    [Header("--- (Hit Absolute) ---")]
    [Header("- (Total: Aimed/not) -")]
    public float totalCubesShot;
    public float totalCubesHit;
    public float totalCubesShotAimed;
    public float totalCubesHitAimed;

    [Header("- (Push) -")]
    public float totalCubesShotPushed;
    public float totalCubesHitPushed;
    [Header("(Aimed/not)")]
    public float totalCubesShotPushedAimed;
    public float totalCubesHitPushedAimed;
    public float totalCubesShotPushedNotAimed;
    public float totalCubesHitPushedNotAimed;
    [Header("(Grabbed/not)")]
    public float totalCubesShotPushedGrabbed;
    public float totalCubesHitPushedGrabbed;
    public float totalCubesShotPushedNotGrabbed;
    public float totalCubesHitPushedNotGrabbed;

    [Header("- (Gattling) -")]
    public float totalCubesShotGattlinged;
    public float totalCubesHitGattlinged;
    [Header("- (Aimed/not) -")]
    public float totalCubesShotGattlingedAimed;
    public float totalCubesHitGattlingedAimed;
    public float totalCubesShotGattlingedNotAimed;
    public float totalCubesHitGattlingedNotAimed;
    [Header("- (Grabbed/not) -")]
    public float totalCubesShotGattlingedGrabbed;
    public float totalCubesHitGattlingedGrabbed;
    public float totalCubesShotGattlingedNotGrabbed;
    public float totalCubesHitGattlingedNotGrabbed;


    [Header("--- (New Log) ---")]
    public float[] playerDmgs;
    public float[] playerDmgsEjector;
    public float[] playerDmgsWorm;
    public float[] dmgToEjectorRates;
    public float[] dmgToWormRates;
    public float[] skillRatesPush;
    public float[] skillRatesGattling;

    private StreamWriter sw;
    private StreamReader sr;

    /*
    void showPlayerLifeLossLog()
    {
        foreach(Vector3 vector in playerLifeLossInWave)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(vector, 1.5f);
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireCube(vector, new Vector3(1.5f, 1.5f, 1.5f));
        }
        foreach (Vector3 vector in playerLifeLossNotInWave)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(vector, 1.5f);
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireCube(vector, new Vector3(1.5f, 1.5f, 1.5f));
        }
    }
    */
    void getPlayerSummaryCsv()
    {
        List<float> cubesGrabbedAverage = new List<float>();
        List<int> cubesGrabbedTotal = new List<int>();
        List<int> cubesPushedTotal = new List<int>();
        List<int> cubesPushedGrab = new List<int>();
        List<int> cubesPushedNonGrab = new List<int>();
        List<int> cubesGattlingedTotal = new List<int>();
        List<int> cubesGattlingedGrab = new List<int>();
        List<int> cubesGattlingedNonGrab = new List<int>();
        List<int> playerTookDmg = new List<int>();
        List<float> playerHpAverage = new List<float>();
        List<int> playerChancesLeft = new List<int>();
        List<float> wavesTime = new List<float>();
        List<float> timeTotal = new List<float>();
        List<int> playerId = new List<int>();
        List<string> playerName = new List<string>();

        string path = m_inputFolder + "\\LogPlayerSummary.csv";
        sr = new StreamReader(path);
        string line;
        
        do
        {
            line = sr.ReadLine();
            if (line == null)
                break;

            string[] logs = line.Split(',');
            
            float tmpFloat;
            if (float.TryParse(logs[0], out tmpFloat))
            {
                try
                {
                    cubesGrabbedAverage.Add(float.Parse(logs[0]));
                    cubesGrabbedTotal.Add(int.Parse(logs[1]));
                    cubesPushedTotal.Add(int.Parse(logs[2]));
                    cubesPushedGrab.Add(int.Parse(logs[3]));
                    cubesPushedNonGrab.Add(int.Parse(logs[4]));
                    cubesGattlingedTotal.Add(int.Parse(logs[5]));
                    cubesGattlingedGrab.Add(int.Parse(logs[6]));
                    cubesGattlingedNonGrab.Add(int.Parse(logs[7]));
                    playerTookDmg.Add(int.Parse(logs[8]));
                    playerHpAverage.Add(float.Parse(logs[9]));
                    playerChancesLeft.Add(int.Parse(logs[10]));
                    wavesTime.Add(float.Parse(logs[11]));
                    timeTotal.Add(float.Parse(logs[12]));
                    playerId.Add(int.Parse(logs[13]));
                    playerName.Add(logs[14]);
                    
                    //playerDmgs[int.Parse(logs[13]) - 1] = int.Parse(logs[8]);
                }
                catch (Exception e)
                {
                    Debug.Log("Error: Properbly parsing error!");
                }
            }
        }
        while (line != null);
    }

    void getWaveInformationCsv()
    {
        List<int> wave = new List<int>();
        List<float> waveTime = new List<float>();
        List<float> waveTimeTotal = new List<float>();
        List<Vector3> playerPosition = new List<Vector3>();
        List<int> playerTookDmg = new List<int>();
        List<int> playerChancesLeft = new List<int>();
        List<float> ejectorOrder = new List<float>();
        List<float> wormOrder = new List<float>();
        List<float> timeTotal = new List<float>();
        List<int> playerId = new List<int>();
        List<string> playerName = new List<string>();

        string path = m_inputFolder + "\\LogWaveInformation.csv";
        sr = new StreamReader(path);
        string line;
        do
        {
            line = sr.ReadLine();
            if (line == null)
                break;

            string[] logs = line.Split(',');
            
            int tmp;
            if (int.TryParse(logs[0], out tmp))
            {
                try
                {
                    List<float> tmp1 = new List<float>();

                    wave.Add(int.Parse(logs[0]));
                    waveTime.Add(float.Parse(logs[1]));
                    waveTimeTotal.Add(float.Parse(logs[2]));
                    playerTookDmg.Add(int.Parse(logs[4]));
                    playerChancesLeft.Add(int.Parse(logs[5]));
                    ejectorOrder.Add(float.Parse(logs[7]));
                    wormOrder.Add(float.Parse(logs[9]));
                    timeTotal.Add(float.Parse(logs[10]));
                    playerId.Add(int.Parse(logs[11]));
                    playerName.Add(logs[12]);

                    Vector3 pp = Vector3.zero;
                    string[] ppS = logs[3].Split('|');
                    pp.x = float.Parse(ppS[0].Substring(0, ppS[0].Length - 2));
                    pp.y = float.Parse(ppS[1].Substring(1, ppS[1].Length - 2));
                    pp.z = float.Parse(ppS[2].Substring(1, ppS[2].Length - 1));
                    playerPosition.Add(pp);

                    if (int.Parse(logs[11]) - 1 < 9)
                    {
                        dmgToEjectorRates[int.Parse(logs[11]) - 1] += 0;//float.Parse(logs[7]);
                        dmgToWormRates[int.Parse(logs[11]) - 1] += 0;// float.Parse(logs[9]);
                    }
                }
                catch (Exception e)
                {
                    Debug.Log("Error: Properbly parsing error!");
                }
            }
        }
        while (line != null);
    }

    void getPlayerDeathCsv()
    {
        List<string> diedBy = new List<String>();
        List<int> diedByInt = new List<int>();
        List<int> wave = new List<int>();
        List<float> waveTime = new List<float>();
        List<float> waveTimeTotal = new List<float>();
        List<float> timeTotal = new List<float>();
        List<Vector3> playerPosition = new List<Vector3>();
        List<int> playerChancesLeft = new List<int>();
        List<int> playerId = new List<int>();
        List<string> playerName = new List<string>();

        string path = m_inputFolder + "\\LogPlayerDeath.csv";
        sr = new StreamReader(path);
        string line;

        do
        {
            line = sr.ReadLine();
            if (line == null)
                break;

            string[] logs = line.Split(',');
            
            if (logs[0] != "" && logs[0] != "died by")
            {
                try
                {
                    diedBy.Add(logs[0]);

                    wave.Add(int.Parse(logs[1]));
                    waveTime.Add(float.Parse(logs[2]));
                    waveTimeTotal.Add(float.Parse(logs[3]));
                    timeTotal.Add(float.Parse(logs[4]));
                    playerChancesLeft.Add(int.Parse(logs[6]));
                    playerId.Add(int.Parse(logs[7]));
                    playerName.Add(logs[8]);

                    Vector3 pp = Vector3.zero;
                    string[] ppS = logs[5].Split('|');
                    pp.x = float.Parse(ppS[0].Substring(0, ppS[0].Length - 2));
                    pp.y = float.Parse(ppS[1].Substring(1, ppS[1].Length - 2));
                    pp.z = float.Parse(ppS[2].Substring(1, ppS[2].Length - 1));
                    playerPosition.Add(pp);

                    switch(logs[0])
                    {
                        case "core":
                            diedByInt.Add(0);
                            break;
                        case "attached ejector":
                            diedByInt.Add(1);
                            break;
                        case "active ejector":
                            diedByInt.Add(2);
                            break;
                        case "attached worm":
                            diedByInt.Add(3);
                            break;
                        case "not specifid":
                            diedByInt.Add(-1);
                            break;
                        default:
                            diedByInt.Add(-2);
                            break;
                    }
                }
                catch (Exception e)
                {
                    Debug.Log("Error: Properbly parsing error!");
                }
            }
        }
        while (line != null);

        positionsPlayerDeath = playerPosition;
    }

    void getSkillGatherCsv()
    {
        List<int> grabbedCount = new List<int>();
        List<int> grabbedBefore = new List<int>();
        List<int> grabbedAfter = new List<int>();

        List<Vector3> playerPosition = new List<Vector3>();
        List<int> wave = new List<int>();
        List<float> waveTime = new List<float>();
        List<float> waveTimeTotal = new List<float>();
        List<float> timeTotal = new List<float>();
        List<int> playerChancesLeft = new List<int>();
        List<int> playerId = new List<int>();
        List<string> playerName = new List<string>();

        string path = m_inputFolder + "\\LogSkillGather.csv";
        sr = new StreamReader(path);
        string line;

        do
        {
            line = sr.ReadLine();
            if (line == null)
                break;

            string[] logs = line.Split(',');

            int tmp;
            if (int.TryParse(logs[0], out tmp))
            {
                try
                {
                    grabbedCount.Add(int.Parse(logs[0]));
                    grabbedBefore.Add(int.Parse(logs[1]));
                    grabbedAfter.Add(int.Parse(logs[2]));

                    wave.Add(int.Parse(logs[4]));
                    waveTime.Add(float.Parse(logs[5]));
                    waveTimeTotal.Add(float.Parse(logs[6]));
                    timeTotal.Add(float.Parse(logs[7]));
                    playerChancesLeft.Add(int.Parse(logs[8]));
                    playerId.Add(int.Parse(logs[9]));
                    playerName.Add(logs[10]);

                    Vector3 pp = Vector3.zero;
                    string[] ppS = logs[3].Split('|');
                    pp.x = float.Parse(ppS[0].Substring(0, ppS[0].Length - 2));
                    pp.y = float.Parse(ppS[1].Substring(1, ppS[1].Length - 2));
                    pp.z = float.Parse(ppS[2].Substring(1, ppS[2].Length - 1));
                    playerPosition.Add(pp);
                    
                }
                catch (Exception e)
                {
                    Debug.Log("Error: Properbly parsing error!");
                }
            }
        }
        while (line != null);

        for(int i = 0; i < grabbedCount.Count; i++)
        {
            gatheredUsed++;
            gatheredCubesTotal += grabbedCount[i];
        }
    }

    void getSkillPushCsv()
    {
        List<int> pushedCount = new List<int>();
        List<bool> fromGrabbed = new List<bool>();
        List<bool> isAimed = new List<bool>();

        List<Vector3> playerPosition = new List<Vector3>();
        List<int> wave = new List<int>();
        List<float> waveTime = new List<float>();
        List<float> waveTimeTotal = new List<float>();
        List<float> timeTotal = new List<float>();
        List<int> playerChancesLeft = new List<int>();
        List<int> playerId = new List<int>();
        List<string> playerName = new List<string>();

        string path = m_inputFolder + "\\LogSkillPush.csv";
        sr = new StreamReader(path);
        string line;

        do
        {
            line = sr.ReadLine();
            if (line == null)
                break;

            string[] logs = line.Split(',');

            int tmp;
            if (int.TryParse(logs[0], out tmp))
            {
                try
                {
                    pushedCount.Add(int.Parse(logs[0]));
                    if (logs[1] == "True")
                        fromGrabbed.Add(true);
                    else
                        fromGrabbed.Add(false);
                    if (logs[2] == "True")
                        isAimed.Add(true);
                    else
                        isAimed.Add(false);

                    wave.Add(int.Parse(logs[4]));
                    waveTime.Add(float.Parse(logs[5]));
                    waveTimeTotal.Add(float.Parse(logs[6]));
                    timeTotal.Add(float.Parse(logs[7]));
                    playerChancesLeft.Add(int.Parse(logs[8]));
                    playerId.Add(int.Parse(logs[9]));
                    playerName.Add(logs[10]);

                    Vector3 pp = Vector3.zero;
                    string[] ppS = logs[3].Split('|');
                    pp.x = float.Parse(ppS[0].Substring(0, ppS[0].Length - 2));
                    pp.y = float.Parse(ppS[1].Substring(1, ppS[1].Length - 2));
                    pp.z = float.Parse(ppS[2].Substring(1, ppS[2].Length - 1));
                    playerPosition.Add(pp);

                    if (int.Parse(logs[9]) - 1 < 9)
                        skillRatesPush[int.Parse(logs[9]) - 1] += int.Parse(logs[0]);
                }
                catch (Exception e)
                {
                    Debug.Log("Error: Properbly parsing error!");
                }
            }
        }
        while (line != null);


        pushUsed = pushedCount.Count;
        
        for(int i = 0; i < pushedCount.Count; i++)
        {
            // total cubes hit
            totalCubesShot += pushedCount[i];

            // total cubes hit aimed
            if(isAimed[i])
                totalCubesShotAimed += pushedCount[i];

            // total cubes pushed
            totalCubesShotPushed += pushedCount[i];

            // total cubes pushed aimed/not
            if (isAimed[i])
                totalCubesShotPushedAimed += pushedCount[i];
            else
                totalCubesShotPushedNotAimed += pushedCount[i];

            // total cubes pushed grabbed/not
            if (fromGrabbed[i])
                totalCubesShotPushedGrabbed += pushedCount[i];
            else
                totalCubesShotPushedNotGrabbed += pushedCount[i];
        }


    }

    void getSkillGattlingCsv()
    {
        List<Vector3> direction = new List<Vector3>();
        List<bool> fromGrabbed = new List<bool>();
        List<bool> isAimed = new List<bool>();

        List<Vector3> playerPosition = new List<Vector3>();
        List<int> wave = new List<int>();
        List<float> waveTime = new List<float>();
        List<float> waveTimeTotal = new List<float>();
        List<float> timeTotal = new List<float>();
        List<int> playerChancesLeft = new List<int>();
        List<int> playerId = new List<int>();
        List<string> playerName = new List<string>();

        string path = m_inputFolder + "\\LogSkillGattling.csv";
        sr = new StreamReader(path);
        string line;

        do
        {
            line = sr.ReadLine();
            if (line == null)
                break;

            string[] logs = line.Split(',');

            int tmp;
            if (logs[0] != "" && logs[0] != "direction")
            {
                try
                {
                    if (logs[1] == "True")
                        fromGrabbed.Add(true);
                    else
                        fromGrabbed.Add(false);
                    if (logs[2] == "True")
                        isAimed.Add(true);
                    else
                        isAimed.Add(false);

                    wave.Add(int.Parse(logs[4]));
                    waveTime.Add(float.Parse(logs[5]));
                    waveTimeTotal.Add(float.Parse(logs[6]));
                    timeTotal.Add(float.Parse(logs[7]));
                    playerChancesLeft.Add(int.Parse(logs[8]));
                    playerId.Add(int.Parse(logs[9]));
                    playerName.Add(logs[10]);

                    Vector3 directionTmp = Vector3.zero;
                    string[] ppS = logs[3].Split('|');
                    directionTmp.x = float.Parse(ppS[0].Substring(0, ppS[0].Length - 2));
                    directionTmp.y = float.Parse(ppS[1].Substring(1, ppS[1].Length - 2));
                    directionTmp.z = float.Parse(ppS[2].Substring(1, ppS[2].Length - 1));
                    direction.Add(directionTmp);

                    Vector3 pp = Vector3.zero;
                    ppS = logs[3].Split('|');
                    pp.x = float.Parse(ppS[0].Substring(0, ppS[0].Length - 2));
                    pp.y = float.Parse(ppS[1].Substring(1, ppS[1].Length - 2));
                    pp.z = float.Parse(ppS[2].Substring(1, ppS[2].Length - 1));
                    playerPosition.Add(pp);

                    if (int.Parse(logs[9]) - 1 < 9)
                        skillRatesGattling[int.Parse(logs[9]) - 1]++;
                }
                catch (Exception e)
                {
                    Debug.Log("Error: Properbly parsing error!");
                }
            }
        }
        while (line != null);


        gattlingUsed = direction.Count;
        

        // total cubes shot
        totalCubesShot += direction.Count;

        // total cubes gattlinged
        totalCubesShotGattlinged = direction.Count;

        
        for(int i = 0; i < direction.Count; i++)
        {
            // total cubes shot aimed
            if (isAimed[i])
                totalCubesShotAimed++;

            // total cubes gattlinged aimed/not
            if (isAimed[i])
                totalCubesShotGattlingedAimed++;
            else
                totalCubesShotGattlingedNotAimed++;

            // total cubes gattlinged grabbed/not
            if (fromGrabbed[i])
                totalCubesShotGattlingedGrabbed++;
            else
                totalCubesShotGattlingedNotGrabbed++;
        }
    }

    void getCubeHitPlayerCsv()
    {
        List<string> cubeType = new List<string>();
        List<int> cubeTypeInt = new List<int>();
        List<string> skillType = new List<string>();
        List<int> skillTypeInt = new List<int>();
        List<bool> fromGrabbed = new List<bool>();
        List<bool> isAimed = new List<bool>();

        List<Vector3> playerPosition = new List<Vector3>();
        List<int> wave = new List<int>();
        List<float> waveTime = new List<float>();
        List<float> waveTimeTotal = new List<float>();
        List<float> timeTotal = new List<float>();
        List<int> playerChancesLeft = new List<int>();
        List<int> playerId = new List<int>();
        List<string> playerName = new List<string>();

        string path = m_inputFolder + "\\LogCubePlayerHit.csv";
        sr = new StreamReader(path);
        string line;

        do
        {
            line = sr.ReadLine();
            if (line == null)
                break;

            string[] logs = line.Split(',');

            int tmp;
            if (logs[0] != "" && logs[0] != "cube type")
            {
                try
                {
                    cubeType.Add(logs[0]);
                    switch(logs[0])
                    {
                        case "attached to Ejector":
                            cubeTypeInt.Add(0);
                            break;
                        case "attached to Worm":
                            cubeTypeInt.Add(1);
                            break;
                        case "core by Ejector":
                            cubeTypeInt.Add(2);
                            break;
                        case "core by Worm":
                            cubeTypeInt.Add(3);
                            break;
                        case "not specified":
                            cubeTypeInt.Add(-1);
                            break;
                        default:
                            cubeTypeInt.Add(-2);
                            break;
                    }

                    skillType.Add(logs[1]);
                    switch(logs[1])
                    {
                        case "push":
                            skillTypeInt.Add(0);
                            break;
                        case "gattling":
                            skillTypeInt.Add(1);
                            break;
                        case "player collision":
                            skillTypeInt.Add(2);
                            break;
                        case "not specified":
                            skillTypeInt.Add(-1);
                            break;
                        default:
                            skillTypeInt.Add(-2);
                            break;
                    }

                    if (logs[2] == "True")
                        fromGrabbed.Add(true);
                    else
                        fromGrabbed.Add(false);
                    if (logs[3] == "True")
                        isAimed.Add(true);
                    else
                        isAimed.Add(false);

                    wave.Add(int.Parse(logs[5]));
                    waveTime.Add(float.Parse(logs[6]));
                    waveTimeTotal.Add(float.Parse(logs[7]));
                    timeTotal.Add(float.Parse(logs[8]));
                    playerChancesLeft.Add(int.Parse(logs[9]));
                    playerId.Add(int.Parse(logs[10]));
                    playerName.Add(logs[11]);


                    Vector3 pp = Vector3.zero;
                    string[] ppS = logs[4].Split('|');
                    pp.x = float.Parse(ppS[0].Substring(0, ppS[0].Length - 2));
                    pp.y = float.Parse(ppS[1].Substring(1, ppS[1].Length - 2));
                    pp.z = float.Parse(ppS[2].Substring(1, ppS[2].Length - 1));
                    playerPosition.Add(pp);

                }
                catch (Exception e)
                {
                    Debug.Log("Error: Properbly parsing error!");
                }
            }
        }
        while (line != null);


        // total cubes hit
        totalCubesHit = cubeType.Count;

        
        for(int i = 0; i < cubeType.Count; i++)
        {
            // total cubes hit aimed
            if (isAimed[i])
                totalCubesHitAimed++;

            // total cubes hit pushed
            if(skillType[i] == "push")
                totalCubesHitPushed++;


            // total cubes hit pushed AIM
            if (skillType[i] == "push" && isAimed[i])
                totalCubesHitPushedAimed++;
            // total cubes hit pushed not AIM
            if (skillType[i] == "push" && !isAimed[i])
                totalCubesHitPushedNotAimed++;

            // total cubes hit pushed GRAB
            if (skillType[i] == "push" && fromGrabbed[i])
                totalCubesHitPushedGrabbed++;
            // total cubes hit pushed not GRAB
            if (skillType[i] == "push" && !fromGrabbed[i])
                totalCubesHitPushedNotGrabbed++;


            // total cubes hit gattlinged
            if (skillType[i] == "gattling")
                totalCubesHitGattlinged++;

            // total cubes hit gattlinged AIM
            if (skillType[i] == "gattling" && isAimed[i])
                totalCubesHitGattlingedAimed++;
            // total cubes hit gattlinged not AIM
            if (skillType[i] == "gattling" && !isAimed[i])
                totalCubesHitGattlingedNotAimed++;

            // total cubes hit gattlinged GRAB
            if (skillType[i] == "gattling" && fromGrabbed[i])
                totalCubesHitGattlingedGrabbed++;
            // total cubes hit gattlinged not GRAB
            if (skillType[i] == "gattling" && !fromGrabbed[i])
                totalCubesHitGattlingedNotGrabbed++;

        }
    }

    void getCubeHitEnemyCsv()
    {
        List<string> cubeType = new List<string>();
        List<int> cubeTypeInt = new List<int>();
        List<float> distance = new List<float>();

        List<Vector3> playerPosition = new List<Vector3>();
        List<int> wave = new List<int>();
        List<float> waveTime = new List<float>();
        List<float> waveTimeTotal = new List<float>();
        List<float> timeTotal = new List<float>();
        List<int> playerChancesLeft = new List<int>();
        List<int> playerId = new List<int>();
        List<string> playerName = new List<string>();

        string path = m_inputFolder + "\\LogCubeEnemyHit.csv";
        sr = new StreamReader(path);
        string line;

        do
        {
            line = sr.ReadLine();
            if (line == null)
                break;

            string[] logs = line.Split(',');

            int tmp;
            if (logs[0] != "" && logs[0] != "cube type")
            {
                try
                {
                    cubeType.Add(logs[0]);
                    
                    switch (logs[0])
                    {
                        case "attached to Ejector":
                            cubeTypeInt.Add(0);
                            if(int.Parse(logs[8]) - 1 < 9)
                                playerDmgsEjector[int.Parse(logs[8]) - 1]++;
                            break;
                        case "attached to Worm":
                            cubeTypeInt.Add(1);
                            if (int.Parse(logs[8]) - 1 < 9)
                                playerDmgsWorm[int.Parse(logs[8]) - 1]++;
                            break;
                        case "core by Ejector":
                            cubeTypeInt.Add(2);
                            if (int.Parse(logs[8]) - 1 < 9)
                                playerDmgsEjector[int.Parse(logs[8]) - 1]++;
                            break;
                        case "core by Worm":
                            cubeTypeInt.Add(3);
                            if (int.Parse(logs[8]) - 1 < 9)
                                playerDmgsWorm[int.Parse(logs[8]) - 1]++;
                            break;
                        case "active ejector":
                            cubeTypeInt.Add(4);
                            if (int.Parse(logs[8]) - 1 < 9)
                                playerDmgsEjector[int.Parse(logs[8]) - 1]++;
                            break;
                        case "not specified":
                            cubeTypeInt.Add(-1);
                            break;
                        default:
                            cubeTypeInt.Add(-2);
                            break;
                    }
                    distance.Add(float.Parse(logs[1]));

                    wave.Add(int.Parse(logs[3]));
                    waveTime.Add(float.Parse(logs[4]));
                    waveTimeTotal.Add(float.Parse(logs[5]));
                    timeTotal.Add(float.Parse(logs[6]));
                    playerChancesLeft.Add(int.Parse(logs[7]));
                    playerId.Add(int.Parse(logs[8]));
                    playerName.Add(logs[9]);


                    Vector3 pp = Vector3.zero;
                    string[] ppS = logs[2].Split('|');
                    pp.x = float.Parse(ppS[0].Substring(0, ppS[0].Length - 2));
                    pp.y = float.Parse(ppS[1].Substring(1, ppS[1].Length - 2));
                    pp.z = float.Parse(ppS[2].Substring(1, ppS[2].Length - 1));
                    playerPosition.Add(pp);

                    
                }
                catch (Exception e)
                {
                    Debug.Log("Error: Properbly parsing error!");
                }
            }
        }
        while (line != null);

        positionsPlayerHpLoss = playerPosition;

        for(int i = 0; i < cubeType.Count; i++)
        {
            if(cubeType[i] == "attached to Ejector")
            {
                dmgByEjectorAttachedTotal++;
                dmgByEjectorTotal++;
            }
            if (cubeType[i] == "attached to Worm")
            {
                dmgByWormAttachedTotal++;
                dmgByWormTotal++;
            }
            if (cubeType[i] == "core by Ejector")
            {
                dmgByEjectorCoreTotal++;
                dmgByEjectorTotal++;
            }
            if (cubeType[i] == "core by Worm")
            {
                dmgByWormCoreTotal++;
                dmgByWormTotal++;
            }
            if (cubeType[i] == "active ejector")
            {
                dmgByEjectorActiveTotal++;
                dmgByEjectorTotal++;
            }
        }
    }


    void visualizeStuff(List<Vector3> positions)
    {
        foreach (Vector3 vector in positions)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(vector, 1.5f);
            //Gizmos.color = Color.cyan;
            //Gizmos.DrawWireCube(vector, new Vector3(1.5f, 1.5f, 1.5f));
        }
        /*foreach (Vector3 vector in positions)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(vector, 1.5f);
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireCube(vector, new Vector3(1.5f, 1.5f, 1.5f));
        }*/
    }

    void initializeDebug()
    {
        totalCubesShot = 0;
        totalCubesHit = 0;
        totalCubesShotAimed = 0;
        totalCubesHitAimed = 0;
        totalCubesShotPushed = 0;
        totalCubesHitPushed = 0;
        totalCubesShotPushedAimed = 0;
        totalCubesHitPushedAimed = 0;
        totalCubesShotPushedNotAimed = 0;
        totalCubesHitPushedNotAimed = 0;
        totalCubesShotGattlinged = 0;
        totalCubesHitGattlinged = 0;
        totalCubesShotGattlingedAimed = 0;
        totalCubesHitGattlingedAimed = 0;
        totalCubesShotGattlingedNotAimed = 0;
        totalCubesHitGattlingedNotAimed = 0;
        totalCubesShotPushedGrabbed = 0;
        totalCubesHitPushedGrabbed = 0;
        totalCubesShotPushedNotGrabbed = 0;
        totalCubesHitPushedNotGrabbed = 0;
        totalCubesShotGattlingedGrabbed = 0;
        totalCubesHitGattlingedGrabbed = 0;
        totalCubesShotGattlingedNotGrabbed = 0;
        totalCubesHitGattlingedNotGrabbed = 0;


        gatheredUsed = 0;
        gatheredCubesTotal = 0;
        pushUsed = 0;
        gattlingUsed = 0;


        dmgByEjectorTotal = 0;
        dmgByWormTotal = 0;
        dmgByEjectorAttachedTotal = 0;
        dmgByWormAttachedTotal = 0;
        dmgByEjectorCoreTotal = 0;
        dmgByWormCoreTotal = 0;
        dmgByEjectorActiveTotal = 0;

        // new Log
        playerDmgs = new float[9];
        playerDmgsEjector = new float[9];
        playerDmgsWorm = new float[9];
        dmgToEjectorRates = new float[9];
        dmgToWormRates = new float[9];
        skillRatesPush = new float[9];
        skillRatesGattling = new float[9];
    }

    void getInfos()
    {
        hitRateTotal = totalCubesHit / totalCubesShot;
        hitRateAimed = totalCubesHitAimed / totalCubesShotAimed;
        pushHitRateTotal = totalCubesHitPushed / totalCubesShotPushed;
        pushHitRateAimed = totalCubesHitPushedAimed / totalCubesShotPushedAimed;
        pushHitRateNotAimed = totalCubesHitPushedNotAimed / totalCubesShotPushedNotAimed;
        pushHitRateGrabbed = totalCubesHitPushedGrabbed / totalCubesShotPushedGrabbed;
        pushHitRateNotGrabbed = totalCubesHitPushedNotGrabbed / totalCubesShotPushedNotGrabbed;
        gattlingHitRate = totalCubesHitGattlinged / totalCubesShotGattlinged;
        gattlingHitRateAimed = totalCubesHitGattlingedAimed / totalCubesShotGattlingedAimed;
        gattlingHitRateNotAimed = totalCubesHitGattlingedNotAimed / totalCubesShotGattlingedNotAimed;
        gattlingHitRateGrabbed = totalCubesHitGattlingedGrabbed / totalCubesShotGattlingedGrabbed;
        gattlingHitRateNotGrabbed = totalCubesHitGattlingedNotGrabbed / totalCubesShotGattlingedNotGrabbed;
        Debug.Log(gattlingHitRateNotGrabbed);
        averageGathered = gatheredCubesTotal / gatheredUsed;
        pushedRate = totalCubesShotPushed / (totalCubesShotPushed + totalCubesShotGattlinged);
        pushedFromGrabbedAverageCubesPushed = totalCubesShotPushedGrabbed / pushUsed;
        gattlingedRate = totalCubesShotGattlinged / (totalCubesShotPushed + totalCubesShotGattlinged);


        dmgByEjectorRate = dmgByEjectorTotal / (dmgByEjectorTotal + dmgByWormTotal);
        dmgByWormRate = dmgByWormTotal / (dmgByEjectorTotal + dmgByWormTotal);
        dmgByEjectorAttachedRate = dmgByEjectorAttachedTotal / (dmgByEjectorAttachedTotal + dmgByWormAttachedTotal + dmgByEjectorCoreTotal + dmgByWormCoreTotal + dmgByEjectorActiveTotal);
        dmgByWormAttachedRate = dmgByWormAttachedTotal / (dmgByEjectorAttachedTotal + dmgByWormAttachedTotal + dmgByEjectorCoreTotal + dmgByWormCoreTotal + dmgByEjectorActiveTotal);
        dmgByEjectorCoreRate = dmgByEjectorCoreTotal / (dmgByEjectorAttachedTotal + dmgByWormAttachedTotal + dmgByEjectorCoreTotal + dmgByWormCoreTotal + dmgByEjectorActiveTotal);
        dmgByWormCoreRate = dmgByWormCoreTotal / (dmgByEjectorAttachedTotal + dmgByWormAttachedTotal + dmgByEjectorCoreTotal + dmgByWormCoreTotal + dmgByEjectorActiveTotal);
        dmgByEjectorActiveRate = dmgByEjectorActiveTotal / (dmgByEjectorAttachedTotal + dmgByWormAttachedTotal + dmgByEjectorCoreTotal + dmgByWormCoreTotal + dmgByEjectorActiveTotal);
    }

    // Write new Log
    void writeNewLogPlayerSummary()
    {
        List<string[]> rowData = new List<string[]>();

        string filePath = getPath("LogEvaluateSummaryTmp.csv");

        // 0. PlayerId
        // 1. last done wave
        // 2. died count
        // 3. took dmg total
        // 4. rate by ejector
        // 5. rate by worm
        // 6. rate to ejector
        // 7. rate to worm
        // 8. skill rate push
        // 9. skill rate gattling
        for (int i = 0; i < 9; i++)
        {

            string[] rowDataTemp = new string[9];

            rowDataTemp[0] = (i + 1).ToString();
            rowDataTemp[3] = playerDmgs[i].ToString();
            rowDataTemp[4] = (playerDmgsEjector[i] / (playerDmgsEjector[i] + playerDmgsWorm[i])).ToString("F2");
            rowDataTemp[5] = (playerDmgsWorm[i] / (playerDmgsEjector[i] + playerDmgsWorm[i])).ToString("F2");
            rowDataTemp[6] = (skillRatesPush[i] / (skillRatesPush[i] + skillRatesGattling[i])).ToString("F2");
            rowDataTemp[7] = (skillRatesGattling[i] / (skillRatesPush[i] + skillRatesGattling[i])).ToString("F2");
            /*
            rowDataTemp[0] = (player.GetComponent<GrabSystem>().logGrabbedCount * player.GetComponent<GrabSystem>().grabbedInterval / waveSystem.GetComponent<MonsterManager>().currentTime).ToString("F2");
            rowDataTemp[1] = player.GetComponent<GrabSystem>().totalGrabbed.ToString();
            rowDataTemp[2] = (player.GetComponent<GrabSystem>().pushedFromGrabTotal + player.GetComponent<SkillPush>().pushedNotFromGrabbedTotal).ToString();
            rowDataTemp[3] = player.GetComponent<GrabSystem>().pushedFromGrabTotal.ToString();
            rowDataTemp[4] = player.GetComponent<SkillPush>().pushedNotFromGrabbedTotal.ToString();
            rowDataTemp[5] = (player.GetComponent<SkillGattling>().gattlingedFromGrabbedTotal + player.GetComponent<SkillGattling>().gattlingedNotFromGrabbedTotal).ToString();
            rowDataTemp[6] = player.GetComponent<SkillGattling>().gattlingedFromGrabbedTotal.ToString();
            rowDataTemp[7] = player.GetComponent<SkillGattling>().gattlingedNotFromGrabbedTotal.ToString();
            rowDataTemp[8] = player.GetComponent<PlayerLife>().lifeLossTotal.ToString();
            rowDataTemp[9] = (player.GetComponent<PlayerLife>().averageHp / waveSystem.GetComponent<MonsterManager>().currentTime).ToString("F2");

            rowDataTemp[10] = ((waveSystem.GetComponent<MonsterManager>().wavesIsActive) ? (waveSystem.GetComponent<MonsterManager>().playerChancesCurrent) : (-1)).ToString();
            rowDataTemp[11] = ((waveSystem.GetComponent<MonsterManager>().wavesIsActive) ? (waveSystem.GetComponent<MonsterManager>().currentTime) : (-1)).ToString("F2");
            rowDataTemp[12] = GetComponent<GameTime>().timePlayed.ToString("F2");
            rowDataTemp[13] = GameObject.Find("LogInformationObject") != null ? GameObject.Find("LogInformationObject").GetComponent<PlayerCounter>().playerNumber.ToString() : "-1";
            rowDataTemp[14] = GameObject.Find("LogInformationObject") != null ? GameObject.Find("LogInformationObject").GetComponent<PlayerCounter>().currentPlayerName : "-1";
            */
            rowData.Add(rowDataTemp);
        }

        string[][] output = new string[rowData.Count][];
        for (int i = 0; i < output.Length; i++)
        {
            output[i] = rowData[i];
        }
        int length = output.GetLength(0);
        string delimiter = ",";
        StringBuilder sb = new StringBuilder();
        for (int index = 0; index < length; index++)
            sb.AppendLine(string.Join(delimiter, output[index]));
        StreamWriter outStream = System.IO.File.AppendText(filePath);
        outStream.WriteLine(sb);
        outStream.Close();

    }

    private string getPath(string dataName)
    {
        return Application.dataPath + "/../LogsEvaluate/" + dataName;
    }

    void OnDrawGizmos()
    {
        if(getStuff)
        {
            initializeDebug();

            if (getPlayerSummary)
                getPlayerSummaryCsv();
            if (getWaveInformation)
                getWaveInformationCsv();
            if (getPlayerDeath)
                getPlayerDeathCsv();
            if (getSkillGather)
                getSkillGatherCsv();
            if (getSkillPush)
                getSkillPushCsv();
            if (getSkillGattling)
                getSkillGattlingCsv();
            if (getCubeHitPlayer)
                getCubeHitPlayerCsv();
            if (getCubeHitEnemy)
                getCubeHitEnemyCsv();


            getStuff = false;
        }

        if(writeStuff)
        {
            if (writePlayerSummary)
                writeNewLogPlayerSummary();

            writeStuff = false;
        }
        
        if(visualize)
        {
            if(getPlayerDeath)
            {
                visualizeStuff(positionsPlayerDeath);
            }
            if (getCubeHitEnemy)
                visualizeStuff(positionsPlayerHpLoss);
        }

        if(getInformation)
        {
            getInfos();
            getInformation = false;
        }
    }
}
