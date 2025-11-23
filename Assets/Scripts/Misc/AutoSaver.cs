using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AutoSaver : MonoBehaviour
{
    [SerializeField] GameObject savePopup;
    bool triggerable = false;
    
    void Awake()
    {
        if(PlayerPrefs.GetInt("progress") % 2 == 1)
            Destroy(gameObject);
        else
            triggerable = true;
    }

    public void SaveSecondaryState()
    {
        if (triggerable)
        {
            PlayerPrefs.SetInt("progress", SceneManager.GetActiveScene().buildIndex * 2 - 3);

            triggerable = false;
            StartCoroutine(ShowSavedPopup());
        }
    }

    IEnumerator ShowSavedPopup()
    {
        savePopup.SetActive(true);

        yield return new WaitForSeconds(1);

        for (int t = 20; t > 0; t--)
        {
            savePopup.transform.localScale = Vector3.one * t * 0.05f;
            yield return null;
        }
        savePopup.SetActive(false);

        Destroy(gameObject);
    }
}
