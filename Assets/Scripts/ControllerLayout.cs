using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerLayout : MonoBehaviour
{
    public GameObject image;

    public void buttonClicked()
    {
        if (image.activeInHierarchy == true)
            image.SetActive(false);
        else
            image.SetActive(true);
        
    }
    
   
    
}
