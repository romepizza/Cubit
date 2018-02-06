using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Log : MonoBehaviour
{
    public GameObject text;

    [Header("----- SETTINGS -----")]
    public bool recordLog;
    public string playerName = "PlayerDrone";
    public string waveSystemName = "Cubes x4";

    [Header("--- (specify log) ---")]
    public bool initialize;
    public bool delete;
    public bool initializeOnStart;
    public bool logPlayerSummary;
    public bool logWaveInformation;
    public bool logPlayerDeath;
    private bool logPlayerLifeLoss;
    private bool logPlayerLifeGain;
    public bool logSkillGather;
    public bool logSkillPush;
    public bool logSkillGattling;
    private bool logCubeGrab;
    public bool logCubePlayerHit;
    private bool logCubeLaunch;
    public bool logCubeEnemyHit;

    [Header("----- DEBUG -----")]
    public GameObject player;
    public GameObject waveSystem;
    public bool testLog;
    public bool testRead;
    public float hitOrderCounter;
    public float hitOrderEjector;
    public float hitOrderWorm;
    //public string path = "D:\\Uni\\Unity3D Workspace\\Cubit\\Assets\\Logs\\statistic.txt";

    public StreamWriter sw;
    public StreamReader sr;

    void Start()
    {
        if (initializeOnStart)
            initializeAll();
    }
    void Update()
    {
        if (SceneManager.GetSceneByBuildIndex(1) == SceneManager.GetActiveScene())
        {
            if (player == null)
                player = GameObject.Find(playerName);
            if (waveSystem == null)
                waveSystem = GameObject.Find(waveSystemName);

            if (player == null)
                Debug.Log("Error: Could not find Player object");
            if (waveSystem == null)
                Debug.Log("Error: Could not find Wave System object");
        }
    }

    void initializeAll()
    {
        string filePath;
        List<string[]> rowData;
        string[] rowDataTemp;
        string[][] output;
        int length;
        string delimiter;
        StringBuilder sb;
        StreamWriter outStream;

        if(logPlayerSummary)
        {
            
            filePath = getPath("LogPlayerSummary.csv");

            rowData = new List<string[]>();
            rowDataTemp = new string[15];
            rowDataTemp[0] = "average grabbed cubes";
            rowDataTemp[1] = "cubes grabbed total";
            rowDataTemp[2] = "cubes pushed total";
            rowDataTemp[3] = "cubes pushed from grab";
            rowDataTemp[4] = "cubes pushed not from grab";
            rowDataTemp[5] = "cubes gattlinged total";
            rowDataTemp[6] = "cubes gattlinged from grab";
            rowDataTemp[7] = "cubes gattlinged not from grab";
            rowDataTemp[8] = "player took dmg total";
            rowDataTemp[9] = "player hp average";

            rowDataTemp[10] = "chances left";
            rowDataTemp[11] = "waves time";
            rowDataTemp[12] = "time total";
            rowDataTemp[13] = "player number";
            rowDataTemp[14] = "player name";
            rowData.Add(rowDataTemp);

            output = new string[rowData.Count][];
            for (int i = 0; i < output.Length; i++)
            {
                output[i] = rowData[i];
            }
            length = output.GetLength(0);
            delimiter = ",";
            sb = new StringBuilder();
            for (int index = 0; index < length; index++)
                sb.AppendLine(string.Join(delimiter, output[index]));
            outStream = System.IO.File.AppendText(filePath);
            outStream.WriteLine(sb);
            outStream.Close();
        }

        if (logWaveInformation)
        {
            // Wave Information
            filePath = getPath("LogWaveInformation.csv");

            rowData = new List<string[]>();
            rowDataTemp = new string[13];
            rowDataTemp[0] = "wave";
            rowDataTemp[1] = "wave took seconds";
            rowDataTemp[2] = "wave time total";
            rowDataTemp[3] = "player position";
            rowDataTemp[4] = "player took dmg";
            rowDataTemp[5] = "chances left";
            rowDataTemp[6] = "ejectors killed";
            rowDataTemp[7] = "average ejector order";
            rowDataTemp[8] = "worms killed";
            rowDataTemp[9] = "average worm order";
            rowDataTemp[10] = "time total";
            rowDataTemp[11] = "player number";
            rowDataTemp[12] = "player name";
            rowData.Add(rowDataTemp);

            output = new string[rowData.Count][];
            for (int i = 0; i < output.Length; i++)
            {
                output[i] = rowData[i];
            }
            length = output.GetLength(0);
            delimiter = ",";
            sb = new StringBuilder();
            for (int index = 0; index < length; index++)
                sb.AppendLine(string.Join(delimiter, output[index]));
            outStream = System.IO.File.AppendText(filePath);
            outStream.WriteLine(sb);
            outStream.Close();
        }

        if (logPlayerDeath)
        {
            // Player Death

            filePath = getPath("LogPlayerDeath.csv");

            rowData = new List<string[]>();
            rowDataTemp = new string[9];
            rowDataTemp = new string[9];
            rowDataTemp[0] = "died by";

            rowDataTemp[1] = "wave";
            rowDataTemp[2] = "wave took seconds";
            rowDataTemp[3] = "wave time total";
            rowDataTemp[4] = "time total";
            rowDataTemp[5] = "player position";
            rowDataTemp[6] = "chances left";
            rowDataTemp[7] = "player number";
            rowDataTemp[8] = "player name";
            rowData.Add(rowDataTemp);

            output = new string[rowData.Count][];
            for (int i = 0; i < output.Length; i++)
            {
                output[i] = rowData[i];
            }
            length = output.GetLength(0);
            delimiter = ",";
            sb = new StringBuilder();
            for (int index = 0; index < length; index++)
                sb.AppendLine(string.Join(delimiter, output[index]));
            outStream = System.IO.File.AppendText(filePath);
            outStream.WriteLine(sb);
            outStream.Close();
        }

        if (logPlayerLifeLoss)
        {
            // Player Life Loss
            /*
            filePath = getPath("LogPlayerLifeLoss.csv");

            rowData = new List<string[]>();
            rowDataTemp = new string[9];
            rowDataTemp = new string[9];
            rowDataTemp[0] = "took dmg by";
            rowDataTemp[1] = "distance";

            rowDataTemp[2] = "wave";
            rowDataTemp[3] = "wave took seconds";
            rowDataTemp[4] = "wave time total";
            rowDataTemp[5] = "time total";
            rowDataTemp[6] = "chances left";
            rowDataTemp[7] = "player number";
            rowDataTemp[8] = "player name";
            rowData.Add(rowDataTemp);

            output = new string[rowData.Count][];
            for (int i = 0; i < output.Length; i++)
            {
                output[i] = rowData[i];
            }
            length = output.GetLength(0);
            delimiter = ",";
            sb = new StringBuilder();
            for (int index = 0; index < length; index++)
                sb.AppendLine(string.Join(delimiter, output[index]));
            outStream = System.IO.File.AppendText(filePath);
            outStream.WriteLine(sb);
            outStream.Close();
            */
        }

        if (logSkillGather)
        {
            // Skill Grab
            filePath = getPath("LogSkillGather.csv");

            rowData = new List<string[]>();
            rowDataTemp = new string[11];
            rowDataTemp = new string[11];
            rowDataTemp[0] = "grabbed count";
            rowDataTemp[1] = "grabbed before";
            rowDataTemp[2] = "grabbed after";
            rowDataTemp[3] = "player position";

            rowDataTemp[4] = "wave";
            rowDataTemp[5] = "wave took seconds";
            rowDataTemp[6] = "wave time total";
            rowDataTemp[7] = "time total";
            rowDataTemp[8] = "chances left";
            rowDataTemp[9] = "player number";
            rowDataTemp[10] = "player name";
            rowData.Add(rowDataTemp);

            output = new string[rowData.Count][];
            for (int i = 0; i < output.Length; i++)
            {
                output[i] = rowData[i];
            }
            length = output.GetLength(0);
            delimiter = ",";
            sb = new StringBuilder();
            for (int index = 0; index < length; index++)
                sb.AppendLine(string.Join(delimiter, output[index]));
            outStream = System.IO.File.AppendText(filePath);
            outStream.WriteLine(sb);
            outStream.Close();
        }

        if (logSkillPush)
        {
            // Skill Push
            filePath = getPath("LogSkillPush.csv");

            rowData = new List<string[]>();
            rowDataTemp = new string[11];
            rowDataTemp = new string[11];
            rowDataTemp[0] = "pushed count";
            rowDataTemp[1] = "from grabbed";
            rowDataTemp[2] = "is aimed";
            rowDataTemp[3] = "player position";

            rowDataTemp[4] = "wave";
            rowDataTemp[5] = "wave took seconds";
            rowDataTemp[6] = "wave time total";
            rowDataTemp[7] = "time total";
            rowDataTemp[8] = "chances left";
            rowDataTemp[9] = "player number";
            rowDataTemp[10] = "player name";
            rowData.Add(rowDataTemp);

            output = new string[rowData.Count][];
            for (int i = 0; i < output.Length; i++)
            {
                output[i] = rowData[i];
            }
            length = output.GetLength(0);
            delimiter = ",";
            sb = new StringBuilder();
            for (int index = 0; index < length; index++)
                sb.AppendLine(string.Join(delimiter, output[index]));
            outStream = System.IO.File.AppendText(filePath);
            outStream.WriteLine(sb);
            outStream.Close();
        }

        if(logSkillGattling)
        {
            // Skill Gattling
            filePath = getPath("LogSkillGattling.csv");

            rowData = new List<string[]>();
            rowDataTemp = new string[11];
            rowDataTemp = new string[11];
            rowDataTemp[0] = "direction";
            rowDataTemp[1] = "from grabbed";
            rowDataTemp[2] = "is aimed";
            rowDataTemp[3] = "player position";

            rowDataTemp[4] = "wave";
            rowDataTemp[5] = "wave took seconds";
            rowDataTemp[6] = "wave time total";
            rowDataTemp[7] = "time total";
            rowDataTemp[8] = "chances left";
            rowDataTemp[9] = "player number";
            rowDataTemp[10] = "player name";
            rowData.Add(rowDataTemp);

            output = new string[rowData.Count][];
            for (int i = 0; i < output.Length; i++)
            {
                output[i] = rowData[i];
            }
            length = output.GetLength(0);
            delimiter = ",";
            sb = new StringBuilder();
            for (int index = 0; index < length; index++)
                sb.AppendLine(string.Join(delimiter, output[index]));
            outStream = System.IO.File.AppendText(filePath);
            outStream.WriteLine(sb);
            outStream.Close();
        }

        if(logCubeGrab)
        {
            // Cube Grab
            filePath = getPath("LogCubeGrab.csv");

            rowData = new List<string[]>();
            rowDataTemp = new string[9];
            rowDataTemp = new string[9];
            rowDataTemp[0] = "cube position";
            rowDataTemp[1] = "player position";

            rowDataTemp[2] = "wave";
            rowDataTemp[3] = "wave took seconds";
            rowDataTemp[4] = "wave time total";
            rowDataTemp[5] = "time total";
            rowDataTemp[6] = "chances left";
            rowDataTemp[7] = "player number";
            rowDataTemp[8] = "player name";
            rowData.Add(rowDataTemp);

            output = new string[rowData.Count][];
            for (int i = 0; i < output.Length; i++)
            {
                output[i] = rowData[i];
            }
            length = output.GetLength(0);
            delimiter = ",";
            sb = new StringBuilder();
            for (int index = 0; index < length; index++)
                sb.AppendLine(string.Join(delimiter, output[index]));
            outStream = System.IO.File.AppendText(filePath);
            outStream.WriteLine(sb);
            outStream.Close();
        }

        if(logCubePlayerHit)
        {
            // Cube Player Hit
            filePath = getPath("LogCubePlayerHit.csv");

            rowData = new List<string[]>();
            rowDataTemp = new string[12];
            rowDataTemp = new string[12];
            rowDataTemp[0] = "cube type";
            rowDataTemp[1] = "skill type";
            rowDataTemp[2] = "from grabbed";
            rowDataTemp[3] = "was aimed";
            rowDataTemp[4] = "player position";

            rowDataTemp[5] = "wave";
            rowDataTemp[6] = "wave took seconds";
            rowDataTemp[7] = "wave time total";
            rowDataTemp[8] = "time total";
            rowDataTemp[9] = "chances left";
            rowDataTemp[10] = "player number";
            rowDataTemp[11] = "player name";
            rowData.Add(rowDataTemp);

            output = new string[rowData.Count][];
            for (int i = 0; i < output.Length; i++)
            {
                output[i] = rowData[i];
            }
            length = output.GetLength(0);
            delimiter = ",";
            sb = new StringBuilder();
            for (int index = 0; index < length; index++)
                sb.AppendLine(string.Join(delimiter, output[index]));
            outStream = System.IO.File.AppendText(filePath);
            outStream.WriteLine(sb);
            outStream.Close();
        }

        if(logCubeLaunch)
        {
            // Cube Launch
            filePath = getPath("LogCubeLaunch.csv");

            rowData = new List<string[]>();
            rowDataTemp = new string[13];
            rowDataTemp = new string[13];
            rowDataTemp[0] = "skill type";
            rowDataTemp[1] = "from grabbed";
            rowDataTemp[2] = "was aimed";
            rowDataTemp[3] = "direction";
            rowDataTemp[4] = "cube position";
            rowDataTemp[5] = "player position";

            rowDataTemp[6] = "wave";
            rowDataTemp[7] = "wave took seconds";
            rowDataTemp[8] = "wave time total";
            rowDataTemp[9] = "time total";
            rowDataTemp[10] = "chances left";
            rowDataTemp[11] = "player number";
            rowDataTemp[12] = "player name";
            rowData.Add(rowDataTemp);

            output = new string[rowData.Count][];
            for (int i = 0; i < output.Length; i++)
            {
                output[i] = rowData[i];
            }
            length = output.GetLength(0);
            delimiter = ",";
            sb = new StringBuilder();
            for (int index = 0; index < length; index++)
                sb.AppendLine(string.Join(delimiter, output[index]));
            outStream = System.IO.File.AppendText(filePath);
            outStream.WriteLine(sb);
            outStream.Close();
        }

        if(logCubeEnemyHit)
        {
            // Cube Enemy Hit
            filePath = getPath("LogCubeEnemyHit.csv");

            rowData = new List<string[]>();
            rowDataTemp = new string[10];
            rowDataTemp = new string[10];
            rowDataTemp[0] = "cube type";
            rowDataTemp[1] = "distance";
            rowDataTemp[2] = "player position";

            rowDataTemp[3] = "wave";
            rowDataTemp[4] = "wave took seconds";
            rowDataTemp[5] = "wave time total";
            rowDataTemp[6] = "time total";
            rowDataTemp[7] = "chances left";
            rowDataTemp[8] = "player number";
            rowDataTemp[9] = "player name";
            rowData.Add(rowDataTemp);

            output = new string[rowData.Count][];
            for (int i = 0; i < output.Length; i++)
            {
                output[i] = rowData[i];
            }
            length = output.GetLength(0);
            delimiter = ",";
            sb = new StringBuilder();
            for (int index = 0; index < length; index++)
                sb.AppendLine(string.Join(delimiter, output[index]));
            outStream = System.IO.File.AppendText(filePath);
            outStream.WriteLine(sb);
            outStream.Close();
        }
    }

    void deleteAll()
    {
        string filePath;
        StreamWriter outStream;
        StringBuilder sb;

        if (logPlayerSummary)
        {
            // Player Summary
            filePath = getPath("LogPlayerSummary.csv");
            outStream = System.IO.File.CreateText(filePath);
            sb = new StringBuilder();
            outStream.WriteLine(sb);
            outStream.Close();
        }

        if (logWaveInformation)
        {
            // Wave Information
            filePath = getPath("LogWaveInformation.csv");
            outStream = System.IO.File.CreateText(filePath);
            sb = new StringBuilder();
            outStream.WriteLine(sb);
            outStream.Close();
        }

        if (logPlayerDeath)
        {
            // Player Death
            filePath = getPath("LogPlayerDeath.csv");
            outStream = System.IO.File.CreateText(filePath);
            sb = new StringBuilder();
            outStream.WriteLine(sb);
            outStream.Close();
        }

        if (logPlayerLifeLoss)
        {
            // Player Lost Life
            filePath = getPath("LogPlayerLifeLoss.csv");
            outStream = System.IO.File.CreateText(filePath);
            sb = new StringBuilder();
            outStream.WriteLine(sb);
            outStream.Close();
        }

        if (logPlayerLifeGain)
        {
            // Player Gained Life
            filePath = getPath("LogPlayerLifeGain.csv");
            outStream = System.IO.File.CreateText(filePath);
            sb = new StringBuilder();
            outStream.WriteLine(sb);
            outStream.Close();
        }
        if (logSkillGather)
        {
            // Skill Gather
            filePath = getPath("LogSkillGather.csv");
            outStream = System.IO.File.CreateText(filePath);
            sb = new StringBuilder();
            outStream.WriteLine(sb);
            outStream.Close();
        }
        if (logSkillPush)
        {
            // Skill Push
            filePath = getPath("LogSkillPush.csv");
            outStream = System.IO.File.CreateText(filePath);
            sb = new StringBuilder();
            outStream.WriteLine(sb);
            outStream.Close();
        }

        if (logSkillGattling)
        {
            // Skill Gattling
            filePath = getPath("LogSkillGattling.csv");
            outStream = System.IO.File.CreateText(filePath);
            sb = new StringBuilder();
            outStream.WriteLine(sb);
            outStream.Close();
        }

        if (logCubeGrab)
        {
            // Cube Grab
            filePath = getPath("LogCubeGrab.csv");
            outStream = System.IO.File.CreateText(filePath);
            sb = new StringBuilder();
            outStream.WriteLine(sb);
            outStream.Close();
        }

        if (logCubePlayerHit)
        {
            // Cube Player Hit
            filePath = getPath("LogCubePlayerHit.csv");
            outStream = System.IO.File.CreateText(filePath);
            sb = new StringBuilder();
            outStream.WriteLine(sb);
            outStream.Close();
        }

        if (logCubeLaunch)
        {
            // Cube Launch
            filePath = getPath("LogCubeLaunch.csv");
            outStream = System.IO.File.CreateText(filePath);
            sb = new StringBuilder();
            outStream.WriteLine(sb);
            outStream.Close();
        }

        if (logCubeEnemyHit)
        {
            // Cube Enemy Hit
            filePath = getPath("LogCubeEnemyHit.csv");
            outStream = System.IO.File.CreateText(filePath);
            sb = new StringBuilder();
            outStream.WriteLine(sb);
            outStream.Close();
        }
    }

    // Update is called once per frame

    public void log()
    {
        List<string[]> rowData = new List<string[]>();

        // Creating First row of titles manually..
        string[] rowDataTemp = new string[3];
        rowDataTemp[0] = "Name";
        rowDataTemp[1] = "ID";
        rowDataTemp[2] = "Income";
        rowData.Add(rowDataTemp);

        // You can add up the values in as many cells as you want.
        for (int i = 0; i < 10; i++)
        {
            rowDataTemp = new string[3];
            rowDataTemp[0] = "Sushanta" + i; // name
            rowDataTemp[1] = "" + i; // ID
            rowDataTemp[2] = "$" + UnityEngine.Random.Range(5000, 10000); // Income
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


        string filePath = getPath("TestLog.csv");

        StreamWriter outStream = System.IO.File.AppendText(filePath);
        outStream.WriteLine(sb);
        outStream.Close();
    }
    
    public void read()
    {
        string line;
        sr = new StreamReader("");
        using (sr)
        {
            do
            {
                line = sr.ReadLine();

                if (line != null)
                {
                    string[] entries = line.Split(',');
                    if (entries.Length > 0)
                        Debug.Log(line);
                }
            }
            while (line != null);  
            sr.Close();
        }
    }
    
    public void logRegisterPlayerSummary()
    {
        if (recordLog && logPlayerSummary)
        {
            List<string[]> rowData = new List<string[]>();

            string filePath = getPath("LogPlayerSummary.csv");

            string[] rowDataTemp = new string[15];
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
            rowData.Add(rowDataTemp);

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
    }

    public void logRegisterWaveInformation()
    {
        if (recordLog && logWaveInformation)
        {
            List<string[]> rowData = new List<string[]>();
           
            string filePath = getPath("LogWaveInformation.csv");

            string[] rowDataTemp = new string[13];
            rowDataTemp[0] = ((waveSystem.GetComponent<MonsterManager>().wavesIsActive) ? (waveSystem.GetComponent<MonsterManager>().currentWave) : (-1)) + "";
            rowDataTemp[1] = ((waveSystem.GetComponent<MonsterManager>().wavesIsActive) ? (waveSystem.GetComponent<MonsterManager>().currentWaveTime) : (-1)).ToString("F2");
            rowDataTemp[2] = ((waveSystem.GetComponent<MonsterManager>().wavesIsActive) ? (waveSystem.GetComponent<MonsterManager>().currentTime) : (-1)).ToString("F2");
            rowDataTemp[3] = player.transform.position.x.ToString() + " | " + player.transform.position.y.ToString() + " | " + player.transform.position.z.ToString();
            rowDataTemp[4] = waveSystem.GetComponent<MonsterManager>().playerDmgThisWave.ToString();
            rowDataTemp[5] = ((waveSystem.GetComponent<MonsterManager>().wavesIsActive) ? (waveSystem.GetComponent<MonsterManager>().playerChancesCurrent) : (-1)).ToString();
            rowDataTemp[6] = waveSystem.GetComponent<MonsterManager>().throwKilledCrurrentLevel.ToString();// + " / " + waveSystem.GetComponent<MonsterManager>().wavesThrowSpawnNumbers[waveSystem.GetComponent<MonsterManager>().currentWave];
            rowDataTemp[7] = (hitOrderEjector / hitOrderCounter).ToString("F2");
            rowDataTemp[8] = waveSystem.GetComponent<MonsterManager>().chaseKilledCrurrentLevel.ToString();// + " / " + waveSystem.GetComponent<MonsterManager>().wavesChaseSpawnNumbers[waveSystem.GetComponent<MonsterManager>().currentWave];
            rowDataTemp[9] = (hitOrderWorm / hitOrderCounter).ToString("F2");
            rowDataTemp[10] = GetComponent<GameTime>().timePlayed.ToString("F2");
            rowDataTemp[11] = GameObject.Find("LogInformationObject") != null ? GameObject.Find("LogInformationObject").GetComponent<PlayerCounter>().playerNumber.ToString() : "-1";
            rowDataTemp[12] = GameObject.Find("LogInformationObject") != null ? GameObject.Find("LogInformationObject").GetComponent<PlayerCounter>().currentPlayerName : "-1";
            rowData.Add(rowDataTemp);

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

            hitOrderCounter = 0;
            hitOrderEjector = 0;
            hitOrderWorm = 0;

            /*
            sw = new StreamWriter("D:\\Uni\\Unity3D Workspace\\Cubit\\Assets\\Logs\\logWaveInformation.txt", true);
            sw.WriteLine("--- Another Wave Ended! ---");
            sw.WriteLine("- wave: " + ((waveSystem.GetComponent<MonsterManager>().wavesIsActive) ? (waveSystem.GetComponent<MonsterManager>().currentWave) : (-1)));
            sw.WriteLine("- wave took seconds: " + ((waveSystem.GetComponent<MonsterManager>().wavesIsActive) ? (waveSystem.GetComponent<MonsterManager>().currentWaveTime) : (-1)));
            sw.WriteLine("- wave time total: " + ((waveSystem.GetComponent<MonsterManager>().wavesIsActive) ? (waveSystem.GetComponent<MonsterManager>().currentTime) : (-1)));
            sw.WriteLine("- player position: " + player.transform.position);
            sw.WriteLine("- player killed ejectors: " + waveSystem.GetComponent<MonsterManager>().throwKilledCrurrentLevel + " / " + waveSystem.GetComponent<MonsterManager>().wavesThrowSpawnNumbers[waveSystem.GetComponent<MonsterManager>().currentWave]);
            sw.WriteLine("- player killed worms: " + waveSystem.GetComponent<MonsterManager>().chaseKilledCrurrentLevel + " / " + waveSystem.GetComponent<MonsterManager>().wavesChaseSpawnNumbers[waveSystem.GetComponent<MonsterManager>().currentWave]);
            sw.WriteLine("- player took dmg: " + waveSystem.GetComponent<MonsterManager>().playerDmgThisWave);
            sw.WriteLine("- time: " + GetComponent<GameTime>().timePlayed);
            sw.WriteLine("-------------------------------------------");
            sw.WriteLine("");
            sw.Close();
            */
        }
    }

    public void logRegisterDeath(int hitType)
    {
        if (recordLog && logPlayerDeath)
        {
            List<string[]> rowData = new List<string[]>();

            string filePath = getPath("LogPlayerDeath.csv");

            string[] rowDataTemp = new string[10];
            string hitTypeString = "";
            switch (hitType)
            {
                case 0:
                    hitTypeString = "core";
                    break;
                case 1:
                    hitTypeString = "attached ejector";
                    break;
                case 2:
                    hitTypeString = "active ejector";
                    break;
                case 3:
                    hitTypeString = "attached worm";
                    break;
                default:
                    hitTypeString = "not specifid";
                    break;
            }
            rowDataTemp[0] = hitTypeString;

            rowDataTemp[1] = ((waveSystem.GetComponent<MonsterManager>().wavesIsActive) ? (waveSystem.GetComponent<MonsterManager>().currentWave + 1) : (-1)).ToString();
            rowDataTemp[2] = ((waveSystem.GetComponent<MonsterManager>().wavesIsActive) ? (waveSystem.GetComponent<MonsterManager>().currentWaveTime) : (-1)).ToString("F2");
            rowDataTemp[3] = ((waveSystem.GetComponent<MonsterManager>().wavesIsActive) ? (waveSystem.GetComponent<MonsterManager>().currentTime) : (-1)).ToString("F2");
            rowDataTemp[4] = GetComponent<GameTime>().timePlayed.ToString("F2");
            rowDataTemp[5] = player.transform.position.x.ToString() + " | " + player.transform.position.y.ToString() + " | " + player.transform.position.z.ToString();
            rowDataTemp[6] = ((waveSystem.GetComponent<MonsterManager>().wavesIsActive) ? (waveSystem.GetComponent<MonsterManager>().playerChancesCurrent) : (-1)).ToString();
            rowDataTemp[7] = GameObject.Find("LogInformationObject") != null ? GameObject.Find("LogInformationObject").GetComponent<PlayerCounter>().playerNumber.ToString() : "-1";
            rowDataTemp[8] = GameObject.Find("LogInformationObject") != null ? GameObject.Find("LogInformationObject").GetComponent<PlayerCounter>().currentPlayerName : "-1";
            rowData.Add(rowDataTemp);

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

            /*
            sw = new StreamWriter("D:\\Uni\\Unity3D Workspace\\Cubit\\Assets\\Logs\\logDeaths.txt", true);
            sw.WriteLine("--- Player died! ---");
            sw.WriteLine("- position: " + player.transform.position);
            string hitTypeString = "";
            switch (hitType)
            {
                case 0:
                    hitTypeString = "core";
                    break;
                case 1:
                    hitTypeString = "attached ejector";
                    break;
                case 2:
                    hitTypeString = "active ejector";
                    break;
                case 3:
                    hitTypeString = "attached worm";
                    break;
                default:
                    hitTypeString = "not specifid";
                    break;
            }
            sw.WriteLine("- died by: " + hitTypeString);

            sw.WriteLine("- wave: " + ((waveSystem.GetComponent<MonsterManager>().wavesIsActive) ? (waveSystem.GetComponent<MonsterManager>().currentWave) : (-1)));
            sw.WriteLine("- wave time current: " + ((waveSystem.GetComponent<MonsterManager>().wavesIsActive) ? (waveSystem.GetComponent<MonsterManager>().currentWaveTime) : (-1)));
            sw.WriteLine("- wave time total: " + ((waveSystem.GetComponent<MonsterManager>().wavesIsActive) ? (waveSystem.GetComponent<MonsterManager>().currentTime) : (-1)));
            sw.WriteLine("- time: " + GetComponent<GameTime>().timePlayed);
            sw.WriteLine("-------------------------------------------");
            sw.WriteLine("");
            sw.Close();
            */
        }
    }
    public void logRegisterLifeLoss(bool whileWave, int hitType)
    {


        /*
        if (recordLog && logPlayerLifeLoss)
        {
            sw = new StreamWriter("D:\\Uni\\Unity3D Workspace\\Cubit\\Assets\\Logs\\logLifeLoss.txt", true);
            sw.WriteLine("--- Player lost HP! ---");
            string hitTypeString = "";
            switch (hitType)
            {
                case 0:
                    hitTypeString = "core";
                    break;
                case 1:
                    hitTypeString = "attached ejector";
                    break;
                case 2:
                    hitTypeString = "active ejector";
                    break;
                case 3:
                    hitTypeString = "attached worm";
                    break;
                default:
                    hitTypeString = "not specifid";
                    break;
            }
            sw.WriteLine("- hit by: " + hitTypeString);
            sw.WriteLine("- hp left after: " + player.GetComponent<PlayerLife>().currentHp);
            sw.WriteLine("- position: " + player.transform.position);
            sw.WriteLine("- wave: " + ((waveSystem.GetComponent<MonsterManager>().wavesIsActive) ? (waveSystem.GetComponent<MonsterManager>().currentWave) : (-1)));
            sw.WriteLine("- wave time: " + ((waveSystem.GetComponent<MonsterManager>().wavesIsActive) ? (waveSystem.GetComponent<MonsterManager>().currentWaveTime) : (-1)));
            sw.WriteLine("- time: " + GetComponent<GameTime>().timePlayed);
            sw.WriteLine("-------------------------------------------");
            sw.WriteLine("");
            sw.Close();
            
        }
        */
    }
    public void logRegisterLifeGain()
    {
        if (recordLog && logPlayerLifeGain)
        {
            /*
            sw = new StreamWriter("D:\\Uni\\Unity3D Workspace\\Cubit\\Assets\\Logs\\logLifeGain.txt", true);
            sw.WriteLine("--- Player gained HP! ---");
            sw.WriteLine("- hp left after: " + player.GetComponent<PlayerLife>().currentHp);
            sw.WriteLine("- position: " + player.transform.position);
            sw.WriteLine("- wave: " + ((waveSystem.GetComponent<MonsterManager>().wavesIsActive) ? (waveSystem.GetComponent<MonsterManager>().currentWave) : (-1)));
            sw.WriteLine("- wave time: " + ((waveSystem.GetComponent<MonsterManager>().wavesIsActive) ? (waveSystem.GetComponent<MonsterManager>().currentWaveTime) : (-1)));
            sw.WriteLine("- time: " + GetComponent<GameTime>().timePlayed);
            sw.WriteLine("-------------------------------------------");
            sw.WriteLine("");
            sw.Close();
            */
        }
    }

    public void logRegisterSkillGather(int grabbed, int grabbedTotal)
    {
        if (recordLog && logSkillGather)
        {
            List<string[]> rowData = new List<string[]>();

            string filePath = getPath("LogSkillGather.csv");

            string[] rowDataTemp = new string[11];
            rowDataTemp[0] = grabbed.ToString();
            rowDataTemp[1] = (grabbedTotal - grabbed).ToString();
            rowDataTemp[2] = grabbedTotal.ToString();
            rowDataTemp[3] = player.transform.position.x.ToString() + " | " + player.transform.position.y.ToString() + " | " + player.transform.position.z.ToString();

            rowDataTemp[4] = ((waveSystem.GetComponent<MonsterManager>().wavesIsActive) ? (waveSystem.GetComponent<MonsterManager>().currentWave + 1) : (-1)) + "";
            rowDataTemp[5] = ((waveSystem.GetComponent<MonsterManager>().wavesIsActive) ? (waveSystem.GetComponent<MonsterManager>().currentWaveTime) : (-1)).ToString("F2");
            rowDataTemp[6] = ((waveSystem.GetComponent<MonsterManager>().wavesIsActive) ? (waveSystem.GetComponent<MonsterManager>().currentTime) : (-1)).ToString("F2");
            rowDataTemp[7] = GetComponent<GameTime>().timePlayed.ToString("F2");
            rowDataTemp[8] = ((waveSystem.GetComponent<MonsterManager>().wavesIsActive) ? (waveSystem.GetComponent<MonsterManager>().playerChancesCurrent) : (-1)).ToString();
            rowDataTemp[9] = GameObject.Find("LogInformationObject") != null ? GameObject.Find("LogInformationObject").GetComponent<PlayerCounter>().playerNumber.ToString() : "-1";
            rowDataTemp[10] = GameObject.Find("LogInformationObject") != null ? GameObject.Find("LogInformationObject").GetComponent<PlayerCounter>().currentPlayerName : "-1";
            rowData.Add(rowDataTemp);

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


            /*
            sw = new StreamWriter("D:\\Uni\\Unity3D Workspace\\Cubit\\Assets\\Logs\\logSkillGather.txt", true);
            sw.WriteLine("--- Player used Gather! ---");
            sw.WriteLine("- grabbed: " + grabbed);
            sw.WriteLine("- grabbed before: " + (grabbedTotal - grabbed));
            sw.WriteLine("- grabbed after: " + grabbedTotal);
            sw.WriteLine("- position: " + player.transform.position);

            sw.WriteLine("- wave: " + ((waveSystem.GetComponent<MonsterManager>().wavesIsActive) ? (waveSystem.GetComponent<MonsterManager>().currentWave) : (-1)));
            sw.WriteLine("- wave time current: " + ((waveSystem.GetComponent<MonsterManager>().wavesIsActive) ? (waveSystem.GetComponent<MonsterManager>().currentWaveTime) : (-1)));
            sw.WriteLine("- wave time total: " + ((waveSystem.GetComponent<MonsterManager>().wavesIsActive) ? (waveSystem.GetComponent<MonsterManager>().currentTime) : (-1)));
            sw.WriteLine("- time: " + GetComponent<GameTime>().timePlayed);
            sw.WriteLine("-------------------------------------------");
            sw.WriteLine("");
            sw.Close();
         */
        }
    }
    public void logRegisterSkillPush(int pushed, bool grabbed, bool isAimed)
    {
        if (recordLog && logSkillPush)
        {
            List<string[]> rowData = new List<string[]>();

            string filePath = getPath("LogSkillPush.csv");

            string[] rowDataTemp = new string[11];
            rowDataTemp[0] = pushed.ToString();
            rowDataTemp[1] = grabbed.ToString();
            rowDataTemp[2] = isAimed.ToString();

            rowDataTemp[3] = player.transform.position.x.ToString() + " | " + player.transform.position.y.ToString() + " | " + player.transform.position.z.ToString();
            rowDataTemp[4] = ((waveSystem.GetComponent<MonsterManager>().wavesIsActive) ? (waveSystem.GetComponent<MonsterManager>().currentWave + 1) : (-1)) + "";
            rowDataTemp[5] = ((waveSystem.GetComponent<MonsterManager>().wavesIsActive) ? (waveSystem.GetComponent<MonsterManager>().currentWaveTime) : (-1)).ToString("F2");
            rowDataTemp[6] = ((waveSystem.GetComponent<MonsterManager>().wavesIsActive) ? (waveSystem.GetComponent<MonsterManager>().currentTime) : (-1)).ToString("F2");
            rowDataTemp[7] = GetComponent<GameTime>().timePlayed.ToString("F2");
            rowDataTemp[8] = ((waveSystem.GetComponent<MonsterManager>().wavesIsActive) ? (waveSystem.GetComponent<MonsterManager>().playerChancesCurrent) : (-1)).ToString();
            rowDataTemp[9] = GameObject.Find("LogInformationObject") != null ? GameObject.Find("LogInformationObject").GetComponent<PlayerCounter>().playerNumber.ToString() : "-1";
            rowDataTemp[10] = GameObject.Find("LogInformationObject") != null ? GameObject.Find("LogInformationObject").GetComponent<PlayerCounter>().currentPlayerName : "-1";
            rowData.Add(rowDataTemp);

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

            /*
            Vector3 direction = (player.transform.position - targetPoint).normalized;
            sw = new StreamWriter("D:\\Uni\\Unity3D Workspace\\Cubit\\Assets\\Logs\\logSkillPush.txt", true);
            sw.WriteLine("--- Player used Push! ---");
            sw.WriteLine("- pushed: " + pushed);
            sw.WriteLine("- from grabbed: " + grabbed);
            sw.WriteLine("- position: " + player.transform.position);

            sw.WriteLine("- wave: " + ((waveSystem.GetComponent<MonsterManager>().wavesIsActive) ? (waveSystem.GetComponent<MonsterManager>().currentWave) : (-1)));
            sw.WriteLine("- wave time current: " + ((waveSystem.GetComponent<MonsterManager>().wavesIsActive) ? (waveSystem.GetComponent<MonsterManager>().currentWaveTime) : (-1)));
            sw.WriteLine("- wave time total: " + ((waveSystem.GetComponent<MonsterManager>().wavesIsActive) ? (waveSystem.GetComponent<MonsterManager>().currentTime) : (-1)));
            sw.WriteLine("- time: " + GetComponent<GameTime>().timePlayed);
            sw.WriteLine("-------------------------------------------");
            sw.WriteLine("");
            sw.Close();
            */
        }
    }
    public void logRegisterSkillGattling(Vector3 targetPoint, bool grabbed, bool isAimed)
    {
        if (recordLog && logSkillGattling)
        {
            List<string[]> rowData = new List<string[]>();

            string filePath = getPath("LogSkillGattling.csv");

            string[] rowDataTemp = new string[11];
            rowDataTemp[0] = (targetPoint - player.transform.position).x.ToString() + " | " + (targetPoint - player.transform.position).y.ToString() + " | " + (targetPoint - player.transform.position).z.ToString();
            rowDataTemp[1] = grabbed.ToString();
            rowDataTemp[2] = isAimed.ToString();
            rowDataTemp[3] = player.transform.position.x.ToString() + " | " + player.transform.position.y.ToString() + " | " + player.transform.position.z.ToString();

            rowDataTemp[4] = ((waveSystem.GetComponent<MonsterManager>().wavesIsActive) ? (waveSystem.GetComponent<MonsterManager>().currentWave + 1) : (-1)) + "";
            rowDataTemp[5] = ((waveSystem.GetComponent<MonsterManager>().wavesIsActive) ? (waveSystem.GetComponent<MonsterManager>().currentWaveTime) : (-1)).ToString("F2");
            rowDataTemp[6] = ((waveSystem.GetComponent<MonsterManager>().wavesIsActive) ? (waveSystem.GetComponent<MonsterManager>().currentTime) : (-1)).ToString("F2");
            rowDataTemp[7] = GetComponent<GameTime>().timePlayed.ToString("F2");
            rowDataTemp[8] = ((waveSystem.GetComponent<MonsterManager>().wavesIsActive) ? (waveSystem.GetComponent<MonsterManager>().playerChancesCurrent) : (-1)).ToString();
            rowDataTemp[9] = GameObject.Find("LogInformationObject") != null ? GameObject.Find("LogInformationObject").GetComponent<PlayerCounter>().playerNumber.ToString() : "-1";
            rowDataTemp[10] = GameObject.Find("LogInformationObject") != null ? GameObject.Find("LogInformationObject").GetComponent<PlayerCounter>().currentPlayerName : "-1";
            rowData.Add(rowDataTemp);

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

            /*
            Vector3 direction = (player.transform.position - targetPoint).normalized;
            sw = new StreamWriter("D:\\Uni\\Unity3D Workspace\\Cubit\\Assets\\Logs\\logSkillGattling.txt", true);
            sw.WriteLine("--- Player used Gattling! ---");
            sw.WriteLine("- direction: " + direction);
            sw.WriteLine("- from grabbed: " + grabbed);
            sw.WriteLine("- position: " + player.transform.position);

            sw.WriteLine("- wave: " + ((waveSystem.GetComponent<MonsterManager>().wavesIsActive) ? (waveSystem.GetComponent<MonsterManager>().currentWave) : (-1)));
            sw.WriteLine("- wave time current: " + ((waveSystem.GetComponent<MonsterManager>().wavesIsActive) ? (waveSystem.GetComponent<MonsterManager>().currentWaveTime) : (-1)));
            sw.WriteLine("- wave time total: " + ((waveSystem.GetComponent<MonsterManager>().wavesIsActive) ? (waveSystem.GetComponent<MonsterManager>().currentTime) : (-1)));
            sw.WriteLine("- time: " + GetComponent<GameTime>().timePlayed);
            sw.WriteLine("-------------------------------------------");
            sw.WriteLine("");
            sw.Close();
            */
        }
    }

    public void logRegisterCubeGrab(Vector3 cubePosition)
    {
        if (recordLog && logCubeGrab)
        {
            List<string[]> rowData = new List<string[]>();

            string filePath = getPath("LogCubeGrab.csv");

            string[] rowDataTemp = new string[9];
            rowDataTemp[0] = cubePosition.x.ToString() + " | " + cubePosition.y.ToString() + " | " + cubePosition.z.ToString();
            rowDataTemp[1] = player.transform.position.x.ToString() + " | " + player.transform.position.y.ToString() + " | " + player.transform.position.z.ToString();

            rowDataTemp[2] = ((waveSystem.GetComponent<MonsterManager>().wavesIsActive) ? (waveSystem.GetComponent<MonsterManager>().currentWave + 1) : (-1)) + "";
            rowDataTemp[3] = ((waveSystem.GetComponent<MonsterManager>().wavesIsActive) ? (waveSystem.GetComponent<MonsterManager>().currentWaveTime) : (-1)).ToString("F2");
            rowDataTemp[4] = ((waveSystem.GetComponent<MonsterManager>().wavesIsActive) ? (waveSystem.GetComponent<MonsterManager>().currentTime) : (-1)).ToString("F2");
            rowDataTemp[5] = GetComponent<GameTime>().timePlayed.ToString("F2");
            rowDataTemp[6] = ((waveSystem.GetComponent<MonsterManager>().wavesIsActive) ? (waveSystem.GetComponent<MonsterManager>().playerChancesCurrent) : (-1)).ToString();
            rowDataTemp[7] = GameObject.Find("LogInformationObject") != null ? GameObject.Find("LogInformationObject").GetComponent<PlayerCounter>().playerNumber.ToString() : "-1";
            rowDataTemp[8] = GameObject.Find("LogInformationObject") != null ? GameObject.Find("LogInformationObject").GetComponent<PlayerCounter>().currentPlayerName : "-1";
            rowData.Add(rowDataTemp);

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

            /*
            sw.WriteLine("- cube position: " + cubePosition);

            sw.WriteLine("- player position: " + player.transform.position);
            sw.WriteLine("- wave: " + ((waveSystem.GetComponent<MonsterManager>().wavesIsActive) ? (waveSystem.GetComponent<MonsterManager>().currentWave) : (-1)));
            sw.WriteLine("- wave time current: " + ((waveSystem.GetComponent<MonsterManager>().wavesIsActive) ? (waveSystem.GetComponent<MonsterManager>().currentWaveTime) : (-1)));
            sw.WriteLine("- wave time total: " + ((waveSystem.GetComponent<MonsterManager>().wavesIsActive) ? (waveSystem.GetComponent<MonsterManager>().currentTime) : (-1)));
            sw.WriteLine("- time: " + GetComponent<GameTime>().timePlayed);
            */
        }
    }
    public void logRegisterCubePlayerHit(int cubeType, int skillType, bool grabbed, bool aimed)
    {
        // cubeType 0: attached Throw, 1: attached Chase, 2: core Throw, 3: core Chase
        // skillType 0: push, 1: gattling, 2: player collision

        if (recordLog && logCubePlayerHit)
        {
            List<string[]> rowData = new List<string[]>();

            string filePath = getPath("LogCubePlayerHit.csv");

            string[] rowDataTemp = new string[12];
            string cubeTypeString = "";
            switch (cubeType)
            {
                case 0:
                    cubeTypeString = "attached to Ejector";
                    hitOrderEjector += hitOrderCounter;
                    hitOrderCounter++;
                    break;
                case 1:
                    cubeTypeString = "attached to Worm";
                    hitOrderWorm += hitOrderCounter;
                    break;
                case 2:
                    cubeTypeString = "core by Ejector";
                    hitOrderEjector += hitOrderCounter;
                    hitOrderCounter++;
                    hitOrderEjector += hitOrderCounter;
                    hitOrderCounter++;
                    break;
                case 3:
                    cubeTypeString = "core by Worm";
                    hitOrderWorm += hitOrderCounter;
                    hitOrderCounter++;
                    hitOrderWorm += hitOrderCounter;
                    hitOrderCounter++;
                    break;
                default: cubeTypeString = "not specified"; hitOrderCounter++; break;
            }
            rowDataTemp[0] = cubeTypeString;
            string skillTypeString = "";
            switch (skillType)
            {
                case 0: skillTypeString = "push"; break;
                case 1: skillTypeString = "gattling"; break;
                case 2: skillTypeString = "player collision"; break;
                default: skillTypeString = "not specified"; break;
            }
            rowDataTemp[1] = skillTypeString;
            rowDataTemp[2] = grabbed.ToString();
            rowDataTemp[3] = aimed.ToString();
            rowDataTemp[4] = player.transform.position.x.ToString() + " | " + player.transform.position.y.ToString() + " | " + player.transform.position.z.ToString();

            rowDataTemp[5] = ((waveSystem.GetComponent<MonsterManager>().wavesIsActive) ? (waveSystem.GetComponent<MonsterManager>().currentWave + 1) : (-1)) + "";
            rowDataTemp[6] = ((waveSystem.GetComponent<MonsterManager>().wavesIsActive) ? (waveSystem.GetComponent<MonsterManager>().currentWaveTime) : (-1)).ToString("F2");
            rowDataTemp[7] = ((waveSystem.GetComponent<MonsterManager>().wavesIsActive) ? (waveSystem.GetComponent<MonsterManager>().currentTime) : (-1)).ToString("F2");
            rowDataTemp[8] = GetComponent<GameTime>().timePlayed.ToString("F2");
            rowDataTemp[9] = ((waveSystem.GetComponent<MonsterManager>().wavesIsActive) ? (waveSystem.GetComponent<MonsterManager>().playerChancesCurrent) : (-1)).ToString();
            rowDataTemp[10] = GameObject.Find("LogInformationObject") != null ? GameObject.Find("LogInformationObject").GetComponent<PlayerCounter>().playerNumber.ToString() : "-1";
            rowDataTemp[11] = GameObject.Find("LogInformationObject") != null ? GameObject.Find("LogInformationObject").GetComponent<PlayerCounter>().currentPlayerName : "-1";
            rowData.Add(rowDataTemp);

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

            

            /*
            string cubeTypeString = "";
            switch (cubeType)
            {
                case 0: cubeTypeString = "attached to Ejector"; break;
                case 1: cubeTypeString = "attached to Worm"; break;
                case 2: cubeTypeString = "core by Ejector"; break;
                case 3: cubeTypeString = "core by Worm"; break;
                default: cubeTypeString = "not specified"; break;
            }
            sw.WriteLine("- cube type: " + cubeTypeString);

            string skillTypeString = "";
            switch (skillType)
            {
                case 0: skillTypeString = "push"; break;
                case 1: skillTypeString = "gattling"; break;
                case 2: skillTypeString = "player collision"; break;
                default: skillTypeString = "not specified"; break;
            }
            sw.WriteLine("- skill type: " + skillTypeString);
            sw.WriteLine("- from grabbed: " + grabbed);
            sw.WriteLine("- was aimed: " + aimed);

            sw.WriteLine("- player position: " + player.transform.position);
            sw.WriteLine("- wave: " + ((waveSystem.GetComponent<MonsterManager>().wavesIsActive) ? (waveSystem.GetComponent<MonsterManager>().currentWave) : (-1)));
            sw.WriteLine("- wave time current: " + ((waveSystem.GetComponent<MonsterManager>().wavesIsActive) ? (waveSystem.GetComponent<MonsterManager>().currentWaveTime) : (-1)));
            sw.WriteLine("- wave time total: " + ((waveSystem.GetComponent<MonsterManager>().wavesIsActive) ? (waveSystem.GetComponent<MonsterManager>().currentTime) : (-1)));
            sw.WriteLine("- time: " + GetComponent<GameTime>().timePlayed);
            */
        }
    }
    public void logRegisterCubeLaunch(Vector3 cubePosition, Vector3 targetPosition, int skillType, bool grabbed, bool aimed)
    {
        // Skill type 0: push, 1: gattling, 2: player collision, 3: ejector
        if (recordLog && logCubeLaunch)
        {
            List<string[]> rowData = new List<string[]>();

            string filePath = getPath("LogCubeLaunch.csv");

            string[] rowDataTemp = new string[12];
            string skillTypeString = "";
            switch (skillType)
            {
                case 0: skillTypeString = "push"; break;
                case 1: skillTypeString = "gattling"; break;
                case 2: skillTypeString = "player collision"; break;
                case 3: skillTypeString = "ejector"; break;
                default: skillTypeString = "not specified"; break;
            }
            rowDataTemp[0] = skillTypeString;
            rowDataTemp[1] = grabbed.ToString();
            rowDataTemp[2] = aimed.ToString();
            rowDataTemp[3] = (targetPosition - cubePosition).x.ToString() + " | " + (targetPosition - cubePosition).y.ToString() + " | " + (targetPosition - cubePosition).z.ToString();
            rowDataTemp[4] = player.transform.position.x.ToString() + " | " + player.transform.position.y.ToString() + " | " + player.transform.position.z.ToString();

            rowDataTemp[5] = ((waveSystem.GetComponent<MonsterManager>().wavesIsActive) ? (waveSystem.GetComponent<MonsterManager>().currentWave + 1) : (-1)) + "";
            rowDataTemp[6] = ((waveSystem.GetComponent<MonsterManager>().wavesIsActive) ? (waveSystem.GetComponent<MonsterManager>().currentWaveTime) : (-1)).ToString("F2");
            rowDataTemp[7] = ((waveSystem.GetComponent<MonsterManager>().wavesIsActive) ? (waveSystem.GetComponent<MonsterManager>().currentTime) : (-1)).ToString("F2");
            rowDataTemp[8] = GetComponent<GameTime>().timePlayed.ToString("F2");
            rowDataTemp[9] = ((waveSystem.GetComponent<MonsterManager>().wavesIsActive) ? (waveSystem.GetComponent<MonsterManager>().playerChancesCurrent) : (-1)).ToString();
            rowDataTemp[10] = GameObject.Find("LogInformationObject") != null ? GameObject.Find("LogInformationObject").GetComponent<PlayerCounter>().playerNumber.ToString() : "-1";
            rowDataTemp[11] = GameObject.Find("LogInformationObject") != null ? GameObject.Find("LogInformationObject").GetComponent<PlayerCounter>().currentPlayerName : "-1";
            rowData.Add(rowDataTemp);

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


            /*
            string skillTypeString = "";
            switch (skillType)
            {
                case 0: skillTypeString = "push"; break;
                case 1: skillTypeString = "gattling"; break;
                case 2: skillTypeString = "player collision"; break;
                case 3: skillTypeString = "ejector"; break;
                default: skillTypeString = "not specified"; break;
            }
            sw.WriteLine("- skill type: " + skillTypeString);
            sw.WriteLine("- from grabbed: " + grabbed);
            sw.WriteLine("- was aimed: " + aimed);
            Vector3 direction = (cubePosition - targetPosition).normalized;
            sw.WriteLine("- direction: " + direction);
            sw.WriteLine("- cube position" + cubePosition);

            sw.WriteLine("- player position: " + player.transform.position);
            sw.WriteLine("- wave: " + ((waveSystem.GetComponent<MonsterManager>().wavesIsActive) ? (waveSystem.GetComponent<MonsterManager>().currentWave) : (-1)));
            sw.WriteLine("- wave time current: " + ((waveSystem.GetComponent<MonsterManager>().wavesIsActive) ? (waveSystem.GetComponent<MonsterManager>().currentWaveTime) : (-1)));
            sw.WriteLine("- wave time total: " + ((waveSystem.GetComponent<MonsterManager>().wavesIsActive) ? (waveSystem.GetComponent<MonsterManager>().currentTime) : (-1)));
            sw.WriteLine("- time: " + GetComponent<GameTime>().timePlayed);
        */
        }
    }
    public void logRegisterCubeEnemyHit(GameObject origin, int cubeType)
    {
        // cubeType 0: attached Throw, 1: attached Chase, 2: core Throw, 3: core Chase, 4: active Ejector
        if (recordLog && logCubeEnemyHit)
        {
            List<string[]> rowData = new List<string[]>();

            string filePath = getPath("LogCubeEnemyHit.csv");

            string[] rowDataTemp = new string[10];
            string cubeTypeString = "";
            switch (cubeType)
            {
                case 0: cubeTypeString = "attached to Ejector"; break;
                case 1: cubeTypeString = "attached to Worm"; break;
                case 2: cubeTypeString = "core by Ejector"; break;
                case 3: cubeTypeString = "core by Worm"; break;
                case 4: cubeTypeString = "active ejector"; break;
                default: cubeTypeString = "not specified"; break;
            }
            rowDataTemp[0] = cubeTypeString;
            rowDataTemp[1] = (origin.transform.position - player.transform.position).magnitude.ToString("F2");
            rowDataTemp[2] = player.transform.position.x.ToString() + " | " + player.transform.position.y.ToString() + " | " + player.transform.position.z.ToString();

            rowDataTemp[3] = ((waveSystem.GetComponent<MonsterManager>().wavesIsActive) ? (waveSystem.GetComponent<MonsterManager>().currentWave + 1) : (-1)) + "";
            rowDataTemp[4] = ((waveSystem.GetComponent<MonsterManager>().wavesIsActive) ? (waveSystem.GetComponent<MonsterManager>().currentWaveTime) : (-1)).ToString("F2");
            rowDataTemp[5] = ((waveSystem.GetComponent<MonsterManager>().wavesIsActive) ? (waveSystem.GetComponent<MonsterManager>().currentTime) : (-1)).ToString("F2");
            rowDataTemp[6] = GetComponent<GameTime>().timePlayed.ToString("F2");
            rowDataTemp[7] = ((waveSystem.GetComponent<MonsterManager>().wavesIsActive) ? (waveSystem.GetComponent<MonsterManager>().playerChancesCurrent) : (-1)).ToString();
            rowDataTemp[8] = GameObject.Find("LogInformationObject") != null ? GameObject.Find("LogInformationObject").GetComponent<PlayerCounter>().playerNumber.ToString() : "-1";
            rowDataTemp[9] = GameObject.Find("LogInformationObject") != null ? GameObject.Find("LogInformationObject").GetComponent<PlayerCounter>().currentPlayerName : "-1";
            rowData.Add(rowDataTemp);

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

            /*
            string cubeTypeString;
            switch (cubeType)
            {
                case 0: cubeTypeString = "attached to Ejector"; break;
                case 1: cubeTypeString = "attached to Worm"; break;
                case 2: cubeTypeString = "core by Ejector"; break;
                case 3: cubeTypeString = "core by Worm"; break;
                default: cubeTypeString = "not specified"; break;
            }
            sw.WriteLine("- cube type: " + cubeTypeString);
            sw.WriteLine("- distance: " + (player.transform.position - origin.transform.position).magnitude);

            sw.WriteLine("- player position: " + player.transform.position);
            sw.WriteLine("- wave: " + ((waveSystem.GetComponent<MonsterManager>().wavesIsActive) ? (waveSystem.GetComponent<MonsterManager>().currentWave) : (-1)));
            sw.WriteLine("- wave time current: " + ((waveSystem.GetComponent<MonsterManager>().wavesIsActive) ? (waveSystem.GetComponent<MonsterManager>().currentWaveTime) : (-1)));
            sw.WriteLine("- wave time total: " + ((waveSystem.GetComponent<MonsterManager>().wavesIsActive) ? (waveSystem.GetComponent<MonsterManager>().currentTime) : (-1)));
            sw.WriteLine("- time: " + GetComponent<GameTime>().timePlayed);
            */
        }
    }

    private string getPath(string dataName)
    {
        return Application.dataPath + "/Log/" + dataName;
    }

    void OnDrawGizmos()
    {
        if (testLog)
        {
            log();
            testLog = false;
        }
        if (testRead)
        {
            read();
            testRead = false;
        }

        if (initialize)
        {
            initializeAll();
            initialize = false;
        }

        if (delete)
        {
            deleteAll();
            delete = false;
        }
    }
}
