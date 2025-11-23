using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Lever : InteractableObject
{
    [SerializeField] GameObject leverChargeIndicator;
    public bool initialState = false;
    public bool secondaryState = false;
    public bool interactable = true;
    public float chargeTime = 5;
    public UnityEvent whenLeverOn;
    public UnityEvent whenLeverOff;
    public MonitorSystem monitorSystem;
    [SerializeField] PuzzleCable[] cables;

    GameObject leverHandle; //Animasyon için
    ParticleSystem sparkEffect; //Açıkken kıvılcım çıkar
    bool isLeverOn = false;
    float electricalPower = 0;  //elektrik gücü, tabancayla güçlenir

    public override void OnStart()
    {
        leverHandle = transform.GetChild(0).gameObject;
        sparkEffect = GetComponentInChildren<ParticleSystem>();

        int lastSaveId = PlayerPrefs.GetInt("progress");
        if (lastSaveId / 2 == UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex - 2 && lastSaveId % 2 == 1)
            SetLeverState(secondaryState);
        else
            SetLeverState(initialState);

        if (!interactable)
        {
            feedback = false;
            leverHandle.SetActive(false);
        }

        monitorSystem.ChangeStates(false);
    }

    public override void UpdateCycle()
    {
        //Güç azaltma
        if(electricalPower > 0)
        {
            electricalPower -= Time.deltaTime;
            if (electricalPower <= 0 && !isLeverOn)
            {
                whenLeverOff.Invoke();
                sparkEffect.Stop();

                monitorSystem.ChangeStates(false);
                foreach (PuzzleCable cable in cables)
                {
                    cable.ChangeCablePowered(false);
                }
            }
        }
    }

    public override void OnInteract(Interaction interactedBy)
    {
        if (interactable)
        {

            isLeverOn = !isLeverOn;

            if (isLeverOn)
            {
                whenLeverOn.Invoke();
            }
            else if (electricalPower <= 0)
            {
                whenLeverOff.Invoke();
                sparkEffect.Stop();
            }

            GetComponent<AudioSource>().Play();
            StartCoroutine(LeverAnimation());
        }

        monitorSystem.ChangeStates(isLeverOn);
        foreach (PuzzleCable cable in cables)
        {
            cable.ChangeCablePowered(isLeverOn);
        }
    }

    public override void OnCharge()
    {
        Charge();

        LeverChargeIndicator indicator = Instantiate(leverChargeIndicator, Vector3.zero, Quaternion.identity, GameObject.Find("Canvas").transform).GetComponent<LeverChargeIndicator>();
        indicator.leverTransform = transform;
        indicator.chargeDuration = chargeTime;
    }

    public void SetLeverState(bool state)
    {
        isLeverOn = state;

        if (isLeverOn)
        {
            whenLeverOn.Invoke();
        }
        else if (electricalPower <= 0)
        {
            whenLeverOff.Invoke();
            sparkEffect.Stop();
        }

        GetComponent<AudioSource>().Play();
        StartCoroutine(LeverAnimation());

        monitorSystem.ChangeStates(isLeverOn);
        foreach (PuzzleCable cable in cables)
        {
            cable.ChangeCablePowered(isLeverOn);
        }
    }

    IEnumerator LeverAnimation()
    {
        bool animCompleted = false;
        while (!animCompleted)
        {
            if (isLeverOn)
            {
                leverHandle.transform.Rotate(2, 0, 0);
                if (leverHandle.transform.localEulerAngles.x > 90 && leverHandle.transform.localEulerAngles.x < 300)
                    animCompleted = true;
            }
            else
            {
                leverHandle.transform.Rotate(-2, 0, 0);
                if (leverHandle.transform.localEulerAngles.x > 60 && leverHandle.transform.localEulerAngles.x < 270)
                    animCompleted = true;

            }

            yield return new WaitForEndOfFrame();
        }

        if(isLeverOn) sparkEffect.Play();
    }

    public void Charge()
    {
        electricalPower = chargeTime;
        whenLeverOn.Invoke();

        monitorSystem.ChangeStates(true);
        foreach (PuzzleCable cable in cables)
        {
            cable.ChangeCablePowered(true);
        }
    }
}
