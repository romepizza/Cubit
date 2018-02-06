using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HitIndicator : MonoBehaviour
{
    public GameObject hitIndicatorText;
    [Header("----- SETTINGS -----")]
    public float maxTimeBetweenHits;
    public float fadeTime;
    public Color[] hitColors;
    public int[] hitCombos;
    public int maxFontSize;
    public int maxFontSizeAt;

    [Header("----- DEBUG -----")]
    public int hitCounter;
    public float hitTimer;
    public float fadeTimer;
    public Color defaultColor;
    public int defaultFontSize;
    public Color currentColor;
    public bool isFading;
       
	// Use this for initialization
	void Start()
    {
        defaultColor = hitIndicatorText.GetComponent<Text>().color;
        defaultFontSize = hitIndicatorText.GetComponent<Text>().fontSize;
    }
	
	// Update is called once per frame
	void Update ()
    {
        manageTimer();
        fadeText();
    }

    void manageTimer()
    {
        if(hitTimer > 0)
            hitTimer -= Time.deltaTime;
        if (hitTimer < 0)
        {
            if (!isFading)
            {
                fadeTimer = fadeTime;
                isFading = true;
            }
            hitCounter = 0;
            //hitIndicatorText.GetComponent<Text>().color = defaultColor;
            //hitIndicatorText.GetComponent<Text>().fontSize = defaultFontSize;
        }
    }

    void fadeText()
    {
        if (fadeTimer >= 0 && isFading)
        {
            fadeTimer -= Time.deltaTime;
            hitIndicatorText.GetComponent<Text>().color = new Color(currentColor.r, currentColor.g, currentColor.b, Mathf.Lerp(0f, currentColor.a, Mathf.Clamp(fadeTimer / fadeTime, 0f, 1f)));
        }

        
    }

    public void registerHit(int i)
    {
        hitCounter += i;
        manageLabel();
    }

    void manageLabel()
    {
        hitTimer = maxTimeBetweenHits;
        isFading = false;
        if (hitCounter >= 5)
        {
            Text text = hitIndicatorText.GetComponent<Text>();

            text.text = hitCounter + "x";

            text.fontSize = (int)(Mathf.Lerp(defaultFontSize, maxFontSize, Mathf.Clamp((float)hitCounter / (float)maxFontSizeAt, 0f, 1f)));

            Color setToColor = Color.white;
            for (int i = 0; i < hitCombos.Length; i++)
            {
                if (i == 0)
                {
                    if (hitCounter <= hitCombos[i])
                    {
                        setToColor = Color.Lerp(defaultColor, hitColors[i], Mathf.Clamp((float)hitCounter / (float)hitCombos[i], 0, 1));
                        break;
                    }
                }
                else if (i < hitCombos.Length - 1)
                {
                    if (hitCounter > hitCombos[i - 1] && hitCounter <= hitCombos[i])
                    {
                        setToColor = Color.Lerp(hitColors[i - 1], hitColors[i], Mathf.Clamp((float)(hitCounter - hitCombos[i - 1]) / (float)(hitCombos[i] - hitCombos[i - 1]), 0, 1));
                        break;
                    }
                }
                else
                {
                    setToColor = hitColors[i];
                }
            }
            currentColor = setToColor;
            text.color = setToColor;
        }
    }
}
