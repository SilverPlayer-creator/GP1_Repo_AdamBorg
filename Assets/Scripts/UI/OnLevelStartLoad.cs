using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
namespace StarterAssets
{
    public class OnLevelStartLoad : MonoBehaviour
    {
        [SerializeField] GameObject _player, _container, _trashParent, _panel, _pauseMenu;
        [SerializeField] Button _button;
        [SerializeField] TextMeshProUGUI _text;
        [SerializeField] float _maxLoadTime;
        List<GameObject> _spawnedTrash;
        List<GameObject> _stillTrash;
        float _progress;
        bool _loadingDone;
        bool _bufferDone;
        bool _textLooped;

        private void Awake()
        {
            Debug.Log("Awake");
            _spawnedTrash = new List<GameObject>();
            _stillTrash = new List<GameObject>();
            _stillTrash.Clear();
        }
        private void Start()
        {
            Debug.Log("Start");
            Debug.Log("Clear trash");
            if (_trashParent != null)
            {
                for (int i = 0; i < _trashParent.transform.childCount; i++)
                {
                    _spawnedTrash.Add(_trashParent.transform.GetChild(i).gameObject);
                }
            }
            _text.text = "Loading.";
            StartCoroutine(StartBuffer());
        }
        private void Update()
        {
            if (!_loadingDone)
            {
                if (!_textLooped)
                {
                    StartCoroutine(LoadingText());
                }
            }
            else
            {
                _text.text = "Finished";
            }
            if (_bufferDone)
            {
                if (_stillTrash.Count != _spawnedTrash.Count && _maxLoadTime > 0)
                {
                    _button.gameObject.SetActive(false);
                    foreach (GameObject item in _spawnedTrash)
                    {
                        if (item.TryGetComponent(out Trash trash))
                        {
                            if (trash.Body.velocity.magnitude <= 5 && !_stillTrash.Contains(trash.gameObject))
                            {
                                _stillTrash.Add(trash.gameObject);
                                _progress = (float)_stillTrash.Count / (float)_spawnedTrash.Count;
                            }
                        }
                    }
                    _maxLoadTime -= Time.deltaTime;
                }
                else
                {
                    _loadingDone = true;
                    StopAllCoroutines();
                    _button.gameObject.SetActive(true);
                }
            }
        }
        public void AddTrashToList(GameObject objectToAdd)
        {
            _spawnedTrash.Add(objectToAdd);
        }
        public void StartGame()
        {
            _panel.SetActive(false);
            _player.SetActive(true);
            _container.SetActive(true);
            if(_pauseMenu != null) _pauseMenu.SetActive(true);
        }
        void OnRestart()
        {
            _spawnedTrash.Clear();
            _stillTrash.Clear();
            Debug.Log("Restart");
        }
        IEnumerator StartBuffer()
        {
            yield return new WaitForSeconds(5f);
            _bufferDone = true;
        }
        IEnumerator LoadingText()
        {
            _textLooped = true;
            _text.text = "Loading.";
            yield return new WaitForSeconds(0.3f);
            _text.text = "Loading..";
            yield return new WaitForSeconds(0.3f);
            _text.text = "Loading...";
            yield return new WaitForSeconds(0.3f);
            _textLooped = false;
        }
    }
}
