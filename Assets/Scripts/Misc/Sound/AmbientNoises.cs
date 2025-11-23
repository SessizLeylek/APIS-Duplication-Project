using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientNoises : MonoBehaviour
{
    public AudioClip[] noiseClips;
    public int delay;

    AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        
        StartCoroutine(NoiseCycle());
    }

    IEnumerator NoiseCycle()
    {
        while (true)
        {
            audioSource.clip = noiseClips[Random.Range(0, noiseClips.Length)];
            audioSource.Play();

            yield return new WaitForSeconds(delay);
        }
    }
}
