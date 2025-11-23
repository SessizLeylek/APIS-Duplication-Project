using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vines : MonoBehaviour
{
    GameObject fireEffect;
    Animator animator;
    AudioSource audioSource;
    bool isBurnt = false;

    void Start()
    {
        fireEffect = transform.GetChild(0).gameObject;
        animator= GetComponent<Animator>();
        audioSource= GetComponent<AudioSource>();   
    }

    void OnTriggerEnter(Collider other)
    {
        animator.Play("VineWiggle");
        audioSource.Play();
    }

    public void Burn()
    {
        if (!isBurnt)
        {
            isBurnt = true;
            fireEffect.SetActive(true);

            fireEffect.transform.SetParent(null);
            Destroy(fireEffect, 7);
            Destroy(gameObject, 5);
        }
    }
}
