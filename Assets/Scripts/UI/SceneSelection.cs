using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSelection : MonoBehaviour
{
    public delegate void RestartDelegeta();
    public event RestartDelegeta OnRestart;
    public void RestartScene()
    {
        Debug.Log("Restart Scene");
        Time.timeScale = 1;
        Scene _currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(_currentScene.name);
        OnRestart?.Invoke();
    }
    public void GoToScene(string _newSceneName)
    {
        Debug.Log("Load Scene: " + _newSceneName);
        Time.timeScale = 1;
        SceneManager.LoadScene(_newSceneName);
    }
}
