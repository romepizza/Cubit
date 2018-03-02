using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLife : MonoBehaviour
{
    public GameObject monsterManager;

    [Header("----- SETTINGS ----")]
    public int maxHp;
    public float regainLifeEachSeconds;

    [Header("----- DEBUG -----")]
    public float currentHp;
    public int lastHitType;

    [Header("--- (Log) ---")]
    public int lifeLossTotal;
    public float averageHp;
    public float averageHpTimer;
    

    [Header("--- (Counter) ---")]
    public float timeLifeGainFinish;
	// Use this for initialization
	void Start ()
    {
	    
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (monsterManager.GetComponent<MonsterManager>().wavesIsActive && !monsterManager.GetComponent<MonsterManager>().isFreeze && !monsterManager.GetComponent<MonsterManager>().waitingForWave)
            averageHpTimer += Time.deltaTime;


        if (timeLifeGainFinish < Time.time)
        {
            gainLife(1);
        }

        if (currentHp <= 0)
            die();
	}

    public void gainLife(int lifeGain)
    {
        averageHp += currentHp * averageHpTimer;
        averageHpTimer = 0;


        currentHp += lifeGain;
        currentHp = Mathf.Clamp(currentHp, 0, maxHp);
        timeLifeGainFinish = regainLifeEachSeconds + Time.time;
    }

    public void loseHp(int damage, int hitType)
    {
        if (monsterManager.GetComponent<MonsterManager>().wavesIsActive)
        {
            averageHp += currentHp * averageHpTimer;
            averageHpTimer = 0;
            lifeLossTotal++;

            currentHp -= damage;
            currentHp = Mathf.Clamp(currentHp, 0, maxHp);

            monsterManager.GetComponent<MonsterManager>().playerDmgThisWave += damage;
            if (currentHp <= 0)
                die();
        }
    }

    void die()
    {
        monsterManager.GetComponent<MonsterManager>().loseChance();
        initializeLife();
    }

    public void initializeLife()
    {
        currentHp = maxHp;
    }
}
