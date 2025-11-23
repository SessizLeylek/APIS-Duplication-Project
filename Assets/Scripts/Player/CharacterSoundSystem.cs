using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSoundSystem : MonoBehaviour
{
    public GameObject SfxSource;
    public AudioClip[] stepSounds;

    //Karakter için ses oynatma sistemi
    public void PlaySound(string soundType, float time)
    {
        GameObject sfxObj = Instantiate(SfxSource, transform.position, Quaternion.identity);
        AudioSource source = sfxObj.GetComponent<AudioSource>();

        switch (soundType)
        {
            case "stone":
                source.clip = stepSounds[Random.Range(0,3)];
                break;
            case "metal":
                source.clip = stepSounds[Random.Range(3, 6)];
                break;
            case "dirt":
                source.clip = stepSounds[Random.Range(6, 9)];
                break;
            case "glass":
                source.clip = stepSounds[Random.Range(9, 12)];
                break;
            case "water":
                source.clip = stepSounds[Random.Range(12, 15)];
                break;
        }

        source.volume = PlayerPrefs.GetFloat("setting_sound");
        source.pitch = Random.Range(0.9f, 1.1f);
        source.Play();
        Destroy(sfxObj, time);
    }
}
