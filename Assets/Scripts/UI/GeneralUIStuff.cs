using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GeneralUIStuff : MonoBehaviour
{
    public GameObject enablePanel;
    public GameObject disablePanel;

    public GameObject logInformationObject;

    public void onClickChangePanel()
    {
        enablePanel.SetActive(true);
        disablePanel.SetActive(false);
    }

    public void loadSceneByIndex(int sceneIndex)
    {
        logInformationObject = GameObject.Find("LogInformationObject");
        if(logInformationObject != null)
        {
            logInformationObject.GetComponent<PlayerCounter>().registerNewPlayer();
        }
        SceneManager.LoadScene(sceneIndex);
    }

    public void resetLevel(int sceneIndex)
    {
        GameObject.Find("GeneralScriptObject").GetComponent<Options>().isFreeze = false;
        Time.timeScale = 1;
        Cursor.visible = true;
        SceneManager.LoadScene(sceneIndex);
    }

    public void quitGame()
    {
        Application.Quit();
    }
}
