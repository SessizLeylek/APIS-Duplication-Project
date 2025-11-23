using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [Header("Scene Objects")]
    public GameObject PostProcessLayer;
    public Transform[] mainMenuButtons;
    public Transform[] settingsMain;
    public Transform[] settingsTabs;    //0 ses, 1 görüntü, 2 kontroller, 3 diğer

    [Header("Settings Objects")]
    public Slider musicSlider;
    public Slider soundSlider;
    public Slider sensitivitySlider;
    public Slider fovSlider;
    public Slider viewRangeSlider;
    public Toggle postprocessToggle;
    public Toggle headbobToggle;
    public Toggle vsyncToggle;
    public Toggle fullscreenToggle;
    public Toggle twinCamToggle;
    public Dropdown resolutionDropdown;
    public Text fovText;
    public Text viewRangeText;
    public Text qualityButton;

    int[,] resolutions = { { 1280, 720 }, { 1366, 768 }, { 1600, 900 }, { 1920, 1080 }, { 2560, 1440 }, { 3840, 2160 } };
    AudioSource musicSource;

    void Start()
    {
        musicSource = GetComponent<AudioSource>();

        //Ayarlar
        if (PlayerPrefs.GetInt("setting_postprocess") == 0)
            PostProcessLayer.SetActive(false);
        
        musicSource.volume = PlayerPrefs.GetFloat("setting_music");
        
        FindObjectOfType<Camera>().fieldOfView = PlayerPrefs.GetInt("setting_fov");

        QualitySettings.vSyncCount = PlayerPrefs.GetInt("setting_vsync");
        QualitySettings.SetQualityLevel(PlayerPrefs.GetInt("setting_quality"));

        int res = PlayerPrefs.GetInt("setting_resolution"); 
        Screen.SetResolution(resolutions[res, 0], resolutions[res, 1], PlayerPrefs.GetInt("setting_fullscreen") == 1);
    }
    
    public void StartGame()
    {
        SceneManager.LoadScene(PlayerPrefs.GetInt("progress") / 2 + 2);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    #region MENU_OBJELERI_GIZLE/GOSTER
    IEnumerator HideShowObjects(Transform[] hideObjects, Transform[] showObjects)
    {
        float sizeY = 1;

        if (hideObjects != null)
        {
            while (sizeY > 0)
            {
                sizeY -= Time.deltaTime * 3f;
                foreach (Transform obj in hideObjects)
                {
                    obj.localScale = new Vector3(obj.localScale.x, sizeY, 1);
                }

                yield return null;
            }
            foreach (Transform obj in hideObjects)
            {
                obj.localScale = new Vector3(0, 0, 1);
            }
        }

        if (showObjects != null)
        {
            sizeY = 0;
            while (sizeY < 1)
            {
                sizeY += Time.deltaTime * 3f;
                foreach (Transform obj in showObjects)
                {
                    obj.localScale = new Vector3(1, sizeY, 1);
                }

                yield return null;
            }
            foreach (Transform obj in showObjects)
            {
                obj.localScale = new Vector3(1, 1, 1);
            }
        }
    }
#endregion

    #region AYARLAR
    public void OpenSettings()
    {
        Transform[] obj2open = new Transform[settingsMain.Length + 1];
        settingsMain.CopyTo(obj2open, 0);
        obj2open.SetValue(settingsTabs[0], settingsMain.Length);

        StartCoroutine(HideShowObjects(mainMenuButtons, obj2open));
        
        musicSlider.value = PlayerPrefs.GetFloat("setting_music");
        soundSlider.value = PlayerPrefs.GetFloat("setting_sound");
        sensitivitySlider.value = PlayerPrefs.GetInt("setting_sensitivity");
        fovSlider.value = PlayerPrefs.GetInt("setting_fov");
        viewRangeSlider.value = PlayerPrefs.GetInt("setting_viewrange");
        postprocessToggle.isOn = PlayerPrefs.GetInt("setting_postprocess") == 1;
        headbobToggle.isOn = PlayerPrefs.GetInt("setting_headbob") == 1;
        twinCamToggle.isOn = PlayerPrefs.GetInt("setting_twincam") == 1;
        vsyncToggle.isOn = PlayerPrefs.GetInt("setting_vsync") == 1;
        fullscreenToggle.isOn = PlayerPrefs.GetInt("setting_fullscreen") == 1;
        fovText.text = "FOV (" + PlayerPrefs.GetInt("setting_fov") + "):";
        viewRangeText.text = "View Range (" + PlayerPrefs.GetInt("setting_viewrange") + "):";
        resolutionDropdown.value = PlayerPrefs.GetInt("setting_resolution");

        int qualityVal = PlayerPrefs.GetInt("setting_quality");
        qualityButton.text = qualityVal == 0 ? "Quality: LOW" : (qualityVal == 1 ? "Quality: MEDIUM" : "Quality: HIGH");
    }

    public void CloseSettings()
    {
        Transform[] obj2close = new Transform[settingsMain.Length + settingsTabs.Length];
        settingsMain.CopyTo(obj2close, 0);
        settingsTabs.CopyTo(obj2close, settingsMain.Length);

        StartCoroutine(HideShowObjects(obj2close, mainMenuButtons));
    }

    public void ChangeTab(int tabNumber)
    {
        Transform[] tab2open = { settingsTabs[tabNumber] };
        StartCoroutine(HideShowObjects(settingsTabs, tab2open));
    }

    //SES AYARLARI
    public void ChangeSoundLevel(float newVal)
    {
        PlayerPrefs.SetFloat("setting_sound", newVal);

        SoundLevelSetting[] soundLevels = FindObjectsOfType<SoundLevelSetting>();
        foreach (SoundLevelSetting snd in soundLevels)
        {
            snd.SetSoundLevel();
        }
    }

    public void ChangeMusicLevel(float newVal)
    {
        PlayerPrefs.SetFloat("setting_music", newVal);
        musicSource.volume = newVal;
    }

    //GÖRÜNTÜ AYARLARI
    public void ChangeQualityMode()
    {
        int val = PlayerPrefs.GetInt("setting_quality");
        if (val == 2)
            val = 0;
        else
            val++;

        PlayerPrefs.SetInt("setting_quality", val);
        QualitySettings.SetQualityLevel(val);

        qualityButton.text = val == 0 ? "Quality: LOW" : (val == 1 ? "Quality: MEDIUM" : "Quality: HIGH");
    }

    public void ChangeResolution(int res)
    {
        PlayerPrefs.SetInt("setting_resolution", res);

        Screen.SetResolution(resolutions[res, 0], resolutions[res, 1], PlayerPrefs.GetInt("setting_fullscreen") == 1);
    }

    public void ChangePostProcessToggle(bool newState)
    {
        PostProcessLayer.SetActive(newState);
        PlayerPrefs.SetInt("setting_postprocess", newState ? 1 : 0);
    }

    public void ChangeHeadBobToggle(bool newState)
    {
        PlayerPrefs.SetInt("setting_headbob", newState ? 1 : 0);
    }

    public void ChangeVsyncToggle(bool newState)
    {
        PlayerPrefs.SetInt("setting_vsync", newState ? 1 : 0);

        QualitySettings.vSyncCount = newState ? 1 : 0;
    }

    public void ChangeFullscreenToggle(bool newState)
    {
        PlayerPrefs.SetInt("setting_fullscreen", newState ? 1 : 0);

        Screen.fullScreen = newState;
    }

    public void ChangeTwinCamToggle(bool newState)
    {
        PlayerPrefs.SetInt("setting_twincam", newState ? 1 : 0);
    }

    public void ChangeFovLevel(float newVal)
    {
        PlayerPrefs.SetInt("setting_fov", Mathf.RoundToInt(newVal));

        fovText.text = "FOV (" + newVal + "):";
        FindObjectOfType<Camera>().fieldOfView = newVal;
    }

    public void ChangeViewRange(float newVal)
    {
        PlayerPrefs.SetInt("setting_viewrange", Mathf.RoundToInt(newVal));

        viewRangeText.text = "View Range (" + newVal + "):";
        FindObjectOfType<Camera>().farClipPlane= newVal;
    }

    //KONTROL AYARLARI
    public void ChangeSensitivityLevel(float newVal)
    {
        PlayerPrefs.SetInt("setting_sensitivity", int.Parse(newVal.ToString()));
    }

    //Veri sıfırlama
    public void ClearData()
    {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene(0);
    }


    #endregion
}
