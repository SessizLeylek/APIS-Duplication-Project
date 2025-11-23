using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Initializer : MonoBehaviour
{
    //Müzik objesini oluşturur, oyun ilk kez açılıyorsa değişkenleri kaydeder
    [SerializeField] GameObject musicObject;

    void Start()
    {
        DontDestroyOnLoad(Instantiate(musicObject));

        if(PlayerPrefs.GetInt("first_init") == 0)
        {
            PlayerPrefs.SetInt("first_init", 1);

            PlayerPrefs.SetInt("progress", 0);
            PlayerPrefs.SetFloat("setting_music", 0.3f);
            PlayerPrefs.SetFloat("setting_sound", 1);
            PlayerPrefs.SetInt("setting_sensitivity", 25);
            PlayerPrefs.SetInt("setting_fov", 70);
            PlayerPrefs.SetInt("setting_postprocess", 1);
            PlayerPrefs.SetInt("setting_headbob", 1);
            PlayerPrefs.SetInt("setting_vsync", 0);
            PlayerPrefs.SetInt("setting_fullscreen", 1);
            PlayerPrefs.SetInt("setting_quality", 0);
            PlayerPrefs.SetInt("setting_twincam", 1);
            PlayerPrefs.SetInt("setting_viewrange", 200);
        }

        SceneManager.LoadScene(1);
    }
}
