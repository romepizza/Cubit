using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ShowTextClickToContinue : MonoBehaviour
{
    public GameObject tutorialEnemy;
    public GameObject tutorialText1;
    public GameObject tutorialText2;
    public GameObject player;
    public GameObject hpText;
    public GameObject grabbedText;


    [Header("----- SETTINGS -----")]
    //public List<List<string>> texts;
    public List<List<tutorialText>> tutorialTexts;

    [Header("----- DEBUG -----")]
    public int currentTextIndexGlobal;
    public int currentTextIndexSub;
    public bool showingTimedText;

    [Header("--- (Counter) ---")]
    public float timedTextFinishTime;

    public struct tutorialText
    {
        public string text;
        public float time;
        public int action;
        // 0: enable player movement
    }

    void Start ()
    {
        /*
        texts = new List<List<string>>();
        List<string> startTexts = new List<string>();
        startTexts.Add("Hi! Zunaechst einmal vielen Dank, dass du mein Spiel testest.");
        startTexts.Add("In diesem Spiel geht es darum, Gegnern und ihren Geschossen auszuweichen und gleichzeitig eben diese mit eigenen Geschossen zu bombardieren und zu vernichten!");
        startTexts.Add("Mit WASD, Space und Shift/Strg kannst du dich bewegen.\n\nVersuch es!");
        startTexts.Add("Als Geschosse dienen die herumschwirrenden ");
        texts.Add(startTexts);
        */
        tutorialTexts = new List<List<tutorialText>>();
        List<tutorialText> startTutorialTexts = new List<tutorialText>();

        registerText(startTutorialTexts, "Hi!\n\nZunaechst einmal vielen Dank, dass du mein Spiel testest!", -1, -1);
        registerText(startTutorialTexts, "In diesem Spiel geht es darum, Gegnern und ihren Geschossen auszuweichen und gleichzeitig diese mit eigenen Geschossen zu bombardieren und zu vernichten!", -1, -1);
        registerText(startTutorialTexts, "In der oberen rechten Ecke des Bildschirms siehst du deine verbleibenden Lebenspunkte.\nDu regenerierst alle paar Sekunden einen Lebenspunkt.\nUnabhaengig davon wirst du zwei mal sterben duerfen, bevor deine Runde vorbei ist.\nDu hast also insgesamt drei Chancen. Diese werden allerdings nicht angezeit.", -1, 0);
        registerText(startTutorialTexts, "Mit WASD, Space und Shift/Strg kannst du dich bewegen.\n\nVersuch es!", 10, 1);
        registerText(startTutorialTexts, "Wenn du einen farblosen Cube beruehrst, wird dieser nach vorne katapultiert und er leuchtet tuerkis. Das bedeutet, dass er von dir aufgeladen (\"charged\") ist.", -1, -1);
        registerText(startTutorialTexts, "Trifft ein von dir geladener Cube einen Gegner, verliert er Lebenspunkte.\nDu kannst Gegner nur schaedigen, indem du ihn mit von dir geladenen Cubes triffst.", -1, -1);
        registerText(startTutorialTexts, "Du kannst Cubes aber auch auf andere Arten chargen. Halte die linke Maustaste gedrueckt, um Cubes in deiner Naehe nacheinander zu chargen und nach vorne zu katapultieren.\n\nVersuch es!", 15, 2);
        registerText(startTutorialTexts, "Alternativ kannst du die linke Maustaste auch kurz druecken, um alle nahen Cubes vor dir gleichzeitig zu chargen und zu verschiessen.\n\nVersuch es!", 15, 3);
        registerText(startTutorialTexts, "Mit deinem dritten und letzten Skill kannst du Cubes vor dir anhauefen und diese als mobiles Munitionsreservoir verwenden.", -1, 4);
        registerText(startTutorialTexts, "Du kannst maximal 100 Cubes gleichzeitig gegrabbt halten. In der unteren linken Ecke des Bildschirms kannst du sehen, wie viele Cubes du momentan gegrabbt hast.", -1, 5);
        registerText(startTutorialTexts, "Druecke die rechte Maustaste, um Cubes in deiner Naehe zu sammeln. Wenn du die rechte Maustaste gedrueckt haelst, wird der Skill auf cooldown aktiviert\n(0.5 Sekunden cooldown)\n\nVersuch es!", 10, 6);
        registerText(startTutorialTexts, "Die beiden Skills auf der linken Maustaste werden immer zuerst versuchen, Cubes aus diesem Reservoir zu verschiessen.\n\nVersuch es!", 15, 7);
        registerText(startTutorialTexts, "Wenn du Cubes verschiesst, waerend sie gegrabbt sind, werden sie im uebrigen staerker beschleunigt und deine Ziele somit warscheinlicher treffen.\n\nBehalte dies im Hinterkopf!", -1, -1);
        registerText(startTutorialTexts, "Kommen wir nun zum letzten Punkt:\nDen Gegnern!", -1, 8);
        registerText(startTutorialTexts, "In der Mitte des Raumes ist grade ein Gegner gespawnt. Schau ihn dir an!", -1, 9);
        registerText(startTutorialTexts, "Gegner bestehen aus zwei Komponenten:\nEinerseits aus dem hier zu sehenden Core,\nandererseits aus den noch nicht zu sehenden, vom Gegner gegrabbten Cubes.", -1, -1);
        registerText(startTutorialTexts, "Der Core wird nach und nach Cubes aus seiner Umgebung an sich binden (attachen) und diese anschliessend nacheinander auf dich abfeuern.\nSieh zunaechst zu, wie sich der Core Cube fuer Cube grabbt.\n\nEr wird dich nicht angreifen!\n(Bzw. warum auch immer doch genau einmal)", -1, -1);
        registerText(startTutorialTexts, "", 10, 10);
        registerText(startTutorialTexts, "Es gibt zwei verschiedene Gegnertypen:\nEjectors und Worms.", -1, -1);
        registerText(startTutorialTexts, "Was du dort siehst ist ein Ejector.\nEr ist stationaer und wird dich mit roten Cubes befeuern.\nDu kannst ihn an der Farbe seiner attachten Cubes erkennen: Orange.", -1, -1);
        registerText(startTutorialTexts, "Der zweite Gegnertyp (Worm) ist mobil und wird dich verfolgen. Seine attachten Cubes sind gruen.\n\nDiesen wirst du aber erst im eigendlichen Spiel zu Gesicht bekommen.", -1, -1);
        registerText(startTutorialTexts, "Triffst du einen attachten Cube, verliert der Gegner einen Lebenspunkt.\nTriffst du seinen Core, verliert er gleich zwei!", -1, -1);
        registerText(startTutorialTexts, "Beruehrst du einen Core, einen attachten Cube oder einen vom Ejector verschossenen Cube, verlierst du einen Lebenspunkt!", -1, -1);
        registerText(startTutorialTexts, "Deine Drohne ist mit einer Zielhilfe ausgestattet. Du musst den Gegner nur anvisieren, und deine verschossenen Cubes suchen sich automatisch ihr Ziel.\n\nDies gilt auch fuer sich bewegende Ziele!\n\n(Auf \"Y\" kannst du die Zielhilfe an- und ausschalten)", -1, -1);
        registerText(startTutorialTexts, "Einen letzten Tipp gebe ich dir noch:\nVersuche so oft wie moeglich mit Space und Shift/Strg hoch und runter zu fliegen. Das wird dir das ausweichen und ein Vielfaches erleichtern!", -1, -1);
        registerText(startTutorialTexts, "Vernichte nun den Gegner!\n\nDruecke nocheinmal \"Q\", um das Tutorial abzuschliessen.", -1, 11);
        registerText(startTutorialTexts, "", -1, 12);

        tutorialTexts.Add(startTutorialTexts);

        currentTextIndexGlobal = 0;
        currentTextIndexSub = 0;
        //showClickText(texts[currentTextIndexGlobal][currentTextIndexSub]);
        showTextTutorial(tutorialTexts[currentTextIndexGlobal][currentTextIndexSub]);
    }
	
	// Update is called once per frame
	void Update ()
    {
		if(Input.GetKeyDown(KeyCode.Q) && !showingTimedText)
        {
            //showNextHint();
            showNextHintTutorial();
        }

        if (Input.GetKeyDown(KeyCode.Tab) && !showingTimedText)
        {
            //showPreviousHint();
            showPreviousHintTutorial();
        }
        


        if (showingTimedText && timedTextFinishTime < Time.time)
        {
            //showNextHint();
            showingTimedText = false;
            tutorialText2.GetComponent<Text>().text = "Druecke Q um fortzufahren!\nDruecke Tab um zum vorherigen Punkt zu wechseln!";
        }
        
        if(showingTimedText && timedTextFinishTime > Time.time)
            tutorialText2.GetComponent<Text>().text = "Es geht weiter in:\n" + (int)(timedTextFinishTime - Time.time);
    }

    public void registerText(List<tutorialText> list, string text, float time, int action)
    {
        tutorialText txt = new tutorialText();
        txt.text = text;
        txt.time = time;
        txt.action = action;
        list.Add(txt);
    }

    public void showNextHintTutorial()
    {
        currentTextIndexSub++;

        if(currentTextIndexSub < tutorialTexts[currentTextIndexGlobal].Count && tutorialTexts[currentTextIndexGlobal][currentTextIndexSub].action >= 0)
        {
            int action = tutorialTexts[currentTextIndexGlobal][currentTextIndexSub].action;


            if (action == 0)
                enableTextHp(true);

            if (action == 1)
                enablePlayerMovement(true);

            if (action == 2)
                enablePlayerSkillGattling(true);

            if (action == 3)
            {
                enablePlayerSkillGattling(false);
                enablePlayerSkillPush(true);
            }

            if (action == 4)
                enablePlayerSkillPush(false);

            if (action == 5)
                enableTextGrabbed(true);

            if (action == 6)
            {
                enablePlayerSkillPush(false);
                enablePlayerSkillGattling(false);
                enablePlayerSkillGather(true);
            }

            if(action == 7)
            {
                enablePlayerSkillPush(true);
                enablePlayerSkillGattling(true);
            }

            if(action == 8)
            {
                player.GetComponent<GrabSystem>().removeAllCubesFromGrabbed();

                enablePlayerSkillPush(false);
                enablePlayerSkillGattling(false);
                enablePlayerSkillGather(false);
            }

            if(action == 9)
            {
                activateTutorialEnemy(true);
            }

            if(action == 10)
            {
                spawnTutorialEnemy();
            }

            if (action == 11)
            {
                enablePlayerSkillPush(true);
                enablePlayerSkillGattling(true);
                enablePlayerSkillGather(true);
            }

            if(action == 12)
            {
                SceneManager.LoadScene(1);
            }

            tutorialText tmp = new tutorialText();
            tmp.text = tutorialTexts[currentTextIndexGlobal][currentTextIndexSub].text;
            tmp.time = tutorialTexts[currentTextIndexGlobal][currentTextIndexSub].time;
            tmp.action = -1;
            tutorialTexts[currentTextIndexGlobal][currentTextIndexSub] = tmp;
        }

        if (currentTextIndexSub >= tutorialTexts[currentTextIndexGlobal].Count)
        {
            currentTextIndexSub = Mathf.Min(currentTextIndexSub, tutorialTexts[currentTextIndexGlobal].Count);
            currentTextIndexGlobal++;
        }

        if (currentTextIndexGlobal >= tutorialTexts.Count)
        {
            currentTextIndexGlobal--;
            eraseClickText();
            return;
        }

        showTextTutorial(tutorialTexts[currentTextIndexGlobal][currentTextIndexSub]);
    }

    public void showPreviousHintTutorial()
    {
        if (currentTextIndexSub > 0)
            currentTextIndexSub--;

        tutorialText tmp = new tutorialText();
        tmp.text = tutorialTexts[currentTextIndexGlobal][currentTextIndexSub].text;
        tmp.time = -1;
        tutorialTexts[currentTextIndexGlobal][currentTextIndexSub] = tmp;


        showTextTutorial(tutorialTexts[currentTextIndexGlobal][currentTextIndexSub]);
    }

    public void showTextTutorial(tutorialText tutText)
    {
        if(tutText.time <= 0)
        {
            tutorialText1.GetComponent<Text>().text = tutText.text;
            tutorialText2.GetComponent<Text>().text = "Druecke Q um fortzufahren!\nDruecke Tab um zum vorherigen Punkt zu wechseln!";
        }
        else
        {
            showingTimedText = true;
            timedTextFinishTime = tutText.time + Time.time;
            tutorialText1.GetComponent<Text>().text = tutText.text;
            tutorialText2.GetComponent<Text>().text = "";
        }
    }

/*
    public void showNextHint()
    {
        currentTextIndexSub++;
        if (currentTextIndexSub >= texts[currentTextIndexGlobal].Count)
        {
            currentTextIndexSub = Mathf.Min(currentTextIndexSub, texts[currentTextIndexGlobal].Count);
            currentTextIndexGlobal++;
        }

        if (currentTextIndexGlobal >= texts.Count)
        {
            currentTextIndexGlobal--;
            eraseClickText();
            return;
        }

        showClickText(texts[currentTextIndexGlobal][currentTextIndexSub]);
    }

    public void showPreviousHint()
    {
        if (currentTextIndexSub > 0)
            currentTextIndexSub--;

        showClickText(texts[currentTextIndexGlobal][currentTextIndexSub]);
    }

    public void showClickText(string text)
    {
        tutorialText1.GetComponent<Text>().text = text;
        tutorialText2.GetComponent<Text>().text = "Druecke Q um fortzufahren!\nDruecke Tab um zum vorherigen Punkt zu wechseln!";
    }

    public void showTimedText(string text, float time)
    {
        showingTimedText = true;
        timedTextFinishTime = time + Time.time;
        tutorialText1.GetComponent<Text>().text = text;
        tutorialText2.GetComponent<Text>().text = "";
    }
    */
    public void eraseClickText()
    {
        tutorialText2.GetComponent<Text>().text = "";
        tutorialText1.GetComponent<Text>().text = "";
    }

    public void enablePlayerMovement(bool enable)
    {
        player.GetComponent<PlayerMovement>().enabled = enable;
        player.GetComponent<PlayerRotation>().enabled = enable;
    }

    public void enablePlayerSkillGather(bool enable)
    {
        player.GetComponent<SkillGather>().enabled = enable;
    }

    public void enablePlayerSkillPush(bool enable)
    {
        player.GetComponent<SkillPush>().enabled = enable;
    }

    public void enablePlayerSkillGattling(bool enable)
    {
        player.GetComponent<SkillGattling>().enabled = enable;
    }

    public void enableTextHp(bool enable)
    {
        hpText.SetActive(enable);
    }

    public void enableTextGrabbed(bool enable)
    {
        grabbedText.SetActive(enable);
    }

    public void activateTutorialEnemy(bool enable)
    {
        tutorialEnemy.GetComponent<CubeMonster>().finishTimeGrab = 10000f;
        tutorialEnemy.GetComponent<CubeMonster>().shootCooldownActual = 10000f;
        tutorialEnemy.SetActive(true);
        tutorialEnemy.GetComponent<CubeMonster>().createMonster();
    }

    public void spawnTutorialEnemy()
    {
        tutorialEnemy.GetComponent<CubeMonster>().grabCooldown = 1;
        tutorialEnemy.GetComponent<CubeMonster>().finishTimeGrab = Time.time + tutorialEnemy.GetComponent<CubeMonster>().grabCooldown;
    }
}
