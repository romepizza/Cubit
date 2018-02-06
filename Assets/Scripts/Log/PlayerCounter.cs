using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerCounter : MonoBehaviour {

    public GameObject inputFieldPlayerName;
    public GameObject toggleNewPlayer;
    public GameObject inputFieldId;
    public GameObject errorLabel;
    public int playerNumber = 0;
    public string currentPlayerName;

	// Use this for initialization
	void Start ()
    {
        inputFieldPlayerName = GameObject.Find("InputFieldName");
        toggleNewPlayer = GameObject.Find("ToggleNewPlayer");
        inputFieldId = GameObject.Find("InputFieldId");
        errorLabel = GameObject.Find("ErrorLabel");
    }
	
	// Update is called once per frame
	void Update ()
    {
		if(SceneManager.GetActiveScene() == SceneManager.GetSceneByBuildIndex(0))
        {
            if(inputFieldPlayerName == null)
            {
                inputFieldPlayerName = GameObject.Find("InputFieldName");
            }
            if(toggleNewPlayer == null)
            {
                toggleNewPlayer = GameObject.Find("ToggleNewPlayer");
                if(toggleNewPlayer != null)
                toggleNewPlayer.GetComponent<Toggle>().onValueChanged.AddListener(delegate { setPlayerIdInput(); });
            }
            if(inputFieldId == null)
            {
                inputFieldId = GameObject.Find("InputFieldId");
                if (inputFieldId != null)
                {
                    inputFieldId.GetComponent<InputField>().text = (playerNumber + 1).ToString();
                    inputFieldId.GetComponent<InputField>().onValueChanged.AddListener(delegate { playerIdInputCheck(); });
                }
            }
            if(errorLabel == null)
            {
                errorLabel = GameObject.Find("ErrorLabel");
            }
        }
	}

    public void registerNewPlayer()
    {
        if (inputFieldPlayerName != null)
            currentPlayerName = inputFieldPlayerName.GetComponent<InputField>().text;
        else
            Debug.Log("inputField not found");

        if (toggleNewPlayer != null)
        {
            if (toggleNewPlayer.GetComponent<Toggle>().isOn)
                playerNumber++;
            else
            {
                try
                {
                    playerNumber = int.Parse(inputFieldId.GetComponent<InputField>().text);
                }
                catch(Exception e)
                {
                    playerNumber = -1;
                    errorLabel.GetComponent<Text>().text = "Invalid Player Id!";
                }
            }
        }
        else
            Debug.Log("toggle not found");
    }

    void playerIdInputCheck()
    {
        try
        {
            if(!toggleNewPlayer.GetComponent<Toggle>().isOn)
                playerNumber = int.Parse(inputFieldId.GetComponent<InputField>().text);
            errorLabel.GetComponent<Text>().text = "";
        }
        catch (Exception e)
        {
            errorLabel.GetComponent<Text>().text = "Invalid Player Id!";
        }
    }

    void setPlayerIdInput()
    {
        if (toggleNewPlayer.GetComponent<Toggle>().isOn)
        {
            inputFieldId.GetComponent<InputField>().text = (playerNumber + 1).ToString();
        }
        else
        {
            inputFieldId.GetComponent<InputField>().text = playerNumber.ToString();
        }
    }
}
