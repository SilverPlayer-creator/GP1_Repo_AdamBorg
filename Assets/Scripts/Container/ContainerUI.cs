using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
namespace StarterAssets
{
    public class ContainerUI : MonoBehaviour
    {
        [SerializeField] Container _container;
        [SerializeField] TextMeshProUGUI _text;
        [SerializeField] GameObject _victoryPanel;

        bool _victory;
        float _countDown = 5;
        private void Awake()
        {
            _container.OnTrashpickUp += UpdateUI;
            _container.OnWinCondition += AnnounceVictory;
        }
        private void Update()
        {

        }
        void UpdateUI(float _trashCollected, float _maxTrash)
        {
            //_text.text = _trashCollected.ToString() + "/" + _maxTrash.ToString();
            if (_trashCollected > 0)
            {
                float percent = (_trashCollected / _maxTrash) * 100;
                _text.text = percent.ToString() + "%";
            }
            else
            {
                _text.text = 0.ToString() + "%";
            }
        }
        void AnnounceVictory()
        {
            Cursor.lockState = CursorLockMode.None;
            _victoryPanel.SetActive(true);
            int nextIndex = SceneManager.GetActiveScene().buildIndex + 1;
            SceneManager.LoadScene(nextIndex);
        }
        private void OnDisable()
        {
            _container.OnTrashpickUp -= UpdateUI;
            _container.OnWinCondition -= AnnounceVictory;
        }
    }

}