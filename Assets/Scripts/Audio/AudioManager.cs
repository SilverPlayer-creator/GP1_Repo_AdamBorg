using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AudioManager 
{
    public static void PlaySound()
    {
        GameObject soundObject = new GameObject("Sound");
        AudioSource source = soundObject.AddComponent<AudioSource>();
    }
}
