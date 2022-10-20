using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class HighscoreScript : MonoBehaviour
{
    private static HighscoreScript _instance;
    public static HighscoreScript Instance { get { return _instance; } }

    [SerializeField] private int _scoresSaved;
    [SerializeField] private TextMeshProUGUI _winningText;
    [SerializeField] private TextMeshProUGUI _yourScoreText;
    [SerializeField] private TextMeshProUGUI _highscoreHeader;
    [SerializeField] private TextMeshProUGUI _highscoreText;
    [SerializeField] private TextMeshProUGUI _casualCongratz;
    private List<float> _highscoreList = new List<float>();
    private static float _heldScore;
    private static bool _holdScore;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }



    }
    private void Start()
    {
        ModeSelecter();
        LoadScore();
        PrintHighScores();
        PrintYourScore();
    }
    private void Update()
    {

    }


    private void OnApplicationQuit()
    {
        SaveScore();
    }

    private void SaveScore()
    {
        if (_highscoreList.Count >= _scoresSaved)
        {
            PlayerPrefs.SetInt("ScoreCount", _scoresSaved);

            for (int i = 0; i < _scoresSaved; i++)
            {
                PlayerPrefs.SetFloat("Score_" + i, _highscoreList[i]);
            }
        }
        else
        {
            PlayerPrefs.SetInt("ScoreCount", _highscoreList.Count);

            for (int i = 0; i < _highscoreList.Count; i++)
            {
                PlayerPrefs.SetFloat("Score_" + i, _highscoreList[i]);
            }

        }
    }
    private void LoadScore()
    {
        int _scoreCount = PlayerPrefs.GetInt("ScoreCount");
        for (int i = 0; i < _scoreCount; i++)
        {
            float score = PlayerPrefs.GetFloat("Score_" + i);
            if (score > 0)
            {
                _highscoreList.Add(score);
            }
        }
    }
    private void ResetScore()
    {
        int _scoreCount = PlayerPrefs.GetInt("ScoreCount");
        for (int i = 0; i < _scoreCount; i++)
        {
            PlayerPrefs.DeleteKey("Score_" + i);
        }
        _highscoreList.Clear();
    }
    private void PrintHighScores()
    {
        if (UICheck())
        {

            string text;
            int placing = 1;

            foreach (float score in _highscoreList)
            {
                int secondsCount = (int)score % 60;
                int minutesCount = (int)score / 60;

                text = placing + ". " + minutesCount.ToString("00") + ":" + secondsCount.ToString("00");
                _highscoreText.text += text + "\n";
                placing++;
            }


        }
        else
        {
            return;
        }
    }
    private void PrintYourScore()
    {
        if (_holdScore && UICheck())
        {
            int secondsCount = (int)_heldScore % 60;
            int minutesCount = (int)_heldScore / 60;
            _yourScoreText.text = "Your time: " + minutesCount.ToString("00") + ":" + secondsCount.ToString("00") + "\n\n";

        }
        else if (!_holdScore && UICheck())
        {
            _yourScoreText.text = "";
        }
        else
        {
            return;
        }
    }
    private void AddScoreToList(float score)
    {
        if (score > 0)
        {
            _highscoreList.Add(score);
            _highscoreList.Sort();
            if (_highscoreList.Count > _scoresSaved)
            {
                _highscoreList.RemoveAt(_scoresSaved);
            }
            SaveScore();
        }
    }
    private void ModeSelecter()
    {
        if (UICheck() && MainMenu.Instance.DisplayScores())
        {
            _winningText.enabled = false;
            _yourScoreText.enabled = false;
            _highscoreHeader.enabled = true;
            _highscoreText.enabled = true;
            _casualCongratz.enabled = false;
        }
        else if (UICheck() && MainMenu.Instance.CheckTimer())
        {
            _winningText.enabled = true;
            _yourScoreText.enabled = true;
            _highscoreHeader.enabled = true;
            _highscoreText.enabled = true;
            _casualCongratz.enabled = false;
        }

        else if (UICheck() && !MainMenu.Instance.CheckTimer())
        {
            _winningText.enabled = true;
            _yourScoreText.enabled = false;
            _highscoreHeader.enabled = false;
            _highscoreText.enabled = false;
            _casualCongratz.enabled = true;
        }
    }
    private bool UICheck()
    {
        if (_winningText == null || _yourScoreText == null || _highscoreHeader == null || _highscoreText == null || _casualCongratz == null)
        {
            return false;
        }
        else
        {
            return true;
        }
    }



    public void CompareScore(float score)
    {
        _holdScore = true;
        _heldScore = score;
        Debug.Log(_holdScore);
        Debug.Log(_heldScore);

        int index = _scoresSaved - 1;
        if (_highscoreList.Count < _scoresSaved || score < _highscoreList[index])
        {
            AddScoreToList(score);
        }
    }
    public List<float> GetScores()
    {
        return _highscoreList;
    }

    public void ReturnToMainMenu()
    {
        _holdScore = false;
        _heldScore = 0;
        MainMenu.Instance.ReturnedToMain();
        SceneManager.LoadScene(0);
    }


}
