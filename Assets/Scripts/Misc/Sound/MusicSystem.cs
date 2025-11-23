using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicSystem : MonoBehaviour
{
    public AudioSource audioSource;

    void Start()
    {
        ResetVolume();
    }

    public void ResetVolume()
    {
        audioSource.volume = PlayerPrefs.GetFloat("setting_music");
    }

    ///<summary>
    ///Aniden muzigi degistirir
    ///</summary>
    public void ChangeMusic(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }

    ///<summary>
    ///Muzik yavasca once sesi kısılır sonra artar
    ///</summary>
    public IEnumerator SlowlyChange(AudioClip clip)
    {
        float musicLevel = PlayerPrefs.GetFloat("setting_music");
        float volumeMultipliler = 1;
        if (audioSource.isPlaying)
        {
            while (volumeMultipliler > 0)
            {
                audioSource.volume = musicLevel * volumeMultipliler;

                volumeMultipliler -= Time.deltaTime;
                yield return null;
            }
        }

        audioSource.clip = clip;
        audioSource.Play();

        while (volumeMultipliler < 1)
        {
            audioSource.volume = musicLevel * volumeMultipliler;

            volumeMultipliler += Time.deltaTime;
            yield return null;
        }

        ResetVolume();
    }
}
