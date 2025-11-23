using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundLevelSetting : MonoBehaviour
{
    AudioSource audioSource;

    void Start()
    {
        SetSoundLevel();
    }

    public void SetSoundLevel()
    {
        if(audioSource == null)
            audioSource = GetComponent<AudioSource>();

        audioSource.volume = PlayerPrefs.GetFloat("setting_sound");
    }
}
