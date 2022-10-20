using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TrashCollectedUi : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _text;
    private void Start()
    {
       
    }
    void UpDateUi(float _currentTrash, float _maxTrash)
    {
        _text.text = _currentTrash.ToString() + "/" + _maxTrash.ToString();
    }
    private void OnDisable()
    {
        
    }
}
