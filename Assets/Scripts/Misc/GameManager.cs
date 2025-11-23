using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject PPV;
    public AudioClip[] sceneMusics;

    void Start()
    {
        print(SystemInfo.graphicsDeviceName);

        if (PlayerPrefs.GetInt("setting_postprocess") == 1)
            PPV.SetActive(true);
    }

    //Sahne geçişi
    public void GoToScene(int levelNumber)
    {
        GameObject.Find("Canvas/LoadingScreen").transform.localScale = new Vector3(1, 1, 1);

        PlayerPrefs.SetInt("progress", levelNumber * 2);
        SceneManager.LoadScene(levelNumber + 1);
    }

    //Müzik oynatma
    public void PlayMusicSudden(int musicNumber)
    {
        MusicSystem musicSystem = FindObjectOfType<MusicSystem>();
        if (musicSystem)
        {
            if(musicSystem.audioSource.clip != sceneMusics[musicNumber])
                musicSystem.ChangeMusic(sceneMusics[musicNumber]);
        }
    }

    public void PlayMusicSlowly(int musicNumber)
    {
        MusicSystem musicSystem = FindObjectOfType<MusicSystem>();
        if (musicSystem)
        {
            if (musicSystem.audioSource.clip != sceneMusics[musicNumber])
                StartCoroutine(musicSystem.SlowlyChange(sceneMusics[musicNumber]));
        }
    }
}
