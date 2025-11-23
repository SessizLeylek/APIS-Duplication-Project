using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EscapeMenu : MonoBehaviour
{
    public CharacterControlling mainPlayer;
    public CharacterControlling duplicatedPlayer;
    public GameObject settingsMenu;
    public Transform[] menuObjects;
    public Slider[] sliders;    //0 ses, 1 müzik, 2 fov, 3 hassasiyet
    public Toggle[] toggles;    //0 altyazılar, 1 sallanma
    public Text fovText;
    public GameObject[] storyRelatedPanels; //Hikaye için açılacak olan diğer ui panelleri

    bool isMenuOn = false;  //Menü açık mı
    int activePanel = -1;   //Aktif panel numarası, -1 : hiçbiri
    MusicSystem musicSystem;

    void Start()
    {
        //Ayar değerlerini ayarlama
        sliders[0].value = PlayerPrefs.GetFloat("setting_sound");
        sliders[1].value = PlayerPrefs.GetFloat("setting_music");
        sliders[2].value = PlayerPrefs.GetInt("setting_fov");
        sliders[3].value = PlayerPrefs.GetInt("setting_sensitivity");
        toggles[0].isOn = PlayerPrefs.GetInt("setting_subtitle") == 1;
        toggles[0].isOn = PlayerPrefs.GetInt("setting_headbob") == 1;
        fovText.text = $"FOV ({PlayerPrefs.GetInt("setting_fov")}): ";

        settingsMenu.SetActive(false);
        ChangeCursorVisibility(false);
    }

    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            isMenuOn = !isMenuOn;
            if (isMenuOn)
            {
                ChangeCursorVisibility(true);
                StartCoroutine(ButtonsAnimation(1));
                activePanel= -1;
            }
            else
            {
                CloseMenu();
            }
        }
    }

    public void ChangeCursorVisibility(bool newState)
    {
        Cursor.visible = newState;
        Cursor.lockState = newState ? CursorLockMode.None : CursorLockMode.Locked;
    }

    public void ActivateStoryPanel(int panelNo)
    {
        activePanel = panelNo;
        isMenuOn = true;

        ChangeCursorVisibility(true);
        storyRelatedPanels[panelNo].SetActive(true);
        Time.timeScale = 0;
    }

    public void CloseMenu()
    {
        isMenuOn = false;
        settingsMenu.SetActive(false);
        ChangeCursorVisibility(false);

        if (activePanel == -1)
            StartCoroutine(ButtonsAnimation(-1));
        else
        {
            Time.timeScale = 1;
            storyRelatedPanels[activePanel].SetActive(false);
        }
    }

    public void Suicide()
    {
        GameObject.Find("MainPlayer").GetComponent<CharacterControlling>().KillPlayer();
        CloseMenu();
    }

    public void BackToMainMenu()
    {
        MusicSystem[] musicObjects = FindObjectsOfType<MusicSystem>();
        foreach (MusicSystem mus in musicObjects)
        {
            Destroy(mus.gameObject);
        }

        Time.timeScale = 1;

        SceneManager.LoadScene(0);
    }

    #region AYARLARI DEĞİŞTİRME
    public void ChangeSoundLevel(float level)
    {
        PlayerPrefs.SetFloat("setting_sound", level);

        SoundLevelSetting[] soundLevels = FindObjectsOfType<SoundLevelSetting>();

        foreach (SoundLevelSetting snd in soundLevels)
        {
            snd.SetSoundLevel();
        }

    }

    public void ChangeMusicLevel(float level)
    {
        PlayerPrefs.SetFloat("setting_music", level);

        if (musicSystem)
            musicSystem.ResetVolume();
        else
            musicSystem = FindObjectOfType<MusicSystem>();

    }

    public void ChangeFovLevel(float level)
    {
        int newVal = int.Parse(level.ToString());
        PlayerPrefs.SetInt("setting_fov", newVal);

        fovText.text = $"FOV ({newVal}): ";

        mainPlayer.camera.SetSettings();
        duplicatedPlayer.camera.SetSettings();
    }

    public void ChangeSensitivityLevel(float level)
    {
        PlayerPrefs.SetInt("setting_sensitivity", int.Parse(level.ToString()));

        mainPlayer.camera.SetSettings();
        duplicatedPlayer.camera.SetSettings();
    }

    public void ChangeHeadBobOn(bool newState)
    {
        PlayerPrefs.SetInt("setting_headbob", newState ? 1 : 0);

        mainPlayer.camera.SetSettings();
        duplicatedPlayer.camera.SetSettings();
    }
    #endregion

    //Butonların hareketli açılması
    IEnumerator ButtonsAnimation(int animDir)
    {
        //animDir; 1 ise açılır, -1 ise kapanır
        float currentSize = (animDir - 1) / -2;
        
        if (animDir == 1)   //aktiflik ayarlama
        {
            foreach (Transform button in menuObjects)
            {
                button.gameObject.SetActive(true);
                button.localScale = new Vector3(1, currentSize, 1);
            }
        }

        float startTime = Time.unscaledTime;
        while (Time.unscaledTime - startTime < 0.25f)   //boyut değiştirme
        {
            currentSize = (Time.unscaledTime - startTime) * 4 * animDir + (animDir - 1) / -2;
            foreach (Transform button in menuObjects)
            {
                button.localScale = new Vector3(1, currentSize, 1);
            }
            yield return null;
        }

        //aktiflik ayarlama
        foreach (Transform button in menuObjects)
        {
            if (animDir == -1)
                button.gameObject.SetActive(false);
            else
                button.localScale = new Vector3(1, (animDir + 1) / 2, 1);
        }

        Time.timeScale = (animDir - 1) / -2;
    }
}
