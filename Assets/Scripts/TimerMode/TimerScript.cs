using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
namespace StarterAssets
{
    public class TimerScript : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _timerUI;
        [SerializeField] Container _container;
        [SerializeField] private bool _timer = true;


        private float _counter;


        private void Awake()
        {
            if (!MainMenu.Instance.CheckTimer())
            {
                this.gameObject.SetActive(false);
            }

            if (_container != null)
            {
                _container.OnWinCondition += StoreTime;
            }

        }
        private void OnDisable()
        {
            if (_container != null)
            {
                _container.OnWinCondition -= StoreTime;
            }
        }
        void Update()
        {
            Timer();
            _timerUI.text = TimeToText(_counter);
        }

        private void Timer()
        {
            if (_timer)
            {
                _counter += Time.deltaTime;
            }
        }
        public void StartTimer()
        {
            _timer = true;
        }
        public void StopTimer()
        {
            _timer = false;
        }

        public void StoreTime()
        {
            if (_timer)
            {
                HighscoreScript.Instance.CompareScore(_counter);
                StopTimer();
            }
        }

        private string TimeToText(float time)
        {
            int secondsCount = (int)time % 60;
            int minutesCount = (int)time / 60;

            string text = minutesCount.ToString("00") + ":" + secondsCount.ToString("00");
            return text;
        }

    }
}
