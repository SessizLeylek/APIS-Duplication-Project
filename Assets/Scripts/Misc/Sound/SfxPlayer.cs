using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SfxPlayer : MonoBehaviour
{
    public AudioClip[] clips;
    public GameObject sourcePrefab;

    public void PlaySFX(int id, Vector3 loc)
    {
        GameObject obj = Instantiate(sourcePrefab.gameObject, loc, Quaternion.identity);
        obj.GetComponent<AudioSource>().clip = clips[id];
        obj.GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("setting_sound");
        obj.GetComponent<AudioSource>().Play();
        Destroy(obj, clips[id].length);
    }
}
