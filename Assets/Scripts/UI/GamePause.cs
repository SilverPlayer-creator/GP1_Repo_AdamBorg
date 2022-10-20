using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StarterAssets
{
    public class GamePause : MonoBehaviour
    {
        [SerializeField] StarterAssetsInputs _input;
        [SerializeField] GameObject pauseMenuUI;
        private bool GameIsPaused = false;
        float _timeToPause;
        void Start()
        {
            Resume();
        }
        void Update()
        {
            if (_input.EscapeIsressed && Time.time >= _timeToPause)
            {
                if (GameIsPaused)
                {
                    Resume();
                }
                else
                {
                    Pause();
                }
                _input.RevertEscape();
                _timeToPause = Time.time + 1 / 2;
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
    }
}
