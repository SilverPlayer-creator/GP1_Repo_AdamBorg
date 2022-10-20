using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSelect : MonoBehaviour
{
    Button _primaryButton;
    private void Awake()
    {
        _primaryButton = GetComponent<Button>();
        _primaryButton.Select();
    }
}
