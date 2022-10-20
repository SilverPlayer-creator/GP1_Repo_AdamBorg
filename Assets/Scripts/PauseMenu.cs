using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
public class PauseMenu : MonoBehaviour


{
    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;
    EventSystem eventSystem;
    [SerializeField] GameObject firstButton;
    bool selectedFirst = false;

    private void Start()
    {
        Resume();
        eventSystem = GameObject.Find("EventSystem_MainLevel").GetComponent<EventSystem>();
    }

    void Update()
    {

        if (pauseMenuUI.activeInHierarchy && !selectedFirst)
        {
            eventSystem.SetSelectedGameObject(firstButton);
            selectedFirst = true;
        }
        else if(!pauseMenuUI.activeInHierarchy)
        {
            selectedFirst = false;
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void Restart()
    {
        Debug.Log("Load first level");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, 0);
    }

    public void GoToMainMenu()
    {
        MainMenu.Instance.ReturnedToMain();
        SceneManager.LoadScene(0);
    }
}
