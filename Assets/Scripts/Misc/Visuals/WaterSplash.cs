using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSplash : MonoBehaviour
{
    //Suya bir nesne düşünce fışkırır
    public GameObject waterSplashEffect;
    SfxPlayer sfxPlayer;

    private void Start()
    {
        sfxPlayer = GameObject.Find("GameManager").GetComponent<SfxPlayer>();
    }

    private void OnTriggerEnter(Collider col)
    {
        Splash(col.transform.position);
    }

    private void OnTriggerExit(Collider col)
    {
        Splash(col.transform.position);
    }

    void Splash(Vector3 position)
    {
        Vector3 spawnPos = position;
        spawnPos.y = transform.position.y + 0.01f;
        Instantiate(waterSplashEffect, spawnPos, Quaternion.identity);
        sfxPlayer.PlaySFX(12, spawnPos);
    }
}
