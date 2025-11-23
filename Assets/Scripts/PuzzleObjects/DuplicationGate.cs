using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuplicationGate : MonoBehaviour
{
    public Vector3 duplicationOffset;
    public bool isActive = true;
    [HideInInspector] public bool duplicatedHaveBattery = false;
    [HideInInspector] public bool duplicatedChargedPistol = false;

    BoxCollider bcollider;
    GameObject grill;   //Kopyalama ızgarası
    ParticleSystem activateParticle;    //ızgara açılınca çıkan efekt

    void Start()
    {
        bcollider = GetComponent<BoxCollider>();
        grill = transform.GetChild(0).gameObject;
        activateParticle = transform.GetChild(1).GetComponent<ParticleSystem>();

        grill.SetActive(isActive);
        bcollider.enabled = isActive;
    }

    public void ChangeState(bool newState)
    {
        isActive = newState;
        grill.SetActive(isActive);
        bcollider.enabled = isActive;

        FindObjectOfType<SfxPlayer>().PlaySFX(newState ? 8 : 9, transform.position);

        activateParticle.Play();
    }
}
