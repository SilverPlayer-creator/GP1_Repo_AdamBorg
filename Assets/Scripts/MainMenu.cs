using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private static MainMenu instance;
    public static MainMenu Instance { get { return instance; } }


    private static bool timerOn;
    private static bool scoreScreenCheck;



    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }
    public void StartCasual()
    {
        timerOn = false;
        scoreScreenCheck = false;
        Debug.Log("Load next scene");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }


    public void StartTimeTrial()
    {
        timerOn = true;
        scoreScreenCheck = false;
        Debug.Log("Load next scene");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Debug.Log("Quit game");
        Application.Quit();
    }

    public void ScoreScreen()
    {
        scoreScreenCheck = true;
        SceneManager.LoadScene("ScoreScreen");
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public bool CheckTimer()
    {
        return timerOn;
    }
    public bool DisplayScores()
    {
        return scoreScreenCheck;
    }
    public void ReturnedToMain()
    {
        scoreScreenCheck = false;
        timerOn = false;
    }
}
